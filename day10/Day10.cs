using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day10
{
    class Day10
    {
        static string LookAndSay(string a)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            do
            {
                int iStart = i;
                char c = a[iStart];
                i++;
                while ((i < a.Length) && (a[i] == c))
                    i++;
                int n = i - iStart;
                sb.Append(n.ToString());
                sb.Append(c);
            }
            while (i < a.Length);
            return sb.ToString();
        }

        static void PartAB()
        {
            string input = "1113122113";
            string result = input;
            for (int i = 0; i < 40; i++)
                result = LookAndSay(result);
            Console.WriteLine("Part A: Result is {0}.", result.Length);
            for (int i = 0; i < 10; i++)
                result = LookAndSay(result);
            Console.WriteLine("Part B: Result is {0}.", result.Length);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day10).Namespace + ":");
            PartAB();
        }
    }
}
