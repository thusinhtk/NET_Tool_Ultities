using log4net;
using System.Reflection;

namespace Ultities.Logger
{
    class Logger
    {
        public static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ILog Log
        {
            get
            {
                return _log;
            }
        }
    }
}
