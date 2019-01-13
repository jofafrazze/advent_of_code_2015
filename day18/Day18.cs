using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day18
{
    class Day18
    {
        static int[,] ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string[] lines = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int[,] data = new int[lines[0].Length, lines.Count()];
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    data[x, y] = (lines[y][x] == '#') ? 1 : 0;
                }
            }
            return data;
        }

        static void PrintMap(int[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    sb.Append(map[x, y] > 0 ? '#' : '.');
                }
                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine();
        }

        static int Map(int[,] map, int x, int y)
        {
            if ((x < 0) || (x >= map.GetLength(0)))
                return 0;
            if ((y < 0) || (y >= map.GetLength(1)))
                return 0;
            return map[x, y];
        }

        static void LightCorners(ref int[,] map)
        {
            map[0, 0] = 1;
            map[map.GetLength(0) - 1, 0] = 1;
            map[map.GetLength(0) - 1, map.GetLength(1) - 1] = 1;
            map[0, map.GetLength(1) - 1] = 1;
        }

        static int[,] RunAnimation(int[,] map, bool cornersAlwaysOn)
        {
            for (int iter = 0; iter < 100; iter++)
            {
                if (cornersAlwaysOn)
                    LightCorners(ref map);
                int[,] nextMap = map.Clone() as int[,];
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        int neighbors =
                            Map(map, x - 1, y - 1) + Map(map, x + 0, y - 1) + Map(map, x + 1, y - 1) +
                            Map(map, x - 1, y + 0) + Map(map, x + 1, y + 0) +
                            Map(map, x - 1, y + 1) + Map(map, x + 0, y + 1) + Map(map, x + 1, y + 1);
                        if (map[x, y] > 0)
                            nextMap[x, y] = ((neighbors == 2) || (neighbors == 3)) ? 1 : 0;
                        else
                            nextMap[x, y] = (neighbors == 3) ? 1 : 0;
                    }
                }
                map = nextMap;
                //PrintMap(map);
            }
            if (cornersAlwaysOn)
                LightCorners(ref map);
            return map;
        }

        static void PartA()
        {
            int[,] map = ReadInput();
            //PrintMap(map);
            map = RunAnimation(map, false);
            Console.WriteLine("Part A: Result is {0}.", map.Cast<int>().Sum());
        }

        static void PartB()
        {
            int[,] map = ReadInput();
            map = RunAnimation(map, true);
            Console.WriteLine("Part B: Result is {0}.", map.Cast<int>().Sum());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day18).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
