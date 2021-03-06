using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;
using MobileDetect.Contracts;

namespace MobileDetect.MatchingRules
{
    /// <inheritdoc/>
    public partial class DefaultRules : BaseRules
    {
        #region Static stuff

        private static readonly Lazy<DefaultRules> DefaultRulesInstance = new Lazy<DefaultRules>(() => GetDefaultRules());

        private const RegexOptions ExpressionOptions = RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
        /// <summary>
        /// Get the regular expressions used to detect browser.
        /// </summary>
        public static Dictionary<string, Regex> GetBrowsersExpressions() => GetExpression(browserRegexStrings, ExpressionOptions);
        /// <summary>
        /// Get the regular expressions used to detect operating system.
        /// </summary>
        public static Dictionary<string, Regex> GetOsExpressions() => GetExpression(osRegexStrings, ExpressionOptions);
        /// <summary>
        /// Get the regular expressions used to detect phone type.
        /// </summary>
        public static Dictionary<string, Regex> GetPhoneExpressions() => GetExpression(phonesRegexStrings, ExpressionOptions);
        /// <summary>
        /// Get the regular expressions used to detect tablet type.
        /// </summary>
        public static Dictionary<string, Regex> GetTabletExpressions() => GetExpression(tabletRegexStrings, ExpressionOptions);
        /// <summary>
        /// Get the instance of the rule.
        /// </summary>
        public static DefaultRules Instance => DefaultRulesInstance.Value;

        private static DefaultRules GetDefaultRules() => new DefaultRules(
            userAgentHeaders,
            mobileHeaders,
            GetPhoneExpressions(),
            GetTabletExpressions(),
            GetOsExpressions(),
            GetBrowsersExpressions()
        );

        private static Dictionary<string, Regex> GetExpression(Dictionary<string, string> stringExpressions, RegexOptions options)
        {
            return stringExpressions.ToDictionary(kvp => kvp.Key, kvp => new Regex(kvp.Value, options));
        }

        #endregion Static stuff

        #region BaseRules Implementation

        private readonly string[] _userAgentHeaders;
        private readonly Dictionary<string, string[]> _knownMobileHeaders;
        private readonly Dictionary<string, Regex> _phoneRules;
        private readonly Dictionary<string, Regex> _tabletsRules;
        private readonly Dictionary<string, Regex> _osRules;
        private readonly Dictionary<string, Regex> _browserRules;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="userAgentHeaders">The user agent headers.</param>
        /// <param name="knownMobileHeaders">Known mobile headers.</param>
        /// <param name="phonesRules">Rules used to match phones.</param>
        /// <param name="tabletRules">Rules used to match tablets.</param>
        /// <param name="osRules">Rules used to match operating systems.</param>
        /// <param name="browserRules">Rules used to detect browser.</param>
        public DefaultRules(
            string[] userAgentHeaders,
            Dictionary<string, string[]> knownMobileHeaders,
            Dictionary<string, Regex> phonesRules,
            Dictionary<string, Regex> tabletRules,
            Dictionary<string, Regex> osRules,
            Dictionary<string, Regex> browserRules)
        {
            _userAgentHeaders = userAgentHeaders ?? throw new ArgumentNullException(nameof(userAgentHeaders));
            _knownMobileHeaders = knownMobileHeaders ?? throw new ArgumentNullException(nameof(knownMobileHeaders));
            _phoneRules = phonesRules ?? throw new ArgumentNullException(nameof(phonesRules));
            _tabletsRules = tabletRules ?? throw new ArgumentNullException(nameof(tabletRules));
            _osRules = osRules ?? throw new ArgumentNullException(nameof(osRules));
            _browserRules = browserRules ?? throw new ArgumentNullException(nameof(browserRules));
        }

        /// <inheritdoc/>
        public override string GetUserAgent(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (requestHeaders == null)
                return null;

            var sb = new StringBuilder();
            foreach (var userAgentHeader in _userAgentHeaders)
            {
                foreach (var item in requestHeaders)
                {
                    if (!userAgentHeader.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                        continue;

                    foreach (var headerValue in item.Value)
                    {
                        if (String.IsNullOrEmpty(headerValue))
                            continue;

                        sb.Append(headerValue);
                        sb.Append(" ");
                    }
                }
            }

            var userAgent = sb.ToString().Trim();
            return String.IsNullOrEmpty(userAgent)
                ? null
                : userAgent;
        }

        /// <inheritdoc/>
        public override bool HasKnownMobileHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (requestHeaders == null)
                return false; //no headers means not mobile

            if (IsCustomHeaderTrue(requestHeaders, "CloudFront-Is-Mobile-Viewer"))
                return true;

            foreach (var mobileHeader in _knownMobileHeaders)
            {
                foreach (var item in requestHeaders)
                {
                    if (!mobileHeader.Key.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (mobileHeader.Value == null || mobileHeader.Value.Length == 0)
                        return true; // the current header does not require a specific value, it's existence means it's mobile

                    foreach (var headerValue in item.Value)
                    {
                        if (String.IsNullOrEmpty(headerValue))
                            continue;

                        foreach (var mobileHeaderValue in mobileHeader.Value)
                        {
                            if (headerValue.IndexOf(mobileHeaderValue, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return base.HasKnownMobileHeaders(requestHeaders);
        }

        /// <inheritdoc/>
        public override bool HasKnownTabletHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (requestHeaders == null)
                return false; //no headers means not tablet

            if (IsCustomHeaderTrue(requestHeaders, "CloudFront-Is-Tablet-Viewer"))
                return true;

            return base.HasKnownTabletHeaders(requestHeaders);
        }

        /// <inheritdoc/>
        public override bool HasKnownMobileUserAgent(string userAgent)
        {
            if (userAgent == null)
                return false;

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

        /// <inheritdoc/>
        public override bool HasKnownTabletUserAgent(string userAgent)
        {
            if (userAgent == null)
                return false;

            if (AnyMatch(_tabletsRules, userAgent))
                return true;

            return base.HasKnownTabletUserAgent(userAgent);
        }

        private static bool IsCustomHeaderTrue(ICollection<KeyValuePair<string, StringValues>> requestHeaders, string headerName)
        {
            foreach (var item in requestHeaders)
            {
                if (!headerName.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var headerValue in item.Value)
                {
                    if (Boolean.TrueString.Equals(headerValue, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private static bool AnyMatch(Dictionary<string, Regex> rules, string testString)
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
