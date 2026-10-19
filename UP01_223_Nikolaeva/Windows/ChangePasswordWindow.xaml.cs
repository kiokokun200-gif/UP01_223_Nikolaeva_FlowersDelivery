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
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(CurrentPass.Password) || string.IsNullOrEmpty(NewPass.Password) || string.IsNullOrEmpty(ConfirmNewPass.Password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            string curretnPass = CurrentPass.Password;
            string newPass = NewPass.Password;
            string confirmNewPass = ConfirmNewPass.Password;

            if (newPass != confirmNewPass)
            {
                MessageBox.Show("Новый пароль не совпадает");
                return;
            }

            else
            {
                string hashedPass = PasswordHash.GetHash(curretnPass);
                var user = Core.context.User.FirstOrDefault(u => u.Email == UserData.CurrentUser.Email && u.Password == hashedPass);
                if (user == null)
                {
                    MessageBox.Show("Текущий пароль введен неверно");
                    return;
                }
                else
                {
                    string message;
                    if(!PasswordChecker.CheckPassword(newPass, out message))
                    {
                        MessageBox.Show(message);
                        return;
                    }
                    else
                    {
                        user.Password = PasswordHash.GetHash(newPass);
                        Core.context.SaveChanges();
                        MessageBox.Show("Пароль сменен");
                        this.Close();
                    }
                }
            }

        }
    }
}
