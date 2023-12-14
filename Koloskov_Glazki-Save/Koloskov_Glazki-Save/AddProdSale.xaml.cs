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
using System.Windows.Shapes;

namespace Koloskov_Glazki_Save
{
    /// <summary>
    /// Логика взаимодействия для AddProdSale.xaml
    /// </summary>
    public partial class AddProdSale : Window
    {
        private Agent currentAgent = new Agent();
        private ProductSale currentProductSale = new ProductSale();
        private Product ProductIndex = new Product();
        public AddProdSale(Agent SelectedAgent)
        {
            InitializeComponent();

            currentAgent = SelectedAgent;

            var currentProduct = Koloskov_GlazkiEntities.GetContext().ProductSale.ToList();

            var currentProdIndex = Koloskov_GlazkiEntities.GetContext().Product.ToList();
            ComboProduct.ItemsSource = currentProdIndex;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboProduct.SelectedItem == null)
            {
                errors.AppendLine("Укажите наименование продукта");
            }
            if (DateSale.Text == "")
            {
                errors.AppendLine("Укажите дату реализации");
            }
            if (string.IsNullOrWhiteSpace(TBoxCountSale.Text))
            {
                errors.AppendLine("Укажите количество реализованной продукции");
            }
            else if (Convert.ToInt32(TBoxCountSale.Text) <= 0)
            {
                errors.AppendLine("Количество реализованной продукции должно быть положительным");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            currentProductSale.SaleDate = Convert.ToDateTime(DateSale.Text);
            currentProductSale.ProductCount = Convert.ToInt32(TBoxCountSale.Text);
            currentProductSale.ProductID = ComboProduct.SelectedIndex + 1;
            currentProductSale.AgentID = currentAgent.ID;

            if (currentProductSale.ID == 0)
                Koloskov_GlazkiEntities.GetContext().ProductSale.Add(currentProductSale);

            try
            {
                Koloskov_GlazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
