using System;
using System.Text;

namespace GitPusher
{
    public class Version
    {
        int[] versionNo;
        int depth;
        string specialAttr = "";
        bool indev;
        public Version(int[] thisnum, string special, bool devbool)//leave blank if none
        {
            this.depth = thisnum.Length;
            this.versionNo = thisnum;
            this.specialAttr = special;
            this.indev = devbool;
        }

        public string ToStr()
        {
            if (specialAttr.Length > 0)
                specialAttr += " ";
            StringBuilder output = new StringBuilder();
            //if indev say so
            if (indev)
                output.Append("InDev ");
            output.Append(specialAttr);
            for (int i = 0; i < depth; i++)
            {
                output.Append(versionNo[i].ToString());
                if (i != depth - 1)
                    output.Append(".");
            }
            return output.ToString();
        }
    }
    public static class VersionController
    {
        static Version curVers = new Version(new int[] { 1, 1, 0 }, "alpha", false);

        public static void WriteVersion()
        {
            string stringval = curVers.ToStr();
            Console.WriteLine("Current Version: " + stringval);
        }
    }
}
