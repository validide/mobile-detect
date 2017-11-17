# mobile-detect
## About
A simple C# library to detect if the request is from a mobile device

This is based on the PHP version [Mobile-Detect](https://github.com/serbanghita/Mobile-Detect) (the current version has limited functionality compared to the original).


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
            var headers = Request.Headers.ToDictionary(k => k.Key, v => v.Value.ToString());
            var detector = new MobileDetector(DefaultRules.Instance, headers);
            var isMobile = detector.IsMobile(); //this is true is the request originates from a mobile phone or tablet
            var isTablet = detector.IsTablet(); //this is true is the request originates from a tablet

            /* Do some stuff */

            return View();
        }
    }
}
```