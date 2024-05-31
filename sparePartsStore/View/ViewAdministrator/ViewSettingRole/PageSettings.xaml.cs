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
    /// Interaction logic for PageSettings.xaml
    /// </summary>
    public partial class PageSettings : Page
    {
        private readonly AuthorizationViewModel _authorizationViewModel; // объект класса
        public PageSettings()
        {
            InitializeComponent();

            // получаем экз AuthorizationViewModel
            _authorizationViewModel = (AuthorizationViewModel)this.Resources["AuthorizationViewModel"];
        }

        #region Popup

        // кнопка удалить
        private void Btn_OpenPopup(object sender, RoutedEventArgs e)
        {
            // отображаем Popup
            DeletePopup.IsOpen = true;
            DarkBackground.Visibility = Visibility.Visible; // Показать затемненный фон
        }

        // скрыть фон при запуске popup
        private void MyPopup_Closed(object sender, EventArgs e)
        {
            DarkBackground.Visibility = Visibility.Collapsed;
        }

        // кнопка удаления данных из таблицы (Popup)
        private void Btn_DeleteData(object sender, RoutedEventArgs e)
        {
            _authorizationViewModel.LogOutYourAccount(); // выходим из аккаунта

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
    }
}
