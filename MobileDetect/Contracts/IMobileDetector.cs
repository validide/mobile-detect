using System.Collections.Generic;

namespace MobileDetect.Contracts
{
    public interface IMobileDetector
    {
        bool IsMobile(Dictionary<string, string> requestHeaders);
        bool IsTablet(Dictionary<string, string> requestHeaders);
    }
}
