using System.Collections.Generic;
using System.Drawing;

namespace WinFormsApp1
{
    public class ScannerTerrainDecorator : TerrainDecorator
    {
        private readonly Scanner scanner;
        public HashSet<Cell> HighlightedCells { get; } = new HashSet<Cell>();
        public bool ShowPatterns { get; set; } = true;
        public ScannerTerrainDecorator(ITerrain inner, Scanner scanner) : base(inner)
        {
            this.scanner = scanner;
        }
        public override void Update()
        {
            base.Update();
            HighlightedCells.Clear();
            var found = scanner.ScanField();
            foreach (var (i, j, p) in found)
            {
                var pattern = scanner.Patterns[p];
                foreach (var pair in pattern)
                {
                    int x = i + pair.x, y = j + pair.y;
                    if (x >= 0 && y >= 0 && x < N && y < N)
                        HighlightedCells.Add(Cells[x, y]);
                }
            }
            if (found.Count > 0) scanner.RaisePatternDetected(found);
        }
        public override void Draw(Graphics g, int cellSize)
        {
            base.Draw(g, cellSize);
            if (!ShowPatterns) return;
            foreach (var cell in HighlightedCells)
                g.FillRectangle(Brushes.LightGreen, cell.X * cellSize, cell.Y * cellSize, cellSize, cellSize);
        }
    }
}
