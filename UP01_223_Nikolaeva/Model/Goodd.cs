using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UP01_223_Nikolaeva.Model
{
    public partial class Good
    {
        public int ReviewCount
        {
            get
            {
                return Review.Count; 
            }
            set { }
        }

        public Visibility VisIsAdmin => UserData.CurrentUser.RoleID == 2 ? Visibility.Visible : Visibility.Collapsed;



    }
}
