using System;
using System.IO;
using System.Text;


namespace TreeCshape
{
    public class DirectoryTree
    {
        DirectoryInfo _dir;
        public int Deep { get; set; } = -1;
        public int Verbose { get; set; } = 2;
        public DirectoryTree(string path) => _dir = new DirectoryInfo(path);
        public override string ToString()
        {
           return  DirToString(_dir);
        }


        string ItemToString(string name, ref string parent, bool end)
        {

            string out_str = "";
            out_str += parent;
            if (end)
            {
                if (parent.Length > 0)
                {
                    out_str += @"└───";
                    parent += @"    ";
                }
                else
                {
                    parent += @" ";
                }
            }
            else
            {
                out_str += @"├───";
                parent += @"│   ";
            }
            out_str += name + "\n";
            return out_str;
        }

        string DirToString(DirectoryInfo dir, string parent = "", bool end = true, int deep = 0)
        {
            string out_str = "";
            bool type_suffix = Verbose >= 1;
            out_str += ItemToString(dir.Name + "  " + (type_suffix ? "[DIR]" : ""), ref parent, end);

            if (Deep != -1)
            {
                if (deep >= Deep)
                    return "";
            }

            var files = dir.GetFiles();
            var dirs = dir.GetDirectories();

            for (int i = 0; i < files.Length; i++)
            {
                bool is_end = ((i == files.Length - 1) && (dirs.Length == 0));
                out_str += FileToString(files[i], parent, is_end);
            }

            for (int i = 0; i < dirs.Length; i++)
            {
                bool is_end = (i == dirs.Length - 1);
                out_str += DirToString(dirs[i], parent, is_end, deep + 1);
            }

            return out_str;
        }

        string FileToString(FileInfo file, string parent, bool end)
        {

            bool type_suffix = Verbose >= 1;
            bool byte_suffix = Verbose >= 2;

            return ItemToString(file.Name + "  " 
                + (type_suffix ? "[FILE] " : "") 
                + (byte_suffix ? "(" + file.Length + " b)" : ""), 
                ref parent, end);
        }
    }
}
