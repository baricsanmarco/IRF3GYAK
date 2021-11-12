using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Factory_pattern.Abstractions //NAGYON FONTOS ELŐSZÖR ÁTNEVEZNI ABSTACTIONS-RE!!!!!! abstrakt osztály, önmagában nem felhasználható, hanem osztályokba épül be
{
    public abstract class Toy : Label //fontos beállítani, hogy abstract, vagyis nem lehet példányosítani, pl.: var t = new Toy();
    {
        public Toy()
        {
            AutoSize = false;
            Width = 50;
            Height = Width;
            Paint += Toy_Paint;
        }

        private void Toy_Paint(object sender, PaintEventArgs e) //ha módosul a design, akkor nem kell újrarajzolni
        {
            DrawImage(e.Graphics);
        }
        protected abstract void DrawImage(Graphics g); //absztrakt függvénynek nincsen kifejtve a tartalma. Protected = kívülről nem lehet meghívni, csak amit engedélyezünk
        
        public void MoveToy()
        {
            Left++;
        }
    }
}
