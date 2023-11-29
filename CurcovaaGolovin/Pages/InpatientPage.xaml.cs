using CurcovaaGolovin.DataBaseControler;
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
            dataE.SelectedDate = DateTime.Now;
            DateN.SelectedDate = DateTime.Now;
        }
        public InpatientPage(Inpatient inpatient)
        {
            InitializeComponent();
            inpatientos = inpatient;
            Woman_tx.ItemsSource = DataBase.entities.WomanInLabor.ToList();
            Doctor.ItemsSource = DataBase.entities.Doctor.ToList();
            dataE.SelectedDate = DateTime.Now;
            DateN.SelectedDate = DateTime.Now;
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
                               if(DateN.DisplayDate > DateTime.Now || dataE.DisplayDate > DateTime.Now) throw new Exception("Выбранная дата не может быть больше нынешней");
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
           
        
    }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
               

            }
            
        }
    }

