using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode2021
{
    internal static class Day10
    {
        public static int totalPoints = 0;
        public static void Solve(string input)
        {
            
            // example data
            
            /*input = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";*/
            

            var inputLines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            List<ulong> autocompleteScores = new List<ulong>();

            foreach (var line in inputLines)
            {   
                Chunk rootChunk = null;
                bool errored = false;
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
                            errored = true;
                            break;
                        }
                    }
                    //Console.WriteLine(rootChunk);
                }
                //finished loopin through line
                if (!errored)
                {
                    var completion = rootChunk.AutoComplete();
                    var score = rootChunk.GetCompletionScore(completion);
                    autocompleteScores.Add(score);
                    Console.WriteLine($"{rootChunk} .. {completion} = {score}");
                }
            }
            //finished with all lines
            Console.WriteLine($"total syntax error score: {totalPoints}");
            
            autocompleteScores.Sort();
            var midIdx = (int) Math.Ceiling(autocompleteScores.Count / 2f);
            Console.WriteLine($"middle score is {autocompleteScores[midIdx-1]}");
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

        private static Dictionary<char,int> errorPointValues = new Dictionary<char, int>
        {
            {')', 3},
            {']', 57},
            {'}', 1197},
            {'>', 25137}
        };

        private static Dictionary<char, int> autocompletePointValues = new Dictionary<char, int>
        {
            {')', 1}, 
            {']', 2}, 
            {'}', 3}, 
            {'>', 4}, 
        };

        public static bool IsOpener (char c) => pairedChars.Keys.Contains(c);
        public static bool IsCloser (char c) => pairedChars.Values.Contains(c);
        
        public char opener;
        public char closer;
        //public bool IsValid => opener != '\0' && closer != '\0';

        public Stack<Chunk> children;

        public static char GetPair(char c) => pairedChars[c];

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
                    Day10.totalPoints += errorPointValues[c];
                    throw new Exception($"Expected {match}, but found {c} instead. {errorPointValues[c]} points!");
                }
                
            }
            throw new Exception($"Token {c} is not an opening or closing token.");
        }

        public string AutoComplete()
        {
            var sb = new StringBuilder();
            while (children.Count > 0)
            {
                Chunk ch = children.Pop();
                sb.Append(Chunk.GetPair(ch.opener));
            }
            sb.Append(Chunk.GetPair(opener));

            return sb.ToString();
        }

        public ulong GetCompletionScore(string compString)
        {
            ulong tally = 0;
            foreach (char c in compString)
            {
                tally *= 5;
                tally += (ulong)autocompletePointValues[c];
            }
            return tally;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(opener);
            foreach (var child in children)
            {
                sb.Append($"{child}"); 
            }
            sb.Append(closer);
            
            return sb.ToString();
        }
    }
}
