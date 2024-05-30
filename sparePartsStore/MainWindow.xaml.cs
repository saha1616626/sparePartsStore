using Microsoft.VisualBasic.Logging;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.View;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
using sparePartsStore.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // объекты нужных страниц
        PageMainHead pageMainHead;
        ViewAuthorization viewAuthorization;

        // класс для работы с авторизацией пользователя
        private AuthorizationViewModel authorizationViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // подгружаем БД, чтобы не ждать запуска при переходе на страницу с БД
            using (SparePartsStoreContext dataContext = new SparePartsStoreContext())
            {
                List<Account> accounts = dataContext.Accounts.ToList();
            }

            // экз класса проверки авторизации
            authorizationViewModel = new AuthorizationViewModel();

            // проверяем, авторизовался пользователь или нет
            bool loginCheck = authorizationViewModel.IsCheckAccountUser();

            if(loginCheck) // если пользователь авторизован
            {
                // получаем роль пользователя
                authorizationViewModel = new AuthorizationViewModel();

                string role = authorizationViewModel.CheckingUserRole();

                if (role != null)
                {
                    if (role == "Администратор")
                    {
                        ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                                                 // запуск страницы администратора
                        pageMainHead = new PageMainHead();
                        mainFrame.Navigate(pageMainHead);
                    }
                    else if (role == "Постващик")
                    {
                        ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                    }
                    else if (role == "Магазин")
                    {
                        ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                    }
                }
            }
            else // если пользователь не авторизован
            {
                ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                // запуск страницы авторизации
                viewAuthorization = new ViewAuthorization();
                mainFrame.NavigationService.Navigate(viewAuthorization);
            }

            // подписываемся на событие входа в аккаунт
            WorkingWithData.userLogin += EntranceAccount;
        }

        // вход на страницу
        private void EntranceAccount(object sender, EventAggregator e)
        {
            // получаем роль пользователя
            authorizationViewModel = new AuthorizationViewModel();

            string role = authorizationViewModel.CheckingUserRole();

            if (role != null)
            {
                if (role == "Администратор")
                {
                    ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                    // запуск страницы администратора
                    pageMainHead = new PageMainHead();
                    mainFrame.Navigate(pageMainHead);
                }
                else if (role == "Постващик")
                {
                    ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                }
                else if (role == "Продавец")
                {
                    ClearMemoryAfterFrame(); // очистка памяти (от фрейма)
                }
            }
        }

        // очистка памяти
        private void ClearMemoryAfterFrame()
        {
            // закрываем предыдущий фрейм
            mainFrame.NavigationService?.RemoveBackEntry();
            mainFrame.Content = null;

            // очистка визуальных элементов
            this.Resources.Clear();

            // очистка всех привязанных элементов
            BindingOperations.ClearAllBindings(this);

            // сборка мусора и освобождение неиспользуемых ресурсов
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}