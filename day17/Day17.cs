using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day17
{
    class Day17
    {
        static List<int> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string[] s = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return s.Select(int.Parse).ToList();
        }

        static void PartAB()
        {
            List<int> containers = ReadInput();
            int bits = containers.Count;
            int combosFound = 0;
            int minContainers = int.MaxValue;
            int nMinContainers = 0;
            for (long i = 0; i < Math.Pow(2, bits); i++)
            {
                int sum = 0;
                int nBits = 0;
                for (int n = 0; n < bits; n++)
                {
                    if ((i & (long)Math.Pow(2, n)) > 0)
                    {
                        sum += containers[n];
                        nBits++;
                    }
                }
                if (sum == 150)
                {
                    combosFound++;
                    if (nBits < minContainers)
                    {
                        minContainers = nBits;
                        nMinContainers = 1;
                    }
                    else if (nBits == minContainers)
                        nMinContainers++;
                }
            }
            Console.WriteLine("Part A: Result is {0}.", combosFound);
            Console.WriteLine("Part B: Result is {0}.", nMinContainers);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day17).Namespace + ":");
            PartAB();
        }
    }
}
