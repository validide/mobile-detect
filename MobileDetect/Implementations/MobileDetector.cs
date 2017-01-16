using System;
using System.Collections.Generic;
using MobileDetect.Contracts;

namespace MobileDetect.Implementations
{
    public class MobileDetector : IMobileDetector
    {
        private readonly BaseRules _matchingRules;
        private readonly Dictionary<string, string> _requestHeaders;
        private readonly string _userAgent;

        public MobileDetector(BaseRules matchingRules, Dictionary<string, string> requestHeaders)
        {
            if (matchingRules == null)
                throw new ArgumentNullException(nameof(matchingRules));

            _matchingRules = matchingRules;
            _requestHeaders = requestHeaders;
            _userAgent = _matchingRules.GetUserAgent(_requestHeaders);
        }

        public bool IsMobile()
        {
            if (_requestHeaders == null)
                return false; //no headers means not mobile
            
            if (_matchingRules.HasKnownMobileHeaders(_requestHeaders))
                return true;

            if (String.IsNullOrEmpty(_userAgent))
                return false;

            return _matchingRules.HasKnownMobileUserAgent(_userAgent);
        }

        public bool IsTablet()
        {
            if (_requestHeaders == null)
                return false; //no headers means not mobile

            if (_matchingRules.HasKnownTabletHeaders(_requestHeaders))
                return true;

            if (String.IsNullOrEmpty(_userAgent))
                return false;

            return _matchingRules.HasKnownTabletUserAgent(_userAgent);
        }
    }
}
