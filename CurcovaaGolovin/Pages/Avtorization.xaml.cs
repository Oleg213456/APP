using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CurcovaaGolovin.DataBaseControler;

namespace CurcovaaGolovin.Pages
{
    /// <summary>
    /// Логика взаимодействия для Avtorization.xaml
    /// </summary>
    public partial class Avtorization : Page
    {
        public Avtorization()
        {
            InitializeComponent();
            init_Data();
        }

        private void LoginButon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Authorization user = DataBase.entities.Authorization.FirstOrDefault(c=>c.Login == LoginTextBox.Text && c.Password == pbPassword.Password);
                if (LoginTextBox.Text == ""){throw new Exception("Поле логина пустое");}
                else
                {
                    if (pbPassword.Password == ""){throw new Exception("Поле пароле пустое");}
                    else
                    {
                        if (user != null)
                        {
                            save_Data();
                            NavigationService.Navigate(new MenuItem(user));
                        }
                        else {throw new Exception("Ошибка авторизации");}
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,"Сообщение",MessageBoxButton.OK,MessageBoxImage.Error); }
        }
        private void pbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        => tblPasswordHint.Visibility = pbPassword.Password.Length == 0 ? Visibility.Visible : Visibility.Hidden;

        private void init_Data()
        {
            if (Properties.Settings.Default.Login != string.Empty)
            {
                if (Properties.Settings.Default.Parame == "yes")
                {
                    LoginTextBox.Text = Properties.Settings.Default.Login;
                    pbPassword.Password = Properties.Settings.Default.Password;
                    Check.IsChecked = true;
                }
                else
                {
                    LoginTextBox.Text = Properties.Settings.Default.Login;
                }
            }
        }
        private void save_Data()
        {
            if ((bool)Check.IsChecked)
            {
                Properties.Settings.Default.Login = LoginTextBox.Text;
                Properties.Settings.Default.Password = pbPassword.Password;
                Properties.Settings.Default.Parame = "yes";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Login = null;
                Properties.Settings.Default.Password = null;
                Properties.Settings.Default.Parame = "no";
                Properties.Settings.Default.Save();
            }
        }

    }
}
