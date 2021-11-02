using System;
using System.Collections.Generic;

namespace BrainfuckInterpreter
{
    public class Interpreter
    {
        public static readonly HashSet<string> Codes = new HashSet<string>()
        {
            ">", "<", "+", "-", ".", ",", "[", "]"
        };

        private Func<int> _inputFn;
        private Func<int, string> _outputFn;
        private Dictionary<string, Action> _actionsBySymbol;
        private int _codeIndex = 0;
        private int _dataIndex = 0;
        private Dictionary<int, int> _data = new Dictionary<int, int>();
        private Dictionary<int, int> _jumpMap = new Dictionary<int, int>();
        private List<string> _code = new List<string>();

        private int CurrentData {
            get => _data.ContainsKey(_dataIndex)
                ? _data[_dataIndex]
                : 0;
            set => _data[_dataIndex] = value;
        }
        
        public Interpreter(InterpreterOptions options)
        {
            _actionsBySymbol = new Dictionary<string, Action>()
            {
                { ">", TapeForward },
                { "<", TapeBackward },
                { "+", Increment },
                { "-", Decrement },
                { ".", Output },
                { ",", Input },
                { "[", JumpForwardIfZero },
                { "]", JumpBackwardIfNotZero },
            };

            _inputFn = options.InputFn;
            _outputFn = options.OutputFn;
        }

        public void SetCode(List<string> code)
        {
            _code = code;
            _jumpMap = CreateJumpMap(_code);
        }

        private Dictionary<int, int> CreateJumpMap(List<string> code)
        {
            Stack<int> stack = new Stack<int>();
            Dictionary<int, int> jumpMap = new Dictionary<int, int>();

            for (int i=0; i<code.Count; i++)
            {
                if ("[".Equals(code[i]))
                {
                    stack.Push(i);
                }
                else if ("]".Equals(code[i]))
                {
                    int startIndex = stack.Pop();
                    int endIndex = i;
                    jumpMap[startIndex] = endIndex + 1;
                    jumpMap[endIndex] = startIndex + 1;
                }
            }

            return jumpMap;
        }

        public void Reset()
        {
            _codeIndex = 0;
            _dataIndex = 0;
            _data = new Dictionary<int, int>();
        }

        public void Run()
        {
            while (_codeIndex < _code.Count)
            {
                string charCode = _code[_codeIndex];
                DoAction(charCode);
            }
        }

        private void DoAction(string charCode)
        {
            if (_actionsBySymbol.ContainsKey(charCode))
            {
                _actionsBySymbol[charCode]();
            }
        }

        private void TapeForward()
        {
            _dataIndex++;
            _codeIndex++;
        }

        private void TapeBackward()
        {
            _dataIndex--;
            _codeIndex++;
        }

        private void Increment()
        {
            CurrentData++;
            _codeIndex++;
        }
        
        private void Decrement()
        {
            CurrentData--;
            _codeIndex++;
        }

        private void Output()
        {
            string output = _outputFn(CurrentData);
            Console.Write(output);
            _codeIndex++;
        }

        private void Input()
        {
            int input = _inputFn();
            CurrentData = input;
            _codeIndex++;
        }

        private void JumpForwardIfZero()
        {
            if (CurrentData == 0)
            {
                _codeIndex = GetJump();
            }
            else
            {
                _codeIndex++;
            }
        }

        private void JumpBackwardIfNotZero()
        {
            if (CurrentData != 0)
            {
                _codeIndex = GetJump();
            }
            else
            {
                _codeIndex++;
            }
        }

        private int GetJump()
        {
            return _jumpMap.ContainsKey(_codeIndex)
                ? _jumpMap[_codeIndex]
                : _codeIndex + 1;
        }
    }
}