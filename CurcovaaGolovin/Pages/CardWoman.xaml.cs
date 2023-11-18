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
    /// Логика взаимодействия для CardWoman.xaml
    /// </summary>
    public partial class CardWoman : Page
    {
        List<WomanInLabor> woman = new List<WomanInLabor>();
        WomanInLabor womanInLabor;
        public CardWoman()
        {
            InitializeComponent();
            woman = DataBase.entities.WomanInLabor.ToList();
            WomanListView.ItemsSource = woman;
        }


        private void WomanListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            womanInLabor = (WomanInLabor)WomanListView.SelectedItem;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                woman = DataBase.entities.WomanInLabor.ToList();
                WomanListView.ItemsSource = woman;
                WomanListView.Visibility = Visibility.Visible;
            }
            else
            {
                woman = DataBase.entities.WomanInLabor.Where(c => c.IDWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower()) || c.SurnameWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower()) || c.NameWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower()) || c.DateOfBirth.ToString().Contains(SearchTextBox.Text.ToLower())).ToList();
                if (woman.Count > 0)
                {
                    WomanListView.ItemsSource = woman;
                    WomanListView.Visibility = Visibility.Visible;
                }
                else
                {
                    WomanListView.Visibility = Visibility.Hidden;
                }
            }
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WomanListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
        }
    }
}
