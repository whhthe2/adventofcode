using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal static class Day10
    {
        public static int totalPoints = 0;
        public static void Solve(string input)
        {
            var inputLines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var line in inputLines)
            {   
                Chunk rootChunk = null;
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
                            var status = rootChunk.Parse(c);
                            if (!status)
                            {
                                throw new Exception($"parsing {c} failed silently");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"error parsing: {rootChunk}\n\t{e}");
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"total syntax error score: {totalPoints}");
        }
    }

    internal class Chunk
    {
        private static Dictionary<char,char> pairedChars = new Dictionary<char,char>
        {
            {'(', ')'},
            {'[', ']'},
            {'{', '}'},
            {'<', '>'}
        };

        private static Dictionary<char,int> pointValues = new Dictionary<char, int>
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137}
        };
        public static bool IsOpener (char c) => pairedChars.Keys.Contains(c);
        public static bool IsCloser (char c) => pairedChars.Values.Contains(c);
        
        public char opener;
        public char closer;
        public bool IsValid => opener != '\0' && closer != '\0';

        public Stack<Chunk> children;

        public Chunk(char c)
        {
            opener = c;
            closer = '\0';
            children = new Stack<Chunk>();
        }

        public bool Parse(char c)
        {
            if (IsOpener(c))
            {
                children.Push(new Chunk(c));
                return true;
            }
            if (IsCloser(c))
            {
                Chunk child;
                char match;
                if (children.TryPop(out child))
                {
                    match = pairedChars[child.opener];
                }
                else
                {
                    match = pairedChars[opener];
                }

                if (match == c)
                {
                    return true;
                }
                else
                {
                    Day10.totalPoints += pointValues[c];
                    throw new Exception($"Expected {match}, but found {c} instead. {pointValues[c]} points!");
                }
                
            }
            throw new Exception($"Token {c} is not an opening or closing token.");
        }
    }
}
