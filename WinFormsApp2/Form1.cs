using System;
using System.Drawing;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private CheckBox drawGridCheckBox;
        private CheckBox showPatternsCheckBox;
        private ComboBox comboBox;
        private Button applyModeButton;
        private Label staticLabel;
        private Button startButton;
        private Button stopButton;
        private ITerrain terrain;//красота
        private Terrain baseTerrain;//главный террейн
        private int cellSize = 16;
        private bool running = false;
        private StatisticsTerrainDecorator statsDecorator;
        private ScannerTerrainDecorator scannerDecorator;
        private FramedCellsTerrainDecorator framedDecorator;
        private ColonyTerrainDecorator colonyDecorator;
        private Scanner scanner;
        private Colonys colonys;
        private IWorldFactory currentFactory;
        private int fieldSize = 40;
        public Form1()
        {
            InitializeComponent();
            pictureBox = new PictureBox { Location = new Point(10, 10)};
            Controls.Add(pictureBox);

            comboBox = new ComboBox { Location = new Point(680, 20), Width = 120 };
            comboBox.Items.AddRange(new[] { "Classic", "Colonies" });
            comboBox.SelectedIndex = 0;
            Controls.Add(comboBox);

            applyModeButton = new Button { Text = "Apply", Location = new Point(800, 20) };
            applyModeButton.Click += (s, e) => ApplyMode();
            Controls.Add(applyModeButton);

            drawGridCheckBox = new CheckBox { Text = "Grid", Location = new Point(680, 60), Checked = true };
            Controls.Add(drawGridCheckBox);

            showPatternsCheckBox = new CheckBox { Text = "Show patterns", Location = new Point(900, 60), Checked = true };
            Controls.Add(showPatternsCheckBox);

            staticLabel = new Label { Location = new Point(680, 100), AutoSize = true, Font = new Font("Consolas", 10) };
            Controls.Add(staticLabel);
            
            startButton = new Button { Text = "Start", Location = new Point(680, 200) };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            stopButton = new Button { Text = "Stop", Location = new Point(760, 200), Enabled = false };
            stopButton.Click += (s, e) => running = false;
            Controls.Add(stopButton);

            ApplyMode();
            pictureBox.Size = new Size(baseTerrain.N * cellSize, baseTerrain.N * cellSize);
            pictureBox.Paint += (s, e) => terrain.Draw(e.Graphics, cellSize);
            drawGridCheckBox.CheckedChanged += (s, e) => { framedDecorator.Grid = drawGridCheckBox.Checked; pictureBox.Invalidate(); };
            showPatternsCheckBox.CheckedChanged += (s, e) => { scannerDecorator.ShowPatterns = showPatternsCheckBox.Checked; pictureBox.Invalidate(); };
            ClientSize = new Size(pictureBox.Width + 300, pictureBox.Height + 50);
        }
        private void ApplyMode()
        {
            var mode = (string)comboBox.SelectedItem;
            currentFactory = mode switch
            {
                "Classic" => new ClassicWorldFactory(),
                "Colonies" => new ColoniesWorldFactory()
            };
            var strategies = currentFactory.CreateStrategies();
            var cellFactory = currentFactory.CreateCellFactory(strategies);
            baseTerrain = new Terrain(fieldSize, cellFactory);
            statsDecorator = new StatisticsTerrainDecorator(baseTerrain);
            if (mode == "Classic")
            {
                colonys = null;
                scanner = new Scanner(statsDecorator, type => type == CellType.White);
            }
            else
            {
                scanner = new Scanner(statsDecorator, type => type == CellType.White || type == CellType.Black);
                colonys = new Colonys(baseTerrain);
            }
            if (colonys !=null)
            {
                framedDecorator = new FramedCellsTerrainDecorator(statsDecorator);
            } else
            {
                scannerDecorator = new ScannerTerrainDecorator(statsDecorator, scanner);
                framedDecorator = new FramedCellsTerrainDecorator(scannerDecorator);
            }
            if (colonys != null)
            {
                terrain = new ColonyTerrainDecorator(framedDecorator, scanner, colonys);
            }
            else terrain = framedDecorator;
            if (pictureBox != null)
            {
                pictureBox.Size = new Size(baseTerrain.N * cellSize, baseTerrain.N * cellSize);
                pictureBox.Invalidate();
            }
        }
        private async void StartButton_Click(object sender, EventArgs e)
        {
            running = true;
            startButton.Enabled = false;
            stopButton.Enabled = true;
            while (running)
            {
                terrain.Update();
                colonys?.CreateColoniesFromScanner(scanner);
                colonys?.MoveColonies();
                pictureBox.Invalidate();
                staticLabel.Text =
                    $"Alive: {statsDecorator.Count}\n" +
                    $"Turn time: {statsDecorator.TimeMs} ms";

                await Task.Delay(300);
            }
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }


    }
}
