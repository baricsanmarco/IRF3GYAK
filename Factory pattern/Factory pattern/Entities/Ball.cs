using Factory_pattern.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factory_pattern.Entities
{
    public class Ball : Toy
    {
        /*  public Ball()  Már benne vannak a Toy osztályban
          {
              AutoSize = false;
              Width = 50;
              Height = Width;
              Paint += Ball_Paint;
          }

          private void Ball_Paint(object sender, PaintEventArgs e) //ha módosul a design, akkor nem kell újrarajzolni
          {
              DrawImage(e.Graphics);
          }*/
        public SolidBrush BallColor { get; set; }
        public Ball(Color color)
        {
            BallColor = new SolidBrush(color);
        }
        protected override void DrawImage(Graphics g) //felül kell írni az absztrakt osztályt
        {
            var ecset = new SolidBrush(Color.Blue);
            g.FillEllipse(
                BallColor,
                0,
                0,
                Width,
                Height); //0.0 pontból indul és ugyanolyan széles és magas ellipszis vagyis egy kör
        }
        /*public void MoveBall()
        {
            Left++;
        }*/
    }
}
