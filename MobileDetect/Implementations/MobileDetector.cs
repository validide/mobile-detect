using MobileDetect.Contracts;
using System;
using System.Collections.Generic;

namespace MobileDetect.Implementations
{
    public class MobileDetector : IMobileDetector
    {
        public bool IsMobile(Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }

        public bool IsTablet(Dictionary<string, string> headers)
        {
            throw new NotImplementedException();
        }
    }
}
