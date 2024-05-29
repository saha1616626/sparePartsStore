using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Helper;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore.View
{
    /// <summary>
    /// Interaction logic for ViewAuthorization.xaml
    /// </summary>
    public partial class ViewAuthorization : Page // страницы авторизации
    {
        private readonly AuthorizationViewModel _authorizationViewModel; // здесь храним экз. класса AuthorizationViewModel
        public ViewAuthorization()
        {
            InitializeComponent();

            // получаем экз AuthorizationViewModel
            _authorizationViewModel = (AuthorizationViewModel)this.Resources["AuthorizationViewModel"];

            // вызваем событие передачи полей и объектов в AuthorizationViewModel
            WorkingWithData.updatePropertyViewAuthorization += UpdateProperty;
        }

        // передаём объекты
        public void UpdateProperty(object sender, EventAggregator e)
        {
            // передаём объекты полей для манипулирования
            _authorizationViewModel.textBoxError = errorInput;
            _authorizationViewModel.textBoxLogin = TbLogin;
            _authorizationViewModel.textBoxPassword = TbPassword;

            // ссылка на анимацию
            _authorizationViewModel._focusAnimation = (Storyboard)FindResource("FocusAnimation");
        }

        // обработчик ввода пароля, так как нельзя напрямую связаться с AuthorizationViewModel напрямую, потому что PasswordBox не наследуется от DependencyObject
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _authorizationViewModel.InputPassword = ((PasswordBox)sender).Password; 
        }
    }
}
