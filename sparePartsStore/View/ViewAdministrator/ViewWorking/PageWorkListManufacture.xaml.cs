using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkListManufacture.xaml
    /// </summary>
    public partial class PageWorkListManufacture : Page
    {
        private readonly ListManufactureViewModel _listManufactureViewModel; // объект класса
        private Storyboard _focusAnimation; // анимация подсветки
        public PageWorkListManufacture()
        {
            InitializeComponent();

            // получаем экз ListManufactureViewModel
            _listManufactureViewModel = (ListManufactureViewModel)this.Resources["ListManufactureViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private ManufactureDPO getManufactureDPOs = new ManufactureDPO();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(ManufactureDPO manufactureDPO)
        {
            getManufactureDPOs = manufactureDPO; // сохраняем данные
            // передаём данные в поля
            this.CbCountry.ItemsSource = _listManufactureViewModel.GetCountryOnComboBox(); // получаем список для ComBox
            CbCountry.Text = manufactureDPO.NameCountry;
            nameManufacture.Text = manufactureDPO.NameManufacture.Trim();
        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // передаём данные в поля                                          
            CbCountry.ItemsSource = _listManufactureViewModel.GetCountryOnComboBox().ToList(); // получаем список для ComBox
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsManufacture;
        protected virtual void TransmitData(ManufactureDPO value)
        {
            EventArgsManufacture?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {
            ManufactureDPO manufactureDPO = new ManufactureDPO();

            Country country = new Country();
            country = (Country)CbCountry.SelectedItem; // получаем ID из ComBox
            manufactureDPO.CountryId = country.CountryId; // передали ID
            manufactureDPO.ManufactureId = getManufactureDPOs.ManufactureId;
            manufactureDPO.NameManufacture = nameManufacture.Text.Trim();
            manufactureDPO.NameCountry = this.CbCountry.Text.Trim();
            TransmitData(manufactureDPO); // передали название марки авто
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (nameManufacture.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(nameManufacture); // запуск анимации
                BeginFadeAnimation(errorInput);
            }
            if (CbCountry.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbCountry);
                BeginFadeAnimation(errorInput);
            }

            else // если есть текст
            {
                _listManufactureViewModel.NameManufactureInput = nameManufacture;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonManufacture.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listManufactureViewModel.CheckingForMatchDB();
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listManufactureViewModel.CheckingForMatchEditDB(getManufactureDPOs);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данный производитель уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                    else
                    {
                        if (_listManufactureViewModel.CheckingForMatchDB())
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
                    WorkingWithData.SaveDataCreateOrEditManufacture();
                }
                else // если если все поля заполнены
                {
                    if (nameManufacture.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(nameManufacture); // запуск анимации
                        BeginFadeAnimation(errorInput);
                    }
                    else
                    {
                        errorInput.Text = "Данный производитель уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteManufacture(object sender, RoutedEventArgs e)
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
