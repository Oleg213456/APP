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

namespace CurcovaaGolovin.Pages
{
    /// <summary>
    /// Логика взаимодействия для InpatientPage.xaml
    /// </summary>
    public partial class InpatientPage : Page
    {
        public InpatientPage()
        {
            InitializeComponent();
        }

        private void ChildbrithButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChildbirthPage());
        }

        private void AddInpatient_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Успешно добавлено","Сообщение",MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
