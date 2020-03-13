using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitPusher
{
    public static class Settings
    {
        public static string configFN = "gitpusher/config.ini";
        public static string curBranchN = "master";
        public static int numRemotes = 1;
        public static List<string> curRemote = new List<string>();
        static string curbranchpre = "curbranch";
        static string curremotepre = "remote";
        static string versionpre = "v";

        static Settings()
        {
            curRemote.Add("origin");
        }

        public static bool Read()
        {
            Console.WriteLine("Reading your config file");
            //load up my file here
            FileIniDataParser configFile = new FileIniDataParser();
            IniData configDat = configFile.ReadFile(Settings.configFN);
            //now what to do?
            bool worked = Settings.ReadDatFile(configDat);
            if (worked)
                PrintLoadSuccess();
            return worked;
        }
        public static bool ReadDatFile(IniData myDat)//read success bool returned
        {
            bool success = false;

            bool readbranch = true;
            try
            {
                curBranchN = myDat["GitPusherSettings"][curbranchpre];
            }
            catch
            {
                readbranch = false;
            }
            if (readbranch)
                success = true;
            else
                return false;
            bool readremote = true; 
            try
            {
                //read remote
                string remotearray = myDat["GitPusherSettings"][curremotepre];
                string[] rarr = remotearray.Split(',');
                curRemote.Clear();
                //check to make sure there are no spaces on any of those
                for (int i = 0; i < rarr.Length; i++)
                {
                    if (rarr[i].Contains(" "))
                        rarr[i] = RemoveSpace(rarr[i]);
                    curRemote.Add(rarr[i]);
                    numRemotes = curRemote.Count;
                }
                //now that we have no 'white spaces'
            }
            catch
            {
                readremote = false;
            }
            if (readremote)
                success = true;
            else
                return false;
            bool readversion = true;
            try
            {
                string versionfile = myDat["GitPusherSettings"][versionpre];
            }
            catch
            {
                readversion = false;
            }
            if (readversion)
                success = true;
            else
                return false;
            //got this far exit with a yay
            return success;
        }

        private static string RemoveSpace(string inp)
        {
            return inp.Replace(" ", "");
        }

        private static string getRemotesString(bool withspaces)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (string item in curRemote)
            {
                sb.Append(item);
                if (i < curRemote.Count - 1)
                    if (withspaces)
                        sb.Append(", ");
                    else
                        sb.Append(",");
                i += 1;
            }
            return sb.ToString();
        }

        public static void PrintLoadSuccess()
        {
            UI.white();
            Console.WriteLine("Load Sucess:");
            UI.cyan();
            Console.WriteLine(" Current Branch:  " + curBranchN);
            Console.Write(" Current Remotes: ");
            Console.Write(getRemotesString(true));
            Console.WriteLine();
            UI.white();
        }

        public static void PromptAddRemote()
        {
            Console.WriteLine("new remote name?");
            AddRemote(Console.ReadLine());
        }

        public static void ChangeCurBranch(string newBranch)
        {
            curBranchN = newBranch;
            SaveConfig();
            PrintLoadSuccess();
        }
        public static void ChangeRemote(int i, string newRemote)
        {
            curRemote[i] = newRemote;
            SaveConfig();
            PrintLoadSuccess();
        }
        public static void SaveConfig()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(configFN);
            //wipe file
            
            //save file
            data["GitPusherSettings"][curbranchpre] = curBranchN;
            data["GitPusherSettings"][curremotepre] = getRemotesString(false);
            data["GitPusherSettings"][versionpre] = VersionController.WriteVersionNoOnly();
            parser.WriteFile(configFN, data);
        }
        public static void PromptChangeBranch()
        {
            Console.WriteLine("Current branch name?");
            ChangeCurBranch(Console.ReadLine());
        }
        public static void PromptChangeRemote()
        {
            int i = getRemoteIndex("Select which remote to change");
            Console.WriteLine("new remote name?");
            ChangeRemote(i, Console.ReadLine());
        }
        public static void CheckoutNewBranch(string branchname)
        {
            curBranchN = branchname;
            string[] cmnds = new string[1];
            cmnds[0] = CMD.g("-b " + branchname);
            CMD.CMDWithCommands(cmnds);
            SaveConfig();
        }
        public static void PromptCheckoutNB()
        {
            Console.WriteLine("new branch name?");
            string resp = Console.ReadLine();
            //make sure it isn't the same branch name naturally
            if (resp != curBranchN)
            {
                if (!resp.Contains(" "))//no spaces?
                    CheckoutNewBranch(resp);
            }
        }

        public static void AddRemote(string remoteN)
        {
            curRemote.Add(remoteN);
            numRemotes += 1;
            SaveConfig();
        }

        private static void removeRemIndex(int i)
        {
            curRemote.RemoveAt(i);
            numRemotes -= 1;
        }

        private static void printRemoteIndexes(string msg)
        {
            Console.WriteLine(msg);
            UI.green();
            for (int i = 0; i < numRemotes; i++)
            {
                Console.WriteLine(curRemote[i] + "(" + i.ToString() + ")");
            }
            UI.white();
            Console.WriteLine();
        }

        public static int getRemoteIndex(string msg)
        {
            printRemoteIndexes(msg);
            string resp = Console.ReadLine();
            return Convert.ToInt32(resp);
        }

        public static void RemoveRemote()
        {
            int gind = getRemoteIndex("select which remote to remove by index");
            removeRemIndex(gind);
            SaveConfig();
        }
    }
}
