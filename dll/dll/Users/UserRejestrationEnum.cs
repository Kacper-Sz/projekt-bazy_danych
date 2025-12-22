using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Users
{
    public enum UserRejestrationEnum
    {
        GOOD,
        UNCOMPLETED_DATA,
        EMAIL_ALREADY_EXISTS,
        WEAK_PASSWORD,
        INVALID_EMAIL_FORMAT,
        PASSWORD_TOO_SHORT,
        PASSWORD_MISSING_UPPERCASE,
        PASSWORD_MISSING_LOWERCASE,
        PASSWORD_MISSING_DIGIT,
        PASSWORD_MISSING_SPECIAL_CHAR,
    }
}
