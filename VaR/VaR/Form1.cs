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
using VaR.Entities;

namespace VaR
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        List<decimal> nyeresegekRendezve;

        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList(); //nem csak másik formátum, hanem másolatot készít az adatokból a lokál memóriában és ott dolgozunk vele, nem az SQL DB-n
            dataGridView1.DataSource = Ticks;
            CreatePortfolio();

            /*int elemszám = Portfolio.Count();
            decimal részvényekSzáma = (from x in Portfolio
                                       select x.Volume).Sum();
            var otp = from x in Ticks
                      where x.Index.Trim().Equals("OTP")
                      select new
                      { 
                          x.Index,
                          x.Price
                      }
                      ;
            Console.WriteLine("OTP darabszám: " + otp.Count());
            //mivel a fenti var is egy lista lehet továbbszűrni

            var top = from o in otp
                      where o.Price > 7000
                      select o;
            Console.WriteLine("OTP darabszám nagyobb mint 7000: " + otp.Count().ToString());
            //MessageBox.Show(string.Format)

            var topsum = (from t in top
                          select t.Price).Sum();

            //c. A legrégebbi kereskedési nap:
            DateTime minDátum = (from x in Ticks select x.TradingDay).Min();

            //d. A legutolsó kereskedési nap:
            DateTime maxDátum = (from x in Ticks select x.TradingDay).Max();
            int elteltNapokSzáma = (maxDátum - minDátum).Days;


            //g. Össze is lehet kapcsolni dolgokat, ez már bonyolultabb:
            var kapcsolt =
                from
                    x in Ticks
                join
                    y in Portfolio
            on x.Index equals y.Index
                select new
                {
                    Index = x.Index,
                    Date = x.TradingDay,
                    Value = x.Price,
                    Volume = y.Volume
                };
            dataGridView1.DataSource = kapcsolt.ToList();*/

            List<decimal> Nyereségek = new List<decimal>();
            int intervalum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            var z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());
        }

        private void CreatePortfolio()
        {
            /*var p = new PortfolioItem();
            p.Index = "OTP";
            p.Volume = 10;
            Portfolio.Add(p);*/
            //egyszerűbben írva
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }
        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim() //trim azért kell, mert nchar(15)-ként van letárolva, ezért a üres részek ki vannak töltve 
                               && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath; //bin debug mappába kerülök automatikusan, ahova mentem a dolgokat

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            using (var sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
            {
                sw.WriteLine("Időszak;Nyereség");
                for (int i = 0; i < nyeresegekRendezve.Count(); i++)
                {
                    sw.WriteLine(string.Format(
                        "{0};{1}",
                        i,
                        nyeresegekRendezve[i]
                        ));
                }
            }

        }
    }
}
