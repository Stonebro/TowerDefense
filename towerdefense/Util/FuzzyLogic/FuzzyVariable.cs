using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerDefense.Util.FuzzyLogic.FuzzySets;

namespace TowerDefense.Util.FuzzyLogic
{
    /// <summary>
    /// This is a conversion of the c++ source code for Fuzzy Logic provided by Mat Buckland.
    /// </summary>
    public class FuzzyVariable
    {
        private Dictionary<string, FuzzySet> _memberSets;
        private double _memberMinRange;
        private double _memberMaxRange;

        //--------------------------- Fuzzify -----------------------------------------
        //
        //  Takes a crisp value and calculates its degree of membership for each set
        //  in the variable.
        //-----------------------------------------------------------------------------
        public void Fuzzify(double val)
        {
            // Asserts the value in within bounds of the variable.
            if (val >= _memberMinRange && val <= _memberMaxRange)
                // For each set in the FLV calculate the DOM for the given value and set the DOM to this value.
                foreach (FuzzySet set in _memberSets.Values) set.MemberDOM = set.CalculateDOM(val);
        }

        //--------------------------- DeFuzzifyMaxAv ----------------------------------
        //
        // Defuzzifies the value by averaging the maxima of the sets that have fired.
        //
        // OUTPUT = sum (maxima * DOM) / sum (DOMs) 
        //-----------------------------------------------------------------------------
        public double DefuzzifyMaxAv()
        {
            double bottom = 0;
            double top = 0;

            foreach(FuzzySet set in _memberSets.Values)
            {
                bottom += set.MemberDOM;
                top += set.MemberRepValue * set.MemberDOM;
            }

            // Dividing by 0 is nonsense check.
            if (bottom == 0) return 0;

            return top / bottom;
        }

        //------------------------- DeFuzzifyCentroid ---------------------------------
        //
        //  Defuzzify the variable using the centroid method.
        //-----------------------------------------------------------------------------
        public double DefuzzifyCentroid(int numSamples)
        {
            // Calculate the step size.
            double stepSize = (_memberMaxRange - _memberMinRange) / (double)numSamples;

            double totalArea = 0;
            double sumOfMoments = 0;

            // Step through the range of this variable in increments equal to StepSize.
            // Adding up the contribution (lower of CalculateDOM or the actual DOM of this
            // variable's fuzzified value) for each subset. This gives an approximation of
            // the total area of the fuzzy manifold.(This is similar to how the area under
            // a curve is calculated using calculus. The heights of lots of 'slices' are
            // summed to give the total area.)
            //
            // In addition the moment of each slice is calculated and summed. Dividing
            // the total area by the sum of the moments gives the centroid. (Just like
            // calculating the center of mass of an object)

            for(int i = 0; i < numSamples; i++)
            {
                foreach(FuzzySet set in _memberSets.Values)
                {
                    double contribution = Math.Min(set.CalculateDOM(_memberMinRange + i * stepSize), set.MemberDOM);

                    totalArea += contribution;
                    sumOfMoments += (_memberMinRange + i * stepSize) * contribution;
                }
            }
            // Dividing by 0 is nonsense check.
            if (totalArea == 0) return 0;

            return (sumOfMoments / totalArea);
        }

        //---------------------------- AdjustRangeToFit -------------------------------
        //
        //  This method is called with the upper and lower bound of a set each time a
        //  new set is added to adjust the upper and lower range values accordingly.
        //-----------------------------------------------------------------------------
        public void AdjustRangeToFit(double minBound, double maxBound)
        {
            if (minBound < _memberMinRange) _memberMinRange = minBound;
            if (maxBound > _memberMaxRange) _memberMaxRange = maxBound;
        }

        //--------------------------- AddLeftShoulder ---------------------------------
        //
        //  Adds a left shoulder type set.
        //-----------------------------------------------------------------------------
        public FzSet AddLeftShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            _memberSets[name] = new FuzzySet_LeftShoulder(peak, peak - minBound, maxBound - peak);

            // Adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }

        //--------------------------- AddRightShoulder ---------------------------------
        //
        //  Adds a right shoulder type set.
        //-----------------------------------------------------------------------------
        public FzSet AddRightShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            _memberSets[name] = new FuzzySet_RightShoulder(peak, peak - minBound, maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }

        //------------------------- AddTriangularSet ----------------------------------
        //
        //  Adds a triangular shaped fuzzy set to the variable.
        //-----------------------------------------------------------------------------
        public FzSet AddTriangularSet(string name,
                                   double minBound,
                                   double peak,
                                   double maxBound)
        {
            _memberSets[name] = new FuzzySet_Triangle(peak, peak - minBound, maxBound - peak);
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }

        //--------------------------- AddSingletonSet ---------------------------------
        //
        //  Adds a singleton to the variable.
        //-----------------------------------------------------------------------------
        public FzSet AddSingletonSet(string name,
                                     double minBound,
                                     double peak,
                                     double maxBound)
        {
            _memberSets[name] = new FuzzySet_Singleton(peak,
                                            peak - minBound,
                                            maxBound - peak);

            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(_memberSets[name]);
        }

        public void PrintDOMs()
        {
            foreach(KeyValuePair<string, FuzzySet> set in _memberSets)
            {
                Console.WriteLine("key = " + set.Key + " value is " + set.Value);
            }

            Console.WriteLine("minbound = " + _memberMinRange + " maxbound = " + _memberMaxRange);
        }
    }
}
