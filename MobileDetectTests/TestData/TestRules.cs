using Microsoft.Extensions.Primitives;
using MobileDetect.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileDetectTests.TestData
{
    public class TestRules: BaseRules
    {
        public const string UserAgentHeaderName = "User-Agent";
        public const string IsMobileDeviceHeaderName = "Is-Mobile-Device";
        public const string IsTabletDeviceHeaderName = "Is-Tablet-Device";
        public const string MobileUserAgent = "Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Mobile Safari/535.19";
        public const string TabletUserAgent = "Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Safari/535.19";


        public override string GetUserAgent(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (requestHeaders == null || !ContainsKey(requestHeaders, UserAgentHeaderName))
                return null;

            return requestHeaders.FirstOrDefault(f=>f.Key == UserAgentHeaderName).Value;
        }

        public override bool HasKnownMobileHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (ContainsKey(requestHeaders, IsMobileDeviceHeaderName))
                return true;

            return base.HasKnownMobileHeaders(requestHeaders);
        }

        public override bool HasKnownTabletHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (ContainsKey(requestHeaders, IsTabletDeviceHeaderName))
                return true;

            return base.HasKnownTabletHeaders(requestHeaders);
        }

        public override bool HasKnownMobileUserAgent(string userAgent)
        {
            if (MobileUserAgent.Equals(userAgent, StringComparison.OrdinalIgnoreCase))
                return true;

            return base.HasKnownMobileUserAgent(userAgent);
        }

        public override bool HasKnownTabletUserAgent(string userAgent)
        {
            if (TabletUserAgent.Equals(userAgent, StringComparison.OrdinalIgnoreCase))
                return true;

            return base.HasKnownTabletUserAgent(userAgent);
        }

        private static bool ContainsKey(ICollection<KeyValuePair<string, StringValues>> items, string key) => items?.Any(a => a.Key == key) ?? false;
    }
}
