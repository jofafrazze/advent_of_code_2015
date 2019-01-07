using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day13
{
    class Day13
    {
        static Dictionary<string, Dictionary<string, int>> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Dictionary<string, Dictionary<string, int>> distances = new Dictionary<string, Dictionary<string, int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(' ').ToArray();
                string name1 = s[0];
                string name2 = s[10].TrimEnd('.');
                int happiness = int.Parse(s[3]);
                if (s[2] == "lose")
                    happiness = -happiness;
                void AddData(string c1, string c2, int d)
                {
                    if (!distances.ContainsKey(c1))
                        distances.Add(c1, new Dictionary<string, int>());
                    distances[c1].Add(c2, d);
                }
                AddData(name1, name2, happiness);
            }
            return distances;
        }

        static List<List<T>> HeapPermutation<T>(List<T> a)
        {
            List<List<T>> result = new List<List<T>>();
            void Swap(ref List<T> b, int i1, int i2)
            {
                T temp = b[i1];
                b[i1] = b[i2];
                b[i2] = temp;
            }
            void Permute(ref List<T> b, int size)
            {
                if (size == 1)
                {
                    result.Add(new List<T>(b));
                }
                else
                {
                    for (int i = 0; i < size - 1; i++)
                    {
                        Permute(ref b, size - 1);
                        Swap(ref b, (size % 2 == 0) ? i : 0, size - 1);
                    }
                    Permute(ref b, size - 1);
                }
            }
            List<T> copy = new List<T>(a);
            Permute(ref copy, copy.Count);
            return result;
        }

        static void PrintInfo(List<List<string>> listList, string name)
        {
            Console.WriteLine("{0}: {1}", name, listList.Count);
            listList.ForEach(list => Console.WriteLine(String.Join(", ", list)));
            Console.WriteLine();
        }

        static int GetMaxTotal(Dictionary<string, Dictionary<string, int>> happiness)
        {
            //List<string> guests = new List<string>() { "A", "B", "C", "D" };
            List<string> guests = happiness.Select(x => x.Key).ToList();
            List<List<string>> guestCombos = HeapPermutation(guests);
            List<List<string>> guestCombosNoCircular = guestCombos.Where(x => x.First() == guests.First()).ToList();
            List<List<string>> guestCombosNoReverse = guestCombos.Where(x => String.Compare(x.First(), x.Last()) < 0).ToList();
            //PrintInfo(guestCombos, "All");
            //PrintInfo(guestCombosNoCircular, "No Circular");
            //PrintInfo(guestCombosNoReverse, "No Reverse");
            List<int> happinesTotals = new List<int>();
            foreach (List<string> list in guestCombosNoCircular)
            {
                int sum = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    string g1 = list[i];
                    string g2 = list[(i + 1) % list.Count];
                    sum += happiness[g1][g2];
                    sum += happiness[g2][g1];
                }
                happinesTotals.Add(sum);
            }
            happinesTotals.Sort();
            return happinesTotals.Last();
        }

        static void PartA()
        {
            var happiness = ReadInput();
            int result = GetMaxTotal(happiness);
            Console.WriteLine("Part A: Result is {0}.", result);
        }

        static void PartB()
        {
            var happiness = ReadInput();
            List<string> guests = happiness.Select(x => x.Key).ToList();
            string me = "me";
            happiness[me] = new Dictionary<string, int>();
            foreach (string g in guests)
            {
                happiness[me].Add(g, 0);
                happiness[g].Add(me, 0);
            }
            int result = GetMaxTotal(happiness);
            Console.WriteLine("Part B: Result is {0}.", result);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day13).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
