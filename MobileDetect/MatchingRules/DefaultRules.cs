using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MobileDetect.Contracts;

namespace MobileDetect.MatchingRules
{
    public partial class DefaultRules: BaseRules
    {
        #region Static stuff

        private static volatile object _instanceLock;
        private static DefaultRules _instance;

        public static readonly RegexOptions ExpressionOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
        public static readonly ReadOnlyDictionary<string, Regex> BrowsersExpressions;
        public static readonly ReadOnlyDictionary<string, Regex> OsExpressions;
        public static readonly ReadOnlyDictionary<string, Regex> PhoneExpressions;
        public static readonly ReadOnlyDictionary<string, Regex> TabletExpressions;

        static DefaultRules()
        {
            _instanceLock = new object();
            BrowsersExpressions = GetExpression(BrowserRegexStrings, ExpressionOptions);
            OsExpressions = GetExpression(OsRegexStrings, ExpressionOptions);
            PhoneExpressions = GetExpression(PhonesRegexStrings, ExpressionOptions);
            TabletExpressions = GetExpression(TabletRegexStrings, ExpressionOptions);
        }

        public static DefaultRules Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DefaultRules(UserAgentHeaders, MobileHeaders, UserAgentHeaders, PhoneExpressions, TabletExpressions, OsExpressions, BrowsersExpressions);
                        }
                    }
                }
                return _instance;
            }
        }

        private static ReadOnlyDictionary<string, Regex> GetExpression(ReadOnlyDictionary<string, string> stringExpressions, RegexOptions options)
        {
            var dictionary = stringExpressions.ToDictionary(kvp => kvp.Key, kvp => new Regex(kvp.Value, options));
            return new ReadOnlyDictionary<string, Regex>(dictionary);
        }
        
        #endregion Static stuff

        #region BaseRules Implementation

        private readonly string[] _userAgentHeaders;
        private readonly ReadOnlyDictionary<string, string[]> _knownMobileHeaders;
        private readonly ReadOnlyDictionary<string, Regex> _phoneRules;
        private readonly ReadOnlyDictionary<string, Regex> _tabletsRules;
        private readonly ReadOnlyDictionary<string, Regex> _osRules;
        private readonly ReadOnlyDictionary<string, Regex> _browserRules;

        public DefaultRules(
            string[] userAgentHeaders,
            ReadOnlyDictionary<string, string[]> knownMobileHeaders,
            string[] knownUserAgentHeaders,
            ReadOnlyDictionary<string, Regex> phonesRules,
            ReadOnlyDictionary<string, Regex> tabletRules,
            ReadOnlyDictionary<string, Regex> osRules,
            ReadOnlyDictionary<string, Regex> browserRules)
            : base()
        {
            if (userAgentHeaders == null)
                throw new ArgumentNullException(nameof(userAgentHeaders));

            if (knownMobileHeaders == null)
                throw new ArgumentNullException(nameof(knownMobileHeaders));

            if (phonesRules == null)
                throw new ArgumentNullException(nameof(phonesRules));

            if (tabletRules == null)
                throw new ArgumentNullException(nameof(tabletRules));

            if (osRules == null)
                throw new ArgumentNullException(nameof(osRules));

            if (browserRules == null)
                throw new ArgumentNullException(nameof(browserRules));


            _userAgentHeaders = userAgentHeaders;
            _knownMobileHeaders = knownMobileHeaders;

            _phoneRules = phonesRules;
            _tabletsRules = tabletRules;
            _osRules = osRules;
            _browserRules = browserRules;
        }

        public override string GetUserAgent(Dictionary<string, string> requestHeaders)
        {
            var sb = new StringBuilder();
            foreach (var userAgentHeader in _userAgentHeaders)
            {
                if (!requestHeaders.ContainsKey(userAgentHeader))
                    continue;

                var headerValue = requestHeaders[userAgentHeader];
                if (String.IsNullOrEmpty(headerValue))
                    continue;

                sb.Append(headerValue);
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        public override bool HasKnownMobileHeaders(Dictionary<string, string> requestHeaders)
        {
            if (IsCloudFrontMobile(requestHeaders))
                return true;

            if (_knownMobileHeaders == null)
                return false; //no headers means not mobile

            foreach (var mobileHeader in _knownMobileHeaders)
            {
                if (!requestHeaders.ContainsKey(mobileHeader.Key))
                    continue;

                if (mobileHeader.Value == null || mobileHeader.Value.Length == 0)
                    return true; // the current header does not require a specific value, it's existence means it's mobile

                var headerValue = requestHeaders[mobileHeader.Key] ?? String.Empty;
                foreach (var mobileHeaderValue in mobileHeader.Value)
                {
                    if (headerValue.IndexOf(mobileHeaderValue, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        return true;
                    }
                }
            }

            return base.HasKnownMobileHeaders(requestHeaders);
        }

        public override bool HasKnownTabletHeaders(Dictionary<string, string> requestHeaders)
        {
            if (IsCloudFrontTablet(requestHeaders))
                return true;

            return base.HasKnownTabletHeaders(requestHeaders);
        }

        public override bool HasKnownMobileUserAgent(string userAgent)
        {
            if (AnyMatch(_phoneRules, userAgent))
                return true;

            if (AnyMatch(_tabletsRules, userAgent))
                return true;

            if (AnyMatch(_osRules, userAgent))
                return true;

            if (AnyMatch(_browserRules, userAgent))
                return true;

            return base.HasKnownMobileUserAgent(userAgent);
        }

        public override bool HasKnownTabletUserAgent(string userAgent)
        {
            if (AnyMatch(_tabletsRules, userAgent))
                return true;

            return base.HasKnownTabletUserAgent(userAgent);
        }

        private bool IsCloudFrontMobile(Dictionary<string, string> requestHeaders)
        {
            const string headerName = "CloudFront-Is-Mobile-Viewer";
            if (!requestHeaders.ContainsKey(headerName))
                return false;

            var isMobile = requestHeaders[headerName];

            return Boolean.TrueString.Equals(isMobile, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsCloudFrontTablet(Dictionary<string, string> requestHeaders)
        {
            const string headerName = "CloudFront-Is-Tablet-Viewer";
            if (!requestHeaders.ContainsKey(headerName))
                return false;

            var isMobile = requestHeaders[headerName];

            return Boolean.TrueString.Equals(isMobile, StringComparison.OrdinalIgnoreCase);
        }

        private static bool AnyMatch(ReadOnlyDictionary<string, Regex> rules, string testString)
        {
            if (rules != null)
            {
                foreach (var rule in rules.Where(w => w.Value != null))
                {
                    if (rule.Value.IsMatch(testString))
                        return true;
                }
            }
            return false;
        }

        #endregion BaseRules Implementation
    }
}
