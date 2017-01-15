using MobileDetect.MatchingRules;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class DefaultRulesTest
    {
        [Fact]
        public void GetUserAgnt_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );


            Assert.Equal(null, rules.GetUserAgent(null));
            Assert.Equal(null, rules.GetUserAgent(new Dictionary<string, string>()));
            Assert.Equal(null, rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", null } }));

            var userAgent = "userAgent test string";

            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", userAgent } }));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "USER-AGENT", userAgent } }));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "user-agent", userAgent } }));

            Assert.Equal(userAgent + " " + userAgent, rules.GetUserAgent(new Dictionary<string, string> {
                { "User-Agent", userAgent },
                { "Test-User-Agent", userAgent}
            }));            
        }

        [Fact]
        public void HasKnownMobileHeaders_Rules_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>
                {
                    {"Known-Mobile-Header", null },
                    {"Known-Mobile-Header-With-Values", new [] { "some-value-a", "some-value-b" } }
                }),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );

            Assert.Equal(false, rules.HasKnownMobileHeaders(null));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string>()));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", null } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", "some-value-a" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "some-value-b" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", null } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", null } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", "some-totally-random-value" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", "some-other-totally-random-value" } }));
        }

        [Fact]
        public void HasKnownMobileHeaders_CloudFront_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );

            Assert.Equal(false, rules.HasKnownMobileHeaders(null));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string>()));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", null } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "" } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "false" } }));
            Assert.Equal(false, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "FAlSE" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "true" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "TRUE" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "tRuE" } }));
            Assert.Equal(true, rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-MOBILE-VIEWER", "tRuE" } }));
        }

        [Fact]
        public void HasKnownTabletHeaders_CloudFront_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );

            Assert.Equal(false, rules.HasKnownTabletHeaders(null));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string>()));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "Not-ATablet", "" } }));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", null } }));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "" } }));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "false" } }));
            Assert.Equal(false, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "FAlSE" } }));
            Assert.Equal(true, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "true" } }));
            Assert.Equal(true, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "TRUE" } }));
            Assert.Equal(true, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "tRuE" } }));
            Assert.Equal(true, rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-TABLET-VIEWER", "tRuE" } }));
        }

        [Fact]
        public void HasKnownMobileUserAgent_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );

            //this should fail so I remember to add more tests
            Assert.Equal(true, rules.HasKnownMobileUserAgent(null));
        }

        [Fact]
        public void HasKnownTabletUserAgent_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new ReadOnlyDictionary<string, string[]>(new Dictionary<string, string[]>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>()),
                new ReadOnlyDictionary<string, Regex>(new Dictionary<string, Regex>())
            );

            //this should fail so I remember to add more tests
            Assert.Equal(true, rules.HasKnownTabletUserAgent(null));
        }
    }
}
