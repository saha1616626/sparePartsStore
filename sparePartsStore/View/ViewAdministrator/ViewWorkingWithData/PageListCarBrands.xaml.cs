using sparePartsStore.Helper;
using sparePartsStore.Model;
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
        private readonly ListCarBrandViewModel _listCarBrandViewModel; // привязанный ListCarBrandViewModel
        public PageListCarBrands()
        {
            InitializeComponent();

            // получаем экз ListCarBrandViewModel
            _listCarBrandViewModel = (ListCarBrandViewModel)this.Resources["ListCarBrandViewModel"];
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

        // событие передачи выбранных данных
        public event EventHandler<MyEventArgsObject> EventDataSelectedItem;
        protected virtual void TransmitSelectedData(CarBrand value)
        {
            EventDataSelectedItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // передаём в событие выбранные данные, в MainHeadViewModel для отображения этих данных на странице редактирования
        public void TransmitData()
        {
            CarBrand carBrand = new CarBrand(); // экз для храненения передаваесых данных 

            carBrand = _listCarBrandViewModel.TransmitBrand(); // получаем выбранные данные

            TransmitSelectedData(carBrand); // передаём данные в событие
        }

    }
}
