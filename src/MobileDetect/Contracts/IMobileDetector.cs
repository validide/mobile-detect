using System.Collections.Generic;

namespace MobileDetect.Contracts
{
    /// <summary>
    /// Base mobile detector contract.
    /// </summary>
    public interface IMobileDetector
    {
        /// <summary>
        /// Returns true if the device is a phone or tablet device.
        /// </summary>
        bool IsMobile();
        /// <summary>
        /// Returns true if the device is a tablet device.
        /// </summary>
        bool IsTablet();
    }
}
