using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzySets
{
    /// <summary>
    ///  This is a rewrite of the FzSet class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public class FzSet : FuzzyTerm
    {

        private FuzzySet _memberSet;

        public FzSet(FuzzySet fuzzySet)
        {
            _memberSet = fuzzySet;
        }
        public void ClearDOM()
        {
            _memberSet.ClearDOM();
        }

        public FuzzyTerm Clone()
        {
            return new FzSet(_memberSet);
        }

        public double GetDOM()
        {
            return _memberSet.MemberDOM;
        }

        public void ORWithDOM(double val)
        {
            _memberSet.OrWithDOM(val);
        }
    }
}
