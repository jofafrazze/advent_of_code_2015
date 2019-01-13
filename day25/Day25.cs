using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day25
{
    class Day25
    {
        static long GetCode(int row, int col)
        {
            int r = 1;
            int c = 1;
            int rStart = 1;
            long current = 20151125;
            const long mul = 252533;
            const long mod = 33554393;
            while ((r != row) || (c != col))
            {
                long previous = current;
                c++;
                r--;
                if (r == 0)
                {
                    rStart++;
                    r = rStart;
                    c = 1;
                }
                current = (previous * mul) % mod;

            }
            return current;
        }

        static void PartA()
        {
            long code = GetCode(2978, 3083);
            Console.WriteLine("Part A: Result is {0}.", code);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day25).Namespace + ":");
            PartA();
        }
    }
}
