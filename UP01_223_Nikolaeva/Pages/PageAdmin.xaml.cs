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
using UP01_223_Nikolaeva.Windows;

namespace UP01_223_Nikolaeva.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        private List<Good> _goods;
        private List<String> _goodsCategories;
        private List<String> _goodsSort;
        private List<Order> _orders;
        private List<String> _orderStatuses;
        private List<String> _orderSort;
        private string search ="";
        private string selectedSort;
        private string selectefFiltr;
        public PageAdmin()
        {
            InitializeComponent();
        }

        private void LoadGoods()
        {
            _goods = Core.context.Good.ToList();
            _goodsCategories = Core.context.GoodCategory.Select(c => c.Name).ToList();
            _goodsCategories.Insert(0, "Все");
            _goodsSort = new List<string>
            {
                "По названию",
                "Сначала дешевле",
                "Сначала дороже",
                "По рейтингу"
            };

        }
        private void LoadOrders()
        {
            _orders = Core.context.Order.ToList();
            _orderStatuses = Core.context.OrderStatus.Select(o => o.Name).ToList();
            _orderStatuses.Insert(0, "Все");
            _orderSort = new List<string>
            {
                "По дате",
                "По итоговой цене"
            };

        }

        private void SearchPlaceHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TxtBoxSearch.Focus();
        }
        private void SetupGoods()
        {
            if (_goods == null) LoadGoods();

            
            ComboBoxSort.SelectedIndex = -1;
            ComboBoxFiltr.SelectedIndex = -1;

            ComboBoxSort.ItemsSource = _goodsSort;
            ComboBoxFiltr.ItemsSource = _goodsCategories;

            ComboBoxFiltr.SelectedIndex = 0; 
            ComboBoxSort.SelectedIndex = 0; 

            selectedSort = ComboBoxSort.SelectedItem?.ToString();
            selectefFiltr = ComboBoxFiltr.SelectedItem?.ToString();

            TxtBoxSearch.Text = "";
            SearchPlaceHolder.Text = "Найти товар";
            SearchPlaceHolder.Visibility = Visibility.Visible;

            DataGridGoods.ItemsSource = _goods;

        }
        private void TxtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(TabControlTable.SelectedItem is TabItem tab))
                return;
            if (TxtBoxSearch.Text.Length > 0)
            {
                SearchPlaceHolder.Visibility = Visibility.Collapsed;

            }
            else { SearchPlaceHolder.Visibility = Visibility.Visible; }

            search = TxtBoxSearch.Text;

            switch (tab.Tag?.ToString())
            {
                case "Good":
                    SearchSortFilterGoods();
                    break;

                case "User":
                    SearchSortFilterUsers();
                    break;

                case "Order":
                    SearchSortFilterOrders();
                    break;
            }


        }
        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSort.SelectedItem == null) return;
            if (!(TabControlTable.SelectedItem is TabItem tab))
                return;

            selectedSort = ComboBoxSort.SelectedItem.ToString();
            switch (tab.Tag?.ToString())
            {
                case "Good":
                    SearchSortFilterGoods();
                    break;

                case "User":
                    SearchSortFilterUsers();
                    break;

                case "Order":
                    SearchSortFilterOrders();
                    break;
            }
        }
        private void ComboBoxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFiltr.SelectedItem == null) return;
            if (!(TabControlTable.SelectedItem is TabItem tab))
                return;
            selectefFiltr = ComboBoxFiltr.SelectedItem.ToString();
            switch (tab.Tag?.ToString())
            {
                case "Good":
                    SearchSortFilterGoods();
                    break;

                case "User":
                    SearchSortFilterUsers();
                    break;

                case "Order":
                    SearchSortFilterOrders();
                    break;
            }
        }

        private void SearchSortFilterGoods()
        {
            var result = _goods;
            if (search.Length > 0)
            {
                result = result.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(selectefFiltr) && selectefFiltr != "Все")
            {
                result = result.Where(s => s.GoodCategory.Name == selectefFiltr).ToList();

            }

            
            if (selectedSort == "По названию")
            {
                result = result.OrderBy(s => s.Name).ToList();
            }
            else if (selectedSort == "Сначала дешевле")
            {
                result = result.OrderBy(u => u.Price).ToList();
            }
            else if (selectedSort == "Сначала дороже")
            {
                result = result.OrderByDescending(u => u.Price).ToList();
            }
            else result = result.OrderByDescending(u => u.Rating).ToList();

            DataGridGoods.ItemsSource = result.ToList();

        }
        private void SearchSortFilterUsers()
        {

        }
        private void SearchSortFilterOrders()
        {
            var result = _orders;
            if (search.Length > 0)
            {
                result = result.Where(s => s.User1.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(selectefFiltr) && selectefFiltr != "Все")
            {
                result = result.Where(s => s.OrderStatus.Name == selectefFiltr).ToList();

            }
            if (selectedSort == "По дате")
            {
                result = result.OrderBy(s => s.Date).ToList();
            }
            
            else result = result.OrderByDescending(u => u.TotalPrice).ToList();

            DataGridOrders.ItemsSource = result;
        }

        private void SetupOrders()
        {
            if (_orders == null) LoadOrders();

            ComboBoxSort.SelectedIndex = -1;
            ComboBoxFiltr.SelectedIndex = -1;

            ComboBoxSort.ItemsSource = _orderSort;
            ComboBoxFiltr.ItemsSource = _orderStatuses;

            ComboBoxFiltr.SelectedIndex = 0; 
            ComboBoxSort.SelectedIndex = 0;  

            selectedSort = ComboBoxSort.SelectedItem?.ToString();
            selectefFiltr = ComboBoxFiltr.SelectedItem?.ToString();

            TxtBoxSearch.Text = "";
            SearchPlaceHolder.Text = "Найти заказ по имени клиента";
            SearchPlaceHolder.Visibility = Visibility.Visible;

            DataGridOrders.ItemsSource = _orders;
        }
        private void TabControlTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source != TabControlTable)
                return;

            if (!(TabControlTable.SelectedItem is TabItem tab))
                return;

            switch (tab.Tag?.ToString())
            {
                case "Good":
                    SetupGoods();
                    BtnDiagram.Visibility = Visibility.Collapsed;
                    BtnAdd.Visibility = Visibility.Visible;
                    break;

                //case "User":
                //    SetupUsers();
                //    break;

                case "Order":
                    SetupOrders();
                    BtnDiagram.Visibility = Visibility.Visible;
                    BtnAdd.Visibility = Visibility.Collapsed;

                    break;
            }
        }

        private void BtnEditGood_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Good good = (Good)btn.DataContext;
            var wind = new EditGoodWindow(good);
            wind.ShowDialog();
            LoadGoods();

        }

        private void BtnDeleteGood_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Good good = (Good)btn.DataContext;
            MessageBoxResult res = MessageBox.Show($"Хотите удалить {good.Name}?", "Подтверждение", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                Core.context.Good.Remove(good);
                Core.context.SaveChanges();
                LoadGoods();
                SearchSortFilterGoods();
                MessageBox.Show("Товар удален");

                LoadGoods();
            }


        }

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Order order = (Order)btn.DataContext;
            var wind = new EditOrderWindow(order);
            wind.ShowDialog();
            LoadOrders();
        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Order order = (Order)btn.DataContext;
            MessageBoxResult res = MessageBox.Show($"Хотите удалить {order.ID} от {order.Date:d}?", "Подтверждение", MessageBoxButton.YesNo);
            if (res == MessageBoxResult.Yes)
            {
                Core.context.Order.Remove(order);
                Core.context.SaveChanges();
                LoadOrders();
                SearchSortFilterOrders();
                MessageBox.Show("Заказ удален");
                LoadOrders();


            }
        }

        private void BtnDiagram_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DiagrammPage());

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var wind = new AddGoodWindow();
            wind.ShowDialog();
            DataGridGoods.ItemsSource = Core.context.Good.ToList();
        }
    }
}
