using System;
using System.IO;
using System.Reflection;

namespace GitPusher
{
    class Program
    {
        static void ExitPrint()
        {
            Console.WriteLine();//added a new blank space for neatness
            Console.WriteLine("El fino");
        }

        static void Main(string[] args)
        {
            //version info print
            UI.yellow();
            VersionController.WriteVersion();
            UI.white();

            //now check if there is an init folder
            git.IsInit();

            while (true)
            {
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
                    Console.WriteLine();
                    //now ask about pushing
                    Console.WriteLine("give commit message for auto push or enter a setting");
                    Console.WriteLine("Settings are:");
                    UI.green();
                    Console.WriteLine("(b)    create/checkout branch from current and be pushing to this");
                    Console.WriteLine("(r)    change remote to commit on");
                    Console.WriteLine("(ar)   add remote to commit on");
                    Console.WriteLine("(rr)   remove remote to commit on");
                    Console.WriteLine("(m)    merge current branch to another");
                    Console.WriteLine("(cmd)  pure cmd input");
                    Console.WriteLine("(l)    see license info");
                    Console.WriteLine("(help) help!");
                    Console.WriteLine("(q)    quit program");
                    UI.white();

                    string resp = Console.ReadLine();
                    if (resp == "r")
                    {
                        Settings.PromptChangeRemote();
                    }
                    else if (resp == "ar")
                    {
                        Settings.PromptAddRemote();
                    }
                    else if (resp == "rr")
                    {
                        Settings.RemoveRemote();
                    }
                    else if (resp == "b")
                    {
                        Settings.PromptCheckoutNB();
                    }
                    else if (resp == "m")
                    {
                        git.PromptMerge();
                    }
                    else if (resp == "cmd")
                    {
                        Console.WriteLine("Give command for cmd to run");
                        string[] cmds = new string[1];
                        cmds[0] = Console.ReadLine();
                        string[] lineoutput = CMD.CMDcmdsLines(cmds, true);
                    }
                    else if (resp == "l")
                    {
                        Licence.DisplayScreen();
                    }
                    else if (resp == "help")
                    {
                        DisplayHelp();
                    }
                    else if (resp == "q")
                    {
                        ExitPrint();
                        break;
                    }
                    else
                    {
                        git.PushAll(resp);
                    }
                }
                Console.WriteLine();
            }
            //all done either reloop or quit
        }

        private static void DisplayHelp()
        {
            UI.displayFilesScreen("help.txt");
        }
    }
}