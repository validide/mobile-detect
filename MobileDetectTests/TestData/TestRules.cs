using MobileDetect.Contracts;
using System;
using System.Collections.Generic;

namespace MobileDetectTests.TestData
{
    public class TestRules: BaseRules
    {
        public const string UserAgentHeaderName = "User-Agent";
        public const string IsMobileDeviceHeaderName = "Is-Mobile-Deice";
        public const string IsTabletDeviceHeaderName = "Is-Tablet-Deice";
        public const string MobileUserAgent = "Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Mobile Safari/535.19";
        public const string TabletUserAgent = "Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Safari/535.19";


        public override string GetUserAgent(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders == null || !requestHeaders.ContainsKey(UserAgentHeaderName))
                return null;

            return requestHeaders[UserAgentHeaderName];
        }

        public override bool HasKnownMobileHeaders(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders.ContainsKey(IsMobileDeviceHeaderName))
                return true;

            return base.HasKnownMobileHeaders(requestHeaders);
        }

        public override bool HasKnownTabletHeaders(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders.ContainsKey(IsTabletDeviceHeaderName))
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
    }
}
