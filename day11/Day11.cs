using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day11
{
    class Day11
    {
        static string PasswordIncrement(string pw)
        {
            string s = pw;
            bool overflow = false;
            int index = pw.Length;
            do
            {
                index--;
                char c = pw[index];
                overflow = (c == 'z');
                c++;
                if (overflow)
                    c = 'a';
                s = s.Substring(0, index) + c + s.Substring(index + 1, pw.Length - index - 1);
            }
            while (overflow);
            return s;
        }

        static bool CheckRequirement1(string pw)
        {
            for (int i = 0; i < pw.Length - 3; i++)
            {
                if ((pw[i] <= 'x') && (pw[i] + 1 == pw[i + 1]) && (pw[i + 1] + 1 == pw[i + 2]))
                    return true;
            }
            return false;
        }

        static bool CheckRequirement2(string pw)
        {
            char[] c = new char[] { 'i', 'o', 'l' };
            return pw.IndexOfAny(c) < 0;
        }

        static bool CheckRequirement3(string pw)
        {
            HashSet<char> found = new HashSet<char>();
            for (int i = 0; i < pw.Length - 1; i++)
            {
                if ((pw[i] == pw[i + 1]) && !found.Contains(pw[i]))
                    found.Add(pw[i]);
            }
            return found.Count >= 2;
        }

        static void PartAB()
        {
            string input = "hepxcrrq";
            string pw = input;
            bool pwOk = false;
            do
            {
                pw = PasswordIncrement(pw);
                pwOk = CheckRequirement1(pw);
                pwOk = pwOk && CheckRequirement2(pw);
                pwOk = pwOk && CheckRequirement3(pw);
            }
            while (!pwOk);
            Console.WriteLine("Part A: Result is {0}.", pw);
            do
            {
                pw = PasswordIncrement(pw);
                pwOk = CheckRequirement1(pw);
                pwOk = pwOk && CheckRequirement2(pw);
                pwOk = pwOk && CheckRequirement3(pw);
            }
            while (!pwOk);
            Console.WriteLine("Part B: Result is {0}.", pw);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day11).Namespace + ":");
            PartAB();
        }
    }
}
