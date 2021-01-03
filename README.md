# mobile-detect
## About
A simple C# library to detect if the request is from a mobile device

This is based on the PHP version [Mobile-Detect](https://github.com/serbanghita/Mobile-Detect) (the current version has limited functionality compared to the original).

## Status
[![Coverage Status](https://coveralls.io/repos/github/validide/mobile-detect/badge.svg?branch=master)](https://coveralls.io/github/validide/mobile-detect?branch=master)
![Tests](https://github.com/validide/mobile-detect/workflows/Tests/badge.svg?branch=master)
[![Nuget](https://img.shields.io/nuget/v/MobileDetect?color=blue)](https://www.nuget.org/packages/MobileDetect/)
## Usage

```
/* Removed usings */
using MobileDetect.Implementations;
using MobileDetect.MatchingRules;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var detector = new MobileDetector(DefaultRules.Instance, Request.Headers);
            var isMobile = detector.IsMobile(); //this is true is the request originates from a mobile phone or tablet
            var isTablet = detector.IsTablet(); //this is true is the request originates from a tablet

            /* Do some stuff */

            return View();
        }
    }
}
```
