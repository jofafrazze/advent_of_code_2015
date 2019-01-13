using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day19
{
    class Day19
    {
        static Dictionary<string, List<string>> ReadInput(ref string molecule)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(' ').ToArray();
                if (s.Count() == 3)
                {
                    if (!dict.ContainsKey(s[0]))
                        dict[s[0]] = new List<string>();
                    dict[s[0]].Add(s[2]);
                }
                else if (s.Count() == 1)
                {
                    molecule = s[0];
                }
            }
            return dict;
        }

        static List<int> FindPositions(string whole, string find)
        {
            List<int> positions = new List<int>();
            int pos = 0;
            while ((pos = whole.IndexOf(find, pos)) != -1)
            {
                positions.Add(pos);
                pos += find.Length;
            }
            return positions;
        }

        static void PartA()
        {
            string baseMolecule = "";
            var replacements = ReadInput(ref baseMolecule);
            HashSet<string> uniqueMolecules = new HashSet<string>();
            foreach (var replacement in replacements)
            {
                string from = replacement.Key;
                List<int> positions = FindPositions(baseMolecule, from);
                foreach (int index in positions)
                {
                    foreach (string to in replacement.Value)
                    {
                        string newMolecule =
                            baseMolecule.Substring(0, index) + to + 
                            baseMolecule.Substring(index + from.Length);
                        uniqueMolecules.Add(newMolecule);
                    }
                }
            }
            Console.WriteLine("Part A: Result is {0}.", uniqueMolecules.Count);
        }

        static void PartB()
        {
            string targetMolecule = "e";
            string baseMolecule = "";
            var replacements = ReadInput(ref baseMolecule);
            Dictionary<string, string> origins = new Dictionary<string, string>();
            foreach (var kvp in replacements)
                foreach (string r in kvp.Value)
                    origins[r] = kvp.Key;
            Dictionary<string, int> uniqueMolecules = new Dictionary<string, int>();
            uniqueMolecules[baseMolecule] = 0;
            do
            {
                // Just use the shortest 100 molecules for the next round
                List<string> uniqueMoleculeList = uniqueMolecules.Select(x => x.Key).OrderBy(x => x.Length).Take(100).ToList();
                //Console.WriteLine("Molecules: {0}, shortest: {1}", 
                //    uniqueMoleculeList.Count,
                //    uniqueMoleculeList.Select(x => x.Length).Min());
                Console.Write(".");
                foreach (string currentMolecule in uniqueMoleculeList)
                {
                    foreach (var origin in origins)
                    {
                        string from = origin.Key;
                        string to = origin.Value;
                        List<int> positions = FindPositions(currentMolecule, from);
                        foreach (int index in positions)
                        {
                            string newMolecule =
                                currentMolecule.Substring(0, index) + to +
                                currentMolecule.Substring(index + from.Length);
                            if (!uniqueMolecules.ContainsKey(newMolecule))
                            {
                                uniqueMolecules[newMolecule] = uniqueMolecules[currentMolecule] + 1;
                            }
                        }
                    }
                }
            }
            while (!uniqueMolecules.ContainsKey(targetMolecule));
            Console.WriteLine();
            Console.WriteLine("Part B: Result is {0}.", uniqueMolecules[targetMolecule]);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day19).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
