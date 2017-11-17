using MobileDetect.Implementations;
using MobileDetectTests.TestData;
using System.Collections.Generic;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class MobileDetectorTests
    {
        [Fact]
        public void Using_BaseRules_Test()
        {
            var rules = new BaseRulesImplementation();
            var nullDetector = new MobileDetector(rules, null);
            var mobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } });
            var tabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } });

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
            var nullMobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, null } });
            var nullTabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, null } });
            var mobileDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } });
            var tabletDetector = new MobileDetector(rules, new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } });

            Assert.False(nullDetector.IsMobile());
            Assert.False(nullDetector.IsTablet());

            Assert.False(nullMobileDetector.IsMobile());
            Assert.False(nullTabletDetector.IsTablet());


            Assert.True(mobileDetector.IsMobile());
            Assert.True(tabletDetector.IsTablet());
        }
    }
}
