using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Exceptions
{
    public static class ExceptionExtensions
    {
        private const string SEPARATOR = " --> ";

        public static string GetFullException(this Exception exception)
        {
            string message = CreateFullMessage(exception);
            return RemoveLastSeparator(message);
        }

        private static string CreateFullMessage(Exception exception)
        {
            string message = "";
            while (exception != null)
            {
                if (!message.Contains(exception.Message))
                    message += $"{exception.Message}{exception.Message}";
                exception = exception.InnerException;
            }
            return message;
        }

        private static string RemoveLastSeparator(string message)
        {
            if (message.Contains(SEPARATOR))
                message.Substring(0, message.Length - SEPARATOR.Length);
            return message;
        }
    }
}
