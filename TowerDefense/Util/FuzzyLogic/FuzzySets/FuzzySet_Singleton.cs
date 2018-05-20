using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzySets
{
    /// <summary>
    ///  This is a rewrite of the FuzzySet_Singleton class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public class FuzzySet_Singleton : FuzzySet
    {
        private double _memberMidPoint;
        private double _memberLeftOffset;
        private double _memberRightOffset;

        public FuzzySet_Singleton(double middle, double left, double right) : base(middle)
        {
            _memberMidPoint = middle;
            _memberLeftOffset = left;
            _memberRightOffset = right;
        }

        public override double CalculateDOM(double val)
        {
            if ((val >= _memberMidPoint - _memberLeftOffset) &&
                (val <= _memberMidPoint + _memberRightOffset))
            {
                return 1.0;
            }

            // Out of range of this FLV, return zero.
            else
            {
                return 0.0;
            }
        }
    }
}
