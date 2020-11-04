using NLog;
using NLog.Web;

namespace Infrastructure.Persistence.Services {
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
