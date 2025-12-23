using System.Drawing;

namespace WinFormsApp1
{
    public class ColonyTerrainDecorator : TerrainDecorator
    {
        private Colonys colonys;
        private Scanner scanner;
        public ColonyTerrainDecorator(ITerrain inner, Scanner sranner, Colonys colonys) : base(inner)
        {
            this.scanner = sranner;
            this.colonys = colonys;
        }
        public override void Update()
        {
            inner.Update();
            colonys.MoveColonies();
        }
        public override void Draw(Graphics g, int cellSize)
        {
            inner.Draw(g, cellSize);

            for (int x = 0; x < N; x++)
            {
                for (int y = 0; y < N; y++)
                {
                    var cell = Cells[x, y];
                    if (cell.Colony != null)
                    {
                        var rect = new Rectangle(
                            x * cellSize,
                            y * cellSize,
                            cellSize,
                            cellSize
                        );
                        g.FillRectangle(Brushes.Black, rect);
                    }
                }
            }
        }
    }
}
