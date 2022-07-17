using System;
using System.Collections.Generic;
using System.Text;

namespace LiteNinja.TimeUtils.Durations
{
    internal enum TokenType
    {
        Number,
        Unit,
        None
    }
    
    internal struct Token
    {
        public TokenType Type;
        public string Value;
    }
    
    internal class Tokenizer
    {
        private int _position;
        private readonly string _input;

        public Tokenizer(string input)
        {
            _input = Clean(input);
            _position = 0;
            Tokens = new List<Token>();
        }
        
        public List<Token> Tokens { get; }
        
        public List<Token> CleanTokens => Tokens.FindAll(t => t.Type != TokenType.None);
        
        public bool HasNonToken => Tokens.FindAll(t => t.Type == TokenType.None).Count > 0;

        public void Tokenize()
        {
            while (_position < _input.Length)
            {
                var token = ReadToken();
                Tokens.Add(token);
            }
        }

        private Token ReadToken()
        {
            var token = new Token
            {
                Type = TokenType.None,
                Value = string.Empty
            };
            var c = _input[_position];
            if (IsNumber(c))
            {
                token.Type = TokenType.Number;
                token.Value = ReadNumber();
            }
            else if (char.IsLetter(c))
            {
                token.Type = TokenType.Unit;
                token.Value = ReadUnit();
            }
            else
            {
                _position++;
            }
            
            return token;
        }
        
        private static bool IsNumber(char c)
        {
            return char.IsDigit(c) || c == '.';
        }
        private string ReadNumber()
        {
            return Read(IsNumber);
        }
        
        private string ReadUnit()
        {
            return Read(char.IsLetter);
        }
        
        private string Read(Func<char, bool> predicate)
        {
            var sb = new StringBuilder();
            while (_position < _input.Length)
            {
                var c = _input[_position];
                if (!predicate(c))
                {
                    break;
                }
                sb.Append(c);
                _position++;
            }
            return sb.ToString();
        }

        public void Reset()
        {
            _position = 0;
            Tokens.Clear();
        }
        
        private static string Clean(string input)
        {
            return input.Trim().ToLower().Replace(" ", "");
        }

    }
    
    
}