using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day14
{
    class Raindeer
    {
        public string name;
        public int activeSpeed; // km/s
        public int activeSecs;
        public int restingSecs;
        public int CycleSecs { get { return activeSecs + restingSecs; } }
    }

    class Day14
    {
        static List<Raindeer> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Raindeer> list = new List<Raindeer>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Raindeer r = new Raindeer();
                string[] s = line.Split(' ').ToArray();
                r.name = s[0];
                r.activeSpeed = int.Parse(s[3]);
                r.activeSecs = int.Parse(s[6]);
                r.restingSecs = int.Parse(s[13]);
                list.Add(r);
            }
            return list;
        }

        static int GetLengthTravelled(Raindeer r, int seconds)
        {
            int cycles = seconds / r.CycleSecs;
            int deltaSecs = seconds % r.CycleSecs;
            int flySecs = cycles * r.activeSecs + Math.Min(deltaSecs, r.activeSecs);
            return flySecs * r.activeSpeed;
        }

        static void PartA()
        {
            List<Raindeer> raindeers = ReadInput();
            int secs = 2503;
            List<int> dist = raindeers.Select(x => GetLengthTravelled(x, secs)).ToList();
            Console.WriteLine("Part A: Result is {0}.", dist.Max());
        }

        static void PartB()
        {
            List<Raindeer> raindeers = ReadInput();
            int secs = 2503;
            Dictionary<Raindeer, int> score = raindeers.ToDictionary(x => x, x => 0);
            for (int i = 1; i <= secs; i++)
            {
                Dictionary<Raindeer, int> dist = raindeers.ToDictionary(x => x, x => GetLengthTravelled(x, i));
                int max = dist.Values.Max();
                List<Raindeer> leading = dist.Where(x => x.Value == max).Select(x => x.Key).ToList();
                leading.ForEach(x => score[x]++);
            }
            Console.WriteLine("Part B: Result is {0}.", score.Values.Max());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day14).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
