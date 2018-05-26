using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzyOperators
{
    /// <summary>
    ///  This is a rewrite of the FzOR class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public class FzOR : IFuzzyTerm
    {

        private List<IFuzzyTerm> MemberTerms = new List<IFuzzyTerm>();

        public FzOR(ref IFuzzyTerm operator1, ref IFuzzyTerm operator2)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());

        }
        public FzOR(ref IFuzzyTerm operator1, ref IFuzzyTerm operator2, ref IFuzzyTerm operator3)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());
            MemberTerms.Add(operator3.Clone());
        }
        public FzOR(ref IFuzzyTerm operator1, ref IFuzzyTerm operator2, ref IFuzzyTerm operator3, ref IFuzzyTerm operator4)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());
            MemberTerms.Add(operator3.Clone());
            MemberTerms.Add(operator4.Clone());
        }

        public FzOR(FzOR copy)
        {
            this.MemberTerms = copy.MemberTerms;
        }

        public void ClearDOM()
        {
            foreach (IFuzzyTerm term in MemberTerms) term.ClearDOM();
        }

        public IFuzzyTerm Clone()
        {
            return new FzOR(this);
        }

        public double GetDOM()
        {
            double largest = Double.MinValue;
            foreach (IFuzzyTerm term in MemberTerms)
            {
                if (term.GetDOM() > largest) largest = term.GetDOM();
            }
            return largest;
        }

        public void ORWithDOM(double val)
        {
            foreach (IFuzzyTerm term in MemberTerms) term.ORWithDOM(val);
        }
    }
}