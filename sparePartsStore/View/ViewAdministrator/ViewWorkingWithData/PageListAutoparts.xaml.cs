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
    /// Interaction logic for PageListAutoparts.xaml
    /// </summary>
    public partial class PageListAutoparts : Page
    {
        private readonly ListAutopartViewModel _listAutopartViewModel; // объект класса
        public PageListAutoparts()
        {
            InitializeComponent();

            // получаем экз ListAutopartViewModel
            _listAutopartViewModel = (ListAutopartViewModel)this.Resources["ListAutopartViewModel"];
        }

        // кнопка подбора аналогов
        private void Btn_AnalogAutoParts(object sender, RoutedEventArgs e)
        {

        }

        // кнопка добавления запчасти
        private void Btn_AddAutoParts(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления
            WorkingWithData.LaunchPageAddAutoparts();
        }

        // кнопка редактирования запчасти
        private void Btn_EditAutoParts(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы редактирования производителя
            WorkingWithData.LaunchPageEditAutoparts();
        }

        // событие передачи выбранных данных из таблицы запчасти
        public event EventHandler<MyEventArgsObject> EventDataSelectedAutopartItem;
        public virtual void TransmitSelectedData(AutopartDPO value)
        {
            EventDataSelectedAutopartItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в собтыие
        public void TransmitData()
        {
            AutopartDPO autopartDPO = new AutopartDPO(); // хранение временных данных
            autopartDPO = _listAutopartViewModel.();
            TransmitSelectedData(autopartDPO); // передаём данные в событие
        }
    }
}
