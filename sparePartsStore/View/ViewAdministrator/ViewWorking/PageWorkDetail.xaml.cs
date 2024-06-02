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
            if (role != null)
            {
                if (role == "Поставщик")
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
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<CarBrand> carBrands = context.CarBrands.ToList();
                List<CarModel> carModels = context.CarModels.ToList();
                List<Unit> units = context.Units.ToList();
                List<Knot> knots = context.Knots.ToList();
                List<Country> countries = context.Countries.ToList();
                List<Manufacture> manufactures = context.Manufactures.ToList();
                List<Account> accounts = context.Accounts.ToList();
                List<Autopart> autoparts = context.Autoparts.ToList();

                getAutopartDPOs = autopartDPO; // сохраняем данные
                                               // передаём данные в поля
                _listAutopartViewModel.NameCountryComboBoxItems = _listAutopartViewModel.GetCountryOnComboBox(); // получаем список для ComBox
                Country country = countries.FirstOrDefault(c => c.CountryId == autopartDPO.CountryId);
                if (country != null)
                {
                    _listAutopartViewModel.SelectedCountry = country;
                    this.CbCountry.Text = autopartDPO.NameCountry;
                }

                Manufacture manufacture = manufactures.FirstOrDefault(m => m.ManufactureId == autopartDPO.ManufactureId);
                if (manufacture != null)
                {
                    _listAutopartViewModel.SelectedManufacture = manufacture;
                    this.CbManufacture.Text = autopartDPO.NameManufacture;
                }

                _listAutopartViewModel.NameCarBrandComboBoxItems = _listAutopartViewModel.GetCarBrandOnComboBox();
                CarBrand carBrand = carBrands.FirstOrDefault(c => c.CarBrandId == autopartDPO.CarBrandId);
                if (carBrand != null)
                {
                    _listAutopartViewModel.SelectedCarBrand = carBrand;
                    this.CbCarBrand.Text = autopartDPO.CarBrandName;
                }


                CarModel carModel = carModels.FirstOrDefault(c => c.CarModelId == autopartDPO.CarModelId);
                if (carModel != null)
                {
                    _listAutopartViewModel.SelectedCarModel = carModel;
                    this.CbCarModel.Text = autopartDPO.NameCarModel;
                }

                _listAutopartViewModel.NameUnitComboBoxItems = _listAutopartViewModel.GetUnitOnComboBox();
                Unit unit = units.FirstOrDefault(u => u.UnitId == autopartDPO.UnitId);
                if(unit != null)
                {
                    _listAutopartViewModel.SelectedUnit = unit;
                    this.CbUnit.Text = autopartDPO.NameUnit;
                }
                

                Knot knot = knots.FirstOrDefault(k => k.KnotId == autopartDPO.KnotId);
                if(knot != null)
                {
                    _listAutopartViewModel.SelectedKnot = knot;
                    this.CbKnot.Text = autopartDPO.NameKnot;
                }
                
                _listAutopartViewModel.ModerationStatusComboBoxItems = _listAutopartViewModel.GetModerationStatusOnComboBox();
                _listAutopartViewModel.SelectedModerationStatus = autopartDPO.ModerationStatus;
                this.CbModerationStatus.Text = autopartDPO.ModerationStatus;

                Autopart autopart = autoparts.FirstOrDefault(a => a.AutopartId == autopartDPO.AutopartId);
                if (autopart != null)
                {
                    NameAutopart.Text = autopart.NameAutopart;
                    PriceSale.Text = autopart.PriceSale.ToString();
                    AvailableityStock.Text = autopart.AvailableityStock.ToString();
                }
            }

        }

        // вывод данных в ComBox при добавлении
        public void DataReceptionAdd()
        {
            // начальные данные для ComBox                                         
            _listAutopartViewModel.NameCountryComboBoxItems = _listAutopartViewModel.GetCountryOnComboBox();
            _listAutopartViewModel.NameManufactureComboBoxItems = _listAutopartViewModel.GetManufactureOnComboBox();
            _listAutopartViewModel.NameCarBrandComboBoxItems = _listAutopartViewModel.GetCarBrandOnComboBox();
            _listAutopartViewModel.NameCarModelComboBoxItems = _listAutopartViewModel.GetCarModelOnComboBox();
            _listAutopartViewModel.NameUnitComboBoxItems = _listAutopartViewModel.GetUnitOnComboBox();
            _listAutopartViewModel.NameKnotComboBoxItems = _listAutopartViewModel.GetKnotOnComboBox();
            //this.CbModerationStatus.ItemsSource = _listAutopartViewModel.GetModerationStatusOnComboBox().ToList();
        }

        // кнопка сброса данных в ComboBox (очищаем поле бренд и марка авто)
        private void clearCarBrandAndCarModel(object sender, RoutedEventArgs e)
        {
            _listAutopartViewModel.editCarBrand = false;
            _listAutopartViewModel.editCarModel = false;
            _listAutopartViewModel.NameCarBrandComboBoxItems = _listAutopartViewModel.GetCarBrandOnComboBox();
            _listAutopartViewModel.NameCarModelComboBoxItems = _listAutopartViewModel.GetCarModelOnComboBox();
        }

        // кнопка сброса данных в ComboBox (очищаем поле узел и агрегат)
        private void clearKnotAndUnit(object sender, RoutedEventArgs e)
        {
            _listAutopartViewModel.editUnit = false;
            _listAutopartViewModel.editKnot = false;
            _listAutopartViewModel.NameUnitComboBoxItems = _listAutopartViewModel.GetUnitOnComboBox();
            _listAutopartViewModel.NameKnotComboBoxItems = _listAutopartViewModel.GetKnotOnComboBox();
        }

        // кнопка сброса данных в ComboBox (очищаем поле производитель и фабрика)
        private void clearManufactureAndCountry(object sender, RoutedEventArgs e)
        {
            _listAutopartViewModel.editCountry = false;
            _listAutopartViewModel.editManufacture = false;
            _listAutopartViewModel.NameCountryComboBoxItems = _listAutopartViewModel.GetCountryOnComboBox(); // получаем список для ComBox
            _listAutopartViewModel.NameManufactureComboBoxItems = _listAutopartViewModel.GetManufactureOnComboBox(); // начальные данные для ComBox
        }

        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> EventArgsAutopart;
        protected virtual void TransmitSelectedData(AutopartDPO value)
        {
            EventArgsAutopart?.Invoke(this, new MyEventArgsObject { Value = value });
        }



        public void TransmitAdd() // передаются данные для редакирования или добавления
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            CarBrand carBrand = (CarBrand)CbCarBrand.SelectedItem;
            if (carBrand != null)
            {
                autopartDPO.CarBrandId = carBrand.CarBrandId;
                autopartDPO.CarBrandName = carBrand.NameCarBrand;
            }
            CarModel carModel = (CarModel)CbCarModel.SelectedItem;
            if (carModel != null)
            {
                autopartDPO.CarModelId = carModel.CarModelId;
                autopartDPO.NameCarModel = carModel.NameCarModel;
            }
            Unit unit = (Unit)CbUnit.SelectedItem;
            if (unit != null)
            {
                autopartDPO.UnitId = unit.UnitId;
                autopartDPO.NameUnit = unit.NameUnit;
            }
            Knot knot = (Knot)CbKnot.SelectedItem;
            if (knot != null)
            {
                autopartDPO.KnotId = knot.KnotId;
                autopartDPO.NameKnot = knot.NameKnot;
            }
            Country country = (Country)CbCountry.SelectedItem;
            if (country != null)
            {
                autopartDPO.CountryId = country.CountryId;
                autopartDPO.NameCountry = country.NameCountry;
            }
            Manufacture manufacture = (Manufacture)CbManufacture.SelectedItem;
            if (manufacture != null)
            {
                autopartDPO.ManufactureId = manufacture.ManufactureId;
                autopartDPO.NameManufacture = manufacture.NameManufacture;
            }
            if (PriceSale.Text != null)
            {
                autopartDPO.PriceSale = decimal.Parse(PriceSale.Text);
            }
            if (AvailableityStock.Text != null)
            {
                autopartDPO.AvailableityStock = int.Parse(AvailableityStock.Text);
            }
            if (CbModerationStatus.Text != null)
            {
                autopartDPO.ModerationStatus = (string)CbModerationStatus.Text;
            }
            if (getAutopartDPOs.AutopartId != null)
            {
                autopartDPO.AutopartId = getAutopartDPOs.AutopartId;
            }
            if (NameAutopart.Text != null)
            {
                autopartDPO.NameAutopart = NameAutopart.Text.Trim();
            }
            if (getAutopartDPOs.NumberAutopart != null)
            {
                autopartDPO.NumberAutopart = getAutopartDPOs.NumberAutopart;
            }

            // получаем ID аккаунта, который добавляет запчасть
            int userId = AuthorizationViewModel.weGetIdUser();
            if (userId != null)
            {
                autopartDPO.AccountId = userId;
            }

            TransmitSelectedData(autopartDPO);
        }

        // передаём данные на сохранение после редактирования
        private AutopartDPO GetTheDataEdit()
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            CarBrand carBrand = (CarBrand)CbCarBrand.SelectedItem;
            if (carBrand != null)
            {
                autopartDPO.CarBrandId = carBrand.CarBrandId;
                autopartDPO.CarBrandName = carBrand.NameCarBrand;
            }
            CarModel carModel = (CarModel)CbCarModel.SelectedItem;
            if (carModel != null)
            {
                autopartDPO.CarModelId = carModel.CarModelId;
                autopartDPO.NameCarModel = carModel.NameCarModel;
            }
            Unit unit = (Unit)CbUnit.SelectedItem;
            if (unit != null)
            {
                autopartDPO.UnitId = unit.UnitId;
                autopartDPO.NameUnit = unit.NameUnit;
            }
            Knot knot = (Knot)CbKnot.SelectedItem;
            if (knot != null)
            {
                autopartDPO.KnotId = knot.KnotId;
                autopartDPO.NameKnot = knot.NameKnot;
            }
            Country country = (Country)CbCountry.SelectedItem;
            if (country != null)
            {
                autopartDPO.CountryId = country.CountryId;
                autopartDPO.NameCountry = country.NameCountry;
            }
            Manufacture manufacture = (Manufacture)CbManufacture.SelectedItem;
            if (manufacture != null)
            {
                autopartDPO.ManufactureId = manufacture.ManufactureId;
                autopartDPO.NameManufacture = manufacture.NameManufacture;
            }
            if (PriceSale.Text != null)
            {
                autopartDPO.PriceSale = decimal.Parse(PriceSale.Text);
            }
            if (AvailableityStock.Text != null)
            {
                autopartDPO.AvailableityStock = int.Parse(AvailableityStock.Text);
            }
            if (CbModerationStatus.Text != null)
            {
                autopartDPO.ModerationStatus = (string)CbModerationStatus.Text;
            }

            return autopartDPO;
        }

        // метод для получения введенных данных для добавления в таблицу
        private AutopartDPO GetTheDataAdd()
        {
            AutopartDPO autopartDPO = new AutopartDPO();

            CarBrand carBrand = (CarBrand)CbCarBrand.SelectedItem;
            if (carBrand != null)
            {
                autopartDPO.CarBrandId = carBrand.CarBrandId;
                autopartDPO.CarBrandName = carBrand.NameCarBrand;
            }
            CarModel carModel = (CarModel)CbCarModel.SelectedItem;
            if (carModel != null)
            {
                autopartDPO.CarModelId = carModel.CarModelId;
                autopartDPO.NameCarModel = carModel.NameCarModel;
            }
            Unit unit = (Unit)CbUnit.SelectedItem;
            if (unit != null)
            {
                autopartDPO.UnitId = unit.UnitId;
                autopartDPO.NameUnit = unit.NameUnit;
            }
            Knot knot = (Knot)CbKnot.SelectedItem;
            if (knot != null)
            {
                autopartDPO.KnotId = knot.KnotId;
                autopartDPO.NameKnot = knot.NameKnot;
            }
            Country country = (Country)CbCountry.SelectedItem;
            if (country != null)
            {
                autopartDPO.CountryId = country.CountryId;
                autopartDPO.NameCountry = country.NameCountry;
            }
            Manufacture manufacture = (Manufacture)CbManufacture.SelectedItem;
            if (manufacture != null)
            {
                autopartDPO.ManufactureId = manufacture.ManufactureId;
                autopartDPO.NameManufacture = manufacture.NameManufacture;
            }
            if (PriceSale.Text != null)
            {
                autopartDPO.PriceSale = decimal.Parse(PriceSale.Text);
            }
            if (AvailableityStock.Text != null)
            {
                autopartDPO.AvailableityStock = int.Parse(AvailableityStock.Text);
            }
            if (CbModerationStatus.Text != null)
            {
                autopartDPO.ModerationStatus = (string)CbModerationStatus.Text;
            }

            // получаем аккаунт, от которого происходит добавление данных
            int userId = AuthorizationViewModel.weGetIdUser();
            if (userId != null)
            {
                autopartDPO.AccountId = userId;
            }

            return autopartDPO;
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
            if (CbCountry.Text.Trim().IsNullOrEmpty() || CbCountry == null)
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
            if (NameAutopart.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(NameAutopart);
                BeginFadeAnimation(errorInput);
            }
            if (CbModerationStatus.Text.Trim().IsNullOrEmpty())
            {
                errorInput.Text = "Заполните все поля!";
                StartAnimation(CbModerationStatus);
                BeginFadeAnimation(errorInput);
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

                bool Checking = true; // проверка на уникальность

                if (RenameButtonAutopart.Content != "Редактировать") // если добавление данных
                {
                    Checking = _listAutopartViewModel.CheckingAddPart(GetTheDataAdd());
                }
                else // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
                {
                    bool CheckingItem = _listAutopartViewModel.CheckingForMatchEditDB(GetTheDataEdit());
                    if (CheckingItem)
                    {
                        Checking = true;
                    }
                    else
                    {
                        errorInput.Text = "Данная запчасть уже есть в базе!";
                        BeginFadeAnimation(errorInput);

                        Checking = false;
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
                        errorInput.Text = "Данная запчасть уже есть в базе!";
                        BeginFadeAnimation(errorInput);
                    }
                }
            }
        }

        // кнопка выхода на главную страницу
        private void ClosePageAddOrDeleteAutopart(object sender, RoutedEventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePageAutopart();
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
