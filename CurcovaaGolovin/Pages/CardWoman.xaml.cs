using CurcovaaGolovin.DataBaseControler;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
//using Word = Microsoft.Office.Interop.Word;
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
            DateofDischargeDatapicer.SelectedDate = DateTime.Now;
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
                                    if(int.Parse(CountBirthTextBox.Text)<0||int.Parse(PeriodTextBox.Text)<0) { throw new Exception("Кол-во родов или срок родов не могут быть меньше 0"); }
                                    else 
                                    {
                                        if(BirthDataPicker.DisplayDate > DateTime.Now || DateofDischargeDatapicer.DisplayDate > DateTime.Now) throw new Exception("Дата рождения или выписки не может быть больше нынешней");
                                        else
                                        {
                                            if(!Regex.IsMatch(SurnameTextBox.Text, @"^((?:[а-я А-Я]\w*))$") || !Regex.IsMatch(NameTextBox.Text, @"^((?:[а-я А-Я]\w*))$") || !Regex.IsMatch(MidleNameTextBox.Text, @"^((?:[а-я А-Я]\w*))$")) throw new Exception("Поля имя, фамилия и отчество не могут содержать ничего кроме букв");
                                            else
                                            {
                                                if(!Regex.IsMatch(FamilyTextBox.Text, @"^((?:[а-я А-Я]\w*)|(?:(?=[\w.]+)(?:[а-я А-Я]\w*|0)\ \w*))$")) throw new Exception("Семейное положение не может содержать ничего кроме букв");
                                                else
                                                {
                                                    if(!Regex.IsMatch(PeriodTextBox.Text, @"^((?:[1-9]\d*))$")|| !Regex.IsMatch(CountBirthTextBox.Text, @"^((?:[1-9]\d*))$")) throw new Exception("Срок беременности и беременность по счету может быть только в целых чисах");
                                                    else
                                                    {
                                                        if (int.Parse(PeriodTextBox.Text) > 45 || int.Parse(PeriodTextBox.Text) < 31) throw new Exception("Проверьте правильность введенных данных,вы за пределами жизнеспособности эмбриона");
                                                        else
                                                        {
                                                            if(int.Parse(CountBirthTextBox.Text)>69) throw new Exception("Проверьте правильность введенных данных,вы указали число выше максимального кол-ва детей рожденных одной женщиной");
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
                                                                MessageBox.Show("Роженица успешно добавлена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                                                NavigationService.Navigate(new CardWoman());
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }  
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
                        try
                        {
                            if (SurnameTextBox.Text == "" || NameTextBox.Text == "" || BirthDataPicker.DisplayDate == null || AdresTextBox.Text == "" || PhoneTextBox.Text == "" || DiscriptionTextBox.Text == "" || AgeTextBox.Text == "" || FamilyTextBox.Text == "" || DateofDischargeDatapicer.DisplayDate == null)
                            {
                                throw new Exception("Одно из полей не заполнено или не выбрана дата");
                            }
                            else
                            {
                                if (int.TryParse(CountBirthTextBox.Text, out int i) == true || int.TryParse(PeriodTextBox.Text, out int i1) == true)
                                {
                                    if (int.Parse(CountBirthTextBox.Text) < 0 || int.Parse(PeriodTextBox.Text) < 0) { throw new Exception("Кол-во родов или срок родов не могут быть меньше 0"); }
                                    else
                                    {
                                        if (BirthDataPicker.DisplayDate > DateTime.Now || DateofDischargeDatapicer.DisplayDate > DateTime.Now) { throw new Exception("Дата рождения или выписки не может быть больше нынешней"); }
                                        else
                                        {
                                            if (!Regex.IsMatch(SurnameTextBox.Text, @"^((?:[а-я А-Я]\w*))$") || !Regex.IsMatch(NameTextBox.Text, @"^((?:[а-я А-Я]\w*))$") || !Regex.IsMatch(MidleNameTextBox.Text, @"^((?:[а-я А-Я]\w*))$")) throw new Exception("Поля имя, фамилия и отчество не могут содержать ничего кроме букв");
                                            else
                                            {
                                                if (!Regex.IsMatch(FamilyTextBox.Text, @"^((?:[а-я А-Я]\w*)|(?:(?=[\w.]+)(?:[а-я А-Я]\w*|0)\ \w*))$")) throw new Exception("Семейное положение не может содержать ничего кроме букв");
                                                else
                                                {
                                                    if (!Regex.IsMatch(PeriodTextBox.Text, @"^((?:[1-9]\d*))$") || !Regex.IsMatch(CountBirthTextBox.Text, @"^((?:[1-9]\d*))$")) throw new Exception("Срок беременности и беременность по счету может быть только в целых чисах");
                                                    else
                                                    {
                                                        if (int.Parse(PeriodTextBox.Text) > 45 || int.Parse(PeriodTextBox.Text) < 31) throw new Exception("Проверьте правильность введенных данных,вы за пределами жизнеспособности эмбриона");
                                                        else
                                                        {
                                                            if (int.Parse(CountBirthTextBox.Text) > 69) throw new Exception("Проверьте правильность введенных данных,вы указали число выше максимального кол-ва детей рожденных одной женщиной");
                                                            else
                                                            {
                                                                DataBase.entities.SaveChanges();
                                                                SurnameTextBox.Text = "";
                                                                NameTextBox.Text = "";
                                                                BirthDataPicker.Text = "";
                                                                MidleNameTextBox.Text = "";
                                                                BirthDataPicker.Text = "";
                                                                AdresTextBox.Text = "";
                                                                PhoneTextBox.Text = "";
                                                                DiscriptionTextBox.Text = "";
                                                                AgeTextBox.Text = "";
                                                                PeriodBirthTextBox.Text = "";
                                                                CountBirthTextBox.Text = "";
                                                                DateofDischargeDatapicer.Text = "";
                                                                PeriodTextBox.Text = "";
                                                                FamilyTextBox.Text = "";
                                                                DateofDischargeDatapicer.Text = "";
                                                                MessageBox.Show("Роженица успешно изменена", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                                                NavigationService.Navigate(new CardWoman());
                                                                WomanListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
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
                }
            }   
            catch (Exception ex) { MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Editewoman;
            WomanListView.ItemsSource = DataBase.entities.WomanInLabor.ToList();
        }

        private void BirthDataPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (BirthDataPicker.DisplayDate > DateTime.Now || DateofDischargeDatapicer.DisplayDate > DateTime.Now) { BirthDataPicker.SelectedDate = DateTime.Now; DateofDischargeDatapicer.SelectedDate = DateTime.Now; throw new Exception("Дата рождения или выписки не может быть больше нынешней"); }
                else
                {
                    DateTime zeroTime = new DateTime(1, 1, 1);
                    DateTime now = DateTime.Now;
                    DateTime then = BirthDataPicker.DisplayDate;
                    TimeSpan span = now - then;
                    int years = (zeroTime + span).Year - 1;
                    AgeTextBox.Text = years.ToString();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
