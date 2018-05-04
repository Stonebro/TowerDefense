using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Entities.Weapons
{
    public class Sniper : Weapon
    {
        public Sniper() {
            attackRange = 10;
            attackPower = 5;
            attackInterval = 20;
        }
    }
}
