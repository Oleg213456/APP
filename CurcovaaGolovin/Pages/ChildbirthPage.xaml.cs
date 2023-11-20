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
        Childbirth birth;
        public ChildbirthPage()
        {
            InitializeComponent();
            birth = new Childbirth();
            childbirths = DataBase.entities.Childbirth.ToList();
            ChilBrirthListView.ItemsSource = childbirths;
            MotherComboBox.ItemsSource = DataBase.entities.WomanInLabor.ToList();
        }
        public ChildbirthPage(Childbirth childbirth)
        {
            InitializeComponent();
            birth = childbirth;
            childbirths = DataBase.entities.Childbirth.ToList();
            ChilBrirthListView.ItemsSource = childbirths;
            MotherComboBox.ItemsSource = DataBase.entities.WomanInLabor.ToList();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = birth;
            ChilBrirthListView.ItemsSource = DataBase.entities.Childbirth.ToList();
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

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (childbirth != null)
            {
                NavigationService.Navigate(new ChildbirthPage(childbirth));
                ChilBrirthListView.ItemsSource = DataBase.entities.Childbirth.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите применить данное сохранения родов", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
                else
                {
                    if (birth.IDInpatient == 0)
                    {
                        try
                        {
                            if ( Birth_PlaceTextBox.Text == "" || StatDate.Text == "" || EndDate.Text == "" || Birth_TypeTextBox.Text == "") throw new Exception("Обязательные поля не заполнены");
                            else
                            {
                                Childbirth birthChild = new Childbirth();
                                birthChild.StartChildbirth = StatDate.DisplayDate;
                                birthChild.EndChildbirth = EndDate.DisplayDate;
                                birthChild.Birth_Place = Birth_PlaceTextBox.Text;
                                birthChild.Birth_Type = Birth_TypeTextBox.Text;
                                birthChild.Description = DescriptionTextBox.Text;
                                birthChild.Operational_manuals = Operational_manualsTextBox.Text;
                                DataBase.entities.Childbirth.Add(birthChild);
                                DataBase.entities.SaveChanges();
                                MessageBox.Show("Роды успешно добавлены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                NavigationService.Navigate(new ChildbirthPage());
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
                    }
                    else
                    {
                        DataBase.entities.SaveChanges();
                        StatDate.Text = "";
                        EndDate.Text = "";
                        Birth_PlaceTextBox.Text = "";
                        Birth_TypeTextBox.Text = "";
                        DescriptionTextBox.Text = "";
                        Operational_manualsTextBox.Text = "";
                        MessageBox.Show("Роды успешно изменены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new ChildbirthPage());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
