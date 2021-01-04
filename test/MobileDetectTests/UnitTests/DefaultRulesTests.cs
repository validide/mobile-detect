using Microsoft.Extensions.Primitives;
using MobileDetect.MatchingRules;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;

namespace MobileDetectTests.UnitTests
{
    public class DefaultRulesTest
    {
        [Fact]
        public void Constructor_Tests()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                null,
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            ));

            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                null,
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            ));

            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                null,
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            ));

            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                null,
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            ));

            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                null,
                new Dictionary<string, Regex>()
            ));

            Assert.Throws<ArgumentNullException>(() => new DefaultRules(
                new string[] { "User-Agent", "Test-User-Agent" },
                new Dictionary<string, string[]>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                null
            ));
        }

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
            Assert.Null(rules.GetUserAgent(new Dictionary<string, string>().ToStringValuesCollection()));
            Assert.Null(rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", null } }.ToStringValuesCollection()));
            Assert.Null(rules.GetUserAgent(new Dictionary<string, string> { { "uSer-AgeTt", null } }.ToStringValuesCollection()));
            Assert.Null(rules.GetUserAgent(new[] {
                new KeyValuePair<string, StringValues>(null, "null")
            }));

            var userAgent = "userAgent test string";

            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "User-Agent", userAgent } }.ToStringValuesCollection()));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "USER-AGENT", userAgent } }.ToStringValuesCollection()));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> { { "user-agent", userAgent } }.ToStringValuesCollection()));
            Assert.Equal(userAgent, rules.GetUserAgent(new Dictionary<string, string> {
                { "some-header", null },
                { "foo", "bar" },
                { "user-agent", userAgent }
            }.ToStringValuesCollection()));

            Assert.Equal(userAgent + " " + userAgent, rules.GetUserAgent(new Dictionary<string, string> {
                { "User-Agent", userAgent },
                { "Test-User-Agent", userAgent}
            }.ToStringValuesCollection()));

            Assert.Equal("A B", rules.GetUserAgent(new[] {
                new KeyValuePair<string, StringValues>("User-Agent", new StringValues(new[] {"A", "B", null}))
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
                    {"Known-Mobile-Header-With-Values", new [] { "some-value-a", "some-value-b" } },
                    {"Known-Mobile-Header-With-Values-2", new [] { "some-value-a", null } }
                },
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>(),
                new Dictionary<string, Regex>()
            );

            Assert.False(rules.HasKnownMobileHeaders(null));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string>().ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", null } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new[] {
                new KeyValuePair<string, StringValues>("KNOWN-MOBILE-HEADER-WITH-VALUES", ""),
                new KeyValuePair<string, StringValues>("some-value", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("some-value", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null")),
                new KeyValuePair<string, StringValues>("bar", new StringValues(new[]{ null, "A", "B"}))
            }));
            Assert.False(rules.HasKnownMobileHeaders(new[] {
                new KeyValuePair<string, StringValues>("KNOWN-MOBILE-HEADER-WITH-VALUES", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("some-value", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null")),
                new KeyValuePair<string, StringValues>("bar", new StringValues(new[]{ null, "A", "B"}))
            }));
            Assert.False(rules.HasKnownMobileHeaders(new[] {
                new KeyValuePair<string, StringValues>("KNOWN-MOBILE-HEADER-WITH-VALUES", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null")),
                new KeyValuePair<string, StringValues>("bar", new StringValues(new[]{ null, "A", "B"}))
            }));
            Assert.False(rules.HasKnownMobileHeaders(new[] {
                new KeyValuePair<string, StringValues>("KNOWN-MOBILE-HEADER-WITH-VALUES", new StringValues(new[]{ null, "A", "B"})),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null"))
            }));
            Assert.False(rules.HasKnownMobileHeaders(new[] {
                new KeyValuePair<string, StringValues>("KNOWN-MOBILE-HEADER-WITH-VALUES", new StringValues(new[]{ null, "A", "B"})),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null"))
            }));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header-With-Values", "some-value-a" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER-WITH-VALUES", "some-value-b" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", null } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", null } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Known-Mobile-Header", "some-totally-random-value" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "KNOWN-MOBILE-HEADER", "some-other-totally-random-value" } }.ToStringValuesCollection()));
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
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string>().ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "Not-A-Mobile", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", null } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "false" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "FAlSE" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "true" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "TRUE" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CloudFront-Is-Mobile-Viewer", "tRuE" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownMobileHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-MOBILE-VIEWER", "tRuE" } }.ToStringValuesCollection()));
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
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string>().ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "Not-ATablet", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", null } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "false" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "FAlSE" } }.ToStringValuesCollection()));
            Assert.False(rules.HasKnownTabletHeaders(new[] {
                new KeyValuePair<string, StringValues>("CloudFront-Is-Tablet-Viewer", "aaa"),
                new KeyValuePair<string, StringValues>("some-value", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("some-value", new StringValues((string)null)),
                new KeyValuePair<string, StringValues>("foo", new StringValues("(string)null")),
                new KeyValuePair<string, StringValues>("bar", new StringValues(new[]{ null, "A", "B"}))
            }));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "true" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "TRUE" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CloudFront-Is-Tablet-Viewer", "tRuE" } }.ToStringValuesCollection()));
            Assert.True(rules.HasKnownTabletHeaders(new Dictionary<string, string> { { "CLOUDFRONT-IS-TABLET-VIEWER", "tRuE" } }.ToStringValuesCollection()));
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
