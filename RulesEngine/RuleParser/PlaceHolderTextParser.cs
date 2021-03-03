using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.RuleParser
{

    public class PlaceHolderTextParser
    {
        
        private readonly string _expression;
        private int _textPos;
        private char _ch;
        private char _escapeChar = '\0';
        private readonly int _textLen;

        public int startplaceHolderpos = 0;
        public int endPlaceHolderpos = 0;

        public PlaceHolderTextParser(string expression)
        {
            this._expression = expression;
            _textLen = expression.Length;
            SetTextPos(0);
        }

        
        public bool NextPlaceHolder(out string placeHolder, out string placeHolderWithParenthesis)
        {
            placeHolder = string.Empty;
            placeHolderWithParenthesis = string.Empty;

            while (char.IsWhiteSpace(_ch))
            {
                NextChar();
            }
            bool startPlaceHolder = false;
            Stack<char> parenthesis=new Stack<char>();
            Queue<char> qplaceHolder=new Queue<char>();

            startplaceHolderpos = 0;
            endPlaceHolderpos = 0;
            while(HasPlaceholder())
            {
               
                if(_ch == '{')
                {
                    if(!startPlaceHolder)
                    {
                        startPlaceHolder = true;
                        startplaceHolderpos = _textPos;
                    }
                    else
                    {
                        qplaceHolder.Enqueue(_ch);
                    }
                    parenthesis.Push(_ch);
                }
                else if(_ch == '}')
                {
                    char exitItem = parenthesis.Pop();
                    if (exitItem != '{')
                        throw new Exception(string.Format("Rule Expression-{0} doesnt have matching placeholder Item", _expression));
                    if(parenthesis.Count ==0)
                    {
                        placeHolder = new string(qplaceHolder.ToArray());
                        placeHolderWithParenthesis = string.Format("{0}{1}{2}", "{", placeHolder, "}");
                        endPlaceHolderpos = _textPos;
                        NextChar();
                        return true;
                    }
                    else
                    {
                        if (startPlaceHolder)
                            qplaceHolder.Enqueue(_ch);
                    }
                }
                else
                {
                    if(startPlaceHolder)
                        qplaceHolder.Enqueue(_ch);
                }

                NextChar();
            }


            return false;
        }

        public bool HasPlaceholder()
        {
            return _ch != _escapeChar;
        }

        private void SetTextPos(int pos)
        {
            _textPos = pos;
            _ch = _textPos < _textLen ? _expression[_textPos] : _escapeChar;
        }

        private void NextChar()
        {
            if (_textPos < _textLen)
            {
                _textPos++;
            }
            _ch = _textPos < _textLen ? _expression[_textPos] : _escapeChar;
        }



    }
}
