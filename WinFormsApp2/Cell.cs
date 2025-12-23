using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1
{
    public record CellContext(
        CellType Self,
        int WhiteN,
        int BlackN,
        int AliveN,
        Cell? FirstBlackNeighbor,
        Cell myCell
    );
    public enum CellType
    {
        Empty,
        White,
        Black
    }
    public class Cell
    {
        public int X { get; }
        public int Y { get; }
        public CellType Type { get; private set; }
        public List<Cell> Neighbors { get; set; } = new List<Cell>();
        public Colony Colony { get; private set; } = null;
        private CellType _nextType;
        private readonly ICellStrategyProvider provider;
        public Cell(int x, int y, CellType type, ICellStrategyProvider provider)
        {
            X = x;
            Y = y;
            Type = type;
            _nextType = type;
            this.provider = provider;
        }
        public void PlanNext(CellContext ctx)
        {
            var strat = provider.For(Type);
            _nextType = strat.GetNext(ctx);
        }
        public void AssignType(CellType type)
        {
            Type = type;
        }
        public void AssignToColony(Colony colony)
        {
            Colony = colony;
            AssignType(CellType.Black);
        }
        public void RemoveFromColony()
        {
            Colony = null;
            AssignType(CellType.Empty);
        }
        public void ApplyNextState()
        {
            Type = _nextType;

            if (Type != CellType.Black)
                Colony = null; 
        }
        public void ClearColony()
        {
            Colony = null;
        }
        public void Draw(Graphics g, int cellSize)
        {
            Brush brush = Type switch
            {
                CellType.Empty => Brushes.LightBlue,
                CellType.White => Brushes.White,
                CellType.Black => Brushes.Black,
            };
            g.FillRectangle(brush, X * cellSize, Y * cellSize, cellSize, cellSize);
        }
    }

}
