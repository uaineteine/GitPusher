using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GitPusher
{
    public static class Licence
    {
        static string fn = "LICENSE";

        public static void DisplayScreen()
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
    }
}
