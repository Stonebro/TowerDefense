using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Entities.Powerups
{
    public abstract class Powerup
    {
        public Image Img { get; set; }
        public Rectangle Pos { get; set; }

        public abstract void Draw(Graphics g);
    }
}
