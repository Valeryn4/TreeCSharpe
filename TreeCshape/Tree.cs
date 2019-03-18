using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TreeCshape
{

    /// <summary>
    /// Структура-дерево
    /// хранит в себе имя и мета данные файла или директории, а так же список дочерних айтемов.
    /// 
    /// </summary>
    struct Item
    {
        public string name;
        public string meta;
        public List<Item> children;
        public enum Type {
            Dir,
            File
        };
        public Type type;
    }

    /// <summary>
    /// Дерево-файловой системы.
    /// 
    /// Хранит в себе корень в виде Item. 
    /// При инициализации парсит переданный в него путь в дерево Item'ов
    /// </summary>
    public class Tree
    {
        Item _tree;
        public Tree() => Parse(@".\");
        public Tree(string dir_path) => Parse(dir_path);

        #region Parse_Dir_To_Tree
        /// <summary>
        /// функция парсинга.
        /// Конструктор вызывает ее по умолчанию.
        /// Деленгирует выполнение своей перегруженной версии
        /// </summary>
        /// <param name="dir_path"></param>
        public void Parse(string dir_path)
        {
            var dir = new DirectoryInfo(dir_path);
            Parse(dir);
        }

        /// <summary>
        /// Тоже самое что и Parse(string).
        /// </summary>
        /// <param name="dir"></param>
        void Parse(DirectoryInfo dir)
        {
            if (!dir.Exists)
                throw new DirectoryNotFoundException();

            ParseDir(dir, ref _tree);
        }

        /// <summary>
        /// Парсинг директорий.
        /// Рекурсивно обходит все вложенные директории и файлы. 
        /// заполняет структуру item данными, полученными при парсинге
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item">ссылка на item данной диреткории.</param>
        static void ParseDir(DirectoryInfo dir, ref Item item)
        {
            item.name = dir.Name;
            item.type = Item.Type.Dir;

            var childrens = new List<Item>();
            var files = dir.GetFiles();
            var dirs = dir.GetDirectories();
            
            foreach (var f in files)
            {
                var file_item = new Item();
                ParseFile(f, ref file_item);
                childrens.Add(file_item);
            }

            foreach (var d in dirs)
            {
                var dir_item = new Item();
                ParseDir(d, ref dir_item);
                childrens.Add(dir_item);
            }

            item.children = childrens;
        }

        /// <summary>
        /// Парсинг файла. Почти ничем не отличается от парсинга директорий, кроме отсутсвия дочерних айтемов
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="item"></param>
        static void ParseFile(FileInfo obj, ref Item item)
        {
            item.name = obj.Name;
            item.type = Item.Type.File;
            item.meta = obj.Length.ToString();
        }

        #endregion
        /// <summary>
        /// сереализует дерево в вывод string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Dump(_tree, 0);
        }


        #region Dump_To_String_From_Tree
        /// <summary>
        /// Рекурсивная функция, преобразует дерево в строку.
        /// </summary>
        /// <param name="item"> текущий айтем</param>
        /// <param name="deep"> глубина дерева</param>
        /// <param name="end_root"> вспомогательный флаг, нужен только для корня, когда определяется последняя директория в корне
        /// нужен просто, что бы графика была красивая.
        /// </param>
        /// <returns></returns>
        static string Dump(Item item, int deep, bool end_root = false)
        {
            string res = "";

            if (item.type == Item.Type.File)
            {
                res += FileLineFromDeep(item, deep, end_root);
            }
            else if (item.children != null)
            {

                res += DirLineFromDeep(item, deep, end_root);
                
                res += EmptyLineFromDeep(deep, end_root);

                var item_count = item.children.Count;
                for (int i = 0; i < item_count; ++i)
                {
                    var child = item.children[i];
                    if (deep == 0 && i == item_count - 1)
                    {
                        res += Dump(child, deep + 1, true);
                    }
                    else
                    {
                        res += Dump(child, deep + 1, end_root);
                    }
                    if (i < item_count - 1)
                    {
                        res += EmptyLineFromDeep(deep, end_root);
                    }
                }
            }

            return res;
        }
        #endregion
        //Вспомогательные функции для отрисовки дерева в виде строки.
        #region HelperFunc
        static string EmptyLineFromDeep(int deep, bool end_root = false)
        {
            string res = " ";

            for (int i = 0; i < deep; ++i)
            {
                if (i == 0 && !end_root)
                    res += "| ";
                else
                    res += "  ";
            }
            res += "| \n";
            return res;
        }

        static string TabLineFromDeep(int deep, bool end_root = false)
        {
            string res = " ";
            if (deep == 0)
                return "";
            for (int i = 0; i < deep; ++i)
            {
                if (i == deep - 1)
                {
                    res += "+-";
                }
                else
                {
                    if (i == 0 && !end_root)
                        res += "| ";
                    else
                        res += "  ";
                }
            }

            return res;
        }

        static string FileLineFromDeep(Item item, int deep, bool end_root = false)
        {
            return TabLineFromDeep(deep, end_root) + item.name + " [" + item.meta + "] byte\n";
        }
        
        static string DirLineFromDeep(Item item, int deep, bool end_root = false)
        {
            return TabLineFromDeep(deep, end_root) + item.name + "\n";
        }

        #endregion
    }
}
