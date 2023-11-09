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

namespace Koloskov_Glazki_Save
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            var currentAgents = Koloskov_GlazkiEntities.GetContext().Agent.ToList();
            AgentsListView.ItemsSource = currentAgents;
            ComboType.SelectedIndex = 0;
            SortBy.SelectedIndex = 0;

            UpdateGlazki();
        }
        private void UpdateGlazki()
        {
            var currentAgents = Koloskov_GlazkiEntities.GetContext().Agent.ToList();
            
            currentAgents = currentAgents.Where(p => (p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("+","").Replace(" ","").Replace(")","").Replace("(","").Replace("-","").Contains(TBoxSearch.Text)
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()))).ToList();

            if (ComboType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "МФО").ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "ЗАО").ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "МКК").ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "ОАО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeString == "ПАО").ToList();
            }

            if (SortBy.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p=> p.Title).ToList();
            }
            if (SortBy.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }
            if (SortBy.SelectedIndex == 3)
            {

            }
            if (SortBy.SelectedIndex == 4)
            {

            }
            if (SortBy.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (SortBy.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }

            AgentsListView.ItemsSource = currentAgents;
        }
        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGlazki();
        }
        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGlazki();
        }
        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGlazki();
        }
    }
}
