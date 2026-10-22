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
            TxtBoxSearch.Text = "";
            SearchPlaceHolder.Text = "Найти товар";

            if (_goods == null) LoadGoods();
            ComboBoxSort.ItemsSource = _goodsSort;
            ComboBoxFiltr.ItemsSource = _goodsCategories;
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

        }
        private void SearchSortFilterUsers()
        {

        }
        private void SearchSortFilterOrders()
        {

        }

        private void SetupOrders()
        {
            TxtBoxSearch.Text = "";
            SearchPlaceHolder.Text = "Найти заказ по имени клиента";

            if (_orders == null) LoadOrders();
            ComboBoxSort.ItemsSource = _orderSort;
            ComboBoxFiltr.ItemsSource = _orderStatuses;
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
                    break;

                //case "User":
                //    SetupUsers();
                //    break;

                case "Order":
                    SetupOrders();
                    break;
            }
        }

        private void BtnEditGood_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteGood_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
