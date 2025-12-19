using dll;
using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Sessions
{
    public static class Session
    {
        public static User? CurrentUser { get; set; }

        public static ShoppingCart? CurrentShoppingCart { get; set; }

        public static void ClearSession()
        {
            CurrentUser = null;
            CurrentShoppingCart = null;
        }
    }
}
