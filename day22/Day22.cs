using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day22
{
    class Effect
    {
        public enum Name
        {
            Shield,
            Poison,
            Recharge
        }
        public Name name;
        public int size;
        public int count;
        public bool activated;
        public Effect(Effect e) : this(e.name, e.size, e.count, e.activated) { }
        public Effect(Name n, int s, int c, bool a = false)
        {
            name = n;
            size = s;
            count = c;
            activated = a;
        }
    }

    class Spell
    {
        public string name;
        public int mana;
        public int damage;
        public int healing;
        public Effect effect;
        public Spell() { }
        public Spell(Spell s)
        {
            name = s.name;
            mana = s.mana;
            damage = s.damage;
            healing = s.healing;
            effect = (s.effect == null) ? null : new Effect(s.effect);
        }
    }

    class Player
    {
        public bool won;
        public int hitPoints;
        public int damage;
        public int armor;
        public int mana;
        public int manaRecharged;
        public int TotalMana { get { return mana + manaRecharged; } }
        public Player() { }
        public Player(Player p)
        {
            won = p.won;
            hitPoints = p.hitPoints;
            damage = p.damage;
            armor = p.armor;
            mana = p.mana;
            manaRecharged = p.manaRecharged;
        }
    }

    class Combatants
    {
        public Player player;
        public Player boss;
        public List<Spell> spells;
        public Combatants(Player p, Player b, List<Spell> s)
        {
            player = new Player(p);
            boss = new Player(b);
            spells = s.ConvertAll(x => new Spell(x));
        }
        public Combatants(Combatants c) : this(c.player, c.boss, c.spells) { }
    }

    class Day22
    {
        static readonly List<Spell> spells = new List<Spell>()
        {
            new Spell() { name = "Magic Missile", mana = 53, damage = 4 },
            new Spell() { name = "Drain", mana = 73, damage = 2, healing = 2 },
            new Spell() { name = "Shield", mana = 113, effect = new Effect(Effect.Name.Shield, 7, 6) },
            new Spell() { name = "Poison", mana = 173, effect = new Effect(Effect.Name.Poison, 3, 6) },
            new Spell() { name = "Recharge", mana = 229, effect = new Effect(Effect.Name.Recharge, 101, 5) },
        };

        static void PrintPlayers(Combatants c, bool playersTurn)
        {
            Console.WriteLine();
            Console.WriteLine("-- {0} turn --", playersTurn ? "Player" : "Boss");
            Console.WriteLine("- Player has {0} hit points, {1} armor, {2} mana", 
                c.player.hitPoints, c.player.armor, c.player.TotalMana);
            Console.WriteLine("- Boss has {0} hit points", c.boss.hitPoints);
        }

        static bool PlayRound(Combatants c, Spell spell, bool easy, bool verbose = false)
        {
            bool BuySpell()
            {
                List<Spell> alreadyActive = c.spells.Where(x => x.name == spell.name).ToList();
                if ((alreadyActive.Count == 0) || (alreadyActive[0].effect.count == 1))
                {
                    c.spells.Add(new Spell(spell));
                    if ((spell.effect != null) && (spell.effect.name == Effect.Name.Shield))
                        c.player.armor += spell.effect.size;
                }
                else
                    c.boss.won = true;
                c.player.mana -= spell.mana;
                if (c.player.TotalMana < 0)
                    c.boss.won = true;
                if (verbose)
                {
                    if (c.boss.won)
                        Console.WriteLine("Boss wins! (Player can't buy spell)");
                    else
                        Console.WriteLine("Player casts {0}", spell.name);
                }
                return !c.boss.won;
            }
            void ApplyEffects()
            {
                for (int i = 0; i < c.spells.Count; i++)
                {
                    Spell s = c.spells[i];
                    if (s.effect != null)
                    {
                        Effect e = s.effect;
                        if (!e.activated)
                        {
                            e.activated = true;
                        }
                        else
                        {
                            if (e.name == Effect.Name.Poison)
                            {
                                c.boss.hitPoints -= e.size;
                            }
                            else if (e.name == Effect.Name.Recharge)
                            {
                                c.player.manaRecharged += e.size;
                            }
                            e.count--;
                            if (verbose)
                                Console.WriteLine("{0} in action, its counter is now {1}", 
                                    s.name, e.count);
                            if (e.count == 0)
                            {
                                if (e.name == Effect.Name.Shield)
                                    c.player.armor -= e.size;
                                c.spells[i] = null;
                            }
                        }
                    }
                    else
                    {
                        c.boss.hitPoints -= s.damage;
                        c.player.hitPoints += s.healing;
                        c.spells[i] = null;
                    }
                }
                c.spells = c.spells.Where(x => x != null).ToList();
                if (c.boss.hitPoints <= 0)
                {
                    c.player.won = true;
                    if (verbose)
                        Console.WriteLine("Player wins!");
                }
            }
            if (verbose)
                PrintPlayers(c, true);
            if (!easy)
            {
                c.player.hitPoints -= 1;
                if (c.player.hitPoints <= 0)
                    c.boss.won = true;
            }
            if (!c.boss.won && BuySpell())
            {
                ApplyEffects();
                if (!c.player.won)
                {
                    if (verbose)
                        PrintPlayers(c, false);
                    ApplyEffects();
                    if (!c.player.won)
                    {
                        int damage = Math.Max(1, c.boss.damage - c.player.armor);
                        c.player.hitPoints -= damage;
                        if (verbose)
                            Console.WriteLine("Boss attacks for {0} damage", damage);
                        if (c.player.hitPoints <= 0)
                        {
                            c.boss.won = true;
                            if (verbose)
                                Console.WriteLine("Boss wins!");
                        }
                    }
                }
            }
            return !c.player.won && !c.boss.won;
        }

        static void Example1()
        {
            Player boss = new Player() { hitPoints = 13, damage = 8, armor = 0 };
            Player me = new Player() { hitPoints = 10, mana = 250 };
            Combatants round = new Combatants(me, boss, new List<Spell>());
            PlayRound(round, spells[3], true, true);
            PlayRound(round, spells[0], true);
        }

        static void Example2()
        {
            Player boss = new Player() { hitPoints = 14, damage = 8, armor = 0 };
            Player me = new Player() { hitPoints = 10, mana = 250 };
            Combatants round = new Combatants(me, boss, new List<Spell>());
            PlayRound(round, spells[4], true, true);
            PlayRound(round, spells[2], true);
            PlayRound(round, spells[1], true);
            PlayRound(round, spells[3], true);
            PlayRound(round, spells[0], true);
        }

        static int PlayGame(bool easyMode)
        {
            Player boss = new Player() { hitPoints = 58, damage = 9, armor = 0 };
            Player me = new Player() { hitPoints = 50, mana = 500 };
            List<Combatants> rounds = new List<Combatants>() { new Combatants(me, boss, new List<Spell>()) };
            int nEndedRounds = 0;
            int playerWonCount = 0;
            int playerWonMaxMana = int.MinValue;
            while ((rounds.Count > 0) && (playerWonCount < 50000000))
            {
                Console.WriteLine("Ongoing: {0}, Ended: {1}, Player won: {2}, max mana left: {3}",
                    rounds.Count, nEndedRounds, playerWonCount, playerWonMaxMana);
                List<Combatants> nextRounds = new List<Combatants>();
                foreach (var round in rounds)
                {
                    foreach (Spell s in spells)
                    {
                        Combatants nextRound = new Combatants(round);
                        if (PlayRound(nextRound, s, easyMode))
                        {
                            if ((playerWonMaxMana < 0) || (nextRound.player.mana > playerWonMaxMana))
                                nextRounds.Add(nextRound);
                        }
                        else
                        {
                            nEndedRounds++;
                            if (nextRound.player.won)
                            {
                                playerWonCount++;
                                if (nextRound.player.mana > playerWonMaxMana)
                                    playerWonMaxMana = nextRound.player.mana;
                            }
                        }
                    }
                }
                nextRounds = nextRounds.Where(x => x.player.mana > playerWonMaxMana).ToList();
                rounds = nextRounds;
            }
            return 500 - playerWonMaxMana;
        }

        static void PartAB()
        {
            int a = PlayGame(true);
            Console.WriteLine("Part A: Result is {0}.", a);
            int b = PlayGame(false);
            Console.WriteLine("Part A: Result is {0}.", b);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day22).Namespace + ":");
            //Example1();
            //Example2();
            PartAB();
        }
    }
}
