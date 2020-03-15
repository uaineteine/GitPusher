using System;
using System.Collections.Generic;
using System.IO;

namespace GitPusher
{
    public static class git
    {
        public static string g(string arg)
        {
            return "git " + arg;
        }
        private static string filterOriginName(string inp)
        {
            int index = inp.IndexOf("\t");
            if (index > 0)
                return inp.Remove(index, inp.Length - index);
            else
                return inp;
        }
        public static string[] getRemoteNames(out bool success)
        {
            string[] cmds = new string[1];
            cmds[0] = g("remote -v");
            string[] feedback = CMD.CMDcmdsLines(cmds, false);
            success = true;
            for (int i = 0; i < feedback.Length; i++)
            {
                if (feedback[i].Contains("fatal:"))
                    success = false;
            }
            if (success)
            {
                //worked now filter out to find those branch names ye ken
                string[] filtered = new string[feedback.Length - 1];
                for (int i = 0; i < filtered.Length; i++)
                {
                    int k = i + 1;
                    filtered[i] = filterOriginName(feedback[k]);
                }
                return FindDouble(filtered);
            }
            else
            {
                string err = "ERROR GIT MIGHT NOT BE INITIALISED";
                UI.giveWarning(err);
                return new string[] { err };
            }
        }

        private static string[] FindDouble(string[] filtered)
        {
            List<string> output = new List<string>();
            string last = "";
            for (int i = 0; i < filtered.Length; i++)
            {
                string cur = filtered[i];
                if (cur == last)
                    if (cur.Length > 0)
                        output.Add(cur);
                last = cur;
            }
            return output.ToArray();
        }

        public static string getBranchFeedback(out bool success)
        {
            string[] cmds = new string[1];
            cmds[0] = g("branch --show-current");
            string[] feedback = CMD.CMDcmdsLines(cmds, false);
            success = true;
            for (int i = 0; i < feedback.Length; i++)
            {
                if (feedback[i].Contains("fatal:"))
                    success = false;
            }
            if (success)
                return feedback[1];
            else
            {
                string err = "ERROR GIT MIGHT NOT BE INITIALISED";
                UI.giveWarning(err);
                return err;
            }
        }
        static void Initalise()
        {
            string[] commands = new string[1];
            commands[0] = git.g("init");
            string[] lineoutput = CMD.CMDcmdsLines(commands, true);
        }

        public static bool IsInit()
        {
            bool output = Directory.Exists(".git");
            if (!output)
            {
                Console.WriteLine("Initalise? (y)");
                string resp = Console.ReadLine();
                if (resp == "y")
                    Initalise();
            }
            return output;
        }
        public static void PushAll(string msg)
        {
            GitIgnore.GitIgnorecheck();

            string[] strCmds = new string[3];
            strCmds[0] = git.g("add--all");
            strCmds[1] = git.g("commit -m \"" + msg + "\"");
            for (int i = 0; i < Settings.curRemote.Count; i++)
            {
                int k = 2 + i;
                strCmds[k] = git.g("push " + Settings.curRemote[i] + " " + Settings.curBranchN);
            }
            //execute
            string[] lineoutput = CMD.CMDcmdsLines(strCmds, true);
        }

        public static void PromptMerge()
        {
            Console.WriteLine("Give branch to merge to:");
            string mergeto = Console.ReadLine();
            Console.WriteLine("choose option:");
            UI.green();
            Console.WriteLine("(0) regular merge");
            Console.WriteLine("(1) squash merge");
            UI.white();
            string select = Console.ReadLine();
            bool mergeT = false;//merge type-regular
                                //using switch to allow conversion to int
            switch (Convert.ToInt32(select))
            {
                case 1:
                    mergeT = true;
                    break;

                default:
                    mergeT = false;
                    break;
            }
            Merge(mergeto, mergeT);
        }

        private static void Merge(string branch, bool squash)
        {
            string[] cmds = new string[2];
            cmds[0] = git.g("checkout " + branch);//checkout this one
            if (squash == false)
                cmds[1] = git.g("merge " + Settings.curBranchN);//merge this one in, regular fast forward
            else
                cmds[1] = git.g("merge --squash" + Settings.curBranchN);
            string[] lineoutput = CMD.CMDcmdsLines(cmds, true);
        }
    }
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
                    for (int i = 0; i < rules.Length; i++)//save this
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
