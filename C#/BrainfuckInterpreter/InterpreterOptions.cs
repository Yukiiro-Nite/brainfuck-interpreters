using System;
using System.Text;

namespace BrainfuckInterpreter
{
    public class InterpreterOptions
    {
        public Func<int> InputFn;
        public Func<int, string> OutputFn;

        public InterpreterOptions()
        {
            this.InputFn = IOModes.CharInput;
            this.OutputFn = IOModes.CharOutput;
        }

        public InterpreterOptions(Func<int> inputFn, Func<int, string> outputFn)
        {
            this.InputFn = inputFn;
            this.OutputFn = outputFn;
        }

        public void SetInputMode(string name)
        {
            InputFn = IOModes.GetInputMode(name);
        }
        
        public void SetOutputMode(string name)
        {
            OutputFn = IOModes.GetOutputMode(name);
        }
    }
}