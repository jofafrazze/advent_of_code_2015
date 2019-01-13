using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day16
{
    class Day16
    {
        static List<Dictionary<string, int>> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Dictionary<string, int>> list = new List<Dictionary<string, int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(' ').ToArray();
                Dictionary<string, int> features = new Dictionary<string, int>();
                features.Add(s[2].TrimEnd(':'), int.Parse(s[3].TrimEnd(',')));
                features.Add(s[4].TrimEnd(':'), int.Parse(s[5].TrimEnd(',')));
                features.Add(s[6].TrimEnd(':'), int.Parse(s[7].TrimEnd(',')));
                list.Add(features);
            }
            return list;
        }

        static readonly Dictionary<string, int> myAuntFeatures = new Dictionary<string, int>()
        {
            { "children", 3 },
            { "cats", 7 },
            { "samoyeds", 2 },
            { "pomeranians", 3 },
            { "akitas", 0 },
            { "vizslas", 0 },
            { "goldfish", 5 },
            { "trees", 3 },
            { "cars", 2 },
            { "perfumes", 1 },
        };

        static bool Less(int i1, int i2) { return i1 < i2; }
        static bool Equal(int i1, int i2) { return i1 == i2; }
        static bool Greater(int i1, int i2) { return i1 > i2; }

        static readonly Dictionary<string, Func<int, int, bool>> auntBComparison = new Dictionary<string, Func<int, int, bool>>()
        {
            { "children", Equal  },
            { "cats", Greater },
            { "samoyeds", Equal },
            { "pomeranians", Less },
            { "akitas", Equal },
            { "vizslas", Equal },
            { "goldfish", Less },
            { "trees", Greater },
            { "cars", Equal },
            { "perfumes", Equal },
        };

        static void PartAB()
        {
            List<Dictionary<string, int>> allAuntFeatures = ReadInput();
            int auntNumberA = -1;
            int auntNumberB = -1;
            for (int i = 0; i < allAuntFeatures.Count; i++)
            {
                var f = allAuntFeatures[i];
                bool matchA = true;
                bool matchB = true;
                foreach (var kvp in f)
                {
                    if (myAuntFeatures.ContainsKey(kvp.Key))
                    {
                        matchA = matchA && (myAuntFeatures[kvp.Key] == kvp.Value);
                        matchB = matchB && (auntBComparison[kvp.Key](kvp.Value, myAuntFeatures[kvp.Key]));
                    }
                }
                if (matchA)
                    auntNumberA = i + 1;
                if (matchB)
                    auntNumberB = i + 1;
            }
            Console.WriteLine("Part A: Result is {0}.", auntNumberA);
            Console.WriteLine("Part B: Result is {0}.", auntNumberB);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day16).Namespace + ":");
            PartAB();
        }
    }
}
