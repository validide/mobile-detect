using System.Collections.Generic;

namespace MobileDetect.Contracts
{
    public interface IMobileDetector
    {
        bool IsMobile();
        bool IsTablet();
    }
}
