using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode2021
{
    internal class BingoBoard
    {
        // size of the board. the board is square.
        public const int size = 5;

        // the array of numbers on your board
        private int[,] board;

        // marks indicate which cells have a bingo chip on them
        public bool[,] marks;

        public bool HasFiveInARow
        {
            get 
            {
                bool inRow = true;
                bool inCol = true;
                for (int y=0; y<size; y++)
                {
                    var row = new bool[size];
                    for (int x=0; x<size; x++)
                    {
                        row[y]=marks[x,y];
                    }
                    Console.WriteLine($"row {row}");
                    
                    foreach(var b in row)
                    {
                        if (b == false)
                        {
                            inRow = false;
                            break;
                        }
                    }
                    if (inRow)
                    {
                        return true; //winner
                    }
                }

                for (int x=0; x< size; x++)
                {
                    var col = new bool[size];
                    for (int y=0; y<size; y++)
                    {
                        col[x] = marks[x,y];
                    }
                    Console.WriteLine($"col {col}");
                    
                    foreach(var b in col)
                    {
                        if (b == false)
                        {
                            inCol = false;
                            break;
                        }
                    }
                    if (inCol)
                    {
                        return true; //winner
                    }
                }
                return false;
            }
        }

        public int score
        {
            get
            {
                int scoreTicker = 0;
                for(int y=0;y<size;y++)
                {
                    for (int x=0;x<size;x++)
                    {
                        //only unmarked spaces on the board count
                        if (!marks[x,y])
                        {
                            scoreTicker += board[x,y];
                        }
                    }
                }
                return scoreTicker;
            }
        }

        public int this[int r, int c]
        {
            get => board[r, c];
            set => board[r, c] = value;
        }
        public BingoBoard(string[] rows)
        {
            board = new int[size,size];
            marks = new bool[size,size];
            int rowId = 0;
            foreach (var r in rows)
            {
                var cols = r.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length != size)
                {
                    throw new Exception($"Row does not have the correct number of elements: {r}");
                }
                int colId = 0;
                foreach (var c in cols)
                {
                    board[rowId,colId] = int.Parse(c);
                    //marks[rowId,colId] = false;
                    colId++;
                }
                rowId++;
            }
        }
    }
    internal static class Day4
    {
        private static Queue<int> valueSequence;
        private static List<BingoBoard> boards;
        public static void Part1(string input)
        {
            //arrange the numbers to draw
            string[] gameBoardsRaw;
            var rawValueSequence = ExtractValueSequence(input, out gameBoardsRaw );
            var splitValueSequence = rawValueSequence.Split(",");
            valueSequence = new Queue<int>();
            foreach (var s in splitValueSequence)
            {
                valueSequence.Enqueue(int.Parse(s));
            }

            //organize boards
            int rowId = 0;
            string[] currentBoard = null;
            boards = new List<BingoBoard>();
            foreach (var s in gameBoardsRaw)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (currentBoard != null)
                    {
                        boards.Add(new BingoBoard(currentBoard));
                    }
                    currentBoard = new string[BingoBoard.size];
                    rowId = 0;
                    continue;
                }
                currentBoard[rowId] = s;
                rowId++;
            }
            currentBoard = null; //ready for gc
            
            //start drawing values and marking boards;
            while (valueSequence.Count > 0)
            {
                // get the next number
                int drawnValue = valueSequence.Dequeue();
                
                // for tracking tiebreaker candidates;
                List<BingoBoard> winningBoards = new List<BingoBoard>();

                //look for boards with matches, mark them, and check for victory
                foreach (var board in boards)
                {
                    for (int y=0; y<BingoBoard.size; y++)
                    {
                        for (int x=0; x<BingoBoard.size; x++)
                        {
                            if (drawnValue == board[x,y]) // match!
                            {
                                board.marks[x,y] = true;
                                //check for victory
                                if (board.HasFiveInARow)
                                {
                                    winningBoards.Add(board);
                                }
                            }
                        }
                    }
                }

                // finally, compare winning boards for the one with the best score
                int bestScore = 0;
                BingoBoard winningBoard = null;
                foreach (var board in winningBoards)
                {
                    if (board.score > bestScore)
                    {
                        winningBoard = board;
                    }
                }
                Console.WriteLine($"winning score: {winningBoard.score}");
            }
        }
        public static string ExtractValueSequence(string input, out string[] remainingValues)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new Exception ("cannot extract from empty input");
            }
            var splitInput = input.Split("\n");
            remainingValues = splitInput[1..^1];
            return splitInput[0];
        }
    }
}
