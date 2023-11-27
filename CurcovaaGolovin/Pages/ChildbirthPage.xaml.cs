using ControlzEx.Standard;
using CurcovaaGolovin.DataBaseControler;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
            KidComboBox.ItemsSource = DataBase.entities.Kid.ToList();
           
        }
        public ChildbirthPage(Childbirth childbirth)
        {
            InitializeComponent();
            birth = childbirth;
            childbirths = DataBase.entities.Childbirth.ToList();
            ChilBrirthListView.ItemsSource = childbirths;
            MotherComboBox.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            KidComboBox.ItemsSource = DataBase.entities.Kid.ToList();
           
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
                            if ( Birth_PlaceTextBox.Text == "" || StatDate.Text == "" || EndDate.Text == "" || Birth_TypeTextBox.Text == "" || TimeN.Text == ""|| TimeE.Text =="") throw new Exception("Обязательные поля не заполнены");
                            else
                            {
                                if (!Regex.IsMatch(TimeN.Text, @"^((?:[1-9]\d*)|(?:(?=[\d.]+)(?:[1-9]\d*|0)\:\d+))$") || !Regex.IsMatch(TimeE.Text, @"^((?:[1-9]\d*)|(?:(?=[\d.]+)(?:[1-9]\d*|0)\:\d+))$")) throw new Exception("Неверный формат записи в поле Время начала родов или Время окончания родов");
                                else
                                {
                                    string t1 = string.Concat(TimeN.Text[0], TimeN.Text[1]);
                                    string t2 = string.Concat(TimeE.Text[0], TimeE.Text[1]);
                                    string t11 = TimeN.Text[2].ToString();
                                    string t22 = TimeN.Text[2].ToString();
                                    string t111 = string.Concat(TimeN.Text[3], TimeN.Text[4]);
                                    string t222 = string.Concat(TimeE.Text[3], TimeE.Text[4]);
                                    if (int.Parse(t1) > 24 || int.Parse(t1) < 0 || int.Parse(t2) > 24 || int.Parse(t1) < 0) throw new Exception("В сутках не может быть больше 24 часов или меньше 0 часов");
                                    else
                                    {
                                        if (t11 != ":" || t22 != ":") throw new Exception("Не соответствие формату,пожалуйста используйте : как 3 симвл в поле Время начала родов или Время окончания родов");
                                        else
                                        {
                                            if (int.Parse(t111) > 60 || int.Parse(t111) < 0 || int.Parse(t222) > 60 || int.Parse(t222) < 0) throw new Exception("В часе не может быть больше 60 минут или меньше 0 минут");
                                            else
                                            {
                                                if(StatDate.DisplayDate > DateTime.Now || EndDate.DisplayDate > DateTime.Now) throw new Exception("Выбранная дата не может быть больше нынешней");
                                                else
                                                {
                                                    if(!Regex.IsMatch(Birth_PlaceTextBox.Text, @"^((?:[а-я А-Я]\w*)|(?:(?=[\w.]+)(?:[а-я А-Я]\w*|0)\ \w*))$") || !Regex.IsMatch(Birth_TypeTextBox.Text, @"^((?:[а-я А-Я]\w*)|(?:(?=[\w.]+)(?:[а-я А-Я]\w*|0)\ \w*))$")) throw new Exception("В полях тип родов и место родов не может быть иных символов кроме русских букв");
                                                    else
                                                    {
                                                        Childbirth birthChild = new Childbirth();
                                                        birthChild.StartChildbirth = StatDate.DisplayDate;
                                                        birthChild.EndChildbirth = EndDate.DisplayDate;
                                                        birthChild.Birth_Place = Birth_PlaceTextBox.Text;
                                                        birthChild.Birth_Type = Birth_TypeTextBox.Text;
                                                        birthChild.Description = DescriptionTextBox.Text;
                                                        birthChild.Operational_manuals = Operational_manualsTextBox.Text;
                                                        birthChild.StartChildbirthTime = TimeN.Text;
                                                        birthChild.EndChildbirthTime = TimeE.Text;
                                                        var table = KidComboBox.SelectedItem as Kid;
                                                        birthChild.IDKid = table.IDKid;
                                                        DataBase.entities.Childbirth.Add(birthChild);
                                                        DataBase.entities.SaveChanges();
                                                        MessageBox.Show("Роды успешно добавлены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                                        NavigationService.Navigate(new ChildbirthPage());
                                                    }
                                                }  
                                            }
                                        }
                                    }
                                }
                                
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
