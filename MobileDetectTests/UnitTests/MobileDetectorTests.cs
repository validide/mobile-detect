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

            Assert.Equal(false, nullDetector.IsMobile());
            Assert.Equal(false, nullDetector.IsTablet());

            Assert.Equal(false, mobileDetector.IsMobile());
            Assert.Equal(false, tabletDetector.IsTablet());
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

            Assert.Equal(false, nullDetector.IsMobile());
            Assert.Equal(false, nullDetector.IsTablet());

            Assert.Equal(false, nullMobileDetector.IsMobile());
            Assert.Equal(false, nullTabletDetector.IsTablet());


            Assert.Equal(true, mobileDetector.IsMobile());
            Assert.Equal(true, tabletDetector.IsTablet());
        }
    }
}
