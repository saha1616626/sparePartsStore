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
    /// Interaction logic for PageWorkDetail.xaml
    /// </summary>
    public partial class PageWorkDetail : Page
    {
        private readonly ListAutopartViewModel _listAutopartViewModel; // объект класса
        private Storyboard _focusAnimation; // анимация подсветки
        AuthorizationViewModel AuthorizationViewModel = new AuthorizationViewModel(); // работа с авторизацией
        public PageWorkDetail()
        {
            InitializeComponent();

            // получаем экз ListAutopartViewModel
            _listAutopartViewModel = (ListAutopartViewModel)this.Resources["ListAutopartViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени

            // если авторизованный пользователь поставщик, то скрываем для него меню выбора статуса отображения запчасти
            string role = AuthorizationViewModel.CheckingUserRole();
            if(role != null)
            {
                if(role == "Поставщик")
                {
                    CbModerationStatus.Visibility = Visibility.Hidden;
                }
            }
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private AutopartDPO getAutopartDPOs = new AutopartDPO();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(AutopartDPO autopartDPO)
        {
            getAutopartDPOs = autopartDPO; // сохраняем данные
            // передаём данные в поля
            this.CbCountry.ItemsSource = _listAutopartViewModel.GetCountryOnComboBox(); // получаем список для ComBox
            this.CbCountry.Text = autopartDPO.NameCountry;

            this.CbManufacture.ItemsSource = _listAutopartViewModel.GetManufactureOnComboBox().ToList();
            this.CbManufacture.Text = autopartDPO.NameManufacture;

            this.CbCarBrand.ItemsSource = _listAutopartViewModel.GetCarBrandOnComboBox().ToList();
            this.CbCarBrand.Text = autopartDPO.CarBrandName;

            this.CbCarModel.ItemsSource = _listAutopartViewModel.GetCarModelOnComboBox().ToList();
            this.CbCarModel.Text = autopartDPO.NameCarModel;

            this.CbUnit.ItemsSource = _listAutopartViewModel.GetUnitOnComboBox().ToList();
            this.CbUnit.Text = autopartDPO.NameUnit;

            this.CbKnot.ItemsSource = _listAutopartViewModel.GetKnotOnComboBox().ToList();
            this.CbKnot.Text = autopartDPO.NameKnot;

            this.CbModerationStatus.ItemsSource = _listAutopartViewModel.GetModerationStatusOnComboBox().ToList();
            this.CbModerationStatus.Text = autopartDPO.ModerationStatus;

            NameAutopart.Text = autopartDPO.NameAutopart.ToString();
            PriceSale.Text = autopartDPO.PriceSale.ToString();
            AvailableityStock.Text = autopartDPO.AvailableityStock.ToString();

        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // передаём данные в поля                                          
            this.CbCountry.ItemsSource = _listAutopartViewModel.GetCountryOnComboBox().ToList(); ; // получаем список для ComBox
            this.CbManufacture.ItemsSource = _listAutopartViewModel.GetManufactureOnComboBox().ToList();
            this.CbCarBrand.ItemsSource = _listAutopartViewModel.GetCarBrandOnComboBox().ToList();
            this.CbCarModel.ItemsSource = _listAutopartViewModel.GetCarModelOnComboBox().ToList();
            this.CbUnit.ItemsSource = _listAutopartViewModel.GetUnitOnComboBox().ToList();
            this.CbKnot.ItemsSource = _listAutopartViewModel.GetKnotOnComboBox().ToList();
            //this.CbModerationStatus.ItemsSource = _listAutopartViewModel.GetModerationStatusOnComboBox().ToList();
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsAutopart;
        protected virtual void TransmitSelectedData(AutopartDPO value)
        {
            EventArgsAutopart?.Invoke(this, new MyEventArgsObject { Value = value });
        }

        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit() // передаются данные для редакирования или добавления
        {

            AutopartDPO autopartDPO = new AutopartDPO();

            CarBrand carBrand = new CarBrand();
            autopartDPO.CarBrandId = getAutopartDPOs.CarBrandId;
            autopartDPO.CarBrandName = this.CbCarBrand.Text.Trim();

            CarModel carModel = new CarModel();
            autopartDPO.CarModelId = getAutopartDPOs.CarModelId;
            autopartDPO.NameCarModel = this.CbCarModel.Text.Trim();

            Unit unit = new Unit();
            autopartDPO.UnitId = getAutopartDPOs.UnitId;
            autopartDPO.NameUnit = this.CbUnit.Text.Trim();

            Knot knot = new Knot();
            autopartDPO.KnotId = getAutopartDPOs.KnotId;
            autopartDPO.NameKnot = this.CbKnot.Text.Trim();

            Country country = new Country();
            autopartDPO.CountryId = getAutopartDPOs.CountryId;
            autopartDPO.NameCountry = this.CbCountry.Text.Trim();

            Manufacture manufacture = new Manufacture();
            autopartDPO.ManufactureId = getAutopartDPOs.ManufactureId;
            autopartDPO.NameManufacture = this.CbManufacture.Text.Trim();

            string status = (string)CbModerationStatus.SelectedItem;

            autopartDPO.AutopartId = getAutopartDPOs.AutopartId;

            int resAvailableityStock;
            int.TryParse(AvailableityStock.Text.Trim(), out resAvailableityStock);
            if(resAvailableityStock != null)
            {
                autopartDPO.AvailableityStock = resAvailableityStock;
            }

            decimal resPriceSale;
            decimal.TryParse(PriceSale.Text.Trim(), out resPriceSale);
            if(resPriceSale != null)
            {
                autopartDPO.PriceSale = resPriceSale;
            }

            autopartDPO.NameAutopart = NameAutopart.Text.Trim();
            autopartDPO.ModerationStatus = _listAutopartViewModel.SelectedModerationStatus;
            autopartDPO.NumberAutopart = getAutopartDPOs.NumberAutopart;
            autopartDPO.AccountId = getAutopartDPOs.AccountId;

            TransmitSelectedData(autopartDPO);
        }

        public void TransmitAdd() // передаются данные для редакирования или добавления
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            CarBrand carBrand = new CarBrand();
            carBrand = this.CbCarBrand.SelectedItem as CarBrand;
            autopartDPO.CarBrandId = carBrand.CarBrandId;
            autopartDPO.CarBrandName = this.CbCarBrand.Text.Trim();

            CarModel carModel = new CarModel();
            carModel = this.CbCarModel.SelectedItem as CarModel;
            autopartDPO.CarModelId = carModel.CarModelId;
            autopartDPO.NameCarModel = this.CbCarModel.Text.Trim();

            Unit unit = new Unit();
            unit = this.CbUnit.SelectedItem as Unit;
            autopartDPO.UnitId = unit.UnitId;
            autopartDPO.NameUnit = this.CbUnit.Text.Trim();

            Knot knot = new Knot();
            knot = this.CbKnot.SelectedItem as Knot;
            autopartDPO.KnotId = knot.KnotId;
            autopartDPO.NameKnot = this.CbKnot.Text.Trim();

            Country country = new Country();
            country = this.CbCountry.SelectedItem as Country;
            autopartDPO.CountryId = country.CountryId;
            autopartDPO.NameCountry = this.CbCountry.Text.Trim();

            Manufacture manufacture = new Manufacture();
            manufacture = this.CbManufacture.SelectedItem as Manufacture;
            autopartDPO.ManufactureId = manufacture.ManufactureId;
            autopartDPO.NameManufacture = this.CbManufacture.Text.Trim();

            string status = (string)CbModerationStatus.SelectedItem;

            autopartDPO.AutopartId = getAutopartDPOs.AutopartId;
            autopartDPO.AvailableityStock = int.Parse(AvailableityStock.Text.Trim());
            autopartDPO.PriceSale = decimal.Parse(PriceSale.Text.Trim());
            autopartDPO.NameAutopart = NameAutopart.Text.Trim();
            autopartDPO.ModerationStatus = _listAutopartViewModel.SelectedModerationStatus;
            autopartDPO.NumberAutopart = getAutopartDPOs.NumberAutopart;

            // получаем ID аккаунта, который добавляет запчасть
            int userId = AuthorizationViewModel.weGetIdUser();
            if (userId != null)
            {
                autopartDPO.AccountId = userId;
            }

            TransmitSelectedData(autopartDPO);
        }

        // сохранение данных после редактирования или добавления данных в таблице
        private void SaveData(object sender, EventArgs e)
        {
            if (CbCarBrand.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbCarBrand); // запуск анимации
                BeginFadeAnimation(errorInput);
            }
            if (CbUnit.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbUnit);
                BeginFadeAnimation(errorInput);
            }
            if (CbCarModel.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbCarModel);
                BeginFadeAnimation(errorInput);
            }
            if (CbCountry.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbCountry);
                BeginFadeAnimation(errorInput);
            }
            if (CbKnot.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbKnot);
                BeginFadeAnimation(errorInput);
            }
            if (CbManufacture.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbManufacture);
                BeginFadeAnimation(errorInput);
            }
            // если добавляет данные поставщик, то нет проверки на поле статуса отображения, так как значение устанавливается по умолчанию
            string role = AuthorizationViewModel.CheckingUserRole();
            if (role != null)
            {
                if (role != "Поставщик")
                {
                    if (CbModerationStatus.Text.Trim().IsNullOrEmpty())
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(CbModerationStatus);
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
            if (PriceSale.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(PriceSale);
                BeginFadeAnimation(errorInput);
            }
            if (AvailableityStock.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(AvailableityStock);
                BeginFadeAnimation(errorInput);
            }
            if (NameAutopart.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(NameAutopart);
                BeginFadeAnimation(errorInput);
            }


            else // если есть текст
            {
                _listAutopartViewModel.NameAutopartInput = NameAutopart;

                bool Checking = true; // по умолчанию не повторяется

                if (RenameButtonAutopart.Content != "Редактировать") // если режим редактирования, то проверка на уникальность не выполняется
                {
                    Checking = _listAutopartViewModel.CheckingForMatchDB(getAutopartDPOs);
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listAutopartViewModel.CheckingForMatchEditDB(getAutopartDPOs);
                    if (CheckingItem)
                    {
                        errorInput.Text = "Данная запчасть уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
                    }
                    else
                    {
                        if (_listAutopartViewModel.CheckingForMatchDB(getAutopartDPOs))
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
                    WorkingWithData.SaveDataCreateOrEditAutoparts();
                }
                else // если если все поля заполнены
                {
                    if (NameAutopart.Text.Trim() == "")
                    {
                        errorInput.Text = "Заполните все поля!";
                        StartAnimation(NameAutopart); // запуск анимации
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
        private void ClosePageAddOrDeleteAutopart(object sender, RoutedEventArgs e)
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
