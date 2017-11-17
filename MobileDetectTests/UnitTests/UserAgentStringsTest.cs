using MobileDetect.Implementations;
using MobileDetect.MatchingRules;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class UserAgentStringsTest
    {
        private static readonly DefaultRules rules = DefaultRules.Instance;

        [Theory]
        [MemberData(nameof(UserAgentStrings))]
        public void GetUserAgnt_Test(UserAgent uaData)
        {
            Assert.Equal(uaData.mobile, rules.HasKnownMobileUserAgent(uaData.user_agent));
            Assert.Equal(uaData.tablet, rules.HasKnownTabletUserAgent(uaData.user_agent));
        }

        [Theory]
        [MemberData(nameof(UserAgentStrings))]
        public void MobileDetector_Test(UserAgent uaData)
        {
            var detector = new MobileDetector(rules, new Dictionary<string, string> { { "User-Agent", uaData.user_agent } });

            Assert.Equal(uaData.mobile, detector.IsMobile());
            Assert.Equal(uaData.tablet, detector.IsTablet());
        }

        public static IEnumerable<object[]> UserAgentStrings
        {
            get
            {
                using (var r = new StreamReader("..\\..\\..\\TestData\\ua-tests.generated.json"))
                {
                    string json = r.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<UserAgentData>(json);
                    return data.user_agents.Select(s => new[] { s });
                }
            }
        }

        public class UserAgentData
        {
            public string hash { get; set; }
            public UserAgent[] user_agents { get; set; }
        }

        public class UserAgent
        {
            public string vendor { get; set; }
            public string user_agent { get; set; }
            public bool mobile { get; set; }
            public bool tablet { get; set; }
            public Dictionary<string, string> version { get; set; }
            public string model { get; set; }
        }
    }
}
