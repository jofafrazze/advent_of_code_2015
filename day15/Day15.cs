using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day15
{
    class Ingredient
    {
        public string name;
        public int capacity;
        public int durability;
        public int flavor;
        public int texture;
        public int calories;
    }
    class Day15
    {
        static List<Ingredient> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Ingredient> list = new List<Ingredient>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Ingredient r = new Ingredient();
                string[] s = line.Split(' ').ToArray();
                r.name = s[0].TrimEnd(':');
                r.capacity = int.Parse(s[2].TrimEnd(','));
                r.durability = int.Parse(s[4].TrimEnd(','));
                r.flavor = int.Parse(s[6].TrimEnd(','));
                r.texture = int.Parse(s[8].TrimEnd(','));
                r.calories = int.Parse(s[10].TrimEnd(','));
                list.Add(r);
            }
            return list;
        }

        static int CalculateScore(List<Ingredient> ingredients, List<int> teaspoons)
        {
            int c = 0, d = 0, f = 0, t = 0;
            for (int i = 0; i < ingredients.Count; i++)
            {
                c += ingredients[i].capacity * teaspoons[i];
                d += ingredients[i].durability * teaspoons[i];
                f += ingredients[i].flavor * teaspoons[i];
                t += ingredients[i].texture * teaspoons[i];
            }
            return Math.Max(c, 0) * Math.Max(d, 0) * Math.Max(f, 0) * Math.Max(t, 0);
        }

        static int CalculateCalories(List<Ingredient> ingredients, List<int> teaspoons)
        {
            int c = 0;
            for (int i = 0; i < ingredients.Count; i++)
                c += ingredients[i].calories * teaspoons[i];
            return c;
        }

        static List<List<int>> GenerateFixSumCombinations(int sum, int components)
        {
            List<List<int>> result = new List<List<int>>();
            void Generate(int[] head, int index)
            {
                int headSum = 0;
                for (int i = 0; i < index; i++)
                    headSum += head[i];
                if (index < components - 1)
                {
                    for (int i = 0; i <= sum - headSum; i++)
                    {
                        head[index] = i;
                        Generate(head, index + 1);
                    }
                }
                else
                {
                    head[index] = sum - headSum;
                    result.Add(new List<int>(head));
                }
            }
            int[] combos = new int[components];
            Generate(combos, 0);
            return result;
        }

        static void PartAB()
        {
            List<Ingredient> ingredients = ReadInput();
            int aMax = 0;
            int bMax = 0;
            var combos = GenerateFixSumCombinations(100, ingredients.Count);
            foreach (List<int> list in combos)
            {
                int score = CalculateScore(ingredients, list);
                aMax = Math.Max(aMax, score);
                if (CalculateCalories(ingredients, list) == 500)
                    bMax = Math.Max(bMax, score);
            }
            Console.WriteLine("Part A: Result is {0}.", aMax);
            Console.WriteLine("Part B: Result is {0}.", bMax);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day15).Namespace + ":");
            PartAB();
        }
    }
}
