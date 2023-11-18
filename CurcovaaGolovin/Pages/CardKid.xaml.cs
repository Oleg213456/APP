using CurcovaaGolovin.DataBaseControler;
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
    /// Логика взаимодействия для CardKid.xaml
    /// </summary>
    public partial class CardKid : Page
    {
        List<Kid> kids = new List<Kid>();
        public CardKid()
        {
            InitializeComponent();
            try
            {
                kids = DataBase.entities.Kid.ToList();
                KidListView.ItemsSource = kids;

            }
            catch  (Exception ex) { MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);}
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Curentkid = (Kid)KidListView.SelectedItem;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                kids = DataBase.entities.Kid.ToList();
                KidListView.ItemsSource = kids;
                KidListView.Visibility = Visibility.Visible;
            }
            else
            {
                kids = DataBase.entities.Kid.Where(c=>c.IDKid.ToString().Contains(SearchTextBox.Text.ToLower())||c.SurnameKid.ToString().Contains(SearchTextBox.Text.ToLower())||c.NameKid.ToString().Contains(SearchTextBox.Text.ToLower())||c.DateOfBirth.ToString().Contains(SearchTextBox.Text.ToLower())).ToList();
                if (kids.Count > 0)
                {
                    KidListView.ItemsSource = kids;
                    KidListView.Visibility = Visibility.Visible;
                }
                else
                {
                    KidListView.Visibility = Visibility.Hidden;
                }
            }
        }
        Kid Curentkid;
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            KidListView.ItemsSource = DataBase.entities.Kid.ToList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
