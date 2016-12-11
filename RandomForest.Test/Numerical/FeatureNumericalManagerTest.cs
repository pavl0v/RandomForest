using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical;
using System.Collections.Generic;
using RandomForest.Lib.Numerical.ItemSet.Feature;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class FeatureNumericalManagerTest
    {
        [TestMethod]
        public void GetFeatureNames_Features4Exc1_Returns3()
        {
            // arrange
            FeatureNumericalManager m = new FeatureNumericalManager();
            m.Add(new FeatureNumerical("F1"));
            m.Add(new FeatureNumerical("F2"));
            m.Add(new FeatureNumerical("F3"));
            m.Add(new FeatureNumerical("F4"));
            string resolutionFeatureName = "F2";

            // act
            List<string> names = m.GetFeatureNames(new List<string> { resolutionFeatureName });

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(3, names.Count);
            CollectionAssert.DoesNotContain(names, resolutionFeatureName);
        }

        [TestMethod]
        public void GetFeatureNames_Features4Exc1Ratio03_Returns1()
        {
            // arrange
            FeatureNumericalManager m = new FeatureNumericalManager();
            m.Add(new FeatureNumerical("F1"));
            m.Add(new FeatureNumerical("F2"));
            m.Add(new FeatureNumerical("F3"));
            m.Add(new FeatureNumerical("F4"));
            string resolutionFeatureName = "F2";

            // act
            List<string> names = m.GetFeatureNames(new List<string> { resolutionFeatureName }, 0.3f);

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(1, names.Count);
            CollectionAssert.DoesNotContain(names, resolutionFeatureName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetFeatureNames_Features4Exc1Ratio1_ThrowException()
        {
            // arrange
            FeatureNumericalManager m = new FeatureNumericalManager();
            m.Add(new FeatureNumerical("F1"));
            m.Add(new FeatureNumerical("F2"));
            m.Add(new FeatureNumerical("F3"));
            m.Add(new FeatureNumerical("F4"));
            string resolutionFeatureName = "F2";

            // act
            List<string> names = m.GetFeatureNames(new List<string> { resolutionFeatureName }, 1);

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(1, names.Count);
            CollectionAssert.DoesNotContain(names, resolutionFeatureName);
        }
    }
}
