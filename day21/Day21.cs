using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day21
{
    class Item
    {
        public string name;
        public int cost;
        public int damage;
        public int armor;
        public Item(string n, int c, int d, int a)
        {
            name = n;
            cost = c;
            damage = d;
            armor = a;
        }
    }

    struct Player
    {
        public int hitPoints;
        public int damage;
        public int armor;
        public int cost;
    }

    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }

    class Day21
    {
        static readonly List<Item> weapons = new List<Item>()
        {
            new Item("Dagger", 8, 4, 0),
            new Item("Shortsword", 10, 5, 0),
            new Item("Warhammer", 25, 6, 0),
            new Item("Longsword", 40, 7, 0),
            new Item("Greataxe", 74, 8, 0),
        };

        static readonly List<Item> armors = new List<Item>()
        {
            new Item("Leather", 13, 0, 1),
            new Item("Chainmail", 31, 0, 2),
            new Item("Splintmail", 53, 0, 3),
            new Item("Bandedmail", 75, 0, 4),
            new Item("Platemail", 102, 0, 5),
        };

        static readonly List<Item> rings = new List<Item>()
        {
            new Item("Damage1", 25, 1, 0),
            new Item("Damage2", 50, 2, 0),
            new Item("Damage3", 100, 3, 0),
            new Item("Defense1", 20, 0, 1),
            new Item("Defense2", 40, 0, 2),
            new Item("Defense3", 80, 0, 3),
        };

        static void AddItem(ref Player p, Item i)
        {
            p.damage += i.damage;
            p.armor += i.armor;
            p.cost += i.cost;
        }

        static bool PlayBoss(Player p, Player boss)
        {
            void Attack(ref Player attacker, ref Player defender)
            {
                int damage = Math.Max(1, attacker.damage - defender.armor);
                defender.hitPoints -= damage;
            }
            while (p.hitPoints > 0 && boss.hitPoints > 0)
            {
                Attack(ref p, ref boss);
                if (boss.hitPoints > 0)
                    Attack(ref boss, ref p);
            }
            return p.hitPoints > 0;
        }

        static void PartAB()
        {
            Player boss = new Player() { hitPoints = 109, damage = 8, armor = 2 };
            Player me = new Player();
            var ringIndexCombos2 = rings.Select((r, i) => i).ToList().Combinations(2).ToList();
            var ringIndexCombos1 = rings.Select((r, i) => i).ToList().Combinations(1).ToList();
            var ringIndexCombos = ringIndexCombos1.Concat(ringIndexCombos2).ToList();
            //ringIndexCombos.ForEach(list => Console.WriteLine(String.Join(", ", list)));
            int minGoldSpent = int.MaxValue;
            int maxGoldSpent = int.MinValue;
            foreach (Item weapon in weapons)
            {
                me = new Player() { hitPoints = 100 };
                AddItem(ref me, weapon);
                for (int a = 0; a <= armors.Count; a++)
                {
                    Player armedMe = me;
                    if (a < armors.Count)
                        AddItem(ref armedMe, armors[a]);
                    for (int r = 0; r <= ringIndexCombos.Count; r++)
                    {
                        Player finalMe = armedMe;
                        if (r < ringIndexCombos.Count)
                            foreach (int i in ringIndexCombos[r])
                                AddItem(ref finalMe, rings[i]);
                        bool playerWon = PlayBoss(finalMe, boss);
                        if (playerWon && (finalMe.cost < minGoldSpent))
                            minGoldSpent = finalMe.cost;
                        if (!playerWon && (finalMe.cost > maxGoldSpent))
                            maxGoldSpent = finalMe.cost;
                    }
                }
            }
            Console.WriteLine("Part A: Result is {0}.", minGoldSpent);
            Console.WriteLine("Part B: Result is {0}.", maxGoldSpent);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day21).Namespace + ":");
            PartAB();
        }
    }
}
