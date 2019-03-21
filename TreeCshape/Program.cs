using System;
using System.IO;
using CommandLine;

namespace TreeCshape
{
    class Program
    {
       
        public class Options
        {
            [Option('v', "verbose",
              Default = 2,
              HelpText = "Вывод дополнительной информации. [0] - вывести только имена файлов и папок. [1] - файлы и папки подписаны типом. [2] - файлы и папки подписаны типом и размером ")]
            public int Verbose { get; set; }

            [Option('d', "deep",
              Default = -1,
              HelpText = "Максимальная глубина вывода дерева файловой системы. [-1] - неограниченно")]
            public int Deep { get; set; }

            [Option('f', "find",
              Default = "",
              HelpText = "Поиск подстроки с выводом пути. Работают регулярные выражения")]
            public string FindSubString { get; set; }

            [Option('i', "ignore-file",
              HelpText = "Игнорирование файлов при выводе")]
            public bool IgnoreFile { get; set; }

            //TODO надо сделать вывод по дереву

            [Value(0, MetaName = "<path>", Default = "../../..", HelpText = "Путь до директории")]
            public string Path { get; set; }

        }

        static void Main(string[] args)
        {

            try
            {
                Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       var tree = new DirectoryTree(o.Path)
                       {
                           Deep = o.Deep,
                           Verbose = o.Verbose,
                           IgnoreFile = o.IgnoreFile
                       };

                       if (o.FindSubString != "")
                       {
                           var list_find = tree.FindToList(o.FindSubString);
                           foreach(var str in list_find)
                           {
                               Console.WriteLine(str);
                           }

                           return;
                       }

                       tree.Print();
                       return;
                   });
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Дирректория не найдена! MSG: " + e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("   " + e.InnerException.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Неизвестное исключение! MSG: " + e.Message);
            }
        }
    }
}
