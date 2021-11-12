using Factory_pattern.Abstractions;
using Factory_pattern.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factory_pattern
{
    public partial class Form1 : Form
    {
        //List<Ball> _balls = new List<Ball>();
        List<Toy> _toys = new List<Toy>();
        Toy _nextToy;

        //private BallFactory _ballFactory;
        private IToyFactory _toyFactory;


        public IToyFactory ToyFactory
        {
            get { return _toyFactory; }
            set { 
                _toyFactory = value;
                DisplayNext();
            }
        }

        public Form1()
        {
            InitializeComponent();
            ToyFactory = new BallFactory();

        }

        /* private void createTimer_Tick(object sender, EventArgs e)
        {
            var ball = BallFactory.CreateNew();
            _balls.Add(ball);
            mainPanel.Controls.Add(ball);
            ball.Left = -ball.Width; //labda éppen hogy a panel bal oldaláról indul
        }*/
        private void createTimer_Tick(object sender, EventArgs e)
        {
            var Toy = ToyFactory.CreateNew();
            _toys.Add(Toy);
            mainPanel.Controls.Add(Toy);
            Toy.Left = -Toy.Width; //labda éppen hogy a panel bal oldaláról indul
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var lastPosition = 0;

            foreach (var item in _toys)
            {
                item.MoveToy(); //hogy ne a form1-nél kelljen módosítani a sebességet
                if (item.Left > lastPosition)
                {
                    lastPosition = item.Left; //melyik a legbaloldalibb item
                }
            }
            if (lastPosition >=1000)
            {
                var oldestToy = _toys[0]; //0-ik elem biztosan az, ami legelőször került bele, először őt kell eltüntetni
                _toys.Remove(oldestToy);
                mainPanel.Controls.Remove(oldestToy);
            }
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            ToyFactory = new CarFactory();
        }

        private void btnBall_Click(object sender, EventArgs e)
        {
            ToyFactory = new BallFactory()
            { 
            BallColor = btnColor.BackColor
            };
            
        }
        private void btnPresent_Click(object sender, EventArgs e)
        {
            ToyFactory = new PresentFactory()
            {
                BoxColor = btnColorBox.BackColor,
                RibbonColor = btnColorRibbon.BackColor
            };
        }

        private void DisplayNext()
        {
            if (_nextToy != null)
            {
                this.Controls.Remove(_nextToy);

                _nextToy = ToyFactory.CreateNew();
                _nextToy.Left = lblNext.Left + lblNext.Width;
                _nextToy.Top = lblNext.Top;
                this.Controls.Add(_nextToy);
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var cd = new ColorDialog();
            cd.Color = button.BackColor;

            if (cd.ShowDialog() != DialogResult.OK) return;
            button.BackColor = cd.Color;
        }

        
    }
}
