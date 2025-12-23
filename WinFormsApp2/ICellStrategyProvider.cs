using System.Windows.Forms;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;
namespace WinFormsApp1
{
    public interface ICellStrategyProvider
    {
        ICellLifeStrategy For(CellType type);
    }
    public class CellStrategyProvider : ICellStrategyProvider
    {
        private readonly Dictionary<CellType, ICellLifeStrategy> strategies;

        public CellStrategyProvider(Dictionary<CellType, ICellLifeStrategy> strategies)
        {
            this.strategies = strategies;
        }

        public ICellLifeStrategy For(CellType type)
        {
            return strategies[type];
        }
    }
}
