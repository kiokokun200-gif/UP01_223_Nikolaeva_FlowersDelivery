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
    /// Логика взаимодействия для EditGoodWindow.xaml
    /// </summary>
    public partial class EditGoodWindow : Window
    {
        private Good _good;
        private List<GoodCategory> _goodCategories;
        public EditGoodWindow(Good good)
        {
            InitializeComponent();
            _good = good;
            this.DataContext = _good;
            _goodCategories = Core.context.GoodCategory.ToList();
            ComboBoxCategory.ItemsSource = _goodCategories;
            ComboBoxCategory.SelectedItem = _good.GoodCategory;
            ComboBoxCategory.DisplayMemberPath = Name;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtBoxName.Text;
            string price = TxtBoxPrice.Text;
            string image = TxtBoxImage.Text;
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price) )
            {
                MessageBox.Show("Поля не могут быть пустыми!");
                return;
            }
            decimal priceDecimal;
            bool parse = decimal.TryParse(price, out priceDecimal); 
            if(!parse)
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }
            else if (priceDecimal <= 0)
            {
                MessageBox.Show("Цена должна быть положительной");
                return;
            }

            _good.Price = priceDecimal;
            _good.ImagePath = image;
            _good.Name = name;
            _good.GoodCategory = (GoodCategory)ComboBoxCategory.SelectedValue;
            Core.context.SaveChanges();

            MessageBox.Show("Изменения сохранены");
            this.Close();
            


        }
    }
}
