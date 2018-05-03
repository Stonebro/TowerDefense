using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic
{
    ///  Interface for classes which are able to be used as terms in a fuzzy if-then rule base. 
    ///  Based on the FuzzyLogic source written by Mat Buckland.
    public interface FuzzyTerm
    {
        FuzzyTerm Clone();
        double GetDOM();
        void ClearDOM();
        void ORWithDOM(double val);
    }
}
