using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace greetingCard
{
    //Alex Spicer
    //ICS3U
    //2025 - 11 - 28
    //Greeting Card Assignment


    public partial class Form1 : Form
    {

        Boolean title = true;
        Timer timer = new Timer();
        Random rnd = new Random();

        // balloon positions
        List<Balloon> balloons = new List<Balloon>();

        public Form1()
        {
            InitializeComponent();
            
            timer.Interval = 30;
            timer.Tick += Timer_Tick;

            // add some balloons
            for (int i = 0; i < 6; i++)
            {
                balloons.Add(new Balloon(
                    rnd.Next(400, 750),   // X
                    rnd.Next(300, 600),   // Y
                    rnd
                ));
            }
        }

        class Balloon
        {
            public float X, Y;
            public Color Color;
            public int RChange, GChange, BChange; // direction of color change

            public Balloon(float x, float y, Random rnd)
            {
                X = x;
                Y = y;

                // start with a random color
                Color = Color.FromArgb(rnd.Next(150, 256), rnd.Next(150, 256), rnd.Next(150, 256));

                // random direction for each color channel
                RChange = rnd.Next(0, 2) == 0 ? 1 : -1;
                GChange = rnd.Next(0, 2) == 0 ? 1 : -1;
                BChange = rnd.Next(0, 2) == 0 ? 1 : -1;
            }

            public void UpdateColor()
            {
                int r = Color.R + RChange;
                int g = Color.G + GChange;
                int b = Color.B + BChange;

                // reverse direction if hitting boundaries
                if (r > 255 || r < 150) RChange *= -1;
                if (g > 255 || g < 150) GChange *= -1;
                if (b > 255 || b < 150) BChange *= -1;

                // apply new color
                Color = Color.FromArgb(
                    Math.Max(150, Math.Min(255, r)),
                    Math.Max(150, Math.Min(255, g)),
                    Math.Max(150, Math.Min(255, b))
                );
            }
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            // Move balloons upward
            for (int i = 0; i < balloons.Count; i++)
            {
                Balloon p = balloons[i];
                p.Y -= 2; // rise

                // reset when off screen
                if (p.Y < -100)
                    p.Y = this.ClientSize.Height + 20;

                balloons[i] = p;
            }

            Invalidate(); // redraw
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //Title Screen

            if (title)
            {
                g.Clear(Color.Pink);

                //The pen just outlines objects!
                Pen drawPen = new Pen(Color.DarkBlue, 10);

                //The brush fills the object
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                //Text Stuff
                Font drawFont = new Font("Arial", 60, FontStyle.Bold);


                // x: sets the x value of the top left point of the rectangle
                // y: sets the y value of the top left point of the rectangle
                // width: sets the width of the rectangle
                // height: sets the height of the rectangle


                g.DrawRectangle(drawPen, 30, 30, 800, 500);
                g.DrawString("Open Card!", drawFont, drawBrush, 200, 200);

                return;
            }

            

            //Message
            g.DrawString("Happy Birthday!",new Font ("Comic Sans MS", 40, FontStyle.Bold), Brushes.Purple, 60, 50);

            g.DrawString("Wishing you a fun-filled day!", new Font("Arial", 20), Brushes.Purple, 60, 120);

            //Balloons
            foreach (Balloon b in balloons)
            {
                DrawBalloon(g, b);
            }

        }

        void DrawBalloon(Graphics g, Balloon b)
        {
            // balloon ellipse
            g.FillEllipse(new SolidBrush(b.Color), b.X - 20, b.Y - 30, 40, 60);

            // string
            g.DrawLine(Pens.Gray, b.X, b.Y + 30, b.X, b.Y + 60);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (title)
            {
                SoundPlayer birthdaySong = new SoundPlayer(Properties.Resources._646119__sergequadrado__happy_birthday_to_you_baby_loop);
                birthdaySong.Play(); //Plays the birthday song sound effect

                title = false; //open the card
                timer.Start(); //start animation
                Invalidate();
            }
        }
    }
}
