using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/*
using NUnit.Framework;
from dotnet cli--
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Nunit3TestAdapter
dotnet add package NUnit
*/

namespace adventofcode2021
{
    internal static class Day04
    {
        private static Queue<int> valueSequence;
        private static List<BingoBoard> boards;
        public static void Solve(string input)
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
                Console.WriteLine($"drew: {drawnValue}");

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
                if(winningBoards.Count == 0)
                {
                    Console.WriteLine("no winners this round");
                }
                else if (winningBoards.Count == 1 )
                {
                    Console.WriteLine($"one winner: {winningBoards[0].score * drawnValue}");
                }
                else if (winningBoards.Count > 1)
                {
                    // compare winning boards for the one with the best and worst score
                    int bestScore = 0;
                    int worstScore = int.MaxValue;
                    BingoBoard bestBoard = null;
                    BingoBoard worstBoard = null;
                    foreach (var board in winningBoards)
                    {
                        if (board.score > bestScore)
                        {
                            bestScore = board.score;
                            bestBoard = board;
                        }
                        if (board.score < worstScore)
                        {
                            worstScore = board.score;
                            worstBoard = board;
                        }
                    }
                    Console.WriteLine($"winning score: {bestBoard.score * drawnValue}");
                    Console.WriteLine($"losing score: {worstBoard.score * drawnValue}");
                }
                if (winningBoards.Count > 0)
                {
                    //remove them from the game
                    foreach (var win in winningBoards)
                    {
                        boards.Remove(win);
                    }
                }
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
