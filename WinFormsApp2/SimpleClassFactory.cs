using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
namespace WinFormsApp1
{
    public interface ICellFactory {
        Cell Create(int x, int y, CellType type);
    }
    public class SimpleCellFactory : ICellFactory
    {
        private readonly ICellStrategyProvider strategies;

        public SimpleCellFactory(ICellStrategyProvider strategies)
        {
            this.strategies = strategies;
        }

        public Cell Create(int x, int y, CellType type)
        {
            return new Cell(x, y, type, strategies);
        }
    }

}

