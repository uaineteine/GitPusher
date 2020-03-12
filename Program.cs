using System;
using System.IO;

namespace GitPusher
{
    class Program
    {
        static void Initalise()
        {
            string[] commands = new string[1];
            commands[0] = "git init";
            CMD.CMDWithCommands(commands);
        }

        static bool IsInit()
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
        static void PushAll(string msg)
        {
            GitIgnore.GitIgnorecheck();

            string[] strCmds = new string[3];
            strCmds[0] = "git add --all";
            strCmds[1] = "git commit -m \"" + msg + "\"";
            strCmds[2] = "git push " + Settings.curRemote;
            strCmds[2] += " " + Settings.curBranchN;
            //execute
            CMD.CMDWithCommands(strCmds);
        }
        static void ExitPrint()
        {
            Console.WriteLine("El fino");
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        static void Main(string[] args)
        {
            //version info print
            UI.yellow();
            VersionController.WriteVersion();
            UI.white();

            bool workedReading = Settings.Read();
            //Settings.ChangeCurBranch("thisbranch");//can change branch like this
            if (!workedReading)
            {
                UI.red();
                Console.WriteLine("Couldn't read config file in:");
                Console.WriteLine(" " + Settings.configFN);
                Console.WriteLine("aborting...");
                UI.white();
                //give offer to return default file
            }
            else
            {
                //now check if there is an init folder
                IsInit();
                //now ask about pushing
                Console.WriteLine("give commit message for auto push or enter a setting");
                Console.WriteLine("Settings are:");
                UI.green();
                Console.WriteLine("(b) change current branch to commit on");
                Console.WriteLine("(r) change remote branch to commit on");
                UI.white();
                //Console.WriteLine("Wish to add all and push? (y/n)");-obsolete
                string resp = Console.ReadLine();
                if (resp == "b")
                {
                    Settings.PromptChangeBranch();
                }
                else if (resp == "r")
                {
                    Settings.PromptChangeRemote();
                }
                else
                {
                    PushAll(resp);
                }
            }
            //exit bit after that
            ExitPrint();
        }
    }
}
