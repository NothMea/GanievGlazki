using Microsoft.Win32;
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

namespace Ганиев_Глазки
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new Agent();


        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();

            if (SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
            }
            
            DataContext = _currentAgent;

            var _currentType = ГаниевГлазкиSaveEntities.GetContext().AgentType.ToList();
            var _currentSales = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList();
            SellListView.ItemsSource = _currentSales.Where(p => p.AgentID == _currentAgent.ID).ToList();
            ComboType.ItemsSource = _currentType;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }
        private AgentType _currentAgentType = new AgentType();
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "").Replace(" ", "").Replace(")", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("Укажите правильно телефон агента");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentAgent.ID == 0)
                ГаниевГлазкиSaveEntities.GetContext().Agent.Add(_currentAgent);


            if (ComboType.SelectedIndex == 0)
                _currentAgent.AgentTypeID = 1;
            if (ComboType.SelectedIndex == 1)
                _currentAgent.AgentTypeID = 2;
            if (ComboType.SelectedIndex == 2)
                _currentAgent.AgentTypeID = 3;
            if (ComboType.SelectedIndex == 3)
                _currentAgent.AgentTypeID = 4;
            if (ComboType.SelectedIndex == 4)
                _currentAgent.AgentTypeID = 5;
            if (ComboType.SelectedIndex == 5)
                _currentAgent.AgentTypeID = 6;

            try
            {
                ГаниевГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;
            var currentDel = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentDel = currentDel.Where(p=>p.AgentID == currentAgent.ID).ToList();
            if(currentDel.Count != 0)
            {
                MessageBox.Show("Невозможно удалить агента. Имеются записи");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ГаниевГлазкиSaveEntities.GetContext().Agent.Remove(currentAgent);
                        ГаниевГлазкиSaveEntities.GetContext().SaveChanges();
                        Manager.MainFrame.Navigate(new AgentPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            
        }

        private void ChangeProductBtn_Click(object sender, RoutedEventArgs e)
        {
            SellWindow myWindow = new SellWindow();
            myWindow.ShowDialog();
            ProductSale newSale = new ProductSale();
            newSale.SaleDate = Convert.ToDateTime(myWindow.SaleDate.Text);
            var currentProducts = ГаниевГлазкиSaveEntities.GetContext().Product.ToList();
            //var currentProduct = currentProducts.Where(p => p.Title == myWindow.ComboProducts.SelectedItem).ToList()[0];
            newSale.ProductID = myWindow.ComboProducts.SelectedIndex + 1;
            newSale.ProductCount = Convert.ToInt32(myWindow.TBSaleCount.Text.ToString());
            newSale.AgentID = _currentAgent.ID;
            ГаниевГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
            try
            {
                ГаниевГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                var currentSales = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList();
                var currentSale = currentSales.Where(p => p.AgentID == _currentAgent.ID).ToList()[0];
                //HakimovGlaskiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ГаниевГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SellListView.ItemsSource = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == _currentAgent.ID).ToList();
                //Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteProductBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SellListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выбери продажу");
                return;
            }
            if (SellListView.SelectedItems.Count == 1)
            {
                var currentSales = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList();
                var currentSale = currentSales.Where(p => p.AgentID == _currentAgent.ID).ToList()[SellListView.SelectedIndex];
                ГаниевГлазкиSaveEntities.GetContext().ProductSale.Remove(currentSale);
                ГаниевГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Продажа удалена");
                ГаниевГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SellListView.ItemsSource = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == _currentAgent.ID).ToList();
                return;
            }
            else
            {
                MessageBox.Show("Выбери 1 продажу");
                return;
            }
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                ГаниевГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SellListView.ItemsSource = ГаниевГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == _currentAgent.ID).ToList();
            }
        }
    }
}
