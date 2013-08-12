namespace LocationImport
{
    /// <summary>
    ///   The detectors.
    /// </summary>
    internal static class Detectors
    {
        /// <summary>
        ///   The comment detector.
        /// </summary>
        /// <param name = "arg">
        ///   The arg.
        /// </param>
        /// <returns>
        ///   The comment detector.
        /// </returns>
        public static bool CommentDetector(string arg)
        {
            return arg.StartsWith("#");
        }
    }
}