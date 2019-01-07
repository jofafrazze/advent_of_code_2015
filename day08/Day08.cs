using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day08
{
    class Day08
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
            List<string> input = ReadInput();
            int sum = 0;
            foreach (string s in input)
            {
                string code = s;
                code = code.Replace("\\\"", "/^");
                code = code.Replace("\"", "'");
                code = code.Replace("\\x", "/x");
                code = code.Replace("\\", "/");
                string literal = s.Substring(1, s.Length - 2);
                literal = literal.Replace("\\\"", "^");
                literal = literal.Replace("\\\\", "/");
                literal = Regex.Replace(literal, @"(\\x[0-9a-f]{2})", "|");
                sum += (code.Length - literal.Length);
            }
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            List<string> input = ReadInput();
            int sum = 0;
            foreach (string s in input)
            {
                string code = s;
                code = code.Replace("\\\"", "/^");
                code = code.Replace("\"", "'");
                code = code.Replace("\\x", "/x");
                code = code.Replace("\\", "/");
                string encode = "'_" + code.Substring(0, code.Length - 1) + "_''";
                encode = encode.Replace("^", "_'");
                encode = encode.Replace("/", "~~");
                sum += (encode.Length - code.Length);
            }
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day08).Namespace + ":");
            PartA();
            PartB();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            //var words = File.ReadAllLines(path);
            //Console.Out.WriteLine(words.Sum(w => w.Length - Regex.Replace(w.Trim('"').Replace("\\\"", "A").Replace("\\\\", "B"), "\\\\x[a-f0-9]{2}", "C").Length));
            //Console.Out.WriteLine(words.Sum(w => w.Replace("\\", "AA").Replace("\"", "BB").Length + 2 - w.Length));
        }
    }
}
