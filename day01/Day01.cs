using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day01
{
    class Day01
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string line = reader.ReadLine();
            return line;
        }

        static void PartA()
        {
            string steps = ReadInput();
            int up = steps.Count(x => x == '(');
            int down = steps.Count(x => x == ')');
            Console.WriteLine("Part A: Result is {0}.", up - down);
        }

        static void PartB()
        {
            string steps = ReadInput();
            int i, floor = 0;
            for (i = 0; (i < steps.Length) && (floor != -1); i++)
                floor += (steps[i] == '(') ? 1 : -1;
            Console.WriteLine("Part B: Result is {0}.", i);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day01).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
