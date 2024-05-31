using Microsoft.VisualBasic.ApplicationServices;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sparePartsStore.ViewModel
{
    public class ListAccountViewModel : INotifyPropertyChanged
    {
        public ListAccountViewModel()
        {
            //чтение данных из БД
            ListAccountRead = GetListUser();

            //вывод данных в таблицу
            ListAccount = LoadAccountBD();
        }

        // выбранные данные из таблицы
        private Account _selectedAccount { get; set; }
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedAccount != null; } // если нажто поле в таблице
            set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
        }

        // список отображения пользователей на экране
        private ObservableCollection<Account> _listAccount { get; set; }
        public ObservableCollection<Account> ListAccount
        {
            get { return _listAccount; }
            set { _listAccount = value; OnPropertyChanged(nameof(ListAccount)); }
        }

        // метод получения списка пользователей для вывода на экран
        private ObservableCollection<Account> LoadAccountBD()
        {
            // создаём преобразованный список
            ObservableCollection<Account> account = new ObservableCollection<Account>();

            if (ListAccountRead != null)
            {
                // проходимся по списку ListUserRead для заполнения списка ListUser
                foreach (var accountItem in ListAccountRead.ToList())
                {
                    if(accountItem.AccountRoleName == "Администратор")
                    {

                    }
                    else
                    {
                        account.Add(accountItem); // добавляем данные 
                    }
                }
            }

            return account;
        }

        // коллекция считанная из БД
        public ObservableCollection<Account> ListAccountRead { get; set; } = new ObservableCollection<Account>();

        // метод для получения коллекции производителей
        private ObservableCollection<Account> GetListUser()
        {
            try
            {
                ObservableCollection<Account> accounts = new ObservableCollection<Account>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Account> accountDB = context.Accounts.ToList(); // получаем список узлов авто
                    if (accountDB != null)
                    {
                        // копируем данные в список из БД
                        foreach (var AccountItem in accountDB)
                        {
                            Account account = new Account();
                            account.AccountId = AccountItem.AccountId;
                            account.AccountLogin = AccountItem.AccountLogin;
                            account.AccountPassword = AccountItem.AccountPassword;
                            account.AccountRoleName = AccountItem.AccountRoleName;
                            account.NameOrganization = AccountItem.NameOrganization;
                            account.Inn = AccountItem.Inn;
                            account.Ogrn = AccountItem.Ogrn;
                            account.Ogrnip = AccountItem.Ogrnip;
                            account.Kpp = AccountItem.Kpp;
                            // добавляем в список
                            accounts.Add(account);
                        }
                    }
                }
                return accounts;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаём получаенные выбранные данные из таблицы
        public Account TransmitionAccount()
        {
            Account account = new Account();
            account = (Account)SelectedAccount;
            return account;
        }

        // переменная, которая хранит текущие данные в поле у PageWorkAccount
        public TextBox NameAccountInput { get; set; }

        // проверяем, есть ли совпадение данных перед добавлением
        public bool CheckingForMatchDB(Account accounts)
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            // один польлзователь может иметь один аккаунт
            Account accountNew = new Account();
            accountNew = SelectedAccount; // получаем ID пользователя, которую выбрал администратор

            // проводим проверку на уникальность пользователя
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            { 
                List<Account> account = context.Accounts.ToList(); // получаем список производителей авто
                List<Account> newAccount = new List<Account>();

                if (account != null)
                {
                    newAccount = account.Where(k => k.NameOrganization.ToLower() == NameAccountInput.Text.ToLower()).ToList(); // получаем список пользователей по заданным параметрам уникальности
                }

                // проверка наличия пользователя в определенной стране
                if (newAccount != null)
                {
                    noCoincidence = !newAccount.Any(num => num.NameOrganization.ToLower() == NameAccountInput.Text.ToLower());
                }
            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(Account account)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            // один польлзователь может иметь один аккаунт
            Account accountNew = new Account();
            accountNew = SelectedAccount; // получаем ID пользователя, которую выбрал администратор

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Account> accounts = context.Accounts.ToList(); // получаем список производителей
                List<Account> newAccount = new List<Account>();
                if (accounts != null)
                {
                    newAccount = accounts.Where(k => k.NameOrganization.ToLower() == NameAccountInput.Text.ToLower()).ToList(); // получаем список пользователей
                }

                if (newAccount != null)
                {
                    foreach (Account a in newAccount)
                    {
                        if (a.AccountId == account.AccountId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                        {
                            continue;
                        }

                        if (a.NameOrganization.ToLower() == NameAccountInput.Text.ToLower()) // если нашли совпадение в таблице
                        {
                            noCoincidence = true;
                        }
                    }
                }
            }

            return noCoincidence;
        }

        // свойство поля для ввода названия организации
        private TextBox _outNameAccount;
        public TextBox OutNameAccount
        {
            get { return _outNameAccount; }
            set
            {
                _outNameAccount = value;
                OnPropertyChanged(nameof(OutNameAccount));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListAccountRead = GetListUser();

            //вывод данных в таблицу
            ListAccount = LoadAccountBD();
        }

        // список для фильтров таблицы
        public ObservableCollection<Account> ListSearch { get; set; } = new ObservableCollection<Account>();

        // поиск пользователя
        public void HandlerTextBoxChanged(string nameAccount)
        {
            if (nameAccount.Trim() != "")
            {
                ListSearch = GetListUser(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resAccount = ListSearch.Where(num => num.NameOrganization.ToLower().Contains(nameAccount.ToLower())).ToList();

                // выведет всё, если ничего не найдено
                //if (resManufacture.Count > 0)
                //{
                //    ListManufactureDPO.Clear();// очищаем основной список
                //    // вносим актуальные данные основного списка
                //    foreach (var manufacturetem in resManufacture)
                //    {
                //        ManufactureDPO manufactureDPO = new ManufactureDPO();
                //        manufactureDPO = manufactureDPO.CopyFromCountry(manufacturetem);
                //        ListManufactureDPO.Add(manufactureDPO); // добавляем данные
                //    }
                //}

                ListAccount.Clear();// очищаем основной список
                                           // вносим актуальные данные основного списка
                foreach (var accountItem in resAccount)
                {
                    ListAccount.Add(accountItem); // добавляем данные
                }
            }

            if (nameAccount.Trim() == "")
            {
                ListAccount.Clear();// очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListUser(); // получаем актуальные данные из БД
                foreach (var knotItem in ListSearch)
                {
                    Account account = new Account();
                    ListAccount.Add(account);
                }
            }
        }

        #region PropertyModerationStatus

        // выбранный элемент списка "статус отображения товара"
        private string _selectedAccountPr;
        public string SelectedAccountPr
        {
            get { return _selectedAccountPr; }
            set
            {
                _selectedAccountPr = value;
                OnPropertyChanged(nameof(SelectedAccountPr));
            }
        }

        // вывод списка "роли пользователя"
        private ObservableCollection<string> _accountComboBoxItems;
        public ObservableCollection<string> AccountComboBoxItems
        {
            get { return _accountComboBoxItems = GetAccountOnComboBox(); }
            set
            {
                _accountComboBoxItems = value;
                OnPropertyChanged(nameof(AccountComboBoxItems));
            }
        }

        // получаем список для ComboBox ролей пользователей
        public ObservableCollection<string> GetAccountOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            ObservableCollection<string> nameRole = new ObservableCollection<string>();
            nameRole.Add("Поставщик");
            nameRole.Add("Магазин");

            return nameRole;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
