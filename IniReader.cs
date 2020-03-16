using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GitPusher
{
    public class IniSec
    {
        public string SecName;
        string[] LHS;
        string[] RHS;
        int n;
        public IniSec(string sec, string[] l, string[] r)
        {
            SecName = makeSec(sec);
            LHS = l;
            RHS = r;
            n = l.Length;
        }
        public IniSec(string sec, string[] lines)
        {
            SecName = makeSec(sec);
            n = lines.Length;
            LHS = new string[n];
            RHS = new string[n];
            for (int i = 0; i < n; i++)
            {
                SeperateKey(lines[i], out LHS[i], out RHS[i]);
            }
        }
        public void Write(StreamWriter sw)
        {
            sw.WriteLine(IniSec.makeSec(SecName));
            for (int i = 0; i < n; i++)
            {
                sw.WriteLine(LHS[i] + "=" + RHS[i]);
            }
        }
        public string get(string key)
        {
            for (int i = 0; i < n; i++)
            {
                if (LHS[i] == key)
                    return RHS[i];
            }
            return "";
        }
        public static string makeSec(string text)
        {
            //check if it isn't already a section
            if (IsSec(text))
                return text;//already fine
            else
                return "[" + text + "]";
        }
        public static bool IsSec(string text)
        {
            if (text[0] == '[' & text[text.Length - 1] == ']')
                return true;
            else
                return false;
        }
        public static bool IsBlankLine(string text)
        {
            if (text == "")
                return true;
            else
                return false;
        }
        public static bool SeperateKey(string line, out string l, out string r)
        {
            bool worked;
            string[] split = line.Split('=');
            l = "";
            r = "";
            if (split.Length > 1)//has it
            {
                worked = true;
                l = split[0];
                r = split[1];
            }
            else
                worked = false;
            return worked;
        }

        internal void Setkey(string curremotepre, string v)
        {
            for (int i = 0; i < n; i++)
            {
                if (LHS[i] == curremotepre)
                    RHS[i] = v;
            }
        }
    }
    public class IniDat
    {
        public List<IniSec> sections;
        public IniDat()
        {
            sections = new List<IniSec>();
        }
        public void addSec(string sec, string[] l, string[] r)
        {
            addSec(new IniSec(sec, l, r));
        }
        public void addSec(IniSec toAdd)
        {
            sections.Add(toAdd);
        }
        public string get(string sec, string key)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (IniSec.makeSec(sections[i].SecName) == IniSec.makeSec(sec))
                    return sections[i].get(key);
            }
            return "";//else it didn't find it
        }
        public IniSec getSec(string sec)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (IniSec.makeSec(sections[i].SecName) == IniSec.makeSec(sec))
                    return sections[i];
            }
            return new IniSec("no sec", new string[1] { "nadah" });
        }
        public void SetKey(string mainsec, string curremotepre, string v)
        {
            int index = 0;
            for (int i = 0; i < sections.Count; i++)
            {
                if (IniSec.makeSec(sections[i].SecName) == IniSec.makeSec(mainsec))
                {
                    index = i;
                }
            }
            sections[index].Setkey(curremotepre, v);
        }
    }
    public class IniParser
    {
        private string fn = "gitpusher/config.ini";//default
        private IniDat theDat;
        public IniParser(string filename)
        {
            fn = filename;
            theDat = new IniDat();
        }
        public bool Save()
        {
            using (FileStream fs = new FileStream(fn, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    for (int i = 0; i < theDat.sections.Count; i++)
                    {
                        theDat.sections[i].Write(sw);
                    }
                    sw.Close();
                }
                fs.Close();
            }
            return true;
        }

        public void SetKey(string mainsec, string curremotepre, string v)
        {
            theDat.SetKey(mainsec, curremotepre, v);
        }

        public bool Read()
        {
            bool worked = File.Exists(fn);
            string[] lines = File.ReadAllLines(fn);
            string curSec = "";
            List<string> SecLines = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (!IniSec.IsBlankLine(lines[i]))
                {
                    if (IniSec.IsSec(lines[i]) & lines[i]  != curSec)
                    {
                        curSec = lines[i];
                        if (SecLines.Count > 0)
                        {
                            //new section so append the last bits
                            IniSec sec = new IniSec(curSec, SecLines.ToArray());
                            theDat.addSec(sec);
                        }
                        SecLines.Clear();
                    }
                    else
                    {
                        SecLines.Add(lines[i]);
                    }
                }
            }
            if (SecLines.Count > 0)
            {
                //new section so append the last bits
                IniSec sec = new IniSec(curSec, SecLines.ToArray());
                theDat.addSec(sec);
            }
            return worked;
        }

        public string getDatFromKey(string sec, string key)
        {
            return theDat.get(sec, key);
        }
    }
}
