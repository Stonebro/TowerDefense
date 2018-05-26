using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Entities.Powerups
{
    public class Coin : Powerup
    {

        public int bounty;

        public Coin(int xPos, int yPos, int width, int height)
        {
            Pos = new Rectangle(xPos, yPos, width, height);
            Img = Resources.Resources.Coin;
        }

        /// <summary>
        /// Used for drawing Coin.
        /// </summary>
        public override void Draw(Graphics g)
        {
            g.DrawImage(Img, Pos);
        }
    }
}
