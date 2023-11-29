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
using Word = Microsoft.Office.Interop.Word;

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
            MotherComboBox.ItemsSource = DataBase.entities.Inpatient.ToList();
            KidComboBox.ItemsSource = DataBase.entities.Kid.ToList();
           
        }
        public ChildbirthPage(Childbirth childbirth)
        {
            InitializeComponent();
            birth = childbirth;
            childbirths = DataBase.entities.Childbirth.ToList();
            ChilBrirthListView.ItemsSource = childbirths;
            MotherComboBox.ItemsSource = DataBase.entities.Inpatient.ToList();
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
                    if (birth.IDChilbirth == 0)
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
                                                        var table1 = MotherComboBox.SelectedItem as Inpatient;
                                                        birthChild.IDInpatient = table1.IDInpatient;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (childbirth != null)
                {
                    var item = ChilBrirthListView.SelectedItem as Childbirth;
                    Word._Application wApp = new Word.Application();
                    Word._Document wDoc = wApp.Documents.Add();
                    wApp.Visible = true;
                    wDoc.Activate();
                    var SubDann = wDoc.Content.Paragraphs.Add();
                    SubDann.Range.Text =
                    $"\t\t\t\t\t ОБМЕННАЯ КАРТА.\n" +
                    $"\t (сведение родильного дома, родильного отделения больницы о родильнице)\n" +
                    $"\t\t\t\t\tДата выдачи «{item.Inpatient.WomanInLabor.DateofDischarge}»\n" +
                    $"\t 1.Фамилия,имя,отчество: {item.Inpatient.WomanInLabor.SurnameWomanInLabor} {item.Inpatient.WomanInLabor.NameWomanInLabor} {item.Inpatient.WomanInLabor.MiddleNameWomanInLabor} \n" +
                    $"\t 2.Возраст: {item.Inpatient.WomanInLabor.WomanAge}  \n" +
                    $"\t 3.Адрес: {item.Inpatient.WomanInLabor.Address} \n" +
                    $"\t 4.Дата поступления: {item.Inpatient.StartInpatient}. Роды произошли: {item.Birth_Place} \n" +
                    $"\t 5.Особенности течения родов: {item.Description} \n" +
                    $"\t 6.Оперативные пособия: {item.Operational_manuals}\n" +
                    $"\t 7.Течение последнего периода:{item.Inpatient.WomanInLabor.Post_birth_time} \n" +
                    $"\t 8.Выписана: {item.Inpatient.EndInpatient} \n" +
                    $"\t 9.Состояние матери при выписки: {item.Inpatient.WomanInLabor.SpecialMarks} \n" +
                    $"\n\n"
                    + $"\t Подпись врача _______________\n";
                    wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.doc");
                    wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.pdf", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                }
                else
                {
                    MessageBox.Show("Выбирете запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (childbirth != null)
                {
                    var item = ChilBrirthListView.SelectedItem as Childbirth;
                    Word._Application wApp = new Word.Application();
                    Word._Document wDoc = wApp.Documents.Add();
                    wApp.Visible = true;
                    wDoc.Activate();
                    var SubDann = wDoc.Content.Paragraphs.Add();
                    SubDann.Range.Text =
                    $"\t\t\t\t\t ОБМЕННАЯ КАРТА.\n" +
                    $"\t (сведение родильного дома, родильного отделения больницы о новорожденном)\n" +
                    $"\t\t\t\t\t Дата выдачи «{item.Inpatient.WomanInLabor.DateofDischarge}»\n" +
                    $"\t 1.Фамилия,имя,отчество родительницы:{item.Inpatient.WomanInLabor.SurnameWomanInLabor} {item.Inpatient.WomanInLabor.NameWomanInLabor} {item.Inpatient.WomanInLabor.MiddleNameWomanInLabor} \n" +
                    $"\t 2.Адрес: {item.Inpatient.WomanInLabor.Address} \n" +
                    $"\t 3.Роды произошли: {item.Birth_Place} \n" +
                    $"\t 4.От какой беременности по счете родился ребенок {item.Inpatient.WomanInLabor.Gestation_Count} со сроком беременности \t\t\t {item.Inpatient.WomanInLabor.Gestation_Period} недель. Предшествуюущие беременности закончились:{item.Inpatient.WomanInLabor.SpecialMarks} \n" +
                    $"\t 5.Вид родов: {item.Birth_Type} \n" +
                    $"\t 6.Особенности течения родов:{item.Description} \n" +
                    $"\t 7.Обезболивание применялось: ДА \n" +
                    $"\t 8.Течение последнего периода:{item.Inpatient.WomanInLabor.Post_birth_time} \n" +
                    $"\t 9.Мать выписана:{item.Inpatient.EndInpatient.ToString("MM-dd-yyyy")} \n" +
                    $"\t 10.Состояние матери при выписки: {item.Inpatient.WomanInLabor.SpecialMarks} \n" +
                    $"\n\n"
                    + $"\t Подпись врача _______________\n";
                    wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.doc");
                    wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.pdf", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                }
                else
                {
                    MessageBox.Show("Выбирете запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (childbirth != null)
            {
                var item = ChilBrirthListView.SelectedItem as Childbirth;
                Word._Application wApp = new Word.Application();
                Word._Document wDoc = wApp.Documents.Add();
                wApp.Visible = true;
                wDoc.Activate();
                var SubDann = wDoc.Content.Paragraphs.Add();
                SubDann.Range.Text =
                $"\t\t КОРЕШОК МЕДИЦИНСКОГО СВИДЕТЕЛЬСТВА О РОЖДЕНИИ.\n" +
                $"\t\t\t\t Серия 00000000  № 0000000\n" +
                $"\t\t\t\t Дата выдачи «{item.Inpatient.WomanInLabor.DateofDischarge}»\n" +
                $"\t 1.Ребенок родился:{item.EndChildbirth.ToString("dd.mm.yyyy, HH:mm")}\n" +
                $"\t 2.Фамилия,имя,отчество матери: {item.Inpatient.WomanInLabor.SurnameWomanInLabor} {item.Inpatient.WomanInLabor.NameWomanInLabor} {item.Inpatient.WomanInLabor.MiddleNameWomanInLabor} \n" +
                $"\t 3.Дата рождения матери: {item.Inpatient.WomanInLabor.DateOfBirth} \n" +
                $"\t 4.Место постоянно жительства (регистрации) матери ребенка: \n" +
                $"\t {item.Inpatient.WomanInLabor.Address} \n" +
                $"\t 5.Местность: {item.Inpatient.WomanInLabor.Locality} \n" +
                $"\t 6.Пол:{item.Kid.Gender} \n" +
                $"\n\n"
                + $"\t Подпись врача _______________\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
                $"\t\t\t МЕДИЦИНСКОГО СВИДЕТЕЛЬСТВА О РОЖДЕНИИ.\n" +
                $"\t\t\t\t Серия 00000000  № 0000000\n" +
                $"\t\t\t\t Дата выдачи «{item.Inpatient.WomanInLabor.DateofDischarge}»\n" +
                $"\t 1.Ребенок родился:{item.EndChildbirth.ToString("dd.mm.yyyy, HH:mm")}\n" +
                $"\t Мать \t\t\t\t\t\t\t\t\t" +
                $"\t\t\t‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾ \n" +
                $"\t 2.Фамилия,имя,отчество:{item.Inpatient.WomanInLabor.SurnameWomanInLabor} {item.Inpatient.WomanInLabor.NameWomanInLabor} {item.Inpatient.WomanInLabor.MiddleNameWomanInLabor}\n" +
                $"\t 3.Дата рождения: {item.Inpatient.WomanInLabor.DateOfBirth}\n" +
                $"\t 4.Место постоянно жительства: {item.Inpatient.WomanInLabor.Address}\n" +
                $"\t 5.Местность: {item.Inpatient.WomanInLabor.Locality}\n" +
                $"\t 6.Семейное положение: {item.Inpatient.WomanInLabor.Family_State}\n" +
                $"\t Ребенок \t\t\t\t\t\t\t\t\t" +
                $"\t\t\t ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾ \n" +
                $"\t 7.Фамилия ребенка:{item.Kid.SurnameKid} {item.Kid.NameKid} {item.Kid.MiddleNameKid}\n" +
                $"\t 8.Место рождения: {item.Inpatient.WomanInLabor.Address} \n" +
                $"\t 9.Местность:{item.Inpatient.WomanInLabor.Address}\n" +
                $"\t 10.Роды произошли: {item.Birth_Place}\n" +
                $"\t 11.Пол:{item.Kid.Gender} \n" +
                $"\n\n"
                + $"\t Подпись врача _______________\n";
                wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.doc");
                wDoc.SaveAs2($@"{Environment.CurrentDirectory}\1.pdf", Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
            }
            else
            {
                MessageBox.Show("Выбирете запись", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
  
}
