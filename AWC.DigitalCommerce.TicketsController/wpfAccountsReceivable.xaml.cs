using AWC.DigitalCommerce.TicketsController.Controls;
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

namespace AWC.DigitalCommerce.TicketsController
{
    public partial class wpfAccountsReceivable : Window
    {
        private List<clsCustomerVIP> lstCustomerVIPLeft = new List<clsCustomerVIP>();
        private List<clsCustomerVIP> lstCustomerVIPRight = new List<clsCustomerVIP>();

        public wpfAccountsReceivable()
        {
            InitializeComponent();

            LoadDatagrids();
        }

        private void LoadDatagrids()
        {
            lstCustomerVIPLeft = DB.GetAccountsReceivableFromTickets();
            dgTickets.ItemsSource = lstCustomerVIPLeft;
            lblOpenAccounts.Content = $"CUENTAS ABIERTAS ({lstCustomerVIPLeft.Count})";

            lstCustomerVIPRight = DB.GetAccountsReceivableFromTickets();
            dgDailyClosing.ItemsSource = lstCustomerVIPRight;
            lblPendingAccounts.Content = $"CUENTAS PENDIENTES ({lstCustomerVIPRight.Count})";
        }

        private void btn_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_Sync(object sender, RoutedEventArgs e)
        {

        }
    }
}
