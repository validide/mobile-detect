using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using MobileDetect.Contracts;

namespace MobileDetect.Implementations
{
    /// <summary>
    /// Base implementation of <see cref="IMobileDetector"/>.
    /// </summary>
    public class MobileDetector : IMobileDetector
    {
        private readonly BaseRules _matchingRules;
        private readonly ICollection<KeyValuePair<string, StringValues>> _requestHeaders;
        private readonly string _userAgent;

        /// <summary>
        /// The default mobile implementation of <see cref="IMobileDetector"/>.
        /// </summary>
        /// <param name="matchingRules">The rules used for matching.</param>
        /// <param name="requestHeaders">The request headers.</param>
        public MobileDetector(BaseRules matchingRules, ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            _matchingRules = matchingRules ?? throw new ArgumentNullException(nameof(matchingRules));
            _requestHeaders = requestHeaders;
            _userAgent = _matchingRules.GetUserAgent(_requestHeaders);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool IsTablet()
        {
            if (_requestHeaders == null)
                return false; //no headers means not tablet

            if (_matchingRules.HasKnownTabletHeaders(_requestHeaders))
                return true;

            if (String.IsNullOrEmpty(_userAgent))
                return false;

            return _matchingRules.HasKnownTabletUserAgent(_userAgent);
        }
    }
}
