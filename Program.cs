using System;
using System.IO;
using System.Reflection;

namespace GitPusher
{
    class Program
    {
        static void Initalise()
        {
            string[] commands = new string[1];
            commands[0] = CMD.g("init");
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
            strCmds[0] = CMD.g("add--all");
            strCmds[1] = CMD.g("commit -m \"" + msg + "\"");
            for (int i = 0; i < Settings.curRemote.Count; i++)
            {
                int k = 2 + i;
                strCmds[k] = CMD.g("push " + Settings.curRemote[i] + " " + Settings.curBranchN);
            }
            //execute
            CMD.CMDWithCommands(strCmds);
        }
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
                    //now check if there is an init folder
                    IsInit();
                    //now ask about pushing
                    Console.WriteLine("give commit message for auto push or enter a setting");
                    Console.WriteLine("Settings are:");
                    UI.green();
                    Console.WriteLine("(b)    change current branch to commit on");
                    Console.WriteLine("(cb)   create/checkout new branch from current commit and push to this");
                    Console.WriteLine("(r)    change remote to commit on");
                    Console.WriteLine("(ar)   add remote to commit on");
                    Console.WriteLine("(rr)   remove remote to commit on");
                    Console.WriteLine("(m)    merge current branch to another");
                    Console.WriteLine("(cmd)  pure cmd input");
                    Console.WriteLine("(l)    see license info");
                    Console.WriteLine("(help) help!");
                    Console.WriteLine("(q)   quit program");
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
                    else if (resp == "ar")
                    {
                        Settings.PromptAddRemote();
                    }
                    else if (resp == "rr")
                    {
                        Settings.RemoveRemote();
                    }
                    else if (resp == "cb")
                    {
                        Settings.PromptCheckoutNB();
                    }
                    else if (resp == "m")
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
                        switch(Convert.ToInt32(select))
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
                    else if (resp == "cmd")
                    {
                        Console.WriteLine("Give command for cmd to run");
                        string[] cmds = new string[1];
                        cmds[0] = Console.ReadLine();
                        CMD.CMDWithCommands(cmds);
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
                        PushAll(resp);
                    }
                }
                Console.WriteLine();
            }
            //all done either reloop or quit
        }

        private static void Merge(string branch, bool squash)
        {
            string[] cmds = new string[2];
            cmds[0] = CMD.g("checkout " + branch);//checkout this one
            if (squash == false)
                cmds[1] = CMD.g("merge " + Settings.curBranchN);//merge this one in, regular fast forward
            else
                cmds[1] = CMD.g("merge --squash" + Settings.curBranchN);
            CMD.CMDWithCommands(cmds);
        }

        private static void DisplayHelp()
        {
            UI.displayFilesScreen("help.txt");
        }
    }
}