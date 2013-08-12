#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

namespace LocationImport
{
// TODO: cope with quoted strings as\when this becomes necessary,
    /// <summary>
    ///   The separated value reader.
    /// </summary>
    public abstract class SeparatedValueReader : IDataReader
    {
        /// <summary>
        ///   The _column headers.
        /// </summary>
        private readonly Dictionary<int, string> _columnHeaders = new Dictionary<int, string>();

        /// <summary>
        ///   The _first row is header.
        /// </summary>
        private readonly bool _firstRowIsHeader;

        /// <summary>
        ///   The _is line comment.
        /// </summary>
        private readonly Func<string, bool> _isLineComment;

        /// <summary>
        ///   The _separator.
        /// </summary>
        private readonly char _separator;

        /// <summary>
        ///   The _current row data.
        /// </summary>
        private List<string> _currentRowData;

        /// <summary>
        ///   The _first row.
        /// </summary>
        private bool _firstRow;

        /// <summary>
        ///   The _reader.
        /// </summary>
        private StreamReader _reader;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "separator">
        ///   The separator.
        /// </param>
        /// <param name = "columnHeaders">
        ///   The column headers.
        /// </param>
        protected SeparatedValueReader(string fileName,
                                       Encoding fileEncoding,
                                       char separator,
                                       IEnumerable<string> columnHeaders)
            : this(fileName, fileEncoding, separator, columnHeaders, null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "separator">
        ///   The separator.
        /// </param>
        /// <param name = "columnHeaders">
        ///   The column headers.
        /// </param>
        /// <param name = "isLineComment">
        ///   The is line comment.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        protected SeparatedValueReader(string fileName,
                                       Encoding fileEncoding,
                                       char separator,
                                       IEnumerable<string> columnHeaders,
                                       Func<string, bool> isLineComment)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileEncoding == null)
            {
                throw new ArgumentNullException("fileEncoding");
            }

            if (columnHeaders == null)
            {
                throw new ArgumentNullException("columnHeaders");
            }

            _separator = separator;
            _isLineComment = isLineComment;

            var ordinal = 0;
            foreach (var column in columnHeaders)
            {
                _columnHeaders.Add(ordinal, column);
                ++ordinal;
            }

            _reader = OpenFile(fileName, fileEncoding);
            _firstRow = false;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "separator">
        ///   The separator.
        /// </param>
        protected SeparatedValueReader(string fileName, Encoding fileEncoding, char separator)
            : this(fileName, fileEncoding, separator, (Func<string, bool>) null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "SeparatedValueReader" /> class.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <param name = "separator">
        ///   The separator.
        /// </param>
        /// <param name = "isLineComment">
        ///   The is line comment.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        protected SeparatedValueReader(string fileName,
                                       Encoding fileEncoding,
                                       char separator,
                                       Func<string, bool> isLineComment)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileEncoding == null)
            {
                throw new ArgumentNullException("fileEncoding");
            }

            _separator = separator;
            _isLineComment = isLineComment;
            _firstRowIsHeader = true;

            _reader = OpenFile(fileName, fileEncoding);
            _firstRow = true;
        }

        #region IDataReader Members

        /// <summary>
        ///   The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   The get name.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get name.
        /// </returns>
        public string GetName(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfColumnOutOfRange(i);

            return _columnHeaders[i];
        }

        /// <summary>
        ///   The get data type name.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get data type name.
        /// </returns>
        public string GetDataTypeName(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfColumnOutOfRange(i);

            return GetFieldType(i).Name;
        }

        /// <summary>
        ///   The get field type.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        /// </returns>
        public Type GetFieldType(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfColumnOutOfRange(i);

            return typeof (string);
        }

        /// <summary>
        ///   The get value.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get value.
        /// </returns>
        public object GetValue(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return GetString(i);
        }

        /// <summary>
        ///   The get values.
        /// </summary>
        /// <param name = "values">
        ///   The values.
        /// </param>
        /// <returns>
        ///   The get values.
        /// </returns>
        /// <exception cref = "ArgumentNullException">
        /// </exception>
        public int GetValues(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();

            const int dimension = 0;

            var items = 0;
            for (var field = values.GetLowerBound(dimension); field <= values.GetUpperBound(dimension); field++)
            {
                values[field] = GetValue(field);
                ++items;
            }

            return items;
        }

        /// <summary>
        ///   The get ordinal.
        /// </summary>
        /// <param name = "name">
        ///   The name.
        /// </param>
        /// <returns>
        ///   The get ordinal.
        /// </returns>
        public int GetOrdinal(string name)
        {
            EnsureHeadersLoaded();

            for (var intColumn = 0; intColumn <= _columnHeaders.Count; intColumn++)
            {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(_columnHeaders[intColumn], name))
                {
                    return intColumn;
                }
            }

            return -1;
        }

        /// <summary>
        ///   The get boolean.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get boolean.
        /// </returns>
        public bool GetBoolean(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return bool.Parse(GetString(i));
        }

        /// <summary>
        ///   The get byte.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get byte.
        /// </returns>
        public byte GetByte(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return byte.Parse(GetString(i));
        }

        /// <summary>
        ///   The get bytes.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <param name = "fieldOffset">
        ///   The field offset.
        /// </param>
        /// <param name = "buffer">
        ///   The buffer.
        /// </param>
        /// <param name = "bufferoffset">
        ///   The bufferoffset.
        /// </param>
        /// <param name = "length">
        ///   The length.
        /// </param>
        /// <returns>
        ///   The get bytes.
        /// </returns>
        /// <exception cref = "NotSupportedException">
        /// </exception>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            throw new NotSupportedException("GetBytes is not supported");
        }

        /// <summary>
        ///   The get char.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get char.
        /// </returns>
        public char GetChar(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();

            ThrowIfColumnOutOfRange(i);

            return char.Parse(GetString(i));
        }

        /// <summary>
        ///   The get chars.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <param name = "fieldoffset">
        ///   The fieldoffset.
        /// </param>
        /// <param name = "buffer">
        ///   The buffer.
        /// </param>
        /// <param name = "bufferoffset">
        ///   The bufferoffset.
        /// </param>
        /// <param name = "length">
        ///   The length.
        /// </param>
        /// <returns>
        ///   The get chars.
        /// </returns>
        /// <exception cref = "NotSupportedException">
        /// </exception>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            throw new NotSupportedException("GetChars is Not Supported");
        }

        /// <summary>
        ///   The get guid.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        /// </returns>
        public Guid GetGuid(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return new Guid(GetString(i));
        }

        /// <summary>
        ///   The get int 16.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get int 16.
        /// </returns>
        public short GetInt16(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return short.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get int 32.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get int 32.
        /// </returns>
        public int GetInt32(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return int.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get int 64.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get int 64.
        /// </returns>
        public long GetInt64(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return long.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get float.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get float.
        /// </returns>
        public float GetFloat(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return float.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get double.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get double.
        /// </returns>
        public double GetDouble(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return double.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get string.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get string.
        /// </returns>
        public string GetString(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return _currentRowData[i];
        }

        /// <summary>
        ///   The get decimal.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The get decimal.
        /// </returns>
        public decimal GetDecimal(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return decimal.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get date time.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        /// </returns>
        public DateTime GetDateTime(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return DateTime.Parse(GetString(i), CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///   The get data.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref = "NotSupportedException">
        /// </exception>
        public IDataReader GetData(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            throw new NotSupportedException("GetData is not implemented");
        }

        /// <summary>
        ///   The is db null.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <returns>
        ///   The is db null.
        /// </returns>
        public bool IsDBNull(int i)
        {
            EnsureHeadersLoaded();
            ThrowIfNoCurrentRecord();
            ThrowIfColumnOutOfRange(i);

            return string.IsNullOrEmpty(GetString(i));
        }

        /// <summary>
        ///   Gets FieldCount.
        /// </summary>
        public int FieldCount
        {
            get
            {
                EnsureHeadersLoaded();

                return _columnHeaders.Count;
            }
        }

        /// <summary>
        ///   The this.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        public object this[int i]
        {
            get
            {
                EnsureHeadersLoaded();
                ThrowIfNoCurrentRecord();
                ThrowIfColumnOutOfRange(i);

                return GetValue(i);
            }
        }

        /// <summary>
        ///   The this.
        /// </summary>
        /// <param name = "name">
        ///   The name.
        /// </param>
        public object this[string name]
        {
            get
            {
                EnsureHeadersLoaded();
                ThrowIfNoCurrentRecord();

                var intColumn = GetOrdinal(name);
                ThrowIfColumnOutOfRange(intColumn);

                return GetValue(intColumn);
            }
        }

        /// <summary>
        ///   The close.
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }
        }

        /// <summary>
        ///   The get schema table.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref = "NotSupportedException">
        /// </exception>
        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException("GetSchemaTable is not supported");
        }

        /// <summary>
        ///   The next result.
        /// </summary>
        /// <returns>
        ///   The next result.
        /// </returns>
        public bool NextResult()
        {
            return false;
        }

        /// <summary>
        ///   The read.
        /// </summary>
        /// <returns>
        ///   The read.
        /// </returns>
        public bool Read()
        {
            // No more data
            EnsureHeadersLoaded();

            if (_firstRow)
            {
                _firstRow = false;
            }
            else
            {
                if (_reader.Peek()
                    == -1)
                {
                    _currentRowData = null;
                    return false;
                }

                var strLine = ReadNextNonCommentLine();
                var strValues = GetColumnData(strLine);

                var colRowData = new List<string>();

                for (var intColumn = 0; intColumn <= strValues.Length - 1; intColumn++)
                {
                    colRowData.Add(strValues[intColumn].Trim());
                }

                _currentRowData = colRowData;
            }

            return true;
        }

        /// <summary>
        ///   Gets Depth.
        /// </summary>
        public int Depth
        {
            get { return 1; }
        }

        /// <summary>
        ///   Gets a value indicating whether IsClosed.
        /// </summary>
        public bool IsClosed
        {
            get { return _reader == null; }
        }

        /// <summary>
        ///   Gets RecordsAffected.
        /// </summary>
        public int RecordsAffected
        {
            get { return 0; }
        }

        #endregion

        /// <summary>
        ///   Finalizes an instance of the <see cref = "SeparatedValueReader" /> class.
        /// </summary>
        ~SeparatedValueReader()
        {
            Dispose(false);
        }

        /// <summary>
        ///   The dispose.
        /// </summary>
        /// <param name = "disposing">
        ///   The disposing.
        /// </param>
        private void Dispose(bool disposing)
        {
            Close();
        }

        /// <summary>
        ///   The get column data.
        /// </summary>
        /// <param name = "pstrLine">
        ///   The pstr line.
        /// </param>
        /// <returns>
        /// </returns>
        private string[] GetColumnData(string pstrLine)
        {
            return pstrLine.Split(_separator);
        }

        /// <summary>
        ///   The ensure headers loaded.
        /// </summary>
        private void EnsureHeadersLoaded()
        {
            if (_columnHeaders.Count == 0)
            {
                var strLine = ReadNextNonCommentLine();
                var strValues = GetColumnData(strLine);

                if (_firstRowIsHeader)
                {
                    for (var intColumn = 0; intColumn <= strValues.Length - 1; intColumn++)
                    {
                        _columnHeaders.Add(intColumn, strValues[intColumn].Trim());
                    }

                    _firstRow = false;
                }
                else
                {
                    var currentRowData = new List<string>();

                    for (var column = 0; column <= strValues.Length - 1; column++)
                    {
                        _columnHeaders.Add(column, "Field" + column);

                        currentRowData.Add(strValues[column].Trim());
                    }

                    _currentRowData = currentRowData;
                    _firstRow = true;
                }
            }
        }

        /// <summary>
        ///   The read next non comment line.
        /// </summary>
        /// <returns>
        ///   The read next non comment line.
        /// </returns>
        private string ReadNextNonCommentLine()
        {
            var line = _reader.ReadLine();
            while ((_isLineComment != null)
                   && _isLineComment(line))
            {
                line = _reader.ReadLine();
            }

            return line;
        }

        /// <summary>
        ///   The throw if no current record.
        /// </summary>
        /// <exception cref = "InvalidOperationException">
        /// </exception>
        private void ThrowIfNoCurrentRecord()
        {
            if (_currentRowData == null)
            {
                throw new InvalidOperationException("No current record");
            }
        }

        /// <summary>
        ///   The throw if column out of range.
        /// </summary>
        /// <param name = "i">
        ///   The i.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// </exception>
        private void ThrowIfColumnOutOfRange(int i)
        {
            if (i < 0)
            {
                throw new ArgumentOutOfRangeException("i");
            }

            if (i >= FieldCount)
            {
                throw new ArgumentOutOfRangeException("i");
            }
        }

        /// <summary>
        ///   The open file.
        /// </summary>
        /// <param name = "fileName">
        ///   The file name.
        /// </param>
        /// <param name = "fileEncoding">
        ///   The file encoding.
        /// </param>
        /// <returns>
        /// </returns>
        private static StreamReader OpenFile(string fileName, Encoding fileEncoding)
        {
            return new StreamReader(fileName, fileEncoding, true);
        }
    }
}