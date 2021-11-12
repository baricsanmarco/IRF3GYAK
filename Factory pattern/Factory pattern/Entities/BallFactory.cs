using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_pattern.Entities
{
    public class BallFactory //meghívóm a BallFactory-t és a Ball-t kapom vissza
    {
        public Ball CreateNew()
        {
            return new Ball();
        }
    }
}
