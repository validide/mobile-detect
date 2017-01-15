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
            var detector = new MobileDetector(rules);

            Assert.Equal(false, detector.IsMobile(null));
            Assert.Equal(false, detector.IsTablet(null));

            Assert.Equal(false, detector.IsMobile(new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } }));
            Assert.Equal(false, detector.IsTablet(new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } }));
        }

        [Fact]
        public void Using_TestRules_Test()
        {
            var rules = new TestRules();
            var detector = new MobileDetector(rules);

            Assert.Equal(false, detector.IsMobile(null));
            Assert.Equal(false, detector.IsTablet(null));

            Assert.Equal(true, detector.IsMobile(new Dictionary<string, string> { { TestRules.IsMobileDeviceHeaderName, null } }));
            Assert.Equal(true, detector.IsTablet(new Dictionary<string, string> { { TestRules.IsTabletDeviceHeaderName, null } }));

            Assert.Equal(true, detector.IsMobile(new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.MobileUserAgent } }));
            Assert.Equal(true, detector.IsTablet(new Dictionary<string, string> { { TestRules.UserAgentHeaderName, TestRules.TabletUserAgent } }));
        }
    }
}
