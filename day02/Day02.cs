using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day02
{
    class Day02
    {
        static List<int[]> ReadInput()
        {
            List<int[]> list = new List<int[]>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int[] d = line.Split('x').Select(int.Parse).ToArray();
                list.Add(d);
            }
            return list;
        }

        static void PartA()
        {
            List<int[]> boxes = ReadInput();
            int sum = 0;
            foreach (int[] b in boxes)
            {
                int l = b[0];
                int w = b[1];
                int h = b[2];
                int[] areas = new int[] { l * w, w * h, h * l };
                sum += areas.Min() + areas.Sum() * 2;
            }
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            List<int[]> boxes = ReadInput();
            int sum = 0;
            foreach (int[] b in boxes)
            {
                int l = b[0];
                int w = b[1];
                int h = b[2];
                int[] sides = new int[] { l, w, h };
                Array.Sort(sides);
                int minPerim = sides[0] * 2 + sides[1] * 2;
                int vol = l * w * h;
                sum += (minPerim + vol);
            }
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day02).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
