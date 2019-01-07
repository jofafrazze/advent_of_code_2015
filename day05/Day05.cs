using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day05
{
    class Day05
    {
        static List<string> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }

        static void PartA()
        {
            List<string> list = ReadInput();
            string vowels = "aeiou";
            string[] notOk = new string[] { "ab", "cd", "pq", "xy" };
            int passed = 0;
            foreach (string s in list)
            {
                int nv = 0;
                foreach (char c in vowels)
                    nv += s.Count(x => x == c);
                bool twoInARow = false;
                char cLast = '\0';
                foreach (char c in s)
                {
                    if (c == cLast)
                        twoInARow = true;
                    cLast = c;
                }
                int n3 = 0;
                foreach (string a in notOk)
                {
                    if (s.IndexOf(a) >= 0)
                        n3++;
                }
                if ((nv >= 3) && twoInARow && (n3 == 0))
                    passed++;
            }
            Console.WriteLine("Part A: Result is {0}.", passed);
        }

        static void PartB()
        {
            List<string> list = ReadInput();
            int passed = 0;
            foreach (string s in list)
            {
                bool b1 = false;
                for (int i = 0; i < s.Length - 3; i++)
                {
                    string a = s.Substring(i, 2);
                    if (s.IndexOf(a, i + 2) >= 0)
                        b1 = true;
                }
                bool b2 = false;
                for (int i = 0; i < s.Length - 2; i++)
                {
                    if (s[i] == s[i + 2])
                        b2 = true;
                }
                if (b1 && b2)
                    passed++;
            }
            Console.WriteLine("Part B: Result is {0}.", passed);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day05).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
