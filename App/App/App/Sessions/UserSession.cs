using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Sessions
{
    public static class UserSession
    {
        public static User? CurrentUser { get; set; }
    }
}
