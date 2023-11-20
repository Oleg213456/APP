using CurcovaaGolovin.DataBaseControler;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics.Contracts;
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
using Word = Microsoft.Office.Interop.Word;
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
            Editewoman = new WomanInLabor();
            woman = DataBase.entities.WomanInLabor.ToList();
            WomanListView.ItemsSource = woman;
        }
        WomanInLabor Editewoman;
        public CardWoman(WomanInLabor womanInLabor)
        {
            InitializeComponent();
            Editewoman = womanInLabor;
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

        private void EditeButton_Click(object sender, RoutedEventArgs e)
        {
            if (womanInLabor != null)
            {
                NavigationService.Navigate(new CardWoman(womanInLabor));
                WomanListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите применить данное сохранения роженицы", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {

                }
                else
                {
                    if (Editewoman.IDWomanInLabor == 0)
                    {
                        try
                        {
                            if (SurnameTextBox.Text == "" || NameTextBox.Text == "" || BirthDataPicker.DisplayDate == null || AdresTextBox.Text == "" || PhoneTextBox.Text == "" || DiscriptionTextBox.Text == "" || AgeTextBox.Text == "" || FamilyTextBox.Text == "" || DateofDischargeDatapicer.DisplayDate == null)
                            {
                                throw new Exception("Одно из полей не заполнено или не выбрана дата");
                            }
                            else
                            {
                                if(int.TryParse(CountBirthTextBox.Text,out int i)==true|| int.TryParse(PeriodTextBox.Text, out int i1) == true)
                                {
                                    if(int.Parse(CountBirthTextBox.Text)<=0||int.Parse(PeriodTextBox.Text)<=0) { throw new Exception("Кол-во родов или срок родов не могут быть меньше 0"); }
                                    else 
                                    {
                                        WomanInLabor woman = new WomanInLabor();
                                        woman.SurnameWomanInLabor = SurnameTextBox.Text;
                                        woman.NameWomanInLabor = NameTextBox.Text;
                                        woman.MiddleNameWomanInLabor = MidleNameTextBox.Text;
                                        woman.DateOfBirth = BirthDataPicker.DisplayDate;
                                        woman.Address = AdresTextBox.Text;
                                        woman.PhoneWomanInLabor = PhoneTextBox.Text;
                                        woman.SpecialMarks = DiscriptionTextBox.Text;
                                        woman.WomanAge = AgeTextBox.Text;
                                        woman.Post_birth_time = PeriodBirthTextBox.Text;
                                        woman.Gestation_Count = Convert.ToInt32(this.CountBirthTextBox.Text);
                                        woman.Gestation_Period = Convert.ToInt32(this.PeriodTextBox.Text);
                                        woman.Family_State = FamilyTextBox.Text;
                                        woman.DateofDischarge = DateofDischargeDatapicer.DisplayDate;
                                        woman.Locality = "Отсутствует запись";
                                        DataBase.entities.WomanInLabor.Add(woman);
                                        DataBase.entities.SaveChanges();
                                        MessageBox.Show("Роженица успешно добавлена","Сообщение",MessageBoxButton.OK,MessageBoxImage.Information);
                                        NavigationService.Navigate(new CardWoman());
                                    }
                                }
                                else
                                {
                                    throw new Exception("Ввод не числа");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        
                        DataBase.entities.SaveChanges();
                        SurnameTextBox.Text = "";
                        NameTextBox.Text = "";
                        MidleNameTextBox.Text = "";
                        BirthDataPicker.Text = "";
                        AdresTextBox.Text = "";
                        PhoneTextBox.Text = "";
                        DiscriptionTextBox.Text = "";
                        AgeTextBox.Text = "";
                        PeriodBirthTextBox.Text = "";
                        CountBirthTextBox.Text = "";
                        PeriodTextBox.Text = "";
                        FamilyTextBox.Text = "";
                        DateofDischargeDatapicer.Text = "";
                        MessageBox.Show("Роженица успешно изменена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new CardWoman());
                    }
                }
            }   
            catch (Exception ex) { MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Editewoman;
            WomanListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            
        }
    }
}
