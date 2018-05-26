using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic
{
    /// <summary>
    ///  This is a rewrite of the FuzzyTerm class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    public interface IFuzzyTerm
    {
        IFuzzyTerm Clone();
        double GetDOM();
        void ClearDOM();
        void ORWithDOM(double val);
    }
}
