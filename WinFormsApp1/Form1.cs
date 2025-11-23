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
    public partial class Form1 : Form
    {
        private Terrain terrain;
        private Scanner scanner;
        private Form2 f2;
        private Button[,] cells;
        private Button startButton;
        private Button stopButton;
        private bool running = false;
        private const int cellSize = 30;

        public Form1()
        {
            InitializeComponent();
            terrain = new Terrain();
            scanner = new Scanner();
            cells = new Button[Terrain.N, Terrain.N];
            for (int i = 0; i < Terrain.N; i++)
            {
                for (int j = 0; j < Terrain.N; j++)
                {
                    var b = new Button
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(j * cellSize, i * cellSize),
                        Enabled = false,
                        BackColor = Color.White
                    };
                    Controls.Add(b);
                    cells[i, j] = b;
                }
            }
            startButton = new Button { Text = "Start", Location = new Point(10, Terrain.N * cellSize + 10) };
            startButton.Click += StartButtonClick;
            Controls.Add(startButton);
            stopButton = new Button { Text = "Stop", Location = new Point(100, Terrain.N * cellSize + 10), Enabled = false };
            stopButton.Click += StopButtonClick;
            Controls.Add(stopButton);
            ClientSize = new Size(Terrain.N * cellSize + 20, Terrain.N * cellSize + 60);
            terrain.TurnFinished += UpdateMainField;
            terrain.TurnFinished += scanner.OnTurnFinishedScan;
        }
        private void UpdateMainField(int[,] field)
        {
            for (int i = 0; i < Terrain.N; i++)
                for (int j = 0; j < Terrain.N; j++)
                    cells[i, j].BackColor = field[i, j] == 1 ? Color.Pink : Color.White;
        }
        private async void StartButtonClick(object sender, EventArgs e)
        {
            if (f2 == null)
            {
                f2 = new Form2(scanner);
                f2.Show();
            }
            startButton.Enabled = false;
            stopButton.Enabled = true;
            running = true;
            while (running)
            {
                terrain.NextTurn();
                await Task.Delay(300);
            }
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }
        private void StopButtonClick(object sender, EventArgs e)
        {
            running = false;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            running = false;
            base.OnFormClosing(e);
        }
    }
}