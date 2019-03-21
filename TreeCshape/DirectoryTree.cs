using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TreeCshape
{

    public class DirectoryTree
    {
        DirectoryInfo _dir;
        public int Deep { get; set; } = -1;
        public int Verbose { get; set; } = 2;
        public bool FullPathFind { get; set; } = true;
        public bool IgnoreFile { get; set; } = false;

        public DirectoryTree(string path) => _dir = new DirectoryInfo(path);
        public override string ToString()
        {
           return  DirToString(_dir);
        }

        public void Print()
        {
            PrintDir(_dir);
        }

        public List<string> FindToList(string sub_str)
        {
            return FindDirsToList(sub_str, _dir);
        }


        #region PrintTree

        void PrintItem(string name, ref string parent, bool end)
        {
            Console.Write(parent);
            if (end)
            {
                if (parent.Length > 0)
                {
                    Console.Write(@"└───");
                    parent += @"    ";
                }
                else
                {
                    parent += @" ";
                }
            }
            else
            {
                Console.Write(@"├───");
                parent += @"│   ";
            }
            Console.WriteLine(name);
        }

        void PrintDir(DirectoryInfo dir, string parent = "", bool end = true, int deep = 0)
        {
            bool type_suffix = Verbose >= 1;
            PrintItem(dir.Name + "  " + (type_suffix ? "[DIR]" : ""), ref parent, end);

            if (Deep != -1)
            {
                if (deep >= Deep)
                    return;
            }

            var dirs = dir.GetDirectories();

            if (!IgnoreFile)
            {
                var files = dir.GetFiles();

                for (int i = 0; i < files.Length; i++)
                {
                    bool is_end = ((i == files.Length - 1) && (dirs.Length == 0));
                    PrintFile(files[i], parent, is_end);
                }
            }

            for (int i = 0; i < dirs.Length; i++)
            {
                bool is_end = (i == dirs.Length - 1);
                PrintDir(dirs[i], parent, is_end, deep + 1);
            }

        }

        void PrintFile(FileInfo file, string parent, bool end)
        {

            bool type_suffix = Verbose >= 1;
            bool byte_suffix = Verbose >= 2;

            PrintItem(file.Name + "  "
                + (type_suffix ? "[FILE] " : "")
                + (byte_suffix ? "(" + file.Length + " b)" : ""),
                ref parent, end);
        }

        #endregion

        #region ToStringTree
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
                    return out_str;
            }

            var dirs = dir.GetDirectories();


            if (!IgnoreFile)
            {
                var files = dir.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    bool is_end = ((i == files.Length - 1) && (dirs.Length == 0));
                    out_str += FileToString(files[i], parent, is_end);
                }
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
        #endregion

        #region FindTree

        List<string> FindDirsToList(string sub_str, DirectoryInfo dir, int deep = 0)
        {
            var list = new List<string>();
            var dirs = dir.GetDirectories();
            var files = dir.GetFiles();

            if (Find(sub_str, dir.Name))
            {
                if (FullPathFind)
                    list.Add(dir.FullName);
                else
                    list.Add(dir.Name);
            }

            if (Deep != -1 && deep >= Deep)
            {
                return list;
            }

            foreach(var f in files)
            {
                FindFileToList(sub_str, f, ref list);
            }

            foreach(var d in dirs)
            {
                list.AddRange(FindDirsToList(sub_str, d, deep + 1));
            }

            return list;
        }

        void FindFileToList(string sub_str, FileInfo file, ref List<string> list)
        {
            if (Find(sub_str, file.Name))
            {
                if (FullPathFind)
                    list.Add(file.FullName);
                else
                    list.Add(file.Name);
            }
        }

        bool Find(string sub_str, string target)
        {
            var regex = new Regex(sub_str);
            if (regex.IsMatch(target))
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
