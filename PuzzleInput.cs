using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventofcode2021
{
    public static class PuzzleInput
    {
        public static string RowSeparator;
        public static string ColSeparator;
        static PuzzleInput()
        {   
            RowSeparator = Environment.NewLine;
            ColSeparator = "";
        }
        
        public static string[,] Parse2DArray(string input)
        {
            var rows = input.Split(RowSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            int rowCount = rows.Length;
            int colCount = rows[0].Split(ColSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;

            string[,] parsedInput = new string[rowCount,colCount];
            for (int r=0; r<rowCount; r++)
            {
                var cols = rows[r].Split(ColSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                for (int c=0; c<colCount; c++)
                {
                    parsedInput[r,c] = cols[c];
                }
            }
            return parsedInput;
        }

        public static Dictionary<Coord, string> ParseDictOfCoords(string input)
        {
            var input2D = Parse2DArray(input);
            Dictionary<Coord, string> outPutDict = new Dictionary<Coord, string>();
            for (int y=0;y<input2D.GetLength(1);y++)
            {
                for (int x=0;x<input2D.GetLength(0);x++)
                {
                    outPutDict.Add(new Coord(x,y), input2D[x,y]);
                }
            }
            return outPutDict;
        }

        public static List<T> ParseList<T>(string input, Func<string[], T> factory)
        {
            List<T> results = new List<T>();
            var entries = input.Split(RowSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            foreach (var e in entries)
            {
                var splitEntry = e.Split(ColSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = factory.Invoke(splitEntry);
                results.Add(result);
            }
            return results;
        }

        public static HashSet<T> ParseHashSet<T>(string input, Func<string[], T> factory)
        {
            HashSet<T> results = new HashSet<T>();
            var entries = input.Split(RowSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var e in entries)
            {
                var splitEntry = e.Split(ColSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var result = factory.Invoke(splitEntry);
                results.Add(result);
            }
            return results;
        }

    }
}