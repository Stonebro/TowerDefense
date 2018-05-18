using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic
{
    /// <summary>
    ///  This is a rewrite of the FuzzyRule class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    class FuzzyRule
    {
        private FuzzyTerm antecedent;
        private FuzzyTerm consequent;

        public FuzzyRule(FuzzyTerm antecedent, FuzzyTerm consequent)
        {
            this.antecedent = antecedent;
            this.consequent = consequent;
        }

        public void SetConfidenceOfConsequentToZero()
        {
            consequent.ClearDOM();
        }

        public void Calculate()
        {
            consequent.ORWithDOM(antecedent.GetDOM());
        }
    }
}
