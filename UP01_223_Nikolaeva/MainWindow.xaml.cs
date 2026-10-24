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
using UP01_223_Nikolaeva.Pages;

namespace UP01_223_Nikolaeva
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigated += MainFrame_Navigated;

        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }

        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page && !string.IsNullOrEmpty(page.Title))
                this.Title = page.Title;
            bool isAuth = MainFrame.Content is Pages.AuthPage || MainFrame.Content is Pages.AccountPage;
            TopRightPanel.Visibility = isAuth ? Visibility.Collapsed : Visibility.Visible;
            UpdateBackButton();
        }
        private void UpdateBackButton()
        {
            bool canGoBack = MainFrame.CanGoBack;
            bool currentIsAuth = MainFrame.Content is Pages.AuthPage;

            BtnBack.Visibility = (canGoBack && !currentIsAuth)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = MessageBox.Show(
                "Вы действительно хотите выйти из приложения?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo);

            if (res != MessageBoxResult.Yes)
                e.Cancel = true;
        }
        private void BtnTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeChanger.ChangeTheme();
        }

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Pages.AccountPage());
        }
    }
}
