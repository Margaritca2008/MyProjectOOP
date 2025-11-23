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

namespace WinFormsApp1
{
    public class Terrain
    {
        public const int N = 20;
        private int[,] field;
        public event Action<int[,]> TurnFinished;
        public Terrain()
        {
            field = new int[N, N];
            var rnd = new Random();
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    field[i, j] = rnd.Next(2);
        }
        public void NextTurn()
        {
            int[,] newField = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    int c = CountNeighbors(i, j);
                    if (field[i, j] == 1 && (c < 2 || c > 3))
                        newField[i, j] = 0;
                    else if (field[i, j] == 0 && c == 3)
                        newField[i, j] = 1;
                    else
                        newField[i, j] = field[i, j];
                }
            }
            field = newField;
            TurnFinished?.Invoke(field);
        }
        public static bool IsInside(int i, int j)
        {
            return i >= 0 && j >= 0 && i < N && j < N;
        }
        private int CountNeighbors(int i, int j)
        {
            int c = 0;
            for (int a = i - 1; a <= i + 1; a++)
                for (int b = j - 1; b <= j + 1; b++)
                    if (IsInside(a, b) && !(a == i && b == j) && field[a, b] == 1)
                        c++;
            return c;
        }
    }
}