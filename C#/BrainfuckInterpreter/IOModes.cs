using System;
using System.Collections.Generic;
using System.Text;

namespace BrainfuckInterpreter
{
    public static class IOModes
    {
        private static readonly Dictionary<string, Func<int>> InputModes = new Dictionary<string, Func<int>>()
        {
            { "number", NumberInput },
            { "char", CharInput }
        };
        
        private static readonly Dictionary<string, Func<int, string>> OutputModes = new Dictionary<string, Func<int, string>>()
        {
            { "number", NumberOutput },
            { "char", CharOutput }
        };

        public static Func<int> GetInputMode(string name)
        {
            return InputModes.ContainsKey(name)
                ? InputModes[name]
                : CharInput;
        }
        
        public static Func<int, string> GetOutputMode(string name)
        {
            return OutputModes.ContainsKey(name)
                ? OutputModes[name]
                : CharOutput;
        }
        
        public static int NumberInput()
        {
            string input;
            int num = -1;
            bool gotInt = false;

            while (!gotInt)
            {
                input = Console.ReadLine();
                try
                {
                    num = Int32.Parse(input);
                    gotInt = true;
                }
                catch (Exception e)
                {
                    
                }
            }

            return num;
        }

        public static int CharInput()
        {
            int num = -1;

            while (num == -1)
            {
                num = Console.Read();
            }

            return num;
        }

        public static string NumberOutput(int val)
        {
            return val.ToString();
        }

        public static string CharOutput(int val)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] intBytes = BitConverter.GetBytes(val);
            return utf8.GetString(intBytes);
        }
    }
}