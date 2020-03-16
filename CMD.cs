using System;
using System.Diagnostics;

namespace GitPusher
{
    public static class CMD
    {
        public static string CMDWithCommands(string[] commands, bool print)
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
            string output = cmd.StandardOutput.ReadToEnd();
            if (print)
                Console.WriteLine(output);
            return output;//return this
        }
        public static string[] CMDcmdsLines(string[] commands, bool print)
        {
            string output = CMDWithCommands(commands, print);
            string[] lines = output.Split(new[] { "\n" },StringSplitOptions.None);
            int noLinestoSkip = 3;
            //remove the LHS directory nonsense
            string[] finalout = new string[lines.Length - noLinestoSkip];
            for (int i = 0; i < lines.Length - noLinestoSkip; i++)
            {
                finalout[i] = StringFilter.cleanLine(lines[i + noLinestoSkip]);
            }
            return finalout;
        }
    }
}
