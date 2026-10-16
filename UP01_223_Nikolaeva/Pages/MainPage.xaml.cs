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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private List<Good> goods;
        private List<string> sort = new List<string> ();
        private List<string> goodCategories;
        private string search;
        private string selectedSort;
        private string selectefFiltr;
        public MainPage()
        {
            InitializeComponent();
            
            LoadData();
            
            
        }

        private void LoadData()
        {
            goods = Core.context.Good.ToList();
            ListBoxGoods.ItemsSource = goods;
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

        }

        private void SearchPlaceHolder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TxtBoxSearch.Focus();
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSort = ComboBoxSort.SelectedItem.ToString();
            SearchSortFilrt();
        }

        private void SearchSortFilrt()
        {
            

        }

        private void ComboBoxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectefFiltr = ComboBoxFiltr.SelectedItem.ToString();
        }
    }
}
