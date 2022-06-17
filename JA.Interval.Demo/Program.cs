using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace JA
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            var list = new Dictionary<string, Interval>()
            {
                ["all"] = Interval.All,
                ["negative"] = Interval.Negative,
                ["positive"] = Interval.Positive,
                ["negative or zero"] = Interval.NegativeOrZero,
                ["positive or zero"] = Interval.PositiveOrZero,
                ["zero"] = Interval.Zero,
                ["finite"] = Interval.DoubleDomain,
                ["nothing"] = Interval.Nothing,
                ["invalid"] = Interval.Invalid,
                ["one"] = Interval.Value(1),
                ["closed"] = Interval.ClosedRange(-1, 1),
                ["open"] = Interval.OpenRange(-1, 1),
                ["upto"] = Interval.NegativeInfinityTo(-1),
                ["from"] = Interval.ToPositiveInfinity(1),
                ["close-open"] = Interval.CloseOpen(-1, 1),
                ["open-close"] = Interval.OpenClosed(-1, 1),
            };
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Ranges");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{"Interval",-18} {"",22} | {"Contains()",-20} | {"Is()"}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{"name",18} {"value",22} | {"0",6} {"-1",6} {"1",6} | ");
            Console.Write($"{"single",6} ");
            Console.Write($"{"range",6} ");
            Console.Write($"{"all",6} ");
            Console.Write($"{"pos",6} ");
            Console.Write($"{"neg",6} ");
            Console.Write($"{"pos|0",6} ");
            Console.Write($"{"neg|0",6} ");
            Console.Write($"{"closed",6} ");
            Console.Write($"{"open",6} ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(new string('-', 22+18+2));
            Console.Write('+');
            Console.Write(new string('-', 20+2));
            Console.Write('+');
            Console.Write(new string('-', 7*9));
            Console.WriteLine();
            foreach (var kvp in list)
            {
                Interval ival = kvp.Value;
                Console.Write($"{kvp.Key,18} {ival.ToString("g3"),22} | {ival.Contains(0),6} {ival.Contains(-1),6} {ival.Contains(1),6} | ");
                Console.Write($"{ival.IsSingular,6} ");
                Console.Write($"{ival.IsRange,6} ");
                Console.Write($"{ival.IsAll,6} ");
                Console.Write($"{ival.IsPositive,6} ");
                Console.Write($"{ival.IsNegative,6} ");
                Console.Write($"{ival.IsPositiveOrZero,6} ");
                Console.Write($"{ival.IsNegativeOrZero,6} ");
                Console.Write($"{ival.IsClosed,6} ");
                Console.Write($"{ival.IsOpen,6} ");
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            var test = new[] {
                Interval.All,
                Interval.Negative,
                Interval.Positive,
                Interval.Value(1),
                Interval.ClosedRange(-1, 1),
                Interval.OpenRange(-1, 1),
                Interval.NegativeInfinityTo(-1),
                Interval.ToPositiveInfinity(1),
             };
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Test Combinations");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{"Intersect",-12}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('|');
            foreach (var b in test)
            {
                Console.Write($"{b,8} ");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(new string('-', 12));
            Console.Write('+');
            Console.Write(new string('-', test.Length*9));
            Console.WriteLine();
            foreach (var a in test)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{a,-12}|");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var b in test)
                {
                    Interval c = a & b;
                    Console.Write($"{c,8} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{"Union",-12}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('|');
            foreach (var b in test)
            {
                Console.Write($"{b,8} ");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(new string('-', 12));
            Console.Write('+');
            Console.Write(new string('-', test.Length*9));
            Console.WriteLine();
            foreach (var a in test)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{a,-12}|");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var b in test)
                {
                    Interval c = a | b;
                    Console.Write($"{c,8} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            var span = Interval.ClosedRange(0, 100);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"divide span = {span} into ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"5 segments");
            Console.ForegroundColor = ConsoleColor.Gray;
            var div = span.Divide(5);
            int index = 0;
            foreach (var sub in div)
            {
                Console.WriteLine($"segment({++index}) = {sub}");
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            var split = new double[] { 30, 45, 75 };
            Console.Write($"split span = {span} into ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Show(split));
            Console.ForegroundColor = ConsoleColor.Gray;
            var parts = span.Split(split);
            index = 0;
            foreach (var sub in parts)
            {
                Console.WriteLine($"segment({++index}) = {sub}");
            }
        }
        public static string Show<T>(params T[] array)
            => new DisplayArray<T>(array, DisplayArray<T>.DefaultFormat);
        public static string Show<T>(T[] array, string format)
            => new DisplayArray<T>(array, format);
        
    }
}
