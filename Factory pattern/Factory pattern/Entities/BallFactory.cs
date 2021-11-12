﻿using Factory_pattern.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_pattern.Entities
{
    public class BallFactory : IToyFactory
    {
        /* public Ball CreateNew() //meghívóm a BallFactory-t és a Ball-t kapom vissza
         {
             return new Ball();
         }*/
        public Color BallColor { get; set; }
        public Toy CreateNew()
        {
            return new Ball(BallColor);
        }
    }
}
