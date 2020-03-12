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
                curBranchN = myDat["GitSettings"][curbranchpre];
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
                curRemote = myDat["GitSettings"][curremotepre];
            }
            catch
            {
                readremote = false;
            }
            if (readremote)
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
            data["GitSettings"][curbranchpre] = curBranchN;
            data["GitSettings"][curremotepre] = curRemote;
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
    }
}
