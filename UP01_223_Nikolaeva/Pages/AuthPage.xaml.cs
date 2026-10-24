using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        private int failedAttemps = 0;
        public AuthPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
       
            string name = TxtBoxRegName.Text;
            string email = TxtBoxRegEmail.Text;
            string login = TxtBoxRegLogin.Text;
            string phone = TxtBoxRegPhone.Text;
            string password = PassRegPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if(!TxtBoxRegPhone.MaskCompleted)
            {
                MessageBox.Show("Введите номер телефона полностью");
                return;
            }
            if (!email.Contains("@") || !email.Contains(".") || email.IndexOf('@') > email.LastIndexOf('.'))
            {
                MessageBox.Show("Некорректный Email");
                return;
            }

            string mess;
            if(!PasswordChecker.CheckPassword(password, out mess))
            {
                MessageBox.Show(mess);
                return;
            }
            



            if (Registr(login, password, phone, name, email))
            {
                MessageBox.Show("Регистрация успешна");
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Для этой почты уже есть учетная запись");
            }
        }

       
        private bool Login(string login, string password)
        {
            string hashPass = PasswordHash.GetHash(password);
            var user = Core.context.User.FirstOrDefault(u => u.Login == login && u.Password == hashPass);
            if (user == null) return false;
            else
            {
                UserData.CurrentUser = user;
                return true;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtBoxLogin.Text;
            string password = PassPassword.Password;

            if (login == null || string.IsNullOrEmpty(login) || password == null || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            string hashedPassword = PasswordHash.GetHash(password);

            if (Login(login, password))
            {
                MessageBox.Show("Успешный вход");
                if(UserData.CurrentUser.RoleID == 2)
                {
                    NavigationService.Navigate(new PageAdmin());
                }
                else NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Аккаунт не найден");
                failedAttemps++;
            }

            if (failedAttemps > 2)
            {
                SwitchCaptcha(true);
            }
        }

        private bool Registr(string login, string password, string phone, string name, string email)
        {
            if (Core.context.User.Any(u => u.Email == email)) return false;
            User newUser = new User()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Password = PasswordHash.GetHash(password),
                RoleID = 1,
                Login = login,

            };
            Core.context.User.Add(newUser);
            Core.context.SaveChanges(); 
            UserData.CurrentUser = newUser;
            return true;
        }
        private void SwitchCaptcha(bool hide)
        {
            if (hide)
            {
                StackLogin.Visibility = Visibility.Collapsed;
                BtnLogin.Visibility = Visibility.Collapsed;
                StackCaptcha.Visibility = Visibility.Visible;
                TxtBoxLogin.Text = string.Empty;
                PassPassword.Password = string.Empty;
                CaptchaChange();
            }
            else
            {
                StackCaptcha.Visibility = Visibility.Collapsed;
                BtnLogin.Visibility = Visibility.Visible;

                StackLogin.Visibility = Visibility.Visible;
            }
        }
        private void CaptchaChange()
        {
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            TxtBoxCaptcha.Text = pwd;
        }


        private void BtnSubmitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBoxCaptchaInput.Text != TxtBoxCaptcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();
            }
            else
            {
                MessageBox.Show("Капча введена успешно, можете продолжить авторизацию", "Успех");

                SwitchCaptcha(false);
                failedAttemps = 0;
            }

        }

        private void TxtBoxCaptchaInput_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
            e.Command == ApplicationCommands.Cut ||
            e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }

        }
    }
}
