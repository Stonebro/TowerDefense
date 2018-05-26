using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzyOperators
{
    /// <summary>
    ///  This is a rewrite of the FzAnd class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public class FzAND : IFuzzyTerm
    {

        private List<IFuzzyTerm> MemberTerms = new List<IFuzzyTerm>();

        public FzAND(IFuzzyTerm operator1, IFuzzyTerm operator2)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());

        }
        public FzAND(IFuzzyTerm operator1, IFuzzyTerm operator2, IFuzzyTerm operator3) {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());
            MemberTerms.Add(operator3.Clone());
        }
        public FzAND(IFuzzyTerm operator1, IFuzzyTerm operator2, IFuzzyTerm operator3, IFuzzyTerm operator4)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());
            MemberTerms.Add(operator3.Clone());
            MemberTerms.Add(operator4.Clone());
        }

        public FzAND(FzAND copy)
        {
            this.MemberTerms = copy.MemberTerms;
        }

        public void ClearDOM()
        {
            foreach (IFuzzyTerm term in MemberTerms) term.ClearDOM();
        }

        public IFuzzyTerm Clone()
        {
            return new FzAND(this);
        }

        public double GetDOM()
        {
            double smallest = Double.MaxValue;
            foreach(IFuzzyTerm term in MemberTerms)
            {
                if (term.GetDOM() < smallest) smallest = term.GetDOM();
            }
            return smallest;
        }

        public void ORWithDOM(double val)
        {
            foreach (IFuzzyTerm term in MemberTerms) term.ORWithDOM(val);
        }
    }
}
