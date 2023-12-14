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
    /// Логика взаимодействия для PriorityEdit.xaml
    /// </summary>
    public partial class PriorityEdit : Window
    {
        public int PriorityCount;
        public bool CheckClick = false;
        public PriorityEdit(int max_priority)
        {
            InitializeComponent();

            PriorityBox.Text = Convert.ToString(max_priority);
            CheckClick = false;
        }

        private void PriorityEditButton_Click(object sender, RoutedEventArgs e)
        {
            CheckClick = true;
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(PriorityBox.Text))
                errors.AppendLine("Укажите приоритет агента");
            else if (Convert.ToInt32(PriorityBox.Text) <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                PriorityCount = Convert.ToInt32(PriorityBox.Text);
                this.Close();
            }
        }
    }
}
