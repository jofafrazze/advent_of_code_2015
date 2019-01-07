using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day07
{
    enum LogicOp
    {
        And,    // 2 in
        Or,     // 2 in
        Andi,   // 1 in
        Not,    // 1 in
        Lshift, // 1 in
        Rshift, // 1 in
        Bypass, // 1 in
        Set,    // 0 in
    }

    class Gate
    {
        public string id;
        public string in1;
        public string in2;
        public LogicOp operation;
        public int immediate;
    }

    class Day07
    {
        static List<Gate> ReadInput()
        {
            List<Gate> list = new List<Gate>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Regex regexSetPass = new Regex(@"^(\d+|[a-z]+) -> ([a-z]+)");
            Regex regexAndOr = new Regex(@"(\d+|[a-z]+) (AND|OR) ([a-z]+) -> ([a-z]+)");
            Regex regexShift = new Regex(@"([a-z]+) (L|R)SHIFT (\d+) -> ([a-z]+)");
            Regex regexNot = new Regex(@"^NOT ([a-z]+) -> ([a-z]+)");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Gate gate = new Gate();
                MatchCollection matches = regexSetPass.Matches(line);
                GroupCollection groups;
                int i = 1;
                if (matches.Count > 0)
                {
                    groups = matches[0].Groups;
                    string s1 = groups[i++].Value;
                    if (int.TryParse(s1, out gate.immediate))
                    {
                        gate.operation = LogicOp.Set;
                    }
                    else
                    {
                        gate.operation = LogicOp.Bypass;
                        gate.in1 = s1;
                    }
                }
                else if ((matches = regexAndOr.Matches(line)).Count > 0)
                {
                    groups = matches[0].Groups;
                    string s1 = groups[i++].Value;
                    string s2 = groups[i++].Value;
                    if (int.TryParse(s1, out gate.immediate))
                    {
                        gate.operation = LogicOp.Andi;
                    }
                    else
                    {
                        gate.in2 = s1;
                        gate.operation = (s2 == "AND") ? LogicOp.And : LogicOp.Or;
                    }
                    gate.in1 = groups[i++].Value;
                }
                else if ((matches = regexShift.Matches(line)).Count > 0)
                {
                    groups = matches[0].Groups;
                    gate.in1 = groups[i++].Value;
                    gate.operation = (groups[i++].Value == "L") ? LogicOp.Lshift : LogicOp.Rshift;
                    gate.immediate = int.Parse(groups[i++].Value);
                }
                else if ((matches = regexNot.Matches(line)).Count > 0)
                {
                    groups = matches[0].Groups;
                    gate.in1 = groups[i++].Value;
                    gate.operation = LogicOp.Not;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
                gate.id = groups[i++].Value;
                list.Add(gate);
            }
            return list;
        }

        static Dictionary<string, int> ExecuteGates(List<Gate> gatesIn)
        {
            List<Gate> gates = new List<Gate>(gatesIn);
            Dictionary<string, int> states = new Dictionary<string, int>();
            int oldGates;
            do
            {
                for (int i = 0; i < gates.Count; i++)
                {
                    Gate g = gates[i];
                    bool b1 = (g.in1 == null) ? false : states.ContainsKey(g.in1);
                    bool b2 = (g.in2 == null) ? false : states.ContainsKey(g.in2);
                    int in1 = b1 ? states[g.in1] : 0;
                    int in2 = b2 ? states[g.in2] : 0;
                    if (g.operation == LogicOp.Set)
                    {
                        states[g.id] = g.immediate;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Andi) && b1)
                    {
                        states[g.id] = in1 & g.immediate;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Not) && b1)
                    {
                        states[g.id] = 65535 ^ in1;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Lshift) && b1)
                    {
                        states[g.id] = in1 << g.immediate;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Rshift) && b1)
                    {
                        states[g.id] = in1 >> g.immediate;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Bypass) && b1)
                    {
                        states[g.id] = in1;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.And) && b1 && b2)
                    {
                        states[g.id] = in1 & in2;
                        gates[i] = null;
                    }
                    else if ((g.operation == LogicOp.Or) && b1 && b2)
                    {
                        states[g.id] = in1 | in2;
                        gates[i] = null;
                    }
                }
                oldGates = gates.Count;
                gates = gates.Where(x => x != null).ToList();
            }
            while ((gates.Count > 0)); // && (gates.Count != oldGates));
            return states;
        }

        static void PartA()
        {
            List<Gate> gates = ReadInput();
            Dictionary<string, int> states = ExecuteGates(gates);
            int value = states["a"];
            Console.WriteLine("Part A: Result is {0}.", value);
        }

        static void PartB()
        {
            List<Gate> gates = ReadInput();
            Dictionary<string, int> states = ExecuteGates(gates);
            int value = states["a"];
            foreach (Gate g in gates)
            {
                if ((g.operation == LogicOp.Set) && (g.id == "b"))
                    g.immediate = value;
            }
            Dictionary<string, int> states2 = ExecuteGates(gates);
            int value2 = states2["a"];
            Console.WriteLine("Part B: Result is {0}.", value2);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day07).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
