using System;
using System.Diagnostics;

namespace Common.Logger
{
    public class DebugLogger : ILogger
    {
        public void Log(Severity severity, string log)
        {
            switch (severity)
            {
                case Severity.Information:
                    Debug.WriteLine($"{severity}  => {log}");
                    break;
                case Severity.Error:
                    Debug.WriteLine($"{severity}  => {log}");
                    break;
                case Severity.Verbose:
                    Debug.WriteLine($"{severity}  => {log}");
                    break;
                case Severity.Warning:
                    Debug.WriteLine($"{severity}  => {log}");
                    break;
                default:
                    throw new NotImplementedException();
            }

            Debug.WriteLine(log);
        }
    }
}
