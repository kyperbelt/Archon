using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archon.engine.utils
{
    public interface ALogger
    { 
        //used for logging messages
        void log(String message, String tag);
        //used for logging 
        void err(String tag, String message);
        void debug(String tag, String message);
    }
}
