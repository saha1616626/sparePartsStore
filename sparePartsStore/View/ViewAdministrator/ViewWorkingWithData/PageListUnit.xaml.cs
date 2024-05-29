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
    /// Interaction logic for PageListUnit.xaml
    /// </summary>
    public partial class PageListUnit : Page
    {
        private readonly ListUnitViewModel _listUnitViewModel; // объект класса ListUnitViewModel
        public PageListUnit()
        {
            InitializeComponent();

            //  получение экз класса ListUnitViewModel из DataContext
            _listUnitViewModel = (ListUnitViewModel)this.Resources["ListUnitViewModel"];
        }

        // кнопка добавления агрегата авто
        private void Btn_AddUnit(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления агрегата
            WorkingWithData.LaunchPageAddUnit();
        }

        // кнопка редактирования авто
        private void Btn_EditUnit(object sender, RoutedEventArgs e)
        {
            // вызываем событие для запуска страницы редактирования агрегата
            WorkingWithData.LaunchpageEditUnit();
        }

        // событие передачи выбранных данных из таблицы агрегаты авто
        public event EventHandler<MyEventArgsObject> EventDataSelectedUnitItem;
        protected virtual void TransmitSelectedData(Unit value)
        {
            EventDataSelectedUnitItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в событие 
        public void TransmitData()
        {
            Unit unit = new Unit();
            unit = _listUnitViewModel.TransmitionUnit();
            TransmitSelectedData(unit); // передаём данные событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            Unit unit = new Unit();
            unit = _listUnitViewModel.TransmitionUnit();
            string deleteItemName = unit.NameUnit.Trim();
            DeleteNameUnit.Text = deleteItemName;
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
            WorkingWithData.SaveDataDeleteUnit();

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
        private void TextBoxNameUnit(object sender, TextChangedEventArgs e)
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
                _listUnitViewModel.HandlerTextBoxChanged(nameModel);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listUnitViewModel.UpdateTabel();
        }

    }
}
