using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace MobileDetect.Contracts
{
    /// <summary>
    /// The base feature detection rule.
    /// </summary>
    /// <remarks>This will not detect anything.</remarks>
    public abstract class BaseRules
    {
        /// <summary>
        /// Get the user agent header value.
        /// </summary>
        public virtual string GetUserAgent(ICollection<KeyValuePair<string, StringValues>> requestHeaders)
        {
            if (requestHeaders == null)
                return null;

            foreach (var header in requestHeaders)
            {
                if ("User-Agent".Equals(header.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return header.Value;
                }
            }
            return null;
        }

        private async System.Threading.Tasks.Task DoSomething() {
            await System.Threading.Tasks.Task.Delay(1);
        }

        /// <summary>
        /// Returns true if the collection contains a header which is a known mobile header.
        /// </summary>
        public virtual bool HasKnownMobileHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders) => false;

        /// <summary>
        /// Returns true if the collection contains a header which is a known tablet header.
        /// </summary>
        public virtual bool HasKnownTabletHeaders(ICollection<KeyValuePair<string, StringValues>> requestHeaders) => false;

        /// <summary>
        /// Returns true if the collection contains a `user agent` string which is a known mobile value.
        /// </summary>
        public virtual bool HasKnownMobileUserAgent(string userAgent) => false;

        /// <summary>
        /// Returns true if the collection contains a `user agent` string which is a known tablet value.
        /// </summary>
        public virtual bool HasKnownTabletUserAgent(string userAgent) => false;
    }
}
