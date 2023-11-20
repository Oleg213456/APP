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
    /// Логика взаимодействия для MenuItem.xaml
    /// </summary>
    public partial class MenuItem : Page
    {
        Authorization NewUser = new Authorization();
        public MenuItem(Authorization user)
        {
            InitializeComponent();
            this.DataContext = user;
            UserTextBox.Text = user.UserNane;
        }            
        

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void ReportsForThePeriodButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsForThePeriod());
        }

        private void CardWomanButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CardWoman());
        }

        private void InpatientButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InpatientPage());
        }

        private void CardKidButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CardKid());
        }
    }
}
