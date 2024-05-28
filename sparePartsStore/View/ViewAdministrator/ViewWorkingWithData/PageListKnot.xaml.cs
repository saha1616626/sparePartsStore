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
    /// Interaction logic for PageListKnot.xaml
    /// </summary>
    public partial class PageListKnot : Page
    {
        private readonly ListKnotViewModel _listKnotViewModel; // объект класса
        public PageListKnot()
        {
            InitializeComponent();

            //  получение экз класса ListKnotViewModel из DataContext
            _listKnotViewModel = (ListKnotViewModel)this.Resources["ListKnotViewModel"];
        }

        // кнопка добавления узла авто
        private void Btn_AddKnot(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления узла авто
            WorkingWithData.LaunchPageAddKnot();
        }

        // кнопка редактирования узла авто
        private void Btn_EditKnot(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы редактирования узла авто
            WorkingWithData.LaunchpageEditKnot();
        }

        // событие передачи выбранных данных из таблицы узла авто
        public event EventHandler<MyEventArgsObject> EventDataSelectedKnotItem;
        public virtual void TransmitSelectedData(KnotDPO value)
        {
            EventDataSelectedKnotItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в собтыие
        public void TransmitData()
        {
            KnotDPO knotDPO = new KnotDPO(); // хранение временных данных
            knotDPO = _listKnotViewModel.TransmitionKnot();
            TransmitSelectedData(knotDPO); // передаём данные в событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            KnotDPO knotDPO = new KnotDPO();
            knotDPO = _listKnotViewModel.TransmitionKnot();
            string deleteItemName = knotDPO.NameKnot.Trim();
            DeleteNameKnot.Text = deleteItemName;
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
            WorkingWithData.SaveDataDeleteKnot();

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

        // событие на ввод данных в текстовое поле поиска узлов авто
        private void TextBoxNameKnot(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameKnot = textBox.Text;

                // передаем данные во ViewModel
                //if(this.DataContext is ListCarBrandViewModel listCarBrandViewModel) // получаем достп к экз ViewModel
                //{
                //    listCarBrandViewModel.HandlerTextBoxChanged(nameBrand);
                //}
                _listKnotViewModel.HandlerTextBoxChanged(nameKnot);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listKnotViewModel.UpdateTabel();
        }
    }
}
