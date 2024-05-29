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
    /// Interaction logic for PageListCountry.xaml
    /// </summary>
    public partial class PageListCountry : Page
    {
        private readonly ListCountryViewModel _listCountryViewModel; // объект класса ListCountryViewModel
        public PageListCountry()
        {
            InitializeComponent();

            //  получение экз класса ListCountryViewModel из DataContext
            _listCountryViewModel = (ListCountryViewModel)this.Resources["ListCountryViewModel"];
        }

        // кнопка добавления страны
        private void Btn_AddCountry(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления страны
            WorkingWithData.LaunchPageAddCountry();
        }

        // кнопка редактирования страны
        private void Btn_EditCountry(object sender, RoutedEventArgs e)
        {
            // вызываем событие для запуска страницы редактирования страны
            WorkingWithData.LaunchpageEditCountry();
        }

        // событие передачи выбранных данных из таблицы страны
        public event EventHandler<MyEventArgsObject> EventDataSelectedUnitItem;
        protected virtual void TransmitSelectedData(Country value)
        {
            EventDataSelectedUnitItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в событие 
        public void TransmitData()
        {
            Country country = new Country();
            country = _listCountryViewModel.TransmitionCountry();
            TransmitSelectedData(country); // передаём данные событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            Country country = new Country();
            country = _listCountryViewModel.TransmitionCountry();
            string deleteItemName = country.NameCountry.Trim();
            DeleteNameCountry.Text = deleteItemName;
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
            WorkingWithData.SaveDataDeleteCountry();

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

        // событие на ввод данных в текстовое поле поиска моделей авто
        private void TextBoxNameCountry(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameModel = textBox.Text;

                // передаем данные во ViewModel
                //if(this.DataContext is ListCarBrandViewModel listCarBrandViewModel) // получаем достп к экз ViewModel
                //{
                //    listCarBrandViewModel.HandlerTextBoxChanged(nameBrand);
                //}
                _listCountryViewModel.HandlerTextBoxChanged(nameModel);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listCountryViewModel.UpdateTabel();
        }
    }
}
