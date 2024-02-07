using Shyjus.BrowserDetection;

namespace CarbonFootprint1.Methods
{
    public class SessionManager : ISessionManagerService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBrowserDetector _browserDetector;

        public SessionManager(
            IHttpContextAccessor contextAccessor,
            IBrowserDetector browserDetector
            )
        {
            _contextAccessor = contextAccessor;
            _browserDetector = browserDetector;
        }


        public bool CheckBrowserSession()
        {
            var isLoggedIn = _contextAccessor.HttpContext.Session.GetString("isLoggedIn");
            var isSessionActive = true;

            var browserData = _browserDetector.Browser;
            var browserinfoNow = browserData.Name + browserData.Version + browserData.DeviceType + browserData.OS;

            var browserInfo = _contextAccessor.HttpContext.Session.GetString("browserInfo");

            if (browserInfo != null)
            {
                if (browserInfo.ToUpper() != browserinfoNow.ToUpper())
                {
                    isSessionActive = false;
                }
                else
                {
                    if (isLoggedIn == null)
                    {
                        isSessionActive = false;
                    }
                    else if (isLoggedIn == "YES")
                    {
                        isSessionActive = true;
                    }
                }
            }
            else
            {
                isSessionActive = false;
            }


            return isSessionActive;
        }

    }
}
