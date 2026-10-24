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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Good> goods;
        private List<string> sort = new List<string> ();
        private List<string> goodCategories;
        private string search = "";
        private string selectedSort;
        private string selectefFiltr;
        public MainPage()
        {
            InitializeComponent();
            LoadGoods();
            LoadData();
            
            
        }
        private void LoadGoods()
        {
            goods = Core.context.Good.ToList();
            ListBoxGoods.ItemsSource = goods;
        }
        private void LoadData()
        {
            
            sort.Add("Сначала лучшие");
            sort.Add("Сначала дешевле");
            sort.Add("Сначала дороже");
            ComboBoxSort.ItemsSource = sort;
            goodCategories = Core.context.GoodCategory.Select(u => u.Name).ToList();
            goodCategories.Insert(0, "Все");
            ComboBoxFiltr.ItemsSource = goodCategories;


        }

        private void TxtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtBoxSearch.Text.Length > 0)
            {
                SearchPlaceHolder.Visibility = Visibility.Collapsed;

            }
            else { SearchPlaceHolder.Visibility = Visibility.Visible; }

            search = TxtBoxSearch.Text;
            SearchSortFilrt();
            

        }

        private void SearchPlaceHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TxtBoxSearch.Focus();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxSort.SelectedItem == null) return;
            selectedSort = ComboBoxSort.SelectedItem.ToString();
            SearchSortFilrt();
        }

        private void SearchSortFilrt()
        {
            var result = goods;
            if (search.Length > 0)
            {
                result = result.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            if(selectefFiltr != "Все")
            {
                result = result.Where(s => s.GoodCategory.Name ==  selectefFiltr).ToList();
            }
            if (selectedSort == "Сначала лучшие")
            {
                result = result.OrderByDescending(s => s.Rating).ToList();
            }
            else if (selectedSort == "Сначала дороже")
            {
                result = result.OrderByDescending(u=> u.Price).ToList();
            }
            else result = result.OrderBy(u=> u.Price).ToList();

            ListBoxGoods.ItemsSource = result;


        }

        private void ComboBoxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxFiltr.SelectedItem == null) return;
            selectefFiltr = ComboBoxFiltr.SelectedItem.ToString();
            SearchSortFilrt();
        }


    }
}
