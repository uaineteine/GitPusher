using IniParser;
using IniParser.Model;
using System;
using System.IO;

namespace GitPusher
{
    public static class Settings
    {
        public static string configFN = "gitpusher/config.ini";
        public static string curBranchN = "master";
        public static string curRemote = "origin";
        static string curbranchpre = "curbranch";
        static string curremotepre = "curremote";
        static string versionpre = "v";

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
                curRemote = myDat["GitPusherSettings"][curremotepre];
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
        public static void PrintLoadSuccess()
        {
            UI.white();
            Console.WriteLine("Load Sucess:");
            UI.cyan();
            Console.WriteLine(" Current Branch: " + curBranchN);
            Console.WriteLine(" Current Remote: " + curRemote);
            UI.white();
        }
        public static void ChangeCurBranch(string newBranch)
        {
            curBranchN = newBranch;
            SaveConfig();
            PrintLoadSuccess();
        }
        public static void ChangeCurRemote(string newRemote)
        {
            curRemote = newRemote;
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
            data["GitPusherSettings"][curremotepre] = curRemote;
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
            Console.WriteLine("Current remote name?");
            ChangeCurRemote(Console.ReadLine());
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
    }
}
