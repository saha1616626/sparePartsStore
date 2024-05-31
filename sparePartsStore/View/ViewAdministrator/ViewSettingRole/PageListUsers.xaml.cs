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

namespace sparePartsStore.View.ViewAdministrator.ViewSettingRole
{
    /// <summary>
    /// Interaction logic for PageListUsers.xaml
    /// </summary>
    public partial class PageListUsers : Page
    {
        private readonly ListAccountViewModel _listAccountViewModel; // объект класса ListAccountViewModel
        public PageListUsers()
        {
            InitializeComponent();

            //  получение экз класса ListCountryViewModel из DataContext
            _listAccountViewModel = (ListAccountViewModel)this.Resources["ListAccountViewModel"];
        }

        // кнопка добавления пользователя
        private void Btn_AddUser(object sender, RoutedEventArgs e)
        {
            // вызваем событие в MainHeadViewModel для запуска страницы добавления пользователя
            WorkingWithData.LaunchPageAddUser();
        }

        // кнопка редактирования пользователя
        private void Btn_EditUser(object sender, RoutedEventArgs e)
        {
            // вызываем событие для запуска страницы редактирования пользователя
            WorkingWithData.LaunchPageEditUser();
        }

        // событие передачи выбранных данных из таблицы пользователи
        public event EventHandler<MyEventArgsObject> EventDataSelectedUserItem;
        protected virtual void TransmitSelectedData(Account value)
        {
            EventDataSelectedUserItem?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод для передачи из таблицы выбранных данных в событие 
        public void TransmitData()
        {
            Account account = new Account();
            account = _listAccountViewModel.TransmitionAccount();
            TransmitSelectedData(account); // передаём данные событие
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон

            // получаем объект для отображения
            Account account = new Account();
            account = _listAccountViewModel.TransmitionAccount();
            string deleteItemName = account.NameOrganization.Trim();
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
            WorkingWithData.SaveDataDeleteUsers();

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
        private void TextBoxNameUser(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameNameUser = textBox.Text;

                // передаем данные во ViewModel
                //if(this.DataContext is ListCarBrandViewModel listCarBrandViewModel) // получаем достп к экз ViewModel
                //{
                //    listCarBrandViewModel.HandlerTextBoxChanged(nameBrand);
                //}
                _listAccountViewModel.HandlerTextBoxChanged(nameNameUser);
            }
        }

        // обновляем список отображения данных в таблице
        public void UpTable()
        {
            _listAccountViewModel.UpdateTabel();
        }

    }
}
