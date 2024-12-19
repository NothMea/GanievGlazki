using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.Windows.Shapes;

namespace Ганиев_Глазки
{
    /// <summary>
    /// Логика взаимодействия для SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {

        public SellWindow()
        {
            InitializeComponent();
            var currentProducts = ГаниевГлазкиSaveEntities.GetContext().Product.ToList();
            ComboProducts.ItemsSource = currentProducts;
        }

        private void AddSaleWindowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBSaleCount.Text))
            {
                MessageBox.Show("Укажите количество продукции");
                return;
            }
            if (Convert.ToInt32(TBSaleCount.Text) < 0)
            {
                MessageBox.Show("Количество продукции должно быть положительное");
                return;
            }
            if (ComboProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукцию");
                return;
            }
            if (SaleDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату продажи");
                return;
            }
            this.Close();
        }

        private void ComboProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
