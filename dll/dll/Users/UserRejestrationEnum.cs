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
    }
}
