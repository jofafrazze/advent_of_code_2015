using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day23
{
    class Day23
    {
        struct OpCode
        {
            public string name;
            public Action<int, int> action;
            public OpCode(string n, Action<int, int> a) { name = n; action = a; }
        };

        static List<uint> registers = new List<uint>() { 0, 0 };
        static int pc = 0;

        static readonly List<OpCode> opCodes = new List<OpCode>()
        {
            new OpCode("hlf", delegate(int r, int p) { registers[r] /= 2; }),
            new OpCode("tpl", delegate(int r, int p) { registers[r] *= 3; }),
            new OpCode("inc", delegate(int r, int p) { registers[r]++; }),
            new OpCode("jmp", delegate(int r, int p) { pc += (p - 1); }),
            new OpCode("jie", delegate(int r, int p) { if ((registers[r] % 2) == 0) pc += (p - 1); }),
            new OpCode("jio", delegate(int r, int p) { if (registers[r] == 1) pc += (p - 1); }),
        };
        static readonly Dictionary<string, int> instructionSet = opCodes.Select((x, i) => new {x, i}).ToDictionary(a => a.x.name, a => a.i);

        struct Instruction
        {
            public OpCode opCode;
            public int register;
            public int parameter;
            public void Execute()
            {
                opCode.action(register, parameter);
            }
        };

        static List<Instruction> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Instruction> list = new List<Instruction>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Instruction i = new Instruction();
                string[] s = line.Split(' ').ToArray();
                i.opCode = opCodes[instructionSet[s[0]]];
                i.register = (s[1][0] - 'a');
                if (s[0] == "jmp") { i.parameter = int.Parse(s[1]); }
                else if (s[0] == "jie") { i.parameter = int.Parse(s[2]); }
                else if (s[0] == "jio") { i.parameter = int.Parse(s[2]); }
                list.Add(i);
            }
            return list;
        }

        static void RunProgram(List<Instruction> program, uint a)
        {
            pc = 0;
            registers[0] = a;
            registers[1] = 0;
            while ((pc >= 0) && (pc < program.Count))
            {
                program[pc].Execute();
                pc++;
            }
        }

        static void PartAB()
        {
            List<Instruction> program = ReadInput();
            RunProgram(program, 0);
            Console.WriteLine("Part A: Result is {0}.", registers[1]);
            RunProgram(program, 1);
            Console.WriteLine("Part B: Result is {0}.", registers[1]);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2015 - " + typeof(Day23).Namespace + ":");
            PartAB();
        }
    }
}
