using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1
{
    public class Terrain : ITerrain
    {
        private Cell[,] cells;
        public static int nextId = 0;
        public Terrain(int N, ICellFactory cellFactory)
        {
            Reinitialize(N, cellFactory);
        }

        public void Reinitialize(int N, ICellFactory cellFactory)
        {
            Random rnd = new Random();
            cells = new Cell[N, N];
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    cells[x, y] = cellFactory.Create(x, y, rnd.Next(8) == 1 ? CellType.White : CellType.Empty);
                }
            }
            //for (int x = 0; x < N; x++)
            //{
            //    for (int y = 0; y < N; y++)
            //    {
            //        cells[x, y] = cellFactory.Create(x, y, CellType.Empty);
            //    }
            //}
            //cells[1, 1].AssignType(CellType.White);
            //cells[1, 2].AssignType(CellType.White);
            //cells[1, 1].AssignType(CellType.White);
            //cells[2, 1].AssignType(CellType.White);
            //cells[2, 2].AssignType(CellType.White);
            //cells[4, 1].AssignType(CellType.White);
            //cells[4, 2].AssignType(CellType.White);
            //cells[5, 1].AssignType(CellType.White);
            //cells[5, 2].AssignType(CellType.White);
            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    cells[x, y].Neighbors = GetNeighbors(x, y);
                }
            }
        }

        public List<Cell> GetNeighbors(int x, int y)
        {
            var list = new List<Cell>();
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if ((dx == 0 && dy == 0)) continue;
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < N && ny < N)
                        list.Add(cells[nx, ny]);
                }
            return list;
        }
        public IEnumerable<Cell> GetNeighbors(Cell cell)
        {
            return GetNeighbors(cell.X, cell.Y);
        }
        public Cell[,] Cells => cells;
        public int N => cells.GetLength(0);

        public CellContext BuildContext(Cell cell)
        {
            int white = 0, black = 0;
            Cell firstBlack = null;

            foreach (var n in cell.Neighbors)
            {
                if (n.Type == CellType.White) white++;
                else if (n.Type == CellType.Black)
                {
                    black++;
                    if (firstBlack == null) firstBlack = n; 
                }
            }

            return new CellContext(cell.Type, white, black, white + black, firstBlack, cell);
        }

        public void Update()
        {
            for (int x = 0; x < N; x++)
                for (int y = 0; y < N; y++)
                    cells[x, y].PlanNext(BuildContext(cells[x, y]));

            for (int x = 0; x < N; x++)
                for (int y = 0; y < N; y++)
                {
                    cells[x, y].ApplyNextState();
                }

        }
        public void Draw(Graphics g, int cellSize)
        {
            for (int x = 0; x < N; x++)
                for (int y = 0; y < N; y++)
                {
                    cells[x, y].Draw(g, cellSize);
                }
                    
        }
    }
}
