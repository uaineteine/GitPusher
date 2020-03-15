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
                finalout[i] = cleanLine(lines[i + noLinestoSkip]);
            }
            return finalout;
        }
        private static string cleanLine(string input)
        {
            //clean dir start if applicable
            string output = cleanDir(input);
            //now remove tabs
            output = cleanTabs(output);
            return cleanCarrige(output);
        }
        private static string cleanTabs(string input)
        {
            int index = input.IndexOf("\t");
            if (index == 0)
                return input.Remove(0, 1);
            else
                return input;
        }
        private static string cleanDir(string input)
        {
            int index = input.IndexOf(">");
            if (index > 0)
                return input.Remove(0, index + 1);
            else
                return input;
        }
        private static string cleanCarrige(string input)
        {
            int index = input.IndexOf("\r");
            if (index == 0)
                input = input.Remove(0, index + 1);
            index = input.IndexOf("\r");
            if (index > 0 & index == input.Length - 1)
                input = input.Remove(index, 1);
            return input;
        }
    }
}
