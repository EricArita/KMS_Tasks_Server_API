using NLog;
using NLog.Web;

namespace MB.Infrastructure.Services.ThirdParty {
    public static class NLoggerService
    {
        private static Logger instance = null;

        public static Logger GetLogger()
        {
            if (instance == null)
            {
                instance = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            }
            return instance;
        }
    }
}
