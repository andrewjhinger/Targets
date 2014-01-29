using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Targets
{
    public partial class MainForm : Form
    {

        private TargetGame _targetGame;

        private int _hits = 0;
        private int _level = 1;
        private int _hitsPerLevel = 4;

        public MainForm()
        {
            InitializeComponent();
            //if (pauseToolStripMenuItem.cl pauseToolStripMenuItem.Text == "Pause")

                pauseToolStripMenuItem.Click += (s, e) =>
                                                                {if(pauseToolStripMenuItem.Text == "Pause")
                                                                    {
                                                                    gameTimer.Enabled = false;
                                                                    pauseToolStripMenuItem.Text = "Resume";
                                                                    }
                                                                    
                                                                else
                                                                    {
                                                                    gameTimer.Enabled = true;
                                                                    pauseToolStripMenuItem.Text = "Pause";
                                                                    }

                                                                };
        }
                
            

        

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Set up status
            updateHitsStatus();

            // Set cursor
            mainPictureBox.Cursor = Cursors.Cross;

            // Lambda statement for game reset
            resetToolStripMenuItem.Click += (from, ea) =>
            {
                _hits = 0;
                _level = 1;
                _targetGame.NumberOfTargets = 1;
                updateHitsStatus();
                _targetGame.Reset();
            };

            // Exit event event lambda expressions
            exitToolStripMenuItem.Click += (from, ea) => this.Close();

            // PictureBox Mouseup lambda expression
            mainPictureBox.MouseUp += (from, ea) => _targetGame.Select(ea.Location);

            // Create target game object
            _targetGame = new TargetGame(boardSize: mainPictureBox.ClientSize);

            // Lambda delegates for Hit
            _targetGame.Hit += (from, ea) => ++_hits;
            _targetGame.Hit += (from, ea) =>
            {
                if (_hits == _hitsPerLevel * _targetGame.NumberOfTargets)
                {
                    _hits = 0;
                    _level++;
                    _targetGame.NumberOfTargets++;

                }
            };
            _targetGame.Hit += (from, ea) => updateHitsStatus();

            // Set up Picturebox Paint event
            mainPictureBox.Paint += (from, ea) => _targetGame.Update(ea.Graphics);

            // Set up Timer Tick event
            gameTimer.Tick += (from, ea) => mainPictureBox.Invalidate();
        }

        private void updateHitsStatus()
        {
            // Update hits status
            hitsToolStripStatusLabel.Text = "Level: " + _level.ToString() + " - Hits: " + _hits.ToString();
        }

    }
}
