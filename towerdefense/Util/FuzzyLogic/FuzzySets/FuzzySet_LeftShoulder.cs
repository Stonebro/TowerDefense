using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzySets
{
    public class FuzzySet_LeftShoulder : FuzzySet
    {
        private double _memberPeak;
        private double _memberLeftOffset;
        private double _memberRightOffset;

        public FuzzySet_LeftShoulder(double peak, double left, double right) : base((peak + left) + peak / 2)
        {
            _memberPeak = peak;
            _memberLeftOffset = left;
            _memberRightOffset = right;
        }

        public override double CalculateDOM(double val)
        {
            // Test for the case where the left or right offsets are zero.
            // This is to prevent divide by 0 errors.

            if ((_memberRightOffset == 0 && _memberPeak == val) || (_memberLeftOffset == 0 && _memberPeak == val)) return 1.0;

            //find DOM if right of center
            else if ((val >= _memberPeak) && (val < (_memberPeak + _memberRightOffset)))
            {
                double grad = 1.0 / -_memberRightOffset;

                return grad * (val - _memberPeak) + 1.0;
            }

            //find DOM if left of center
            else if ((val < _memberPeak) && (val >= _memberPeak - _memberLeftOffset))
            {
                return 1.0;
            }

            //out of range of this FLV, return zero
            else
            {
                return 0.0;
            }
        }
    }
}
