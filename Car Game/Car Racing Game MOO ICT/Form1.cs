using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Car_Racing_Game_MOO_ICT
{
    public partial class Form1 : Form
    {
        int roadSpeed;
        int trafficSpeed;
        int playerSpeed = 12;
        int score;
        int carImage;

        Random rand = new Random();
        Random carPosition = new Random();

        bool goleft, goright;

        List<int> scoreHistory = new List<int>();

        public Form1()
        {
            InitializeComponent();
            ResetGame();
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = false;
            }
        }

        private void gameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            score++;

            if (goleft == true && player.Left > 10)
            {
                player.Left -= playerSpeed;
            }
            if (goright == true && player.Left < 415)
            {
                player.Left += playerSpeed;
            }

            roadTrack1.Top += roadSpeed;
            roadTrack2.Top += roadSpeed;

            if (roadTrack2.Top > 519)
            {
                roadTrack2.Top = -519;
            }
            if (roadTrack1.Top > 519)
            {
                roadTrack1.Top = -519;
            }

            trafficCar1.Top += trafficSpeed;
            trafficCar2.Top += trafficSpeed;

            if (trafficCar1.Top > 530)
            {
                changeTrafficCars(trafficCar1);
            }

            if (trafficCar2.Top > 530)
            {
                changeTrafficCars(trafficCar2);
            }

            if (player.Bounds.IntersectsWith(trafficCar1.Bounds) || player.Bounds.IntersectsWith(trafficCar2.Bounds))
            {
                gameOver();
            }

            if (score > 40 && score < 500)
            {
                award.Image = Properties.Resources.bronze;
            }

            if (score > 500 && score < 2000)
            {
                award.Image = Properties.Resources.silver;
                roadSpeed = 20;
                trafficSpeed = 22;
            }

            if (score > 2000)
            {
                award.Image = Properties.Resources.gold;
                trafficSpeed = 27;
                roadSpeed = 25;
            }
        }

        private void changeTrafficCars(PictureBox tempCar)
        {
            carImage = rand.Next(1, 9);

            switch (carImage)
            {
                case 1:
                    tempCar.Image = Properties.Resources.ambulance;
                    break;
                case 2:
                    tempCar.Image = Properties.Resources.carGreen;
                    break;
                case 3:
                    tempCar.Image = Properties.Resources.carGrey;
                    break;
                case 4:
                    tempCar.Image = Properties.Resources.carOrange;
                    break;
                case 5:
                    tempCar.Image = Properties.Resources.carPink;
                    break;
                case 6:
                    tempCar.Image = Properties.Resources.CarRed;
                    break;
                case 7:
                    tempCar.Image = Properties.Resources.carYellow;
                    break;
                case 8:
                    tempCar.Image = Properties.Resources.TruckBlue;
                    break;
                case 9:
                    tempCar.Image = Properties.Resources.TruckWhite;
                    break;
            }

            tempCar.Top = carPosition.Next(100, 400) * -1;

            if ((string)tempCar.Tag == "carLeft")
            {
                tempCar.Left = carPosition.Next(5, 200);
            }
            if ((string)tempCar.Tag == "carRight")
            {
                tempCar.Left = carPosition.Next(245, 422);
            }
        }

        private void gameOver()
        {
            playSound();
            gameTimer.Stop();

            award.Visible = true;
            award.BringToFront();

            btnStart.Enabled = true;

            scoreHistory.Add(score);
            ShowGameOverPanel();
        }

        private void ShowGameOverPanel()
        {
            Panel gameOverPanel = new Panel
            {
                Size = new Size(400, 350),
                Location = new Point(50, 100),
                BackColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblGameOver = new Label
            {
                Text = "Game Over!",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Size = new Size(300, 30),
                Location = new Point(50, 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblLastScore = new Label
            {
                Text = $"Last Score : {score}",
                Font = new Font("Arial", 12),
                Size = new Size(300, 30),
                Location = new Point(50, 50)
            };

            Label lblHighScore = new Label
            {
                Text = $"High Score : {scoreHistory.Max()}",
                Font = new Font("Arial", 12),
                Size = new Size(300, 30),
                Location = new Point(50, 90)
            };

            Label lblScoreHistory = new Label
            {
                Text = "Score History :",
                Font = new Font("Arial", 12),
                Size = new Size(300, 30),
                Location = new Point(50, 130)
            };

            ListBox lstScoreHistory = new ListBox
            {
                Size = new Size(300, 100),
                Location = new Point(50, 170)
            };

            int scoreNumber = 0;

            foreach (int s in scoreHistory)
            {
                scoreNumber++;
                lstScoreHistory.Items.Add($"{scoreNumber}) {s}");
            }

            Button btnClose = new Button
            {
                Text = "Close",
                Size = new Size(100, 30),
                Location = new Point(150, 280)
            };

            btnClose.Click += (s, e) => { this.Controls.Remove(gameOverPanel); };

            gameOverPanel.Controls.Add(lblGameOver);
            gameOverPanel.Controls.Add(lblLastScore);
            gameOverPanel.Controls.Add(lblHighScore);
            gameOverPanel.Controls.Add(lblScoreHistory);
            gameOverPanel.Controls.Add(lstScoreHistory);
            gameOverPanel.Controls.Add(btnClose);

            this.Controls.Add(gameOverPanel);
            gameOverPanel.BringToFront();
        }

        private void ResetGame()
        {
            btnStart.Enabled = false;
            award.Visible = false;
            goleft = false;
            goright = false;
            score = 0;
            award.Image = Properties.Resources.bronze;

            roadSpeed = 12;
            trafficSpeed = 15;

            trafficCar1.Top = carPosition.Next(200, 500) * -1;
            trafficCar1.Left = carPosition.Next(5, 200);

            trafficCar2.Top = carPosition.Next(200, 500) * -1;
            trafficCar2.Left = carPosition.Next(245, 422);

            gameTimer.Start();
        }

        private void restartGame(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void playSound()
        {
            System.Media.SoundPlayer playCrash = new System.Media.SoundPlayer(Properties.Resources.hit);
            playCrash.Play();
        }
    }
}
