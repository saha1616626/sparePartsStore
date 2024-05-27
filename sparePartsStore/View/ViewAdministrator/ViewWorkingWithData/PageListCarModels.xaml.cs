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

        #region Popup

        // кнопка выбора адреса
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            CarModelDPO carModelDPO = new CarModelDPO();
            carModelDPO = _listCarModelViewModel.TransmitModel();
            string deleteItemName = carModelDPO.NameCarModel.Trim();
            DeleteNameCarBrand.Text = deleteItemName;


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
            WorkingWithData.SaveDataDeleteCarModels();

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
        private void TextBoxNameModel(object sender, TextChangedEventArgs e)
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
                _listCarModelViewModel.HandlerTextBoxChanged(nameModel);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listCarModelViewModel.UpdateTabel();
        }
    }
}
