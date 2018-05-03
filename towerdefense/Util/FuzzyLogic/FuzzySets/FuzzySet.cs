﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzySets
{
    public abstract class FuzzySet
    {
        protected double _memberDOM;
        protected double _memberRepValue;
        
        public double MemberDOM {
            get { return _memberDOM; }
            set {
                if (!(value > 1 || value < 0)) _memberDOM = value;
            }
        }
        public double MemberRepValue { get { return _memberRepValue; } }


        public FuzzySet(double repvalue)
        {

        }

        public abstract double CalculateDOM(double val);

        public void OrWithDOM(double val)
        {
           if (val > _memberDOM) _memberDOM = val;
        }

        public void ClearDOM()
        {
            _memberDOM = 0;
        }
    }
}
