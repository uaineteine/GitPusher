using System;
using System.IO;
using System.Reflection;

namespace GitPusher
{
    public static class UI
    {
        #region colours
        public static void white()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void yellow()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        public static void cyan()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        public static void red()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void green()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        #endregion

        public static void blank(int n)
        {
            for (int i = 0; i < n; i++)
                Console.WriteLine();
        }

        public static void displayFilesScreen(string fn)
        {
            Console.Clear();
            var resourceName = "GitPusher." + fn;
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            UI.blank(2);
            Console.WriteLine("Enter to exit");
            Console.ReadLine();
            Console.Clear();
        }

        public static void giveWarning(string msg)
        {
            UI.yellow();
            Console.WriteLine("WARNING: " + msg);
            UI.white();
        }

        private static void printIndexes(string msg, string[] options)
        {
            Console.WriteLine(msg);
            UI.green();
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(options[i] + "(" + i.ToString() + ")");
            }
            UI.white();
            Console.WriteLine();
        }
        public static int getIndex(string msg, string[] options)
        {
            int o = 0;
            while (true)
            {
                printIndexes(msg, options);
                string resp = Console.ReadLine();
                try
                {
                    o = Convert.ToInt32(resp);
                    break;
                }
                catch
                {
                    //didn't work
                    Console.WriteLine("Please give intger without any spaces");
                }
            }
            return o;
        }
    }
}
