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
        Kid newkid;
        public CardKid()
        {
            InitializeComponent();
            newkid = new Kid();
            kids = DataBase.entities.Kid.ToList();
            KidListView.ItemsSource = kids;
            GenderComboBox.ItemsSource = DataBase.entities.Kid.ToList();
            PalceComboBox.ItemsSource = DataBase.entities.Kid.ToList();
        }
        public CardKid(Kid Curentkid)
        {
            InitializeComponent();
            newkid = Curentkid;
            kids = DataBase.entities.Kid.ToList();
            KidListView.ItemsSource = kids;
            GenderComboBox.ItemsSource = DataBase.entities.Kid.ToList();
            PalceComboBox.ItemsSource = DataBase.entities.Kid.ToList();
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
            this.DataContext = newkid;
            KidListView.ItemsSource = DataBase.entities.Kid.ToList();
        }

        private void EditeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Curentkid != null)
            {
                NavigationService.Navigate(new CardKid(Curentkid));
                KidListView.ItemsSource = DataBase.entities.Kid.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите применить данное сохранения родов", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) { }
                else
                {
                    if (newkid.IDInpatient == 0)
                    {
                        try
                        {
                            if (KidSurnameBox.Text == "" || BirthDatePicker.Text == "" || DateofDischargePicker.Text == "" || KidNameBox.Text == "" || HeightTextBox.Text == "" || WeightTextBox.Text == "") throw new Exception("Обязательные поля не заполнены");
                            else
                            {
                                Kid kid = new Kid();
                                kid.SurnameKid = KidSurnameBox.Text;
                                kid.NameKid = KidNameBox.Text;
                                kid.MiddleNameKid = KidMidleNameBox.Text;
                                kid.Gender = GenderComboBox.Text;
                                kid.DateOfBirth = BirthDatePicker.DisplayDate;
                                kid.Height = Double.Parse(HeightTextBox.Text);
                                kid.Weight = Double.Parse(WeightTextBox.Text);
                                kid.Locality = PalceComboBox.Text;
                                kid.DateofDischarge = DateofDischargePicker.DisplayDate;
                                kid.IDInpatient = 6;
                                DataBase.entities.Kid.Add(kid);
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
                        MessageBox.Show("Роды успешно изменены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new ChildbirthPage());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
