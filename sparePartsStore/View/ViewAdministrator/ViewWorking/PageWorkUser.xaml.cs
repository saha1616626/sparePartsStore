using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Logging;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkUser.xaml
    /// </summary>
    public partial class PageWorkUser : Page
    {
        private readonly ListAccountViewModel _listAccountViewModel; // здесь храним экз. класса ListAccountViewModel
        private Storyboard _focusAnimation; // анимация подсветки
        public PageWorkUser()
        {
            InitializeComponent();

            // получаем экз ListCountryViewModel
            _listAccountViewModel = (ListAccountViewModel)this.Resources["ListAccountViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private Account getAccount = new Account();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(Account account)
        {
            getAccount = account; // сохроняем данные из MainHeadViewModel
            // передаём данные в поля
            if(account.AccountLogin != null)
            {
                Login.Text = account.AccountLogin.Trim();
            }
            if (account.AccountPassword != null)
            {
                Password.Text = account.AccountPassword.Trim();
            }
            if (account.NameOrganization != null)
            {
                nameOrganization.Text = account.NameOrganization.Trim();
            }
            if (account.Ogrn != null)
            {
                OGRN.Text = account.Ogrn.Trim();
            }
            if(account.AccountRoleName != null)
            {
                this.CbAccount.Text = account.AccountRoleName.Trim();
            }
            if(account.Inn != null)
            {
                INN.Text = account.Inn.Trim();
            }
            if (account.Ogrn != null)
            {
                OGRN.Text = account.Ogrn.Trim();
            }
            if (account.Ogrn != null)
            {
                Ogrnip.Text = account.Ogrn.Trim();
            }
        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // передаём данные в поля                                          
            CbAccount.ItemsSource = _listAccountViewModel.GetAccountOnComboBox().ToList(); // получаем список для ComBox
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsAccount;
        protected virtual void TransmitData(Account value)
        {
            EventArgsAccount?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            Account account = new Account();

            account.AccountId = getAccount.AccountId;
            account.AccountRoleName = (string)CbAccount.SelectedItem;
            account.AccountLogin = Login.Text;
            account.AccountPassword = Password.Text;
            account.NameOrganization = nameOrganization.Text;
            account.Ogrn = OGRN.Text;
            account.Inn = INN.Text;
            account.Ogrnip = Ogrnip.Text;
            account.Kpp = KPP.Text;

            TransmitData(account); // передали название марки авто
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (Login.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(Login); // запуск анимации
                BeginFadeAnimation(errorInput);
            }
            if (Password.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(Password);
                BeginFadeAnimation(errorInput);
            }
            if (nameOrganization.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameOrganization);
                BeginFadeAnimation(errorInput);
            }
            if (CbAccount.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbAccount);
                BeginFadeAnimation(errorInput);
            }
            if (INN.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(INN);
                BeginFadeAnimation(errorInput);
            }

            else // если есть текст
            {
                _listAccountViewModel.NameAccountInput = nameOrganization; // передаём название организации для поиска и проверки уникальности пользователя

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonUser.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listAccountViewModel.CheckingForMatchDB(getAccount);
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listAccountViewModel.CheckingForMatchEditDB(getAccount);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данный пользователь уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                }

                // проверяем нет ли совпадения в БД
                if (Checking)
                {
                    // вызываем событие для сохранения данных в классе MainHeadViewModel
                    WorkingWithData.SaveDataCreateOrEditUser();
                }
                else // если если все поля заполнены
                {
                    if (nameOrganization.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameOrganization); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данный пользователь уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }


        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteAccount(object sender, RoutedEventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePageUser();
        }

        // анимация уведомления
        private void BeginFadeAnimation(TextBlock textBox)
        {
            textBox.IsEnabled = true;
            textBox.Opacity = 1.0;

            Storyboard storyboard = new Storyboard();
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2)
            };
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(TextBlock.OpacityProperty));
            storyboard.Children.Add(fadeAnimation);
            storyboard.Completed += (s, e) => textBox.IsEnabled = false;
            storyboard.Begin(textBox);
        }

        // запуск анимации
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }
        // запуск анимации с перегрузкой
        private void StartAnimation(ComboBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }
    }
}
