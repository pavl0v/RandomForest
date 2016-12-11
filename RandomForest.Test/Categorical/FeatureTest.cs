using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib;
using RandomForest.Lib.Categorical;

namespace RandomForest.Test.Categorical
{
    [TestClass]
    public class FeatureTest
    {
        [TestMethod]
        public void ToString_Feature_String()
        {
            // arrange
            Feature<int> feature = new Feature<int>("Feature1");

            // act
            string str = feature.ToString();

            // assert
            Assert.AreEqual("Feature1:System.Int32", str);
        }

        [TestMethod]
        public void IsNumerical_FeatureIsNumerical_ReturnsTrue()
        {
            // arrange
            Feature<double> feature = new Feature<double>("Feature1");

            // act

            // assert
            Assert.AreEqual(true, feature.IsNumerical);
        }

        [TestMethod]
        public void IsNumerical_FeatureIsNotNumerical_ReturnsFalse()
        {
            // arrange
            Feature<string> feature = new Feature<string>("Feature1");

            // act

            // assert
            Assert.AreEqual(false, feature.IsNumerical);
        }
    }
}
