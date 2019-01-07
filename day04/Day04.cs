using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day04
{
    class Day04
    {
        static void PartA()
        {
            string input = "iwrupvqb";
            MD5 md5 = MD5.Create();
            int i = 0;
            byte[] hash;
            do
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input + i.ToString());
                hash = md5.ComputeHash(bytes);
                i++;
            }
            while ((hash[0] != 0) || (hash[1] != 0) || (hash[2] > 15));
            Console.WriteLine("Part A: Result is {0}.", i - 1);
        }

        static void PartB()
        {
            string input = "iwrupvqb";
            MD5 md5 = MD5.Create();
            int i = 0;
            byte[] hash;
            do
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input + i.ToString());
                hash = md5.ComputeHash(bytes);
                i++;
            }
            while ((hash[0] != 0) || (hash[1] != 0) || (hash[2] != 0));
            Console.WriteLine("Part B: Result is {0}.", i - 1);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day04).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
