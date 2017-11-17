using MobileDetect.MatchingRules;
using System.Collections.Generic;
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
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            );


            Assert.Null(rules.GetUserAgent(null));
            Assert.Null(rules.GetUserAgent(new Dictionary<string, string>()));
            Assert.Null(rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", null } }));

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
                new Dictionary<string, string[]>
                {
                    {"Known-Mobile-Header", null },
                    {"Known-Mobile-Header-With-Values", new [] { "some-value-a", "some-value-b" } }
                },
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            );

            Assert.False(rules.HasKnownMobileHeaders(null));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string>()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", null } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", "some-value-a" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "some-value-b" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", null } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", null } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", "some-totally-random-value" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", "some-other-totally-random-value" } }));
        }

        [Fact]
        public void HasKnownMobileHeaders_CloudFront_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            );

            Assert.False(rules.HasKnownMobileHeaders(null));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string>()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", null } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "" } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "false" } }));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "FAlSE" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "true" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "TRUE" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "tRuE" } }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-MOBILE-VIEWER", "tRuE" } }));
        }

        [Fact]
        public void HasKnownTabletHeaders_CloudFront_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            );

            Assert.False(rules.HasKnownTabletHeaders(null));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string>()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "Not-ATablet", "" } }));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", null } }));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "" } }));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "false" } }));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "FAlSE" } }));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "true" } }));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "TRUE" } }));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "tRuE" } }));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-TABLET-VIEWER", "tRuE" } }));
        }

        [Fact]
        public void HasKnownMobileUserAgent_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>
                {
                    { "phone", new Regex("a-mobile-phone", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "tablet", new Regex("a-tablet", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "os", new Regex("mobile-os", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "phone", new Regex("mobile-browser", RegexOptions.IgnoreCase) }
                }
            );

            Assert.False(rules.HasKnownMobileUserAgent(null));
            Assert.False(rules.HasKnownMobileUserAgent("not-a-mobile"));
            Assert.True(rules.HasKnownMobileUserAgent("A-MOBILE-PHONE"));
            Assert.True(rules.HasKnownMobileUserAgent("a-tablet"));
            Assert.True(rules.HasKnownMobileUserAgent("mobile-os"));
            Assert.True(rules.HasKnownMobileUserAgent("MOBILE-BROWSer"));
        }

        [Fact]
        public void HasKnownTabletUserAgent_Test()
        {
            var rules = new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>
                {
                    { "phone", new Regex("a-mobile-phone", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "tablet", new Regex("a-tablet", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "os", new Regex("mobile-os", RegexOptions.IgnoreCase) }
                },
                new Dictionary<string, Regex>
                {
                    { "phone", new Regex("mobile-browser", RegexOptions.IgnoreCase) }
                }
            );

            Assert.False(rules.HasKnownTabletUserAgent(null));
            Assert.False(rules.HasKnownTabletUserAgent("not-a-mobile"));
            Assert.False(rules.HasKnownTabletUserAgent("A-MOBILE-PHONE"));
            Assert.True(rules.HasKnownTabletUserAgent("a-tablet"));
            Assert.False(rules.HasKnownTabletUserAgent("mobile-os"));
            Assert.False(rules.HasKnownTabletUserAgent("MOBILE-BROWSer"));
        }
    }
}
