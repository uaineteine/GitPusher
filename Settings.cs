using System;
using System.Collections.Generic;
using System.Text;

namespace GitPusher
{
    public static class Settings
    {
        static IniParser parser;
        public static string configFN = "gitpusher/config.ini";
        static string mainsec = "GitPusherSettings";
        public static string curBranchN = "master";
        public static List<string> curRemote = new List<string>();
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
            parser = new IniParser(configFN);
            parser.Read();

            bool feedbackb = false;
            curBranchN = git.getBranchFeedback(out feedbackb);

            bool worked = Settings.ReadDatFile(parser);
            if (worked & feedbackb)
                PrintLoadSuccess();
            else
                worked = false;
            return worked;
        }
        public static bool ReadDatFile(IniParser parser)//read success bool returned
        {
            bool success = false;

            bool readremote = true;
            try
            {
                //read remote
                string remotearray = parser.getDatFromKey(mainsec, curremotepre);
                string[] rarr = remotearray.Split(',');
                curRemote.Clear();
                //check to make sure there are no spaces on any of those
                for (int i = 0; i < rarr.Length; i++)
                {
                    if (rarr[i].Contains(" "))
                        rarr[i] = StringFilter.RemoveSpace(rarr[i]);
                    curRemote.Add(rarr[i]);
                }
                //now no white spaces
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
                string versionfile = parser.getDatFromKey(mainsec, versionpre);
                string curVersion = VersionController.WriteVersionNoOnly();
                if (versionfile != curVersion)
                {
                    UI.giveWarning("Different version used in saved config data");
                }
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
            parser.SetKey(mainsec, versionpre, VersionController.WriteVersionNoOnly());
            parser.SetKey(mainsec, curremotepre, getRemotesString(false));
            parser.Save();
        }
        public static void PromptChangeRemote()
        {
            int i = UI.getIndex("Select which remote to change", curRemote.ToArray());
            Console.WriteLine("new remote name?");
            ChangeRemote(i, Console.ReadLine());
        }
        public static void CheckoutBranch(string branchname, bool newb)
        {
            curBranchN = branchname;
            string[] cmnds = new string[1];
            if (newb)
                cmnds[0] = git.g("checkout -b " + branchname);
            else
                cmnds[0] = git.g("checkout " + branchname);
            string[] lineoutput = CMD.CMDcmdsLines(cmnds, true);
            SaveConfig();
        }
        private static void prompNB()
        {
            Console.WriteLine("new branch name?");
            string resp = Console.ReadLine();
            //make sure it isn't the same branch name naturally
            if (resp != curBranchN)
            {
                if (!resp.Contains(" "))//no spaces?
                    CheckoutBranch(resp, true);
            }
        }
        public static void PromptCheckoutNB()
        {
            //first work out if we want to chekout a 'new' branch?
            //we might not
            //get list of all branches please
            string[] brancN = git.getAllBranchNames();
            if (brancN.Length == 1)//only the 1, making new
            {
                Console.WriteLine("Only 1 branch found, making new branch is the only option here");
                prompNB();
            }
            else
            {
                //print them out please
                int index = UI.getIndex("pick branch to checkout", brancN);
                CheckoutBranch(brancN[index], false);
            }
           
        }
        public static void AddRemote(string remoteN)
        {
            curRemote.Add(remoteN);
            SaveConfig();
        }
        private static void removeRemIndex(int i)
        {
            curRemote.RemoveAt(i);
        }
        public static void RemoveRemote()
        {
            int gind = UI.getIndex("select which remote to remove by index", curRemote.ToArray());
            removeRemIndex(gind);
            SaveConfig();
        }
    }
}
