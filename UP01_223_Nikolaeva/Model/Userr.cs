using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UP01_223_Nikolaeva.Model
{
    public partial class User
    {
        public Visibility VisIsAdmin => RoleID == 2 ? Visibility.Visible : Visibility.Collapsed;
    }
}
