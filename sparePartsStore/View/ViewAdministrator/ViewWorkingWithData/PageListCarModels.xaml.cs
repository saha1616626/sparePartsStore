using sparePartsStore.Helper;
using sparePartsStore.Model;
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
        public PageListCarModels()
        {
            InitializeComponent();
        }

        // кнопка добавления модели авто
        private void AddCarModel(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления моделей авто
            WorkingWithData.LaunchinPageAddCarModel();
        }

        // событие передачи выбранных данных в таблице моделей авто
        public event EventHandler<MyEventArgsObject> EventDataSelectedCarModelItem;
        protected virtual void TransmitSelectedData(CarModel carModelq) {
    }
}
