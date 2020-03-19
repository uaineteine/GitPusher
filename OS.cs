using System;
using System.Runtime.InteropServices;

namespace GitPusher
{
    public static class OS
    {
        static int myOS = -1; //-1 don't know, 0 windows, 1 Linux, 2 OSx

        public static void PrintOS()
        {
            UI.white();
            Console.Write("Current OS: ");
            UI.yellow();
            Console.Write(RuntimeInformation.OSDescription);
            Console.Write(Environment.NewLine);
            UI.white();
        }

        public static int IdentifyOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                myOS = 0;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                myOS = 1;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                myOS = 2;
            return myOS;
        }
    }
}
