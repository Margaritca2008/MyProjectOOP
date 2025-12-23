using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
using System.Diagnostics;
namespace WinFormsApp1
{
    public class StatisticsTerrainDecorator : TerrainDecorator
    {
        private int PreviousCount = 0;
        public int Count { get; private set; }
        public double Delta { get; private set; }
        public long TimeMs { get; private set; }
        public StatisticsTerrainDecorator(Terrain t) : base(t)
        {
        }
        public override void Update()
        {
            var sw = Stopwatch.StartNew();
            base.Update();
            sw.Stop();

            TimeMs = sw.ElapsedMilliseconds;

            int count = Cells.Cast<Cell>().Count(c => c.Type != CellType.Empty);
            Delta = count - PreviousCount;
            Count = count;
            PreviousCount = count;
        }
    }
}