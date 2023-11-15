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

        
    }
}
