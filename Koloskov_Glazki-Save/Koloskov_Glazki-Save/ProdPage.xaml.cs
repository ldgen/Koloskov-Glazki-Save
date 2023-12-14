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
    /// Логика взаимодействия для ProdPage.xaml
    /// </summary>
    public partial class ProdPage : Page
    {
        private Agent currentAgent = new Agent();
        public ProdPage(Agent SelectedAgent)
        {
            InitializeComponent();

            currentAgent = SelectedAgent;

            var currentProduct = Koloskov_GlazkiEntities.GetContext().ProductSale.ToList();
            ProductHistoryListView.ItemsSource = currentProduct.Where(p => p.AgentID == currentAgent.ID);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProdSale addProdSale = new AddProdSale(currentAgent);
            addProdSale.ShowDialog();
            UpdateProduct();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var _currentProduct = (sender as Button).DataContext as ProductSale;

            var _currentProductSale = Koloskov_GlazkiEntities.GetContext().ProductSale.ToList();
            _currentProductSale = _currentProductSale.Where(p => p.ProductID == _currentProduct.ID).ToList();

            try
            {
                Koloskov_GlazkiEntities.GetContext().ProductSale.Remove(_currentProduct);
                Koloskov_GlazkiEntities.GetContext().SaveChanges();
                UpdateProduct();
                //Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void UpdateProduct()
        {
            var currentProduct = Koloskov_GlazkiEntities.GetContext().ProductSale.ToList();
            ProductHistoryListView.ItemsSource = currentProduct.Where(p => p.AgentID == currentAgent.ID);
        }
    }
}
