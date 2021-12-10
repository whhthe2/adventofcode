using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal class Chunk
    {
        private char _opener;
        private char _closer;
        private Chunk content;
        private string validChars = "()[]{}<>";
        private static Dictionary<char,char> pairedChars = new Dictionary<char,char>
        {
            {'(', ')'},
            {'[', ']'},
            {'{', '}'},
            {'<', '>'}
        };
        
        public static bool IsOpener (char c) => pairedChars.Keys.Contains(c);
        public static bool IsCloser (char c) => pairedChars.Values.Contains(c);
        
        public bool IsValid => _opener != '\0' && _closer != '\0';
        
        public Chunk(char opener)
        {
            _opener = opener;
            _closer = '\0';
        }

        //returns true if there is more to 
        public bool Parse(char c)
        {
            validate(c);

            if (IsOpener(c))
            {
                if (content == null)
                {
                    content = new Chunk(c);
                    return true;
                }
                else
                {
                    return content.Parse(c);
                }
            }
            else if (IsCloser(c))
            {
                if (Close(c))
                {
                    return true;
                }
                else
                {
                    return content.Parse(c);
                }
            }
            else
            {
                throw new Exception($"encountered invalid character even though it was previously validated...");
            }
        }
        private void validate(char c) 
        {
            if (!validChars.Contains(c))
            {
                throw new Exception($"invalid char {c}");
            }
        }

        public bool Close(char c)
        {
            validate(c);
            
            if (pairedChars[_opener] == c)
            {
                _closer = c;
                return true;
            }
            else
            {
                
                Console.WriteLine($"Expected {pairedChars[_opener]}, but found {c} instead.");
                return false;
            }

        }

        public override string ToString()
        {
            return $"{_opener}{content}{_closer}";
        }
    }

    internal static class Day10
    {
        
        public static void Solve(string input)
        {
            var inputLines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var line in inputLines)
            {
                var syntaxStack = new Stack<char>();
                
                Chunk rootChunk = null;

                foreach( char c in line )
                {
                    bool ok = true;
                    if (rootChunk == null)
                    {
                        rootChunk = new Chunk(c);
                    }
                    else
                    {
                        ok = rootChunk.Parse(c);
                    }

                    if (!ok)
                    {
                        //Console.WriteLine($"{rootChunk}");
                    }
                    else
                    {
                        Console.WriteLine($"{rootChunk}");
                    }                 
                }
            }

            //Console.WriteLine(input);
        }
    }
}
