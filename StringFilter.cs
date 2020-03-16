using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitPusher
{
    public static class StringFilter
    {
        public static string RemoveSpace(string inp)
        {
            return inp.Replace(" ", "");
        }
        public static string filterAstrix(string inp)
        {
            inp = inp.Replace("*", "");
            return RemoveSpace(inp);
        }
        public static string[] FindDouble(string[] filtered)
        {
            List<string> output = new List<string>();
            string last = "";
            for (int i = 0; i < filtered.Length; i++)
            {
                string cur = filtered[i];
                if (cur == last)
                    if (cur.Length > 0)
                        output.Add(cur);
                last = cur;
            }
            return output.ToArray();
        }
        public static string cleanLine(string input)
        {
            //clean dir start if applicable
            string output = cleanDir(input);
            //now remove tabs
            output = cleanTabs(output);
            return cleanCarrige(output);
        }
        public static string cleanTabs(string input)
        {
            int index = input.IndexOf("\t");
            if (index == 0)
                return input.Remove(0, 1);
            else
                return input;
        }
        public static string cleanDir(string input)
        {
            int index = input.IndexOf(">");
            if (index > 0)
                return input.Remove(0, index + 1);
            else
                return input;
        }
        public static string cleanCarrige(string input)
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
