using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GitPusher
{
    public static class Licence
    {
        static string fn = "LICENSE";

        public static void DisplayScreen()
        {
            UI.displayFilesScreen(fn);
        }
    }
}
