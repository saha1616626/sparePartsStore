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
            autopartDPO = _listAutopartViewModel.TransmitionAutopart();
            TransmitSelectedData(autopartDPO); // передаём данные в событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            AutopartDPO autopartDPO = new AutopartDPO();
            autopartDPO = _listAutopartViewModel.TransmitionAutopart();
            string deleteItemName = autopartDPO.NameManufacture.Trim();
            DeleteNameAutopart.Text = deleteItemName;
        }

        // скрыть фон при запуске popup
        private void MyPopup_Closed(object sender, EventArgs e)
        {
            DarkBackground.Visibility = Visibility.Collapsed;
        }

        // потеря фокуса popup
        private void DarkBackground_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //AddresPopup.IsOpen = false; // Закрыть Popup при щелчке на затемненном фоне
            //DarkBackground.Visibility = Visibility.Collapsed; // Скрыть затемненный фон
        }

        // кнопка удаления данных из таблицы (Popup)
        private void Btn_DeleteData(object sender, RoutedEventArgs e)
        {
            // вызываем событие удаления данных из таблицы
            WorkingWithData.SaveDataDeleteAutoparts();

            DeletePopup.IsOpen = false; // Закрыть Popup при щелчке на затемненном фоне
            DarkBackground.Visibility = Visibility.Collapsed; // Скрыть затемненный фон
        }

        // закрываем Popup
        private void closePopup(object sender, RoutedEventArgs e)
        {
            DeletePopup.IsOpen = false; // Закрыть Popup при щелчке на затемненном фоне
            DarkBackground.Visibility = Visibility.Collapsed; // Скрыть затемненный фон
        }

        #endregion

        // событие на ввод данных в текстовое поле поиска производителя
        private void TextBoxNameAutoParts(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameAutoParts = textBox.Text;

                // передаем данные во ViewModel
                //if(this.DataContext is ListCarBrandViewModel listCarBrandViewModel) // получаем достп к экз ViewModel
                //{
                //    listCarBrandViewModel.HandlerTextBoxChanged(nameBrand);
                //}
                _listAutopartViewModel.HandlerTextBoxChanged(nameAutoParts);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listAutopartViewModel.UpdateTabel();
        }
    }
}
