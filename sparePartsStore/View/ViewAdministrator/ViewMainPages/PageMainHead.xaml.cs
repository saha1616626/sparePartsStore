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

namespace sparePartsStore.View.ViewAdministrator.ViewMainPages
{
    /// <summary>
    /// Interaction logic for PageMainHead.xaml
    /// </summary>
    public partial class PageMainHead : Page
    {
        public PageMainHead()
        {
            InitializeComponent();

            // тестово запускаем страницу посик запчастей
            //administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListManufacture.xaml", UriKind.Relative));
            administratorFrame.Navigate(new Uri("/View/ViewSearchSpareParts.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
