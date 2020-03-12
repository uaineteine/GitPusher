using System;
using System.Collections.Generic;
using System.IO;

namespace GitPusher
{
    public static class GitIgnore//this is the gitignore file settings and generator
    {
        public static string[] filetypes;
        public static string ignorepath = ".gitignore";

        public static void LoadGitIgnore()
        {
            filetypes = File.ReadAllLines(ignorepath);
        }
        public static void GenerateGitIgnore(string[] rules)
        {
            filetypes = rules;
            //create file
            using (FileStream fs = new FileStream(ignorepath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for(int i = 0; i < rules.Length; i++)//save this
                    {
                        sw.WriteLine(rules[i]);
                    }
                    sw.Close();
                }
                fs.Close();
            }
        }
        public static bool GitIgnorecheck()
        {
            bool output = File.Exists(ignorepath);
            if (!output)
            {
                UI.yellow();
                Console.WriteLine("Warning no gitignore found, create one? (y/n)");
                string resp = Console.ReadLine();
                UI.white();
                if (resp == "y")
                {
                    GenerateGitIgnore(GetIgnoreRules());
                    return true;
                }
                else
                    return false;
            }
            return true;
        }

        private static string[] GetIgnoreRules()
        {
            List<string> rules = new List<string>();
            Console.WriteLine("Want rules? (y/n)");
            string resp = Console.ReadLine();
            if (resp == "y")
            {
                Console.WriteLine("Give the rules per line to go into the text file");
                Console.WriteLine("to finish use 'end' without any quotation marks");
                //get them
                while (true)//loop
                {
                    string line = Console.ReadLine();
                    if (line == "end")
                        break;//move on
                    else
                        rules.Add(line);
                }
            }
            return rules.ToArray();
        }
         
        public static void PrintSettings()
        {
            UI.white();
            Console.WriteLine("Current ignore settings:");
            UI.cyan();
            for (int i = 0; i < filetypes.Length; i++)
            {
                Console.WriteLine(" " + filetypes[i]);
            }
            UI.white();
        }
    }
}
