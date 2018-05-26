using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Entities.Weapons
{
    public class Sniper : Weapon
    {
        public Sniper()
        {
            attackRange = 10;
            // Amount of damage in health points.
            attackPower = 5;
            attackInterval = 20;
        }
    }
}
