using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day24
{
    class Day24
    {
        static List<int> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<int> list = new List<int>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(int.Parse(line));
            }
            return list;
        }

        //static List<List<int>> GetCombinations(List<int> input)
        //{
        //    List<List<int>> results = new List<List<int>>();
        //    for (int i = 0; i < input.Count; i++)
        //    {
        //        List<int> current = new List<int>() { input[i] };
        //        int size = results.Count;
        //        for (int r = 0; r < size; r++)
        //        {
        //            results.Add(results[r].Concat(current).ToList());
        //        }
        //        results.Add(current);
        //    }
        //    return results;
        //}

        static List<List<int>> GetCombinationsWithSum(List<int> input, int targetSum)
        {
            List<Tuple<List<int>, int>> results = new List<Tuple<List<int>, int>>();
            for (int i = 0; i < input.Count; i++)
            {
                List<int> current = new List<int>() { input[i] };
                int size = results.Count;
                int sum = int.MinValue;
                for (int r = 0; r < size; r++)
                {
                    List<int> next = results[r].Item1.Concat(current).ToList();
                    sum = results[r].Item2 + input[i];
                    if (sum <= targetSum)
                        results.Add(Tuple.Create(next, sum));
                }
                results.Add(Tuple.Create(current, input[i]));
            }
            return results.Where(x => x.Item2 == targetSum).Select(x => x.Item1).ToList();
        }

        static List<List<int>> GetCombinationsWithSum2(List<int> input, int targetSum)
        {
            List<List<int>> results = new List<List<int>>();
            int bits = input.Count;
            for (long i = 0; i < ((long)1 << bits); i++)
            {
                int sum = 0;
                for (int n = 0; n < bits; n++)
                {
                    if ((i & ((long)1 << n)) > 0)
                    {
                        sum += input[n];
                    }
                }
                if (sum == targetSum)
                {
                    List<int> result = new List<int>();
                    for (int n = 0; n < bits; n++)
                    {
                        if ((i & ((long)1 << n)) > 0)
                        {
                            result.Add(input[n]);
                        }
                    }
                    results.Add(result);
                }
            }
            return results;
        }

        static List<List<int>> GetCombinationsWithSum3(List<int> input, int targetSum)
        {
            List<List<int>> results = new List<List<int>>();
            int bits = input.Count;
            long[] bitList = new long[bits + 1];
            for (int i = 0; i <= bits; i++)
                bitList[i] = (long)1 << i;
            for (long i = 0; i < bitList[bits]; i++)
            {
                int sum = 0;
                for (int n = 0; n < bits; n++)
                {
                    if ((i & bitList[n]) > 0)
                    {
                        sum += input[n];
                    }
                }
                if (sum == targetSum)
                {
                    List<int> result = new List<int>();
                    for (int n = 0; n < bits; n++)
                    {
                        if ((i & bitList[n]) > 0)
                        {
                            result.Add(input[n]);
                        }
                    }
                    results.Add(result);
                }
            }
            return results;
        }

        static long QuantumEntanglement(List<int> list)
        {
            long m = 1;
            foreach (int i in list)
                m *= i;
            return m;
        }

        static long FindBestQuantumEntanglement(int groups)
        {
            List<int> all = ReadInput();
            int groupSum = all.Sum() / groups;
            var allWithSum = GetCombinationsWithSum(all, groupSum);
            int items = allWithSum.Select(x => x.Count).Min();
            var allWithNItems = allWithSum.Where(x => x.Count == items).ToList();
            List<int> idealGroup = allWithNItems.OrderBy(x => QuantumEntanglement(x)).ToList().First();
            //Console.WriteLine(String.Join(", ", idealGroup));
            return QuantumEntanglement(idealGroup);
        }

        static void PartA()
        {
            long result = FindBestQuantumEntanglement(3);
            Console.WriteLine("Part A: Result is {0}.", result);
        }

        static void PartB()
        {
            long result = FindBestQuantumEntanglement(4);
            Console.WriteLine("Part B: Result is {0}.", result);
        }

        static Func<TRes> Curry<T1, T2, TRes>(Func<T1, T2, TRes> func, T1 arg1, T2 arg2)
        {
            return () => func(arg1, arg2);
        }

        static void TimeFunction(Func<List<List<int>>> func, string name)
        {
            var watch = Stopwatch.StartNew();
            List<List<int>> ints = func();
            watch.Stop();
            Console.WriteLine("Test: {0} took {1} ms. ({2} elems)", name, watch.ElapsedMilliseconds, ints.Count);
        }

        static void Tests()
        {
            List<int> input = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            TimeFunction(Curry(GetCombinationsWithSum, input, 100), "GetCombinationsWithSum");
            TimeFunction(Curry(GetCombinationsWithSum2, input, 100), "GetCombinationsWithSum2");
            TimeFunction(Curry(GetCombinationsWithSum3, input, 100), "GetCombinationsWithSum3");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day24).Namespace + ":");
            //Tests();
            PartA();
            PartB();
        }
    }
}
