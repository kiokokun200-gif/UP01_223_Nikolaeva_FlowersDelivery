using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP01_223_Nikolaeva.Model;

namespace UP01_223_Nikolaeva.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrderPage.xaml
    /// </summary>
    public partial class EditOrderPage : Page
    {
        private Order _order;
        private List<User> _couriers;
        private List<User> _florists;
        //private List<string> _couriersNames;
        //private List<string> _floristsNames;
        public EditOrderPage(Order order)
        {
            InitializeComponent();
            _order = order;
            DataContext = _order;
            _couriers = Core.context.User.Where(u => u.RoleID == 4).ToList();
            _florists = Core.context.User.Where(u => u.RoleID == 3).ToList();
            //_couriersNames = _couriers.Select(us => us.Name).ToList();
            //_floristsNames = _florists.Select(us => us.Name).ToList();
            
            ComboBoxCourier.ItemsSource = _couriers;
            ComboBoxFlorist.ItemsSource = _florists;
         }

    }
}
