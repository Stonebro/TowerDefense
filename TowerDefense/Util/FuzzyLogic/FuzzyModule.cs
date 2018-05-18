using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense.Util.FuzzyLogic
{
    /// <summary>
    ///  This is a rewrite of the FuzzyModule class authored by Mat Buckland and all credit goes to him.
    /// </summary>
    class FuzzyModule
    {
        private Dictionary<string, FuzzyVariable> _memberVarMap = new Dictionary<string, FuzzyVariable>();
        private List<FuzzyRule> _memberRules = new List<FuzzyRule>();

        public enum DefuzzifyMethod {MAX_AV, CENTROID}
        public const int NUMSAMPLES = 15;

        private void SetConfidencesOfConsequentsToZero()
        {
            foreach (FuzzyRule rule in _memberRules) rule.SetConfidenceOfConsequentToZero();
        }

        public FuzzyVariable CreateFLV(string name)
        {
            _memberVarMap[name] = new FuzzyVariable();
            return _memberVarMap[name];
        }

        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequent)
        {
            _memberRules.Add(new FuzzyRule(antecedent, consequent));
        }

        public void Fuzzify(string FLVName, double val)
        {
            if (_memberVarMap.ContainsKey(FLVName)) _memberVarMap[FLVName].Fuzzify(val);
        }

        public double DeFuzzify(string FLVName, DefuzzifyMethod method)
        {
            if (_memberVarMap.ContainsKey(FLVName))
            {
                SetConfidencesOfConsequentsToZero();

                foreach (FuzzyRule rule in _memberRules) rule.Calculate();

                switch(method)
                {
                    case DefuzzifyMethod.MAX_AV: return _memberVarMap[FLVName].DefuzzifyMaxAv();
                    case DefuzzifyMethod.CENTROID: return _memberVarMap[FLVName].DefuzzifyCentroid(NUMSAMPLES);
                }
            }
            return 0;
        } 

        public FuzzyVariable GetVar(string name)
        { 
            return _memberVarMap[name];
        }

        public void PrintAllDOMS()
        {
            foreach (FuzzyVariable var in _memberVarMap.Values) var.PrintDOMs(); 
        }
    }
}
