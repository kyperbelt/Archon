using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archon.engine.utils
{
    public interface ALogger
    { 
        void log(String tag, String message);
        void err(String tag, String message);
        void debug(String tag, String message);
    }
}
