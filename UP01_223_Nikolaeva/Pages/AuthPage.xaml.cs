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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtBoxLogin.Text;
            string password = PassPassword.Password;

            if (login == null || string.IsNullOrEmpty(login) || password == null || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if(Login(login, password))
            {
                MessageBox.Show("Успешный вход");
            }
            else
            {
                MessageBox.Show("Аккаунт не найден")
            }

        }

        private bool Login(string login, string password)
        {
            var user = Core.context.User.FirstOrDefault(u => u.Email == login && u.Password == password);
            if (user == null) return false;
            else
            {
                UserData.CurrentUser = user;
                return true;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            string name = TxtBoxRegName.Text;
            string login = TxtBoxRegLogin.Text;
            string phone = TxtBoxRegPhone.Text;
            string password = PassRegPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }


            if(Registr(login, password, phone, name))
            {
                MessageBox.Show("Регистрация успешна");
            }
        }

        private bool Registr(string login, string password, string phone, string name)
        {
            User newUser = new User()
            {
                Name = name,
                Email = login,
                Phone = phone,
                Password = password,
                RoleID = 1,

            };
            Core.context.User.Add(newUser);
            Core.context.SaveChanges(); 
            UserData.CurrentUser = newUser;
            return true;
        }
    }
}
