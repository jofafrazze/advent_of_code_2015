using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day03
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

    class Day03
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
            string dirs = ReadInput();
            Position pos = new Position();
            HashSet<Position> set = new HashSet<Position>();
            set.Add(pos);
            foreach (char c in dirs)
            {
                if (c == '^') pos.y--;
                if (c == 'v') pos.y++;
                if (c == '<') pos.x--;
                if (c == '>') pos.x++;
                set.Add(pos);
            }
            Console.WriteLine("Part A: Result is {0}.", set.Count);
        }

        static void PartB()
        {
            string dirs = ReadInput();
            Position[] pos = new Position[] { new Position(), new Position() };
            HashSet<Position> set = new HashSet<Position>();
            set.Add(pos[0]);
            set.Add(pos[1]);
            int which = 0;
            foreach (char c in dirs)
            {
                if (c == '^') pos[which].y--;
                if (c == 'v') pos[which].y++;
                if (c == '<') pos[which].x--;
                if (c == '>') pos[which].x++;
                set.Add(pos[which]);
                which = (which == 1) ? 0 : 1;
            }
            Console.WriteLine("Part B: Result is {0}.", set.Count);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day03).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
