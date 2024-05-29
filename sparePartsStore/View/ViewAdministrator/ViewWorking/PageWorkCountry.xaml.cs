using Microsoft.IdentityModel.Tokens;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
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
    /// Interaction logic for PageWorkCountry.xaml
    /// </summary>
    public partial class PageWorkCountry : Page
    {
        private readonly ListCountryViewModel _listCountryViewModel; // здесь храним экз. класса ListCountryViewModel
        private Storyboard _focusAnimation; // анимация подсветки

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private Country getCountry = new Country();
        public PageWorkCountry()
        {
            InitializeComponent();

            // получаем экз ListCountryViewModel
            _listCountryViewModel = (ListCountryViewModel)this.Resources["ListCountryViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(Country country)
        {
            getCountry = country; // сохроняем данные из MainHeadViewModel
            // передаём данные в поля
            nameCountry.Text = country.NameCountry.Trim();
        }

        // событие передачи данных из текстового поля
        public EventHandler<MyEventArgsObject> EventArgsCountry;
        protected virtual void TransmitData(Country value)
        {
            EventArgsCountry?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            Country country = new Country();
            country.CountryId = getCountry.CountryId;
            country.NameCountry = nameCountry.Text.Trim();
            TransmitData(country); // передаем страну
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (nameCountry.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameCountry); // запуск анимации
                BeginFadeAnimation(errorInput);
            }

            else // если все поля заполнены
            {
                _listCountryViewModel.NameCountryInput = nameCountry;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonBrand.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listCountryViewModel.CheckingForMatchDB();
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listCountryViewModel.CheckingForMatchEditDB(getCountry);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данная страна уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                    else
                    {
                        if (_listCountryViewModel.CheckingForMatchDB())
                        {
                            Checking = true;
                        }
                        else
                        {
                            errorInput.Text = "";
                            Checking = false;
                        }

                    }
                }

                // проверяем нет ли совпадения в БД
                if (Checking)
                {
                    // вызываем событие для сохранения данных в классе MainHeadViewModel
                    WorkingWithData.SaveDataCreateOrEditCountry();
                }
                else // если все поля заполнены
                {
                    if (nameCountry.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameCountry); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данная страна уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteCountry(object sender, RoutedEventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePage();
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

        // запуск анимации для TextBox
        private void StartAnimation(TextBox textBox)
        {
            _focusAnimation.Begin(textBox);
        }
    }
}
