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

            [Value(0, MetaName = "<path>", Default = ".", HelpText = "Путь до директории")]
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
                           Verbose = o.Verbose
                       };

                       Console.WriteLine(tree.ToString());
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
