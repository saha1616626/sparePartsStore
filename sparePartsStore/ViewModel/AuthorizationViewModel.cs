using sparePartsStore.Helper;
using sparePartsStore.View.ViewAdministrator.ViewWorkingWithData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Model;
using sparePartsStore.Helper.Authorization;
using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System.Data;
using Microsoft.Identity.Client.NativeInterop;

namespace sparePartsStore.ViewModel
{
    public class AuthorizationViewModel : INotifyPropertyChanged // для страницы авторизации
    {
        // путь к json
        readonly string path = @"E:\3comm\Documents\Предметы\Курс 3.2\Учебная практика\Приложение\sparePartsStore\sparePartsStore\Helper\Authorization\AuthorizationStatus.json";
        public string Error { get; set; } // формирование сообщения при генирации ошибки 

        // конструктор
        public AuthorizationViewModel()
        {
    
        }

        // свойства страницы авторизации
        #region Properties

        // поле для вывода ошибки
        private string _errorInput { get; set; }
        public string ErrorInput
        {
            get { return _errorInput; }
            set
            {
                _errorInput = value; OnPropertyChanged(nameof(ErrorInput)); 
            }
        }

        public TextBlock textBoxError; // свойство логина, ViewAuthorization

        // логин
        private string _inputLogin;
        public string InputLogin
        {
            get { return _inputLogin; }
            set
            {
                _inputLogin = value;
                OnPropertyChanged(nameof(InputLogin)); // Уведомляем об изменении свойства
            }
        }

        public TextBox textBoxLogin; // свойство логина, передаётся из ViewAuthorization


        // пароль
        private string _inputPassword; // пароль
        public string InputPassword
        {
            get { return _inputPassword; }
            set
            {
                _inputPassword = value;
                OnPropertyChanged(nameof(InputPassword)); // Уведомляем об изменении свойства
            }
        }

        public PasswordBox textBoxPassword; // свойство пароля, передаётся из ViewAuthorization

        // свойство анимации
        public Storyboard _focusAnimation;


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

        // запуск анимации для TextBox
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }

        private void StartAnimation(PasswordBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }

        #endregion


        // команда входа в аккаунт
        private RelayCommand _entrance {  get; set; }
        public RelayCommand Entrance
        {
            get
            {
                return _entrance ??
                    (_entrance = new RelayCommand(obj =>
                    {
                        // передаём поля из ViewAuthorization
                        WorkingWithData.UpdatePropertyViewAuthorization();

                        // проверка заполнения данных
                        if (!string.IsNullOrEmpty(InputLogin) && !string.IsNullOrEmpty(InputPassword))
                        {
                            String? inputLogin = InputLogin.Trim(); // логин пользователя
                            String? inputPassword = InputPassword.Trim(); // пароль пользователя   

                            // подключаем БД
                            using(SparePartsStoreContext dataContext = new SparePartsStoreContext())
                            {
                                List<Model.Account> accounts = dataContext.Accounts.ToList();

                                // находим логин в БД, совпадающий с веденным пользователем (Идентификация)
                                Model.Account account = accounts.FirstOrDefault(a => a.AccountLogin == inputLogin);
                                // если данные нашли
                                if (account != null)
                                {
                                    // проверяем введенный пароль (Аутентификация)
                                    bool passwordCheck = PasswordHasher.VerifyPassword(InputPassword, account.AccountPassword);

                                    // если пароль совпал
                                    if(passwordCheck)
                                    {
                                        MessageBox.Show("Добро пожаловать: " + account.NameOrganization);
                                        // передаём в JSON состояние, что мы вошли в аккаунт
                                        AuthorizationEntrance authorizationEntrance = new AuthorizationEntrance(); // класс авторизации
                                        authorizationEntrance.Entrance = true; // пользователь вошёл в аккаунт
                                        authorizationEntrance.UserId = account.AccountId;
                                        authorizationEntrance.UserRole = account.AccountRoleName;

                                        try
                                        {
                                            var jsonAuthorization = JsonConvert.SerializeObject(authorizationEntrance); //  перзапись данных в формате json
                                            // записываем обновленные данные в JSON
                                            File.WriteAllText(path, jsonAuthorization);
                                            // события перхода на главную страницу авторизованного пользователя
                                            WorkingWithData.UserLogin();
                                        }
                                        catch (Exception ex)
                                        {
                                            Error = "Ошибка записи в json файла /n" + ex.Message;
                                        }
                                    }
                                    else
                                    {
                                        StartAnimation(textBoxPassword); // запуск анимации
                                        textBoxError.Text = "Неверный пароль!";
                                        BeginFadeAnimation(textBoxError);
                                    }
                                }
                                else
                                {
                                    StartAnimation(textBoxLogin); // запуск анимации
                                    textBoxError.Text = "Пользователь не найден!";
                                    BeginFadeAnimation(textBoxError);
                                }
                            }

                        }
                        else // если какое либо поле пустое
                        {
                            if (string.IsNullOrEmpty(InputLogin) && string.IsNullOrEmpty(InputPassword))
                            {
                                StartAnimation(textBoxPassword); // запуск анимации
                                StartAnimation(textBoxLogin);
                                textBoxError.Text = "Заполните все поля!";
                                BeginFadeAnimation(textBoxError);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(InputPassword))
                                {
                                    StartAnimation(textBoxPassword); // запуск анимации
                                    textBoxError.Text = "Введите пароль!";
                                    BeginFadeAnimation(textBoxError);
                                }

                                if (string.IsNullOrEmpty(InputLogin))
                                {
                                    StartAnimation(textBoxLogin); // запуск анимации
                                    textBoxError.Text = "Введите логин!";
                                    BeginFadeAnimation(textBoxError);
                                }
                            }
                        }

                    }, (obj) => true));
            }
        }

        // проверяем роль, под которой вошел польозватель
        public string CheckingUserRole()
        {
            string role = string.Empty;

            // чтение JSON
            string JSON = File.ReadAllText(path);
            // получаем данные из JSON
            AuthorizationEntrance? account = JsonConvert.DeserializeObject<AuthorizationEntrance>(JSON);

            if(account.UserRole != null)
            {
                role = account.UserRole;
            }

            return role;
        }


        // проверяем, авторизовался пользователь или нет
        public bool IsCheckAccountUser()
        {
            bool IsCheck = false; // по умолчанию не авторизовался

            // чтение JSON
            string JSON = File.ReadAllText(path);
            // получаем данные из JSON
            AuthorizationEntrance? account = JsonConvert.DeserializeObject<AuthorizationEntrance>(JSON);

            if (account.Entrance != null)
            {
                IsCheck = account.Entrance;
            }

            return IsCheck;
        }

        // получаем Id пользователя, который авторизован
        public int weGetIdUser()
        {
            int id = 0;

            // чтение JSON
            string JSON = File.ReadAllText(path);
            // получаем данные из JSON
            AuthorizationEntrance? account = JsonConvert.DeserializeObject<AuthorizationEntrance>(JSON);

            if (account.UserId != null)
            {
                id = account.UserId;
            }

            return id;
        }

        // метод выхода из аккаунта
        public void LogOutYourAccount()
        {
            // очищаем JSON
            AuthorizationEntrance authorizationEntrance = new AuthorizationEntrance(); // класс авторизации
            authorizationEntrance.Entrance = false; // пользователь вышел из аккаунт

            try
            {
                var jsonAuthorization = JsonConvert.SerializeObject(authorizationEntrance); //  перзапись данных в формате json
                                                                                            // записываем обновленные данные в JSON
                File.WriteAllText(path, jsonAuthorization);
                // события перхода на главную страницу авторизованного пользователя
                WorkingWithData.UserLogin();
            }
            catch (Exception ex)
            {
                Error = "Ошибка записи в json файла /n" + ex.Message;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
