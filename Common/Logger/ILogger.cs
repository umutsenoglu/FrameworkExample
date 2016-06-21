using System;
using System.Diagnostics;

namespace Common.Logger
{
    public interface ILogger
    {
        void Log(Severity severity, string log);
    }
   

}
