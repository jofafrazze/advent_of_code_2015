using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day06
{
    public struct Position : IComparable<Position>
    {
        public int x;
        public int y;

        public Position(Position p)
        {
            x = p.x;
            y = p.y;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(Position p)    // Reading order
        {
            if (x == p.x && y == p.y)
                return 0;
            else if (y == p.y)
                return (x < p.x) ? -1 : 1;
            else
                return (y < p.y) ? -1 : 1;
        }
        public override bool Equals(Object obj)
        {
            return obj is Position && Equals((Position)obj);
        }
        public bool Equals(Position p)
        {
            return (x == p.x) && (y == p.y);
        }
        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }
        public static Position operator +(Position p1, int k)
        {
            return p1 + new Position(k, k);
        }
        public static Position operator +(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x += p2.x;
            p.y += p2.y;
            return p;
        }
        public static Position operator -(Position p1, int k)
        {
            return p1 + (-k);
        }
        public static Position operator -(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x -= p2.x;
            p.y -= p2.y;
            return p;
        }
        public static Position operator *(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x *= k;
            p.y *= k;
            return p;
        }
        public static Position operator /(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x /= k;
            p.y /= k;
            return p;
        }
    }

    class Direction
    {
        public int action;  // 0 = off, 1 = on, 2 = toggle
        public Position p1;
        public Position p2;
    }

    public class Map
    {
        public int width;
        public int height;
        public Position start;
        public char[,] map;

        public Map(int w, int h, Position s = new Position(), char fill = '\0')
        {
            width = w;
            height = h;
            start = s;
            map = new char[w, h];
            for (int i = 0; i < w * h; i++)
            {
                map[i % w, i / w] = fill;
            }
        }

        public char this[Position p]
        {
            get
            {
                return map[p.x, p.y];
            }
            set
            {
                map[p.x, p.y] = value;
            }
        }
    }

    class Day06
    {
        static List<Direction> ReadInput()
        {
            List<Direction> list = new List<Direction>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Regex regex = new Regex(@"^(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                MatchCollection matches = regex.Matches(line);
                if (matches.Count > 0)
                {
                    GroupCollection groups = matches[0].Groups;
                    int i = 1;
                    Direction dir = new Direction();
                    string action = groups[i++].Value;
                    if (action == "turn off") dir.action = 0;
                    else if (action == "turn on") dir.action = 1;
                    else dir.action = 2;
                    dir.p1.x = int.Parse(groups[i++].Value);
                    dir.p1.y = int.Parse(groups[i++].Value);
                    dir.p2.x = int.Parse(groups[i++].Value);
                    dir.p2.y = int.Parse(groups[i++].Value);
                    list.Add(dir);
                }
            }
            return list;
        }

        static void PartA()
        {
            List<Direction> directions = ReadInput();
            Map map = new Map(1000, 1000);
            foreach (Direction d in directions)
            {
                for (int x = d.p1.x; x <= d.p2.x; x++)
                {
                    for (int y = d.p1.y; y <= d.p2.y; y++)
                    {
                        if (d.action == 2)
                            map.map[x, y] = (char)((map.map[x, y] + 1) % 2);
                        else
                            map.map[x, y] = (char)d.action;
                    }
                }
            }
            int sum = map.map.Cast<char>().Select(x => (int)x).ToList().Sum();
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            List<Direction> directions = ReadInput();
            int[,] map = new int[1000, 1000];
            foreach (Direction d in directions)
            {
                for (int x = d.p1.x; x <= d.p2.x; x++)
                {
                    for (int y = d.p1.y; y <= d.p2.y; y++)
                    {
                        if (d.action == 0)
                        {
                            if (map[x, y] > 0)
                                map[x, y]--;
                        }
                        else if (d.action == 1)
                            map[x, y]++;
                        else
                            map[x, y] += 2;
                    }
                }
            }
            int sum = map.Cast<int>().ToList().Sum();
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day06).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
