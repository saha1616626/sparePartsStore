using sparePartsStore.Helper;
using sparePartsStore.ViewModel;
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

namespace sparePartsStore.View.ViewAdministrator.ViewWorkingWithData
{
    /// <summary>
    /// Interaction logic for PageListCarBrands.xaml
    /// </summary>
    public partial class PageListCarBrands : Page
    {
        public PageListCarBrands()
        {
            InitializeComponent();
        }

        // переход на страницу добавить марку авто
        private void AddCarBrand(object sender, EventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления марок авто
            WorkingWithData.LaunchPageAddCarBrand(); 
        }

        // переход на страницу редакирования марки авто
        private void EditCarBrand(object sender, EventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы редактирования марок авто
            WorkingWithData.LaunchPageEditCarBrand();
        }
    }
}
