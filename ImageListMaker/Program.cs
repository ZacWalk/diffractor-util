#region

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#endregion

namespace ImageListMaker
{
    internal class Program
    {
        public class TupleList<T1, T2, T3> : List<System.Tuple<T1, T2, T3>>
        {
            public void Add(T1 item, T2 item2, T3 item3)
            {
                Add(new System.Tuple<T1, T2, T3>(item, item2, item3));
            }
        }

        private static void Main(string[] args)
        {
            // http://www.flaticon.com/packs/photography-2

            var navigation = @"c:\dev\icons\material-design-icons\navigation\1x_web";
            var action = @"c:\dev\icons\material-design-icons\action\1x_web";
            var av = @"c:\dev\icons\material-design-icons\av\1x_web";
            var image = @"c:\dev\icons\material-design-icons\image\1x_web";
            var device = @"c:\dev\icons\material-design-icons\device\1x_web";
            var toggle = @"c:\dev\icons\material-design-icons\toggle\1x_web";
            var file = @"c:\dev\icons\material-design-icons\file\1x_web";
            var editor = @"c:\dev\icons\material-design-icons\editor\1x_web";
            var content = @"c:\dev\icons\material-design-icons\content\1x_web";
            var arrows = @"c:\dev\icons\arrows-set-2\png";
            var social = @"c:\dev\icons\simpleicon-social-media\png";
            var communication = @"c:\dev\icons\material-design-icons\communication\1x_web";


            var images = new TupleList<string, string, string>
                             {
                                { "add", content, "ic_add_black" },
                                { "audio", image, "ic_audiotrack_black" },
                                { "attachment", file, "ic_attachment_black" },
                                { "back", navigation, "ic_arrow_back_black" },
                                { "back_image", av, "ic_skip_previous_black" },
                                { "back_folder", navigation, "ic_chevron_left_black" },
                                { "batch", image, "ic_collections_black" },
                                { "camera", image, "ic_photo_camera_black" },
                                { "cancel", navigation, "ic_close_black" },
                                { "check", navigation, "ic_check_black" },
                                { "color", image, "ic_color_lens_black" },
                                { "convert", image, "ic_transform_black" },
                                { "crop", image, "ic_crop_black" },
                                { "delete", action, "ic_delete_black" },
                                { "disk", av, "ic_album_black" },
                                { "display", av, "ic_add_to_queue_black" },
                                { "document", editor, "ic_insert_drive_file_black" },
                                { "edit", image, "ic_edit_black" },
                                { "facebook", social, "facebook29" },
                                { "flag", image, "ic_assistant_photo_black" },
                                { "flickr", social, "flickr9" },
                                { "folders", file, "ic_folder_open_black" },
                                { "forward30", av, "ic_forward_30_black" },
                                { "fullscreen", navigation, "ic_fullscreen_black" },
                                { "fullscreen_exit", navigation, "ic_fullscreen_exit_black" },
                                { "import", file, "ic_file_download_black" },
                                { "items", navigation, "ic_apps_black" },
                                { "link", editor, "ic_insert_link_black" },
                                { "location", action, "ic_room_black" },
                                { "mail", communication, "ic_email_black" },
                                { "navigation", navigation, "ic_menu_black" },
                                { "next", navigation, "ic_arrow_forward_black" },
                                { "next_image", av, "ic_skip_next_black" },
                                { "next_folder", navigation, "ic_chevron_right_black" },
                                { "open", action, "ic_launch_black" },
                                { "more", navigation, "ic_more_horiz_black" },
                                { "more_vert", navigation, "ic_more_vert_black" },
                                { "settings", action, "ic_settings_black" },
                                { "parent", navigation, "ic_arrow_upward_black" },
                                { "pause", av, "ic_pause_black" },
                                { "copyright", action, "ic_copyright_black" },
                                { "photo", image, "ic_panorama_black" },
                                { "play", av, "ic_play_arrow_black" },
                                { "print", action, "ic_print_black" },
                                { "repeat", av, "ic_repeat_black" },
                                { "reset", navigation, "ic_refresh_black" },
                                { "resize", image, "ic_photo_size_select_large_black" },
                                { "rotate_left", image, "ic_rotate_left_black" },
                                { "rotate_right", image, "ic_rotate_right_black" },
                                { "save", content, "ic_save_black" },
                                { "scale_up", action, "ic_aspect_ratio_black" },
                                { "search", action, "ic_search_black" },
                                { "shuffle", av, "ic_shuffle_black" },
                                { "slideshow", image, "ic_slideshow_black" },
                                { "sort", action, "ic_swap_vert_black" },
                                { "sound0", av, "ic_volume_mute_black" },
                                { "sound1", av, "ic_volume_down_black" },
                                { "sound2", av, "ic_volume_up_black" },
                                { "stats", action, "ic_timeline_black" },
                                { "star", toggle, "ic_star_border_black" },
                                { "star_solid", toggle, "ic_star_black" },
                                { "stop", av, "ic_stop_black" },
                                { "tag", action, "ic_label_outline_black" },
                                { "text", editor, "ic_short_text_black" },
                                { "time", action, "ic_schedule_black" },
                                { "twitter", social, "twitter21" },
                                { "video", av, "ic_movie_black" },
                                { "zoom_in", action, "ic_zoom_in_black" },
                                { "zoom_out", action, "ic_zoom_out_black" },
                                { "wallpaper", device, "ic_wallpaper_black" },
                                { "lightbulb", action, "ic_lightbulb_outline_black" },
                                { "overview", action, "ic_dashboard_black" },
                                { "fingerprint", action, "ic_fingerprint_black" },
                                { "favorite", action, "ic_favorite_black" },
                                { "compare", image, "ic_compare_black" },
                             };

            var tb32 = new Bitmap(32 * images.Count, 32, PixelFormat.Format32bppArgb);
            var tb16 = new Bitmap(16 * images.Count, 16, PixelFormat.Format32bppArgb);

            int x = 0;

            using (Graphics g32 = Graphics.FromImage(tb32))
            using (Graphics g16 = Graphics.FromImage(tb16))
            using (new HighQualityRendering(g32))
            using (new HighQualityRendering(g16))
            {
                //g16.Clear(Color.DarkGray);

                foreach (var i in images)
                {
                    string path16;
                    string path32;

                    var not_material = i.Item2 == arrows || i.Item2 == social;

                    if (not_material)
                    {
                        path16 = Path.Combine(i.Item2, i.Item3 + ".png");
                        path32 = Path.Combine(i.Item2, i.Item3 + ".png");
                    }
                    else
                    {
                        path16 = Path.Combine(i.Item2, i.Item3 + "_18dp.png");
                        path32 = Path.Combine(i.Item2, i.Item3 + "_36dp.png");
                    }

                    var b16 = new Bitmap(path16);
                    var b32 = new Bitmap(path32);

                    //bool smaller = i.Item1 == "NextImage" || i.Item1 == "NextFolder" || i.Item1 == "BackImage" || i.Item1 == "BackFolder";

                    if (not_material)
                    {
                        g32.DrawImage(b32, x * 32, 0, 32, 32);
                        g16.DrawImage(b16, x * 16, 0, 16, 16);
                    }
                    else
                    {
                        g32.DrawImage(b32, new Rectangle(x * 32, 0, 32, 32), new Rectangle(2, 2, 32, 32), GraphicsUnit.Pixel);
                        g16.DrawImage(b16, new Rectangle(x * 16, 0, 16, 16), new Rectangle(1, 1, 16, 16), GraphicsUnit.Pixel);
                    }

                    System.Console.WriteLine(i.Item1 + ",");

                    x += 1;
                }
            }

            Directory.CreateDirectory("out");


            tb32.Save(@"out\tb_large.bmp", ImageFormat.Bmp);
            tb16.Save(@"out\tb_small.bmp", ImageFormat.Bmp);
        }

        //public class TupleList<T1, T2> : List<System.Tuple<T1, T2>>
        //{
        //    public void Add(T1 item, T2 item2)
        //    {
        //        Add(new System.Tuple<T1, T2>(item, item2));
        //    }
        //}

        //private static void Main(string[] args)
        //{
        //    // http://www.flaticon.com/packs/photography-2

        //    var images = new TupleList<string, string>
        //                     {
        //                        { "Add", "Add" },
        //                        { "Audio", "Music" },
        //                        { "Back", "Arrow left" },
        //                        { "BackImage", "Previous" },
        //                        { "BackFolder", "Previous media" },
        //                        { "Bars", "List" },
        //                        { "Batch", "Documents" },                                
        //                        { "Camera", "Pictures" },
        //                        { "Cancel", "Delete" },
        //                        { "Check", "Check" },
        //                        { "Color", "Contrast" },
        //                        { "Convert", "Conversation" },
        //                        { "Crop", "Screensharing" },
        //                        { "Disk", "Disc" },
        //                        { "Display", "Display" },
        //                        { "Document", "Document" },
        //                        { "DontScaleUp", "Exit fullscreen" },
        //                        { "Edit", "Edit" },
        //                        { "Facebook", "Share" },
        //                        { "Flag", "Flag" },
        //                        { "Flickr", "Share" },
        //                        { "Folders", "Folder" },
        //                        { "Globe", "Globe" },
        //                        { "Import", "Inbox" },
        //                        { "Info", "Info" },
        //                        { "Items", "Thumbnails small" },
        //                        { "Link", "Link" },
        //                        { "Location", "Pin location" },
        //                        { "Mail", "Mail" },
        //                        { "New", "Factory" },
        //                        { "Next", "Arrow right" },
        //                        { "NextImage", "Next" },
        //                        { "NextFolder", "Next media" },
        //                        { "Options", "Settings" },
        //                        { "Overview", "Dashboard" },
        //                        { "Parent", "Arrow up" },
        //                        { "Pause", "Pause" },                                
        //                        { "Person", "People" },
        //                        { "Photo", "Picture" },
        //                        { "Play", "Play" },
        //                        { "Print", "Print" },
        //                        { "Repeat", "Repeat" },
        //                        { "Reset", "Reload" },
        //                        { "Resize", "Coverflow" },
        //                        { "RotateLeft", "Z axis rotation" },
        //                        { "RotateRight", "Z axis rotation" },
        //                        { "Save", "Check" },
        //                        { "ScaleUp", "Fullscreen" },
        //                        { "Search", "Search" },
        //                        { "Shuffle", "Shuffle" },
        //                        { "Sort", "Navigation vertical" },
        //                        { "Sound0", "Sound off" },
        //                        { "Sound1", "Sound low" },
        //                        { "Sound2", "Sound medium" },
        //                        { "Sound3", "Sound" },
        //                        { "Stats", "Statistic bar" },
        //                        { "Star", "Rate" },
        //                        { "Stop", "Stop" },
        //                        { "Tag", "Tag" },
        //                        { "Tags", "Tags" },
        //                        { "Text", "Checkbox dotted" },
        //                        { "Time", "Time" },
        //                        { "Twitter", "Share" },
        //                        { "Video", "Movie" },
        //                        { "ZoomIn", "Zoom in" },
        //                        { "ZoomOut", "Zoom out" },
        //                     };

        //    var tb32 = new Bitmap(32 * images.Count, 32, PixelFormat.Format32bppArgb);
        //    var tb16 = new Bitmap(16 * images.Count, 16, PixelFormat.Format32bppArgb);

        //    int x = 0;

        //    using (Graphics g32 = Graphics.FromImage(tb32))
        //    using (Graphics g16 = Graphics.FromImage(tb16))
        //    using (new HighQualityRendering(g32))
        //    using (new HighQualityRendering(g16))
        //    {
        //        //g16.Clear(Color.DarkGray);

        //        foreach (var i in images)
        //        {
        //            var b32 = new Bitmap(Path.Combine(@"c:\dev\icons\helveticons\Png\32x32", i.Item2 + " 32x32.png"));
        //            var b16 = new Bitmap(Path.Combine(@"c:\dev\icons\helveticons\Png\16x16", i.Item2 + " 16x16.png"));

        //            if (i.Item1 == "RotateRight")
        //            {
        //                b32.RotateFlip(RotateFlipType.Rotate160FlipX);
        //            }

        //            g32.DrawImage(b32, x * 32, 0, 32, 32);
        //            g16.DrawImage(b16, x * 16, 0, 16, 16);

        //            System.Console.WriteLine(i.Item1 + ",");

        //            x += 1;
        //        }
        //    }

        //    Directory.CreateDirectory("out");


        //    tb32.Save(@"out\tb_large.bmp", ImageFormat.Bmp);
        //    tb16.Save(@"out\tb_small.bmp", ImageFormat.Bmp);
        //}
    }
}