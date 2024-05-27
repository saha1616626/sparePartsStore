using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
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
    /// Interaction logic for PageListCarModels.xaml
    /// </summary>
    public partial class PageListCarModels : Page
    {
        // для экз текущего ViewModel
        private readonly ListCarModelViewModel _listCarModelViewModel;
        public PageListCarModels()
        {
            InitializeComponent();

            // получаем экз текущего класса
            _listCarModelViewModel = (ListCarModelViewModel)this.Resources["ListCarModelViewModel"];
        }

        // кнопка добавления модели авто
        private void AddCarModel(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления моделей авто
            WorkingWithData.LaunchinPageAddCarModel();
        }

        // кнопка добавления редактирования модели авто
        private void EditCarModel(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы редактирования моделей авто
            WorkingWithData.LaunchpageEditCarModel();
        }

        // событие передачи выбранных данных из таблицы моделей авто
        public event EventHandler<MyEventArgsObject> EventDataSelectedCarModelItem;
        protected virtual void TransmitSelectedData(CarModelDPO value)
        {
            EventDataSelectedCarModelItem?.Invoke(this, new MyEventArgsObject { Value = value });
        } 

        // метод для передачи из таблицы выбранных данных в собтыие
        public void TransmitiData()
        {
            CarModelDPO carModelDPO = new CarModelDPO(); // хранение временных данных
            carModelDPO = _listCarModelViewModel.TransmitionBrand();
            TransmitSelectedData(carModelDPO); // передаём данные в событие
        }
    }
}
