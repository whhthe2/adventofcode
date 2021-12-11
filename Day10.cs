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
        public char _error;
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
            _error = '\0';
        }

        //returns true if valid syntax was encountered 
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
                if (pairedChars[_opener] == c)
                {
                    _closer = c;
                    return true;
                }
                else
                {
                    if (content != null)
                    {
                        return content.Parse(c);
                    }
                    else
                    {
                        _error = c;
                        throw new Exception($"reached end of nested content. looking for {_opener} but found {c}");
                    }
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
                Chunk rootChunk = null;
                bool ok = true;

                foreach( char c in line )
                {
                    
                    if (rootChunk == null)
                    {
                        rootChunk = new Chunk(c);
                    }
                    else
                    {
                        try
                        {
                            rootChunk.Parse(c);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"error: {e}");
                            break;
                        }
                    }         
                }
                Console.WriteLine($"Finished parsing: {rootChunk}");
            }

            //Console.WriteLine(input);
        }
    }
}
