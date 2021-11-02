using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrainfuckInterpreter
{
    internal class Program
    {
        private static readonly HashSet<string> Options = new HashSet<string>()
        {
            "--inputMode", "--outputMode"
        };

        public static void Main(string[] args)
        {
            // Populate argument flags
            Dictionary<string, string> flags = new Dictionary<string, string>();
            List<string> otherArgs = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string currentArg = args[i];
                if (Options.Contains(currentArg))
                {
                    flags[currentArg] = args[i + 1];
                    i++;
                }
                else
                {
                    otherArgs.Add(currentArg);
                }
            }

            // Create interpreter options
            InterpreterOptions interpreterOptions = new InterpreterOptions();

            if (flags.ContainsKey("--inputMode"))
            {
                interpreterOptions.SetInputMode(flags["--inputMode"]);
            }
            
            if (flags.ContainsKey("--outputMode"))
            {
                interpreterOptions.SetOutputMode(flags["--outputMode"]);
            }

            // Exit early if we don't have a code path
            if (otherArgs.Count == 0)
            {
                Console.WriteLine("No path to bf code included. Please add the path to the bf code after the command.");
                return;
            }

            string codePath = otherArgs[0]; 

            // Exit early if the code path doesn't exist
            if (!File.Exists(codePath))
            {
                Console.WriteLine("No path to bf code included. Please add the path to the bf code after the command.");
                return;
            }

            string rawCode = File.ReadAllText(codePath);
            List<string> code = rawCode
                .ToCharArray()
                .Select(c => c.ToString())
                .Where((str) => Interpreter.Codes.Contains(str))
                .ToList();

            Interpreter interpreter = new Interpreter(interpreterOptions);
            interpreter.SetCode(code);
            interpreter.Run();
        }
    }
}