using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonParser
{
    public class JsonParser
    {
        int position = 0;
        int depth = 0;
        char[] array;

        public void Print(string str)
        {
            array = str.ToCharArray();
            depth = 0;
            position = 0;
            Print();
        }

        void Print()
        {
            ParseObject();
            Console.WriteLine();
        }

        void ParseObject()
        {
            Token token = GetNextToken();
            if (token.TokenType != TokenType.BeginObject)
                return;
            Indent("{\n");
            depth++;
            while (position < array.Length)
            {
                Token value = ParseKeyValuePair();
                if (value.TokenType == TokenType.EndObject)
                {
                    depth--;
                    Indent("}");
                    return;
                }
            }
        }

        Token ParseKeyValuePair()
        {
            // get key
            Token key = GetNextToken();
            if (key.TokenType != TokenType.String)
                return key;

            // get equals
            Token equals = GetNextToken();
            if (equals.TokenType == TokenType.Colon)
            {
                // this is actually an object
                Indent(String.Format("{0}:\n", key.Value));
                ParseObject();
                Token commaToken = GetNextToken();
                if (commaToken.TokenType == TokenType.Comma)
                {
                    Console.WriteLine(",");
                    return commaToken;
                }
                if (commaToken.TokenType == TokenType.EndObject)
                {
                    Console.WriteLine();
                    return new Token() { TokenType = TokenType.EndObject };
                }
            }
            else
            {
                if (equals.TokenType != TokenType.Equals)
                    return equals;
            }

            // get value
            Token value = GetNextToken();
            Indent(String.Format("{0}={1}", key.Value, value.Value));

            // get comma
            Token comma = GetNextToken();
            if (comma.TokenType == TokenType.Comma)
            {
                Console.WriteLine(",");
                return comma;
            }
            if (comma.TokenType == TokenType.EndObject)
            {
                Console.WriteLine();
                return new Token() { TokenType = TokenType.EndObject };
            }
            return comma;
        }

        Token GetNextToken()
        {
            char ch = GetChar();
            switch (ch)
            {
                case '{':
                    return new Token() { TokenType = TokenType.BeginObject };
                case '}':
                    return new Token() { TokenType = TokenType.EndObject };
                case '=':
                    return new Token() { TokenType = TokenType.Equals };
                case ',':
                    return new Token() { TokenType = TokenType.Comma };
                case ':':
                    return new Token() { TokenType = TokenType.Colon };
                default:
                    StringBuilder sb = new StringBuilder();
                    sb.Append(ch);
                    while (position < array.Length &&
                           array[position] != '{' &&
                           array[position] != '}' &&
                           array[position] != '=' &&
                           array[position] != ',' &&
                           array[position] != ':' &&
                           array[position] != ' ' &&
                           array[position] != '\n' &&
                           array[position] != '\t')
                        sb.Append(array[position++]);
                    return new Token() { TokenType = TokenType.String, Value = sb.ToString() };
            }
        }

        void Indent(string str)
        {
            for (int i = 0; i < depth; i++)
                Console.Write("    ");
            Console.Write(str);
        }

        char GetChar()
        {
            char ch = array[position++];
            while (ch == ' ' || ch == '\t' || ch == '\n' && position < array.Length)
                ch = array[position++];
            return ch;
        }
    }


    public enum TokenType
    {
        BeginObject,
        EndObject,  
        Equals,
        Comma,
        Colon,
        String,
    }
   
    class Token
    {
        public TokenType TokenType {get; set;}
        public string Value {get; set;}
    }
}
