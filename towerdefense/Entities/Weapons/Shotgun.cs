using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Entities.Weapons
{
    public class Shotgun : Weapon
    {
        public Shotgun() {
            attackRange = 3;
            attackPower = 40; // This is a percentage!
            attackInterval = 10;
        }
    }
}
