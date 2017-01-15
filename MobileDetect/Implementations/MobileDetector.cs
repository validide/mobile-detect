using System;
using System.Collections.Generic;
using MobileDetect.Contracts;

namespace MobileDetect.Implementations
{
    public class MobileDetector : IMobileDetector
    {
        private readonly BaseRules _matchingRules;

        public MobileDetector(BaseRules matchingRules)
        {
            if (matchingRules == null)
                throw new ArgumentNullException(nameof(matchingRules));

            _matchingRules = matchingRules;
        }

        public bool IsMobile(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders == null)
                return false; //no headers means not mobile
            
            if (_matchingRules.HasKnownMobileHeaders(requestHeaders))
                return true;

            var userAgent = _matchingRules.GetUserAgent(requestHeaders);
            if (String.IsNullOrEmpty(userAgent))
                return false;

            return _matchingRules.HasKnownMobileUserAgent(userAgent);
        }

        public bool IsTablet(Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders == null)
                return false; //no headers means not mobile

            if (_matchingRules.HasKnownTabletHeaders(requestHeaders))
                return true;

            var userAgent = _matchingRules.GetUserAgent(requestHeaders);
            if (String.IsNullOrEmpty(userAgent))
                return false;

            return _matchingRules.HasKnownTabletUserAgent(userAgent);
        }
    }
}
