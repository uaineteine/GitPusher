﻿using System;
using System.Diagnostics;

namespace GitPusher
{
    public static class CMD
    {
        public static string g(string arg)
        {
            return "git " + arg;
        }
        public static void CMDWithCommands(string[] commands)
        {
            //start up the command to run
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            foreach (string command in commands)
            {
                cmd.StandardInput.WriteLine(command);
            }
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
