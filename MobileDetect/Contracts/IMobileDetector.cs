using System.Collections.Generic;

namespace MobileDetect.Contracts
{
    public interface IMobileDetector
    {
        bool IsMobile(Dictionary<string, string> headers);
        bool IsTablet(Dictionary<string, string> headers);
    }
}
