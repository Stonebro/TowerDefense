﻿using System;
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
        private IFuzzyTerm antecedent;
        private IFuzzyTerm consequent;

        public FuzzyRule(IFuzzyTerm antecedent, IFuzzyTerm consequent)
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
