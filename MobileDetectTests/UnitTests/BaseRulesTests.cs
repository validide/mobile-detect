using MobileDetectTests.TestData;
using System.Collections.Generic;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class BaseRulesTest
    {
        [Fact]
        public void Get_UserAgnt_Test()
        {
            var rules = new BaseRulesImplementation();
            var userAgent = "testUserAgent";

            Assert.Equal(null, rules.GetUserAgent(null));
            Assert.Equal(null, rules.GetUserAgent(new Dictionary<string, string>()));
            Assert.Equal(null, rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", null } }));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", userAgent } }));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "USER-AGENT", userAgent } }));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "user-agent", userAgent } }));
        }
    }
}
