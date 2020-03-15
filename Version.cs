using System;
using System.Text;
using System.Linq;

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

        public void changeDevStatus(bool newstatus)
        {
            indev = newstatus;
        }

        public string ToStr()
        {
            string toadd = specialAttr;
            if (specialAttr.Length > 0)
                toadd += " ";

            StringBuilder output = new StringBuilder();
            //if indev say so
            if (indev)
                output.Append("InDev ");
            output.Append(toadd);
            for (int i = 0; i < depth; i++)
            {
                output.Append(versionNo[i].ToString());
                if (i != depth - 1)
                    output.Append(".");
            }
            return output.ToString();
        }

        public bool isANewer(Version a)
        {
            bool newer = false;
            Version iteratorVersion = a;
            if (a.depth < this.depth)//there are more numbers in b
                iteratorVersion = this;

            for(int i = 0; i < iteratorVersion.depth; i++)
            {
                if (a.versionNo[i] > this.versionNo[i])
                {
                    newer = true;
                    break;
                }
            }
            return newer;
        }

        //if b > a                                          return 0
        //if a > b                                          return 1
        //a and b the same                                  return 2
        //if not the same 'type' with the special attribute return 3
        public int compareVersion(Version a)
        {
            if (a.specialAttr == this.specialAttr)
            {
                if (a.versionNo.SequenceEqual(this.versionNo))
                    return 2;
                else if (this.isANewer(a))//a is newer so...
                    return 1;
                else
                    return 0;
            }
            else
                return 3; //not the same type
        }
    }
    public static class VersionController
    {
        static Version curVers = new Version(new int[] { 1, 0, 2}, "beta", false);

        public static void WriteVersion()
        {
            string stringval = curVers.ToStr();
            Console.WriteLine("Current Version: " + stringval);
        }

        public static int compareToVersion(Version a)
        {
            return curVers.compareVersion(a);
        }

        public static string WriteVersionNoOnly()
        {
            Version tmpv = curVers;
            tmpv.changeDevStatus(false);
            return tmpv.ToStr();
        }
    }
}
