using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp1
{
    public class FramedCellsTerrainDecorator : TerrainDecorator
    {
        public bool Grid { get; set; } = true;
        public FramedCellsTerrainDecorator(ITerrain inner) : base(inner) { }

        public override void Draw(Graphics g, int cellSize)
        {
            base.Draw(g, cellSize);
            if (!Grid) return;
            var pen = Pens.Sienna;
            for (int x = 0; x < N; x++)
                for (int y = 0; y < N; y++)
                    g.DrawRectangle(pen, x * cellSize, y * cellSize, cellSize, cellSize);
        }
    }
}
