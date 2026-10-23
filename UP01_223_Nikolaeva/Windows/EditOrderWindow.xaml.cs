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
using System.Windows.Shapes;
using UP01_223_Nikolaeva.Model;

namespace UP01_223_Nikolaeva.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditOrderWindow.xaml
    /// </summary>
    public partial class EditOrderWindow : Window
    {
        private Order _order;
        private List<User> _couriers;
        private List<User> _florists;
        private List<OrderStatus> _statuses;
        public EditOrderWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            DataContext = _order;
            _couriers = Core.context.User.Where(u => u.RoleID == 4).ToList();
            var nullCourier = new User { ID = 0, Name = "[ Не выбран ]" };
            _couriers.Insert(0, nullCourier);

            _florists = Core.context.User.Where(u => u.RoleID == 3).ToList();
            var nullFlorist = new User { ID = 0, Name = "[ Не выбран ]" };
            _florists.Insert(0, nullFlorist);

            _statuses = Core.context.OrderStatus.ToList();
            ComboBoxCourier.ItemsSource = _couriers;
            ComboBoxFlorist.ItemsSource = _florists;
            ComboBoxStatus.ItemsSource = _statuses;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Выберите статус заказа");
                return;
            }
            if (_order.CourierID == 0) _order.CourierID = null;
            if (_order.FloristID == 0) _order.FloristID = null;

            Core.context.SaveChanges();

        }
    }
}
