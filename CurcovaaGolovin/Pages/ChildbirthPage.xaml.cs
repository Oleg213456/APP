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
    /// Логика взаимодействия для ChildbirthPage.xaml
    /// </summary>
    public partial class ChildbirthPage : Page
    {
        List<Childbirth> childbirths = new List<Childbirth>();
        Childbirth childbirth;
        public ChildbirthPage()
        {
            InitializeComponent();
            childbirths = DataBase.entities.Childbirth.ToList();
            ChilBrirthListView.ItemsSource = childbirths;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ChilBrirthListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
        }

        private void ChilBrirthListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            childbirth = (Childbirth)ChilBrirthListView.SelectedItem;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                childbirths = DataBase.entities.Childbirth.ToList();
                ChilBrirthListView.ItemsSource = childbirths;
                ChilBrirthListView.Visibility = Visibility.Visible;
            }
            else
            {
                childbirths = DataBase.entities.Childbirth.Where(c => c.StartChildbirth.ToString().Contains(SearchTextBox.Text.ToLower()) || c.EndChildbirth.ToString().Contains(SearchTextBox.Text.ToLower()) || c.Inpatient.WomanInLabor.NameWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower()) || c.Inpatient.WomanInLabor.SurnameWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower())).ToList();
                if (childbirths.Count > 0)
                {
                    ChilBrirthListView.ItemsSource = childbirths;
                    ChilBrirthListView.Visibility = Visibility.Visible;
                }
                else
                {
                    ChilBrirthListView.Visibility = Visibility.Hidden;
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
