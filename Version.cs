using System;

namespace GitPusher
{
    public static class VersionController
    {
        static Uaine.VersionController.Version curVers = new Uaine.VersionController.Version(new int[] { 2, 0, 4}, "beta", false);

        public static void WriteVersion()
        {
            UI.white();
            Console.Write("Current Version: ");
            UI.yellow();
            string stringval = curVers.ToStr();
            Console.Write(stringval);
            Console.Write(Environment.NewLine);
            UI.white();
        }

        public static int compareToVersion(Uaine.VersionController.Version a)
        {
            return curVers.compareVersion(a);
        }

        public static string WriteVersionNoOnly()
        {
            Uaine.VersionController.Version tmpv = curVers;
            tmpv.changeDevStatus(false);
            return tmpv.ToStr();
        }
    }
}
