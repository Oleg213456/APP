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
using Word = Microsoft.Office.Interop.Word;
namespace CurcovaaGolovin.Pages
{
    /// <summary>
    /// Логика взаимодействия для InpatientPage.xaml
    /// </summary>
    public partial class InpatientPage : Page
    {
        List<Inpatient> inpatients = new List<Inpatient>();
        Inpatient inpatient;
        Inpatient inpatientos;
        public InpatientPage()
        {
            InitializeComponent();
            inpatientos = new Inpatient();
            inpatients = DataBase.entities.Inpatient.ToList();
            InpatientListView.ItemsSource = inpatients;
            Woman_tx.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            Doctor.ItemsSource = DataBase.entities.Doctor.ToList();
        }
        public InpatientPage(Inpatient inpatient)
        {
            InitializeComponent();
            inpatientos = inpatient;
            Woman_tx.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            Doctor.ItemsSource = DataBase.entities.Doctor.ToList();
        }

        private void ChildbrithButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChildbirthPage());
        }

        private void AddInpatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите применить данное сохранения стационара", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No){}
                else
                {
                    if (inpatientos.IDInpatient == 0)
                    {
                        try
                        {
                            if (Woman_tx.Text == null || Doctor.Text == null || dataE.Text == "" || DateN.Text == "") throw new Exception("Обязательные поля не заполнены");
                            else
                            {
                                Inpatient inpat = new Inpatient();
                                var table = Woman_tx.SelectedItem as WomanInLabor;
                                inpat.IDWomanInLabor = table.IDWomanInLabor;
                                var table2 = Doctor.SelectedItem as Doctor;
                                inpat.IDDoctor = table2.IDDoctor;
                                inpat.StartInpatient = DateN.DisplayDate;
                                inpat.EndInpatient = dataE.DisplayDate;
                                inpat.Description = Opisanie.Text;
                                DataBase.entities.Inpatient.Add(inpat);
                                DataBase.entities.SaveChanges();
                                MessageBox.Show("Стационар успешно заполнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                NavigationService.Navigate(new InpatientPage());
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
                    }
                    else
                    {
                        DataBase.entities.SaveChanges();
                        dataE.Text = "";
                        DateN.Text = "";
                        Opisanie.Text = "";
                        MessageBox.Show("Стационар успешно изменен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new InpatientPage());
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void InpatientListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            inpatient = (Inpatient)InpatientListView.SelectedItem;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
            {
                inpatients = DataBase.entities.Inpatient.ToList();
                InpatientListView.ItemsSource = inpatients;
                InpatientListView.Visibility = Visibility.Visible;
            }
            else
            {
                inpatients = DataBase.entities.Inpatient.Where(c => c.StartInpatient.ToString().Contains(SearchTextBox.Text.ToLower()) || c.EndInpatient.ToString().Contains(SearchTextBox.Text.ToLower()) || c.Doctor.SurnameDoctor.ToString().Contains(SearchTextBox.Text.ToLower()) || c.WomanInLabor.SurnameWomanInLabor.ToString().Contains(SearchTextBox.Text.ToLower())).ToList();
                if (inpatients.Count > 0)
                {
                    InpatientListView.ItemsSource = inpatients;
                    InpatientListView.Visibility = Visibility.Visible;
                }
                else
                {
                    InpatientListView.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = inpatientos;
            InpatientListView.ItemsSource = DataBase.entities.Inpatient.ToList();
        }

        private void EditeButton_Click(object sender, RoutedEventArgs e)
        {
            if (inpatient != null)
            {
                NavigationService.Navigate(new InpatientPage(inpatient));
                InpatientListView.ItemsSource = DataBase.entities.Inpatient.ToList();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (inpatient != null)
                {
                    var item = InpatientListView.SelectedItem as Inpatient;
                    Word._Application wApp = new Word.Application();
                    Word._Document wDoc = wApp.Documents.Add();
                    wApp.Visible = true;
                    wDoc.Activate();
                    var SubDann = wDoc.Content.Paragraphs.Add();
                    SubDann.Range.Text =
                    $"\t\t\t\t\t ОБМЕННАЯ КАРТА.\n" +
                    $"\t (сведение родильного дома, родильного отделения больницы о родильнице)\n" +
                    $"\t\t\t\t\tДата выдачи «{item.WomanInLabor.DateofDischarge}»\n" +
                    $"\t 1.Фамилия,имя,отчество: {item.WomanInLabor.SurnameWomanInLabor} {item.WomanInLabor.NameWomanInLabor} {item.WomanInLabor.MiddleNameWomanInLabor} \n" +
                    $"\t 2.Возраст: {item.WomanInLabor.WomanAge}  \n" +
                    $"\t 3.Адрес: {item.WomanInLabor.Address} \n" +
                    $"\t 4.Дата поступления: {item.StartInpatient}. Роды произошли: {item.Childbirth.Birth_Place} \n" +
                    $"\t 5.Особенности течения родов: {item.Childbirth.Description} \n" +
                    $"\t 6.Оперативные пособия: {item.Childbirth.Operational_manuals}\n" +
                    $"\t 7.Обезболивание применялось: ДА \n" +
                    $"\t 8.Течение последнего периода:{item.WomanInLabor.Post_birth_time} \n" +
                    $"\t 9.Выписана: {item.EndInpatient} \n" +
                    $"\t 10.Состояние матери при выписки: {item.WomanInLabor.SpecialMarks} \n" +
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
                if (inpatient != null)
                {
                    var item = InpatientListView.SelectedItem as Inpatient;
                    Word._Application wApp = new Word.Application();
                    Word._Document wDoc = wApp.Documents.Add();
                    wApp.Visible = true;
                    wDoc.Activate();
                    var SubDann = wDoc.Content.Paragraphs.Add();
                    SubDann.Range.Text =
                    $"\t\t\t\t\t ОБМЕННАЯ КАРТА.\n" +
                    $"\t (сведение родильного дома, родильного отделения больницы о новорожденном)\n" +
                    $"\t\t\t\t\t Дата выдачи «{item.WomanInLabor.DateofDischarge}»\n" +
                    $"\t 1.Фамилия,имя,отчество родительницы:{item.WomanInLabor.SurnameWomanInLabor} {item.WomanInLabor.NameWomanInLabor} {item.WomanInLabor.MiddleNameWomanInLabor} \n" +
                    $"\t 2.Адрес: {item.WomanInLabor.Address} \n" +
                    $"\t 3.Роды произошли: {item.Childbirth.Birth_Place} \n" +
                    $"\t 4.От какой беременности по счете родился ребенок {item.WomanInLabor.Gestation_Count} со сроком беременности \t\t\t {item.WomanInLabor.Gestation_Period} недель. Предшествуюущие беременности закончились:{item.WomanInLabor.SpecialMarks} \n" +
                    $"\t 5.Вид родов: {item.Childbirth.Birth_Type} \n" +
                    $"\t 6.Особенности течения родов:{item.Childbirth.Description} \n" +
                    $"\t 7.Обезболивание применялось: ДА \n" +
                    $"\t 8.Течение последнего периода:{item.WomanInLabor.Post_birth_time} \n" +
                    $"\t 9.Мать выписана:{item.EndInpatient.ToString("MM-dd-yyyy")} \n" +
                    $"\t 10.Состояние матери при выписки: {item.WomanInLabor.SpecialMarks} \n" +
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
    }
}
