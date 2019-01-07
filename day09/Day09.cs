using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day09
{
    class Day09
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
                string city1 = s[0];
                string city2 = s[2];
                int dist = int.Parse(s[4]);
                void AddData(string c1, string c2, int d)
                {
                    if (!distances.ContainsKey(c1))
                        distances.Add(c1, new Dictionary<string, int>());
                    distances[c1].Add(c2, d);
                }
                AddData(city1, city2, dist);
                AddData(city2, city1, dist);
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

        static void PartAB()
        {
            var distances = ReadInput();
            List<string> cities = distances.Select(x => x.Key).ToList();
            List<List<string>> cityCombos = HeapPermutation(cities);
            List<int> routeLengths = new List<int>();
            foreach (List<string> route in cityCombos)
            {
                int length = 0;
                for (int i = 0; i < route.Count - 1; i++)
                    length += distances[route[i]][route[i + 1]];
                routeLengths.Add(length);
            }
            Console.WriteLine("Part A: Result is {0}.", routeLengths.Min());
            Console.WriteLine("Part B: Result is {0}.", routeLengths.Max());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day09).Namespace + ":");
            PartAB();
        }
    }
}
