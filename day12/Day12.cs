using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day12
{
    class Day12
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string line = reader.ReadLine();
            return line;
        }

        static int GetNumbersSum(string line)
        {
            Regex regex = new Regex(@"-?[\d]+");
            int sum = 0;
            foreach (Match match in regex.Matches(line))
            {
                sum += int.Parse(match.Value);
            }
            return sum;
        }

        static void SaveString(string s, string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\" + fileName);
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(s);
            writer.Close();
        }

        static void PartAB()
        {
            string line = ReadInput();
            int a = GetNumbersSum(line);
            Console.WriteLine("Part A: Result is {0}.", a);
            StringBuilder sb = new StringBuilder(line);
            int red = 0;
            int lastRed = red;
            do
            {
                red = line.IndexOf(":\"red", lastRed);
                if (red >= 0)
                {
                    lastRed = red + 1;
                    int depth1 = 1;
                    int i = 0;
                    int n = 0;
                    for (i = red; depth1 > 0; i--)
                    {
                        if (line[i] == '{') depth1--;
                        if (line[i] == '}') depth1++;
                    }
                    i++;
                    int depth2 = 1;
                    for (n = red; depth2 > 0; n++)
                    {
                        if (line[n] == '{') depth2++;
                        if (line[n] == '}') depth2--;
                    }
                    for (int m = i; m < n; m++)
                        sb[m] = ' ';
                }
            }
            while (red >= 0);
            string s = sb.ToString();
            SaveString(s, "output.txt");
            int sum = GetNumbersSum(s);
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day12).Namespace + ":");
            PartAB();
        }
    }
}
