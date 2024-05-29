using sparePartsStore.Helper;
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
    /// Interaction logic for PageListManufacture.xaml
    /// </summary>
    public partial class PageListManufacture : Page
    {
        private readonly ListManufactureViewModel _listManufactureViewModel; // объект класса
        public PageListManufacture()
        {
            InitializeComponent();

            // получаем экз ListManufactureViewModel
            _listManufactureViewModel = (ListManufactureViewModel)this.Resources["ListManufactureViewModel"];

        }

        // кнопка добавления производителя
        private void Btn_AddManufacture(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления производителя
            WorkingWithData.LaunchPageAddManufacture();
        }

        // кнопка редактирования производителя
        private void Btn_EditManufacture(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы редактирования производителя
            WorkingWithData.LaunchPageEditManufacture();
        }

        // событие передачи выбранных данных из таблицы производителя
        public event EventHandler<MyEventArgsObject> EventDataSelectedManufactureItem;
        public virtual void TransmitSelectedData(ManufactureDPO value)
        {
            EventDataSelectedManufactureItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в собтыие
        public void TransmitData()
        {
            ManufactureDPO manufactureDPO = new ManufactureDPO(); // хранение временных данных
            manufactureDPO = _listManufactureViewModel.TransmitionKnot();
            TransmitSelectedData(manufactureDPO); // передаём данные в событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            ManufactureDPO manufactureDPO = new ManufactureDPO();
            manufactureDPO = _listManufactureViewModel.TransmitionKnot();
            string deleteItemName = manufactureDPO.NameManufacture.Trim();
            DeleteNameManufacture.Text = deleteItemName;
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
            WorkingWithData.SaveDataDeleteManufacture();

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
        private void TextBoxNameManufacture(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameManufacture = textBox.Text;

                // передаем данные во ViewModel
                //if(this.DataContext is ListCarBrandViewModel listCarBrandViewModel) // получаем достп к экз ViewModel
                //{
                //    listCarBrandViewModel.HandlerTextBoxChanged(nameBrand);
                //}
                _listManufactureViewModel.HandlerTextBoxChanged(nameManufacture);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listManufactureViewModel.UpdateTabel();
        }
    }
}
