using System;
using System.IO;

namespace TreeCshape
{
    class Program
    {
        /// <summary>
        /// Инициализирует Tree класс и выводит сразу же в консоль
        /// Если директория не найдена - выпускает DirectoryNotFoundException внутри Tree
        /// </summary>
        /// <param name="str">путь до директории, относительный или абсолютный</param>
        static void PrintTree(string str)
        {
            var tree = new Tree(str);

            var print_tree = tree.ToString();
            Console.WriteLine(print_tree);
            
        }

        /// <summary>
        /// Хелп-функция, выводит справку в консоль, если str является -h/--help/help
        /// иначе вернет false
        /// </summary>
        /// <param name="str">аргумент (обычно arg[1])</param>
        /// <returns>если аргумент был валидный - верне true</returns>
        static bool IsHelpAndPrint(string str)
        {
            if (str == "-h" || str == "--help" || str == "help")
            {
                var print_str = "";
                print_str += "[-h | --help | help] - help command\n";
                print_str += "[path_to_dir] - printing tree <path_to_dir> directory\n";
                print_str += "  example:\n";
                print_str += "   dotnet.exe .\\TreeCShape.dll C:\\Windows\n";
                print_str += "   Windows\n";
                print_str += "    |\n";
                print_str += "    +-System32\n";
                print_str += "    |\n";
                print_str += "   ...\n";

                Console.WriteLine(print_str);

                return true;
            }
            return false;
        }

#if DEBUG
        static void TestPrint()
        {
            PrintTree(@"..\..\..\");
            Console.WriteLine("\n\nPreesKey");
            Console.ReadKey();
        }
#endif

        static void Main(string[] args)
        {

#if DEBUG
            TestPrint();
#endif
            try
            {
                if (args.Length == 0)
                {
                    PrintTree(@".\");
                }
                else if (args.Length > 0)
                {
                    if (IsHelpAndPrint(args[0]))
                        return;
                    PrintTree(args[0]);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Дирректория не найдена! MSG: " + e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine(" |\n +-" + e.InnerException.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Неизвестное исключение! MSG: " + e.Message);
            }
        }
    }
}
