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
        int CountAgents;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

        public ServicePage()
        {
            InitializeComponent();
            var currentAgents = Koloskov_GlazkiEntities.GetContext().Agent.ToList();
            AgentsListView.ItemsSource = currentAgents;
            ComboType.SelectedIndex = 0;
            SortBy.SelectedIndex = 0;

            UpdateGlazki();
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountAgents = TableList.Count;
            if (CountAgents % 10 > 0)
            {
                CountPage = CountAgents / 10 + 1;
            }
            else
            {
                CountPage = CountAgents / 10;
            }

            Boolean Ifupdate = true;
            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountAgents ? CurrentPage * 10 + 10 : CountAgents;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountAgents ? CurrentPage * 10 + 10 : CountAgents;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountAgents ? CurrentPage * 10 + 10 : CountAgents;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountAgents ? CurrentPage * 10 + 10 : CountAgents;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountAgents.ToString();

                AgentsListView.ItemsSource = CurrentPageList;

                AgentsListView.Items.Refresh();
            }
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
            TableList = currentAgents;
            ChangePage(0, 0);
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

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Koloskov_GlazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentsListView.ItemsSource = Koloskov_GlazkiEntities.GetContext().Agent.ToList();
            }
            UpdateGlazki();
        }

        private void EditAgentBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
            UpdateGlazki();
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void PriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int max_priority = 0;
            foreach (Agent agentPriority in AgentsListView.SelectedItems)
            {
                if (max_priority < agentPriority.Priority)
                    max_priority = agentPriority.Priority;
            }

            PriorityEdit priorityEdit = new PriorityEdit(max_priority);
            priorityEdit.ShowDialog();

            if (string.IsNullOrEmpty(priorityEdit.PriorityBox.Text))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(priorityEdit.PriorityBox.Text) && Convert.ToInt32(priorityEdit.PriorityBox.Text) > 0 && priorityEdit.CheckClick == true)
            {

                foreach (Agent agentPriority in AgentsListView.SelectedItems)
                {
                    agentPriority.Priority = Convert.ToInt32(priorityEdit.PriorityBox.Text);
                }
                try
                {
                    Koloskov_GlazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdateGlazki();
        }

        private void AgentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentsListView.SelectedItems.Count > 1)
            {
                PriorityButton.Visibility = Visibility.Visible;
            }
            else
            {
                PriorityButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
