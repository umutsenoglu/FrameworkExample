using System;

namespace DataAccess.Exceptions
{
    public class DataAccessException:Exception
    {
        public string Statement { get; set; }

        public DataAccessException()
            :base()
        {

        }

        public DataAccessException(string message)
            :base(message)
        {

        }
        public DataAccessException(string message,Exception innerException)
            :base(message,innerException)
        {

        }
    }
}
