using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using Quanti.Utils.Extensions;

namespace Quanti.Utils.UnitTests
{
    [TestClass]
    public class EnumHelperUnitTests
    {
        private enum TestEnumType
        {
            None,
            Value1,
            Value2,
            Value3
        }

        private enum TestEnum2Type
        {
            Value
        }

        private const string DisplayAttributeName = "Test value 1";

        private enum TestEnumDisplayAttributeType
        {
            [Display(Name =DisplayAttributeName)]
            Value1,
            Value2
        }

        /// <summary>
        /// Tests that missing localization does not cause exception and that localization is equal to the name of the value in enumeration.
        /// </summary>
        [TestMethod]
        public void TestMissingLocalization()
        {
            var localizations = Quanti.Utils.Helpers.EnumHelper.GetLocalizations<TestEnum2Type>(Resources.ResourceManager);
            var localization = localizations.Single();
            Assert.IsTrue(localization.Text == localization.Value.ToString());
        }

        [TestMethod]
        public void TestCount()
        {
            var localizations = Quanti.Utils.Helpers.EnumHelper.GetLocalizations<TestEnumType>(Resources.ResourceManager);
            Assert.IsTrue(localizations.Count() == Enum.GetValues(typeof(TestEnumType)).Length);
        }

        /// <summary>
        /// Tests that when default value should be ignored that result localizations contains
        /// all values except one and that missing value is default value.
        /// </summary>
        [TestMethod]
        public void TestIgnoreDefaultValue()
        {
            var defaultValue = TestEnumType.None;
            var localizations = Quanti.Utils.Helpers.EnumHelper.GetLocalizations<TestEnumType>(Resources.ResourceManager, defaultValue);
            Assert.IsTrue(localizations.Count() == Enum.GetValues(typeof(TestEnumType)).Length - 1);
            Assert.IsFalse(localizations.Any(l => l.Value == defaultValue));

            Assert.IsTrue(false); // git lab test of handling failure job
        }

        [TestMethod]
        public void TestCulture()
        {
            var cultureInfo = new CultureInfo("cs-Cz");
            var localizations = Quanti.Utils.Helpers.EnumHelper.GetLocalizations<TestEnumType>(Resources.ResourceManager, cultureInfo);

            // get localization for value we want to test
            var localization = localizations.Single(l => l.Value == TestEnumType.Value1);

            // get text from resource for test culture
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            var resourceText = Resources.TestEnumType_Value1;

            Assert.IsTrue(localization.Text == resourceText);
        }

        [TestMethod]
        public void TestDisplayAttribute()
        {
            var localizations = Quanti.Utils.Helpers.EnumHelper.GetLocalizationsByDisplayAttribute<TestEnumDisplayAttributeType>();
        }

        [TestMethod]
        public void TestDisplayAttributeExtension()
        {
            var value = TestEnumDisplayAttributeType.Value1;
            var text = value.GetNameFromDisplayAttribute();
            Assert.IsTrue(text == DisplayAttributeName);
        }
    }
}
