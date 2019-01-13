using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day20
{
    class Day20
    {
        static void PartA()
        {
            int target = 33100000;
            int ReachTarget()
            {
                for (int h = 1; true; h++)
                {
                    int sum = 0;
                    void AddToSum(int a)
                    {
                        sum += a * 10;
                    }
                    for (int i = 1; i <= Math.Sqrt(h); i++)
                    {
                        if (h % i == 0)
                        {
                            AddToSum(i);
                            if (h / i != i)
                                AddToSum(h / i);
                        }
                    }
                    if (sum >= target)
                        return h;
                }
            }
            int result = ReachTarget();
            Console.WriteLine("Part A: Result is {0}.", result);
        }

        static void PartB()
        {
            int target = 33100000;
            int ReachTarget()
            {
                for (int h = 1; true; h++)
                {
                    int sum = 0;
                    void AddToSum(int a, int h2)
                    {
                        if (h2 / a <= 50)
                        sum += a * 11;
                    }
                    for (int i = 1; i <= Math.Sqrt(h); i++)
                    {
                        if (h % i == 0)
                        {
                            AddToSum(i, h);
                            if (h / i != i)
                                AddToSum(h / i, h);
                        }
                    }
                    if (sum >= target)
                        return h;
                }
            }
            int result = ReachTarget();
            Console.WriteLine("Part B: Result is {0}.", result);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day20).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
