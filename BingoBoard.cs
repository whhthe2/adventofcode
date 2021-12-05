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
                for (int y=0; y<size; y++)
                {
                    var row = new bool[size];
                    for (int x=0; x<size; x++)
                    {
                        row[y]=marks[x,y];
                    }
                    if (IsAllTrue(row))
                    {
                        return true; //winner
                    }
                }

                for (int x=0; x< size; x++)
                {
                    var col = new bool[size];
                    for (int y=0; y<size; y++)
                    {
                        col[y] = marks[x,y];
                    }
                    if (IsAllTrue(col))
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
                if (scoreTicker == 0)
                {
                    throw new Exception("board has a score of zero, which should not be possible");
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

        private bool IsAllTrue(bool[] line)
        {
            foreach (bool b in line)
            {
                if (b==false)
                {
                    return false;
                }
            }
            return true;
        }

    }
}