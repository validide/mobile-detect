using MobileDetect.Implementations;
using MobileDetectTests.TestData;
using System;
using System.Collections.Generic;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class MobileDetectorTests
    {
        [Fact]
        public void Constructor_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new MobileDetector(null, null));
        }

        [Fact]
        public void Using_BaseRules_Test()
        {
            var rules = new BaseRulesImplementation();
            var nullDetector = new MobileDetector(rules, null);
            var mobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } }.ToStringValuesCollection());
            var tabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } }.ToStringValuesCollection());

            Assert.False(nullDetector.IsMobile());
            Assert.False(nullDetector.IsTablet());

            Assert.False(mobileDetector.IsMobile());
            Assert.False(tabletDetector.IsTablet());
        }

        [Fact]
        public void Using_TestRules_Test()
        {
            var rules = new TestRules();
            var nullDetector = new MobileDetector(rules, null);
            var nullMobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, null } }.ToStringValuesCollection());
            var nullTabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, null } }.ToStringValuesCollection());
            var mobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } }.ToStringValuesCollection());
            var tabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } }.ToStringValuesCollection());
            var customMobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.IsMobileDeviceHeaderName, null } }.ToStringValuesCollection());
            var customTabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.IsTabletDeviceHeaderName, null } }.ToStringValuesCollection());

            Assert.False(nullDetector.IsMobile());
            Assert.False(nullDetector.IsTablet());

            Assert.False(nullMobileDetector.IsMobile());
            Assert.False(nullTabletDetector.IsTablet());

            Assert.True(mobileDetector.IsMobile());
            Assert.True(tabletDetector.IsTablet());

            Assert.True(customMobileDetector.IsMobile());
            Assert.True(customTabletDetector.IsTablet());
        }
    }
}
