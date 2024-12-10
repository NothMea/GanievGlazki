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

namespace Ганиев_Глазки
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = ГаниевГлазкиSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;


            ComboType.SelectedIndex = 0;
        }
        private void UpdateAgent()
        {

            var currentAgent = ГаниевГлазкиSaveEntities.GetContext().Agent.ToList();


            string searchText = TBoxSearch.Text.ToLower();


            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(searchText) || p.Email.ToLower().Contains(searchText) || p.Phone.ToLower().Contains(searchText)
            ).ToList();

            if (ComboSort.SelectedIndex == 0)
            {
                currentAgent = currentAgent.Where(p =>
                    p.AgentType.Title.Contains("ООО") ||
                    p.AgentType.Title.Contains("ОАО") ||
                    p.AgentType.Title.Contains("ПАО") ||
                    p.AgentType.Title.Contains("ЗАО") ||
                    p.AgentType.Title.Contains("МФО") ||
                    p.AgentType.Title.Contains("МКК")
                ).ToList();
            }
            if (ComboSort.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("ООО"))).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("ОАО"))).ToList();
            }
            if (ComboSort.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("ПАО"))).ToList();
            }
            if (ComboSort.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("ЗАО"))).ToList();
            }
            if (ComboSort.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("МФО"))).ToList();
            }
            if (ComboSort.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => (p.AgentType.Title.Contains("МКК"))).ToList();
            }

            if (ComboType.SelectedIndex == 0)
            {
                currentAgent = currentAgent.Where(p =>
                     p.AgentType.Title.Contains("ООО") ||
                     p.AgentType.Title.Contains("ОАО") ||
                     p.AgentType.Title.Contains("ПАО") ||
                     p.AgentType.Title.Contains("ЗАО") ||
                     p.AgentType.Title.Contains("МФО") ||
                     p.AgentType.Title.Contains("МКК")
                 ).ToList();
            }

            if(ComboType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }

            if (ComboType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }

            if (ComboType.SelectedIndex == 5)
            { 
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList(); 
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }

            AgentListView.ItemsSource = currentAgent.ToList();
            AgentListView.ItemsSource = currentAgent;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgent();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAgent();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();

        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }
    }
}
