using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic.FuzzyOperators
{
    public class FzOR : FuzzyTerm
    {

        private List<FuzzyTerm> MemberTerms;

        public FzOR(ref FuzzyTerm operator1, ref FuzzyTerm operator2)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());

        }
        public FzOR(ref FuzzyTerm operator1, ref FuzzyTerm operator2, ref FuzzyTerm operator3)
        {
            MemberTerms.Add(operator1.Clone());
            MemberTerms.Add(operator2.Clone());
            MemberTerms.Add(operator3.Clone());
        }
        public FzOR(ref FuzzyTerm operator1, ref FuzzyTerm operator2, ref FuzzyTerm operator3, ref FuzzyTerm operator4)
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
            foreach (FuzzyTerm term in MemberTerms) term.ClearDOM();
        }

        public FuzzyTerm Clone()
        {
            return new FzOR(this);
        }

        public double GetDOM()
        {
            double largest = Double.MinValue;
            foreach (FuzzyTerm term in MemberTerms)
            {
                if (term.GetDOM() > largest) largest = term.GetDOM();
            }
            return largest;
        }

        public void ORWithDOM(double val)
        {
            foreach (FuzzyTerm term in MemberTerms) term.ORWithDOM(val);
        }
    }
}