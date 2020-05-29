using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.Controllers;

namespace Tetris
{
    public partial class Form1 : Form
    {
        
        
        string playerName;
        
        public Form1()
        {
            InitializeComponent();
            //if (!File.Exists(RecordsController.recordPath))
                //File.Create(RecordsController.recordPath);
            playerName = Microsoft.VisualBasic.Interaction.InputBox("Введите имя игрока","Настройка игрока","Новый игрок");
            if(playerName == "")
            {
                playerName = "Новый игрок";
            }
            this.KeyUp += new KeyEventHandler(keyFunc);
            Init();
        }

        public void Init()
        {
            //RecordsController.ShowRecords(label3);
            this.Text = "Тетрис: Текущий игрок - " + playerName;
            MapController.size = 25;
            MapController.score = 0;
            MapController.linesRemoved = 0;
            MapController.currentShape = new Shape(3, 0);
            MapController.Interval = 300;
            label1.Text = "Score: " + MapController.score;
            label2.Text = "Lines: " + MapController.linesRemoved;

           

            timer1.Interval = MapController.Interval;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            

            Invalidate();
        }

        private void keyFunc(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:

                    if (!MapController.IsIntersects())
                    {
                        MapController.ResetArea();
                        MapController.currentShape.RotateShape();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Space:
                    timer1.Interval = 10;
                    break;
                case Keys.Right:
                    if (!MapController.CollideHor(1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveRight();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
                case Keys.Left:
                    if (!MapController.CollideHor(-1))
                    {
                        MapController.ResetArea();
                        MapController.currentShape.MoveLeft();
                        MapController.Merge();
                        Invalidate();
                    }
                    break;
            }
        }

        
        private void update(object sender, EventArgs e)
        {
            MapController.ResetArea();
            if (!MapController.Collide())
            {
                MapController.currentShape.MoveDown();
            }
            else
            {
                MapController.Merge();
                MapController.SliceMap(label1,label2);
                timer1.Interval = MapController.Interval;
                MapController.currentShape.ResetShape(3,0);
                if (MapController.Collide())
                {
                    //RecordsController.SaveRecords(playerName);
                    MapController.ClearMap();
                    timer1.Tick -= new EventHandler(update);
                    timer1.Stop();
                    MessageBox.Show("Ваш результат: " + MapController.score);
                    Init();
                }
            }
            MapController.Merge();
            Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            MapController.DrawGrid(e.Graphics);
            MapController.DrawMap(e.Graphics);
            MapController.ShowNextShape(e.Graphics);
        }

        private void OnPauseButtonClick(object sender, EventArgs e)
        {
            var pressedButton = sender as ToolStripMenuItem;
            if (timer1.Enabled)
            {
                pressedButton.Text = "Продолжить";
                timer1.Stop();
            }
            else
            {
                pressedButton.Text = "Пауза";
                timer1.Start();
            }
        }

        private void OnAgainButtonClick(object sender, EventArgs e)
        {
            timer1.Tick -= new EventHandler(update);
            timer1.Stop();
            MapController.ClearMap();
            Init();
        }


        private void OnInfoPressed(object sender, EventArgs e)
        {
            string infoString = "";
            infoString = "Для управление фигурами используйте стрелочку влево/вправо.\n";
            infoString += "Чтобы ускорить падение фигуры - нажмите 'Пробел'.\n";
            infoString += "Для поворота фигуры используйте 'A'.\n";
            MessageBox.Show(infoString,"Справка");
        }


    }
}
