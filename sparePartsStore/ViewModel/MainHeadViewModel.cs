using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using sparePartsStore.Helper;
using sparePartsStore.Helper.Authorization;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.View;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.View.ViewAdministrator.ViewSettingRole;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
using sparePartsStore.View.ViewAdministrator.ViewWorkingWithData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace sparePartsStore.ViewModel
{
    public class MainHeadViewModel : INotifyPropertyChanged // класс для переключения страниц в меню администратора
    {

        // свойство для запуска фреймов страниц
        private Frame _mainFrame;
        public Frame MainFrame
        {
            get { return _mainFrame; }
            set
            {
                _mainFrame = value;
                OnPropertyChanged(nameof(MainFrame));
            }
        }

        AuthorizationViewModel authorizationViewModel = new AuthorizationViewModel(); // работа с авторизацией

        // переменная, которая хранит состояние меню. true - значит было открыто основное меню. false - значит настройка пользователей.
        private bool typeMenu = true; // по умолчанию было открыто основное меню

        // свойства страницы PageMainHead
        #region PropertiesPageMainHead

        // свойство для отображения иконки настроек
        private Visibility _isSettingVisible = Visibility.Visible;
        public Visibility IsSettingVisible
        {
            get { return _isSettingVisible; }
            set
            {
                _isSettingVisible = value;
                // Здесь вы можете выполнять дополнительные действия, например, обновлять UI
                OnPropertyChanged(nameof(IsSettingVisible));
            }
        }

        // свойство для отображения иконки выхода на главное меню
        private Visibility _isOutMenu = Visibility.Collapsed;
        public Visibility IsOutMenu
        {
            get { return _isOutMenu; }
            set { _isOutMenu = value; OnPropertyChanged(nameof(IsOutMenu)); }
        }

        // свойство для отображения любого меню адмнистратора
        private Visibility _isMenu = Visibility.Visible;
        public Visibility IsMenu
        {
            get { return _isMenu; }
            set { _isMenu = value; OnPropertyChanged(nameof(IsMenu)); }
        }

        // свойство для отображения меню c данными для запчастей
        private Visibility _isBasicMenu = Visibility.Visible;
        public Visibility IsBasicMenu
        {
            get { return _isBasicMenu; }
            set { _isBasicMenu = value; OnPropertyChanged(nameof(IsBasicMenu)); }
        }

        // свойство для отображения меню настроек администратора
        private Visibility _isSettingMenu = Visibility.Hidden;
        public Visibility IsSettingMenu
        {
            get { return _isSettingMenu; }
            set { _isSettingMenu = value; OnPropertyChanged(nameof(IsSettingMenu)); }
        }

        #endregion

        public MainHeadViewModel()
        {
            // событие на закрытие страницы
            WorkingWithData.closePage += CloseLastOnePage;


            // подписываемя на событие запуска страницы добавления марки авто
            WorkingWithData.launchPageAddCarBrand += LaunchPageAddCarBrand;
            // подписываемя на событие запуска страницы редактирования марки авто
            WorkingWithData.launchPageEditCarBrand += LaunchPageEditCarBrand;
            // подписываемся на событие сохранения данных марки авто после редактирования или добваления данных
            WorkingWithData.saveDataCreateOrEditCarBrands += WorkDataBrand;
            // подписываемся на событие удаления данных марок авто
            WorkingWithData.saveDataDeleteCarBrands += DeleteDataBrand;


            // подписываемся на событие запуска страницы добавления модели авто
            WorkingWithData.launchinPageAddCarModel += LaunchPageAddCarModel;
            // подписываемся на событие запуска страницы редактирования модели авто
            WorkingWithData.launchpageEditCarModel += LaunchPageEditCarModel;
            // подписываемся на событие сохранения данных модели авто после редактирования или добваления данных
            WorkingWithData.saveDataCreateOrEditCarModels += WorkDataModel;
            // подписываемся на событие удаления данных марок авто
            WorkingWithData.saveDataDeleteCarModels += DeleteDataModel;


            // подписываемся на событие запуска страницы добавления агрегата авто
            WorkingWithData.launchPageAddUnit += LaunchPageAddUnit;
            // подписываемся на событие запуска страницы редактирования агрегата авто
            WorkingWithData.launchpageEditUnit += LaunchPageEditUnit;
            // подписываемся на событие сохранения данных агрегата авто после редактирования или добваления данных
            WorkingWithData.saveDataCreateOrEditUnit += WorkDataUnit;
            // подписываемся на событие удаления данных агрегатов авто
            WorkingWithData.saveDataDeleteUnit += DeleteDataUnit;


            // подписываемся на событие запуска страницы добавления узла авто
            WorkingWithData.launchPageAddKnot += LaunchPageAddKnot;
            // подписываемся на событие запуска страницы редактирования узла авто
            WorkingWithData.launchpageEditKnot += LaunchPageEditKnot;
            // подписываемся на событие сохранения данных узла авто после редактирования или добваления данных
            WorkingWithData.saveDataCreateOrEditKnot += WorkDataKnot;
            // подписываемся на событие удаления данных узла авто
            WorkingWithData.saveDataDeleteKnot += DeleteDataKnot;


            // подписываемся на событие запуска страницы добавления страны
            WorkingWithData.launchPageAddCountry += LaunchPageAddCountry;
            // подписываемся на событие запуска страницы редактирования страны
            WorkingWithData.launchpageEditCountry += LaunchpageEditCountry;
            // подписываемся на событие сохранения данных страны после редактирования или добваления данных
            WorkingWithData.saveDataCreateOrEditCountry += WorkDataCountry;
            // подписываемся на событие удаления данных страны
            WorkingWithData.saveDataDeleteCountry += DeleteDataCountry;


            // подписываемся на событие запуска страницы добавления производителя
            WorkingWithData.launchPageAddManufacture += LaunchPageAddManufacture;
            // подписываемся на событие запуска страницы редактирования производителя
            WorkingWithData.launchPageEditManufacture += LaunchPageEditManufacture;
            // подписываемся на событие сохранения данных производителя после редактирования или добваления
            WorkingWithData.saveDataCreateOrEditManufacture += WorkDataManufacture;
            // подписываемся на событие удаления данных производителя
            WorkingWithData.saveDataDeleteManufacture += DeleteDataManufacture;


            // подписываемся на событие запуска страницы добавления запчасти
            WorkingWithData.launchPageAddAutoparts += LaunchPageAddAutoparts;
            // подписываемся на событие запуска страницы редактирования запчасти
            WorkingWithData.launchPageEditAutoparts += LaunchPageEditAutoparts;
            // подписываемся на событие сохранения данных запчасти после редактирования или добваления
            WorkingWithData.saveDataCreateOrEditAutoparts += WorkDataAutoparts;
            // подписываемся на событие удаления данных запчасти
            WorkingWithData.saveDataDeleteAutoparts += DeleteDataAutoparts;


            // подписываемся на событие запуска страницы добавления пользователя
            WorkingWithData.launchPageAddUser += LaunchPageAddUser;
            // подписываемся на событие запуска страницы редактирования пользователя
            WorkingWithData.launchPageEditUser += LaunchPageEditUser;
            // подписываемся на событие сохранения данных пользователя после редактирования или добваления
            WorkingWithData.saveDataCreateOrEditUser += WorkDataUser;
            // подписываемся на событие удаления данных пользователя
            WorkingWithData.saveDataDeleteUsers += DeleteDataUser;


            // подписываемся на событие запуска страницы аналоги
            WorkingWithData.launchPageAddAnalog += LaunchPageAddAnalog;
        }

        // запуск страницы - поиск запчастей
        #region launchWorkPage

        // кнопка запуска страницы - поиск запчастей
        private RelayCommand _btn_SearchParts { get; set; }
        public RelayCommand Btn_SearchParts
        {
            get
            {
                return _btn_SearchParts ??
                    (_btn_SearchParts = new RelayCommand(obj =>
                    {
                        startMainPage(); // запускаем страницу - "поиск запчастей"
                    }, (obj) => true));
            }
        }

        // начальная страница - поиск запчастей
        public void startMainPage()
        {
            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            // автоматичесое отображение страницы при входе в учетную запись адинистратора
            ViewSearchSpareParts viewSearchSpareParts = new ViewSearchSpareParts();
            MainFrame.Navigate(viewSearchSpareParts);

        }

        #endregion

        // страница - марка авто
        #region CarBrand

        // кнопка запуска страницы - марки авто
        PageListCarBrands pageListCarBrands; // объект класса отображения списка марок авто
        private RelayCommand _btn_carBrand { get; set; }
        public RelayCommand Btn_CarBrand
        {
            get
            {
                return _btn_carBrand ??
                    (_btn_carBrand = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListCarBrands = new PageListCarBrands();
                        MainFrame.NavigationService.Navigate(pageListCarBrands);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных марок авто
        PageWorkListBrand pageWorkListBrand;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditBrand; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления марки авто
        private void LaunchPageAddCarBrand(object sender, EventAggregator e)
        {
            pageWorkListBrand = new PageWorkListBrand(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkListBrand);
            pageWorkListBrand.RenameButtonBrand.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditBrand = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
        }

        // запуск страницы редактирования марки авто
        private void LaunchPageEditCarBrand(object sender, EventAggregator e)
        {
            pageWorkListBrand = new PageWorkListBrand(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkListBrand); // запуск страницы
            pageWorkListBrand.RenameButtonBrand.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditBrand = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListCarBrands.EventDataSelectedItem += (sender, args) =>
            {
                CarBrand carBrand = (CarBrand)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                pageWorkListBrand.DataReception(carBrand);
            };
            // вызываем событие для передачи данных
            pageListCarBrands.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataBrand(object sender, EventAggregator e)
        {
            if (addOrEditBrand) // если добавляем данные в таблицу марок авто
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext()) 
                {
                    List<CarBrand> carBrands = sparePartsStoreContext.CarBrands.ToList(); // получаем список марок авто

                    // Создаём экз carBrands для добавлени данных
                    CarBrand carBrand = new CarBrand();
                    pageWorkListBrand.MyEventArgsObject += (sender, args) =>
                    {
                        CarBrand carBrands = (CarBrand)args.Value;
                        carBrand.NameCarBrand = carBrands.NameCarBrand;
                        sparePartsStoreContext.Add(carBrand); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkListBrand.Transmit(); // вызываем событие, чтобы полчить данные для записи в БД
                }
            }
            else // если редактируем данные в таблице марок авто
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<CarBrand> carBrands = sparePartsStoreContext.CarBrands.ToList(); // получаем список марок авто

                    pageWorkListBrand.MyEventArgsObject += (sender, args) =>
                    {
                        CarBrand carBrand = (CarBrand)args.Value; // получаем отредактированные данные
                        // получаем id объекта для редактирования

                        // получаем объект из БД, чтобы внести в него изменения. Id берем из GetCarBrand
                        CarBrand cBrand = carBrands.FirstOrDefault(carBrands => carBrands.CarBrandId == carBrand.CarBrandId);

                        if (cBrand != null)
                        {
                            // обновляем список БД
                            cBrand.NameCarBrand = carBrand.NameCarBrand;
                            sparePartsStoreContext.Update(cBrand);
                            //sparePartsStoreContext.Entry(c).Property(p => p.NameCarBrand).IsModified = true; // альтернатива обновления данных
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }

                    };
                    pageWorkListBrand.Transmit(); // вызываем событие, чтобы полчить данные для изменения в БД
                }
            }

            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            pageListCarBrands = new PageListCarBrands(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListCarBrands);
            selectedMenu(); // отображаем меню
        }


        // удаляем данные из таблицы
        private void DeleteDataBrand(object sender, EventAggregator e)
        {
                // получаем выбранные данные для удаления
                pageListCarBrands.EventDataSelectedItem += (sender, args) =>
                {
                    // подключаем БД
                    using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                    {
                        List<CarBrand> carBrands = sparePartsStoreContext.CarBrands.ToList(); // получаем список марок авто

                        CarBrand carBrand = (CarBrand)args.Value; // получаем выбранные данные

                        // удаляем данные из БД
                        CarBrand cBrand = carBrands.FirstOrDefault(carBrands => carBrands.CarBrandId == carBrand.CarBrandId);

                        if (cBrand != null)
                        {
                            sparePartsStoreContext.CarBrands.Remove(cBrand); // удаляем объект
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд

                            // обновляем список
                            pageListCarBrands.UpTable();
                        }
                    }
                };
                // вызываем событие для передачи данных
                pageListCarBrands.TransmitData();
        }


        #endregion

        // страница - модель авто
        #region CarModel 

        PageListCarModels pageListCarModel; // объект класса отображения списка моделей авто

        // запуск страницы модели авто
        private RelayCommand _btn_carModel {  get; set; }
        public RelayCommand Btn_carModel
        {
            get 
            {
                return _btn_carModel ??
                    (_btn_carBrand = new RelayCommand((obj) =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        // автоматичесое отображение страницы при входе в учетную запись адинистратора
                        pageListCarModel = new PageListCarModels();
                        MainFrame.Navigate(pageListCarModel);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных моделей авто
        PageWorkListModel pageWorkListModel;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditModel; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления модели авто
        private void LaunchPageAddCarModel(object sender, EventAggregator e)
        {
            pageWorkListModel = new PageWorkListModel(); // экз страницы для добавления модели авто

            MainFrame.NavigationService.Navigate(pageWorkListModel);
            pageWorkListModel.RenameButtonBrand.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditModel = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
            // добавлем данные в ComBox
            pageWorkListModel.DataReceptionAdd();
        }

        // запуск страницы редактирования модели авто
        private void LaunchPageEditCarModel(object sender, EventAggregator e)
        {
            pageWorkListModel = new PageWorkListModel(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkListModel);
            pageWorkListModel.RenameButtonBrand.Content = "Редактировать"; // измененяем кнопку

            // поднимаем флаг, что мы редактируем данные
            addOrEditModel = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранные данные для редактирования
            pageListCarModel.EventDataSelectedCarModelItem += (sender, args) =>
            {
                CarModelDPO carModelDPO = (CarModelDPO)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображения в полях)
                pageWorkListModel.DataReception(carModelDPO);
            };
            // вызываем событие для предачи данных
            pageListCarModel.TransmitiData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataModel(object sender, EventAggregator e)
        {
            if (addOrEditModel) // если добавляем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<CarModel> carModels = sparePartsStoreContext.CarModels.ToList(); // получаем список моделей авто

                    // создаём экз для добавления данных
                    CarModel carModel = new CarModel();
                    pageWorkListModel.EventArgsCarModel += (sender, args) =>
                    {
                        CarModelDPO carModelDPO = (CarModelDPO)args.Value;
                        // преобразовываем CarModelDPO в CarModel
                        CarModel carModels = carModel.CopyFromCarModelDPO(carModelDPO);

                        // переносим данные
                        carModel.NameCarModel = carModels.NameCarModel;
                        carModel.CarBrand = carModels.CarBrand;

                        sparePartsStoreContext.Add(carModel); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkListModel.Transmit();
                }
            } 
            else // если редактируем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<CarModel> carModels = sparePartsStoreContext.CarModels.ToList(); // получаем список моделей авто

                    pageWorkListModel.EventArgsCarModel += (sender, args) =>
                    {
                        CarModelDPO carModelDPO = (CarModelDPO)args.Value;

                        // получаем объект из БД, чтобы внести в него изменения. Id берем из getCarModelDPOs
                        CarModel cModel = carModels.FirstOrDefault(carModel => carModel.CarModelId == carModelDPO.CarModelId);
                        if (cModel != null)
                        {
                            // обновляем БД
                            CarModel model = new CarModel();
                            model = model.CopyFromCarModelDPO(carModelDPO);
                            cModel.NameCarModel = model.NameCarModel;
                            cModel.CarBrand = model.CarBrand;
                            sparePartsStoreContext.Update(cModel);// вносим данные в бд
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkListModel.Transmit(); // вызываем событие, чтобы полчить данные для изменения в БД
                }
            }

            WorkingWithData.ClearMemoryAfterFrame();
            pageListCarModel = new PageListCarModels(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListCarModel);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataModel(object sender, EventAggregator e)
        {
            // получаем данные для удаления
            pageListCarModel.EventDataSelectedCarModelItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<CarModel> carModels = sparePartsStoreContext.CarModels.ToList(); // получаем список из БД
                    CarModelDPO carModelDPO = (CarModelDPO)args.Value; // получили выбранные данные из таблицы
                    // находим в списке БД элемент для удаления
                    CarModel carModel = carModels.FirstOrDefault(carModel => carModel.CarModelId == carModelDPO.CarModelId);
                    if(carModel != null)
                    {
                        sparePartsStoreContext.CarModels.Remove(carModel);
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListCarModel.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListCarModel.TransmitiData();
        }

        #endregion

        // страница - агрегат авто
        #region Unit

        // кнопка запуска страницы - марки авто
        PageListUnit pageListUnit; // объект класса отображения списка марок авто

        private RelayCommand _btn_unit {  get; set; }
        public RelayCommand Btn_unit
        {
            get
            {
                return _btn_unit ??
                    (_btn_unit = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListUnit = new PageListUnit();
                        MainFrame.NavigationService.Navigate(pageListUnit);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных агрегатов авто
        PageWorkUnit pageWorkUnit;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditUnit; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления агрегата
        private void LaunchPageAddUnit(object sender, EventAggregator e)
        {
            pageWorkUnit = new PageWorkUnit(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkUnit);
            pageWorkUnit.RenameButtonBrand.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditUnit = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
        }

        // запуск страницы редактирования агрегата авто 
        private void LaunchPageEditUnit(object sender, EventAggregator e)
        {
            pageWorkUnit = new PageWorkUnit(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkUnit); // запуск страницы
            pageWorkUnit.RenameButtonBrand.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditUnit = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListUnit.EventDataSelectedUnitItem += (sender, args) =>
            {
                Unit unit = (Unit)args.Value; // получаем данные

                // передаём данные в поля ввода
                pageWorkUnit.DataReception(unit);
            };
            // вызываем событие для передачи данных
            pageListUnit.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataUnit(object sender, EventAggregator e)
        {
            if(addOrEditUnit) // если добавляем данные в таблицу агрегатов авто
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    // создаём экз Unit для добавления данных
                    Unit unit = new Unit();
                    pageWorkUnit.EventArgsUnit += (sender, args) =>
                    {
                        Unit units = (Unit)args.Value;
                        unit.NameUnit = units.NameUnit;
                        sparePartsStoreContext.Add(unit); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkUnit.Transmit(); // вызываем событие, чтобы полчить данные для записи в БД
                }
            }
            else // если редактируем данные в таблице марок авто
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Unit> unitList = sparePartsStoreContext.Units.ToList();

                    pageWorkUnit.EventArgsUnit += (sender, args) =>
                    {
                        Unit unitsInput = (Unit)args.Value; // получаем отредактированные данные

                        // получаем id объекта для редактирования
                        // получаем объект из БД, чтобы внести в него изменения. Id берем из GetUnit
                        Unit unit = unitList.FirstOrDefault(units => units.UnitId == unitsInput.UnitId);
                        if (unit != null)
                        {
                            // обновляем список БД
                            unit.NameUnit = unitsInput.NameUnit;
                            sparePartsStoreContext.Update(unit);
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkUnit.Transmit(); // вызываем событие, чтобы полчить данные для записи в БД
                }
            }

            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            pageListUnit = new PageListUnit(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListUnit);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataUnit(object sender, EventAggregator e)
        {
            // получаем выбранные данные для удаления
            pageListUnit.EventDataSelectedUnitItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Unit> units = sparePartsStoreContext.Units.ToList(); // получаем список агрегатов
                    Unit unitInput = (Unit)args.Value; // получаем выбранные данные для удаления
                    // удаляем данные из БД
                    Unit unitDelete = units.FirstOrDefault(unit => unit.UnitId == unitInput.UnitId);
                    if (unitDelete != null)
                    {
                        sparePartsStoreContext.Units.Remove(unitDelete); // удаляем объект
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListUnit.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListUnit.TransmitData();
        }

        #endregion

        // страница - узел авто
        #region Knot

        PageListKnot pageListKnot; // объект класса отображения списка узлов авто
        private RelayCommand _btn_Knot { get; set; }
        public RelayCommand Btn_Knot
        {
            get
            {
                return _btn_Knot ??
                    (_btn_Knot = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListKnot = new PageListKnot();
                        MainFrame.NavigationService.Navigate(pageListKnot);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных узлов авто
        PageWorkKnot pageWorkKnot;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditKnot; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления узла авто
        private void LaunchPageAddKnot(object sender, EventAggregator e)
        {
            pageWorkKnot = new PageWorkKnot(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkKnot);
            pageWorkKnot.RenameButtonBrand.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditKnot = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
            // добавлем данные в ComBox
            pageWorkKnot.DataReceptionAdd();
        }

        // запуск страницы редактирования узла авто
        private void LaunchPageEditKnot(object sender, EventAggregator e)
        {
            pageWorkKnot = new PageWorkKnot(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkKnot); // запуск страницы
            pageWorkKnot.RenameButtonBrand.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditKnot = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListKnot.EventDataSelectedKnotItem += (sender, args) =>
            {
                KnotDPO knotDPO = (KnotDPO)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                pageWorkKnot.DataReception(knotDPO);
            };
            // вызываем событие для передачи данных
            pageListKnot.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataKnot(object sender, EventAggregator e)
        {
            if (addOrEditKnot) // если добавляем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Knot> carModels = sparePartsStoreContext.Knots.ToList(); // получаем список узлов авто

                    // создаём экз для добавления данных
                    Knot knot = new Knot();
                    pageWorkKnot.EventArgsKnot += (sender, args) =>
                    {
                        KnotDPO knotDPO = (KnotDPO)args.Value;
                        // преобразовываем KnotDPO в Knot
                        Knot knots = knot.CopyFromKnotDPO(knotDPO);

                        // переносим данные
                        knot.NameKnot = knots.NameKnot;
                        knot.UnitId = knots.UnitId;

                        sparePartsStoreContext.Add(knot); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkKnot.Transmit();
                }
            }
            else // если редактируем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Knot> knots = sparePartsStoreContext.Knots.ToList(); // получаем список узлов авто

                    pageWorkKnot.EventArgsKnot += (sender, args) =>
                    {
                        KnotDPO knotDPO = (KnotDPO)args.Value;

                        // получаем объект из БД, чтобы внести в него изменения. Id берем из getKnotDPOs
                        Knot knot = knots.FirstOrDefault(knot => knot.KnotId == knotDPO.KnotId);
                        if (knot != null)
                        {
                            // обновляем БД
                            Knot knotUp = new Knot();
                            knotUp = knotUp.CopyFromKnotDPO(knotDPO);
                            knot.NameKnot = knotUp.NameKnot;
                            knot.UnitId = knotUp.UnitId;
                            sparePartsStoreContext.Update(knot);// вносим данные в бд
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkKnot.Transmit(); // вызываем событие, чтобы полчить данные для изменения в БД
                }
            }

            WorkingWithData.ClearMemoryAfterFrame();
            pageListKnot = new PageListKnot(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListKnot);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataKnot(object sender, EventAggregator e)
        {
            // получаем данные для удаления
            pageListKnot.EventDataSelectedKnotItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Knot> knots = sparePartsStoreContext.Knots.ToList(); // получаем список из БД
                    KnotDPO knotDPO = (KnotDPO)args.Value; // получили выбранные данные из таблицы
                    // находим в списке БД элемент для удаления
                    Knot knot = knots.FirstOrDefault(carModel => carModel.KnotId == knotDPO.KnotId);
                    if (knot != null)
                    {
                        sparePartsStoreContext.Knots.Remove(knot);
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListKnot.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListKnot.TransmitData();
        }

        #endregion

        // страница - страна
        #region country

        // кнопка запуска страницы - страны
        PageListCountry pageListCountry; // объект класса отображения списка стран

        private RelayCommand _btn_Country { get; set; }
        public RelayCommand Btn_Country
        {
            get
            {
                return _btn_Country ??
                    (_btn_Country = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListCountry = new PageListCountry();
                        MainFrame.NavigationService.Navigate(pageListCountry);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных стран
        PageWorkCountry pageWorkCountry;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditCountry; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления страны
        private void LaunchPageAddCountry(object sender, EventAggregator e)
        {
            pageWorkCountry = new PageWorkCountry(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkCountry);
            pageWorkCountry.RenameButtonBrand.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditCountry = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
        }

        // запуск страницы редактирования страны
        private void LaunchpageEditCountry(object sender, EventAggregator e)
        {
            pageWorkCountry = new PageWorkCountry(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkCountry); // запуск страницы
            pageWorkCountry.RenameButtonBrand.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditCountry = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListCountry.EventDataSelectedUnitItem += (sender, args) =>
            {
                Country unit = (Country)args.Value; // получаем данные

                // передаём данные в поля ввода
                pageWorkCountry.DataReception(unit);
            };
            // вызываем событие для передачи данных
            pageListCountry.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataCountry(object sender, EventAggregator e)
        {
            if (addOrEditCountry) // если добавляем данные в таблицу стран
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    // создаём экз Unit для добавления данных
                    Country сountry = new Country();
                    pageWorkCountry.EventArgsCountry += (sender, args) =>
                    {
                        Country countries = (Country)args.Value;
                        сountry.NameCountry = countries.NameCountry;
                        sparePartsStoreContext.Add(сountry); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkCountry.Transmit(); // вызываем событие, чтобы полчить данные для записи в БД
                }
            }
            else // если редактируем данные в таблице стран
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Country> countryList = sparePartsStoreContext.Countries.ToList();

                    pageWorkCountry.EventArgsCountry += (sender, args) =>
                    {
                        Country countryInput = (Country)args.Value; // получаем отредактированные данные

                        // получаем id объекта для редактирования
                        // получаем объект из БД, чтобы внести в него изменения. Id берем из GetCountry
                        Country сountry = countryList.FirstOrDefault(units => units.CountryId == countryInput.CountryId);
                        if (сountry != null)
                        {
                            // обновляем список БД
                            сountry.NameCountry = countryInput.NameCountry;
                            sparePartsStoreContext.Update(сountry);
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkCountry.Transmit(); // вызываем событие, чтобы полчить данные для записи в БД
                }
            }

            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            pageListCountry = new PageListCountry(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListCountry);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataCountry(object sender, EventAggregator e)
        {
            // получаем выбранные данные для удаления
            pageListCountry.EventDataSelectedUnitItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Country> countries = sparePartsStoreContext.Countries.ToList(); // получаем список стран
                    Country сountryInput = (Country)args.Value; // получаем выбранные данные для удаления
                    // удаляем данные из БД
                    Country countryDelete = countries.FirstOrDefault(unit => unit.CountryId == сountryInput.CountryId);
                    if (countryDelete != null)
                    {
                        sparePartsStoreContext.Countries.Remove(countryDelete); // удаляем объект
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListCountry.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListCountry.TransmitData();
        }

        #endregion

        // страница - производителей
        #region Manufacture

        PageListManufacture pageListManufacture; // объект класса отображения списка произоводителей
        private RelayCommand _btn_Manufacture { get; set; }
        public RelayCommand Btn_Manufacture
        {
            get
            {
                return _btn_Manufacture ??
                    (_btn_Manufacture = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListManufacture = new PageListManufacture();
                        MainFrame.NavigationService.Navigate(pageListManufacture);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных производителей
        PageWorkListManufacture pageWorkListManufacture;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditManufacture; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления производителя
        private void LaunchPageAddManufacture(object sender, EventAggregator e)
        {
            pageWorkListManufacture = new PageWorkListManufacture(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkListManufacture);
            pageWorkListManufacture.RenameButtonManufacture.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditManufacture = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
            // добавлем данные в ComBox
            pageWorkListManufacture.DataReceptionAdd();
        }

        // запуск страницы редактирования производителя
        private void LaunchPageEditManufacture(object sender, EventAggregator e)
        {
            pageWorkListManufacture = new PageWorkListManufacture(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkListManufacture); // запуск страницы
            pageWorkListManufacture.RenameButtonManufacture.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditManufacture = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListManufacture.EventDataSelectedManufactureItem += (sender, args) =>
            {
                ManufactureDPO manufactureDPO = (ManufactureDPO)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                pageWorkListManufacture.DataReception(manufactureDPO);
            };
            // вызываем событие для передачи данных
            pageListManufacture.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataManufacture(object sender, EventAggregator e)
        {
            if (addOrEditManufacture) // если добавляем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Manufacture> manufacturies = sparePartsStoreContext.Manufactures.ToList(); // получаем список узлов авто

                    // создаём экз для добавления данных
                    Manufacture manufacture = new Manufacture();
                    pageWorkListManufacture.EventArgsManufacture += (sender, args) =>
                    {
                        ManufactureDPO manufactureDPO = (ManufactureDPO)args.Value;
                        // преобразовываем KnotDPO в Knot
                        Manufacture manuf = manufacture.CopyFromCountryDPO(manufactureDPO);

                        // переносим данные
                        manufacture.NameManufacture = manuf.NameManufacture;
                        manufacture.CountryId = manuf.CountryId;

                        sparePartsStoreContext.Add(manufacture); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkListManufacture.Transmit();
                }
            }
            else // если редактируем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Manufacture> manufacturies = sparePartsStoreContext.Manufactures.ToList(); // получаем список узлов авто

                    pageWorkListManufacture.EventArgsManufacture += (sender, args) =>
                    {
                        ManufactureDPO manufactureDPO = (ManufactureDPO)args.Value;

                        // получаем объект из БД, чтобы внести в него изменения. Id берем из getKnotDPOs
                        Manufacture manufacture = manufacturies.FirstOrDefault(knot => knot.ManufactureId == manufactureDPO.ManufactureId);
                        if (manufacture != null)
                        {
                            // обновляем БД
                            Manufacture manufactureUp = new Manufacture();
                            manufactureUp = manufactureUp.CopyFromCountryDPO(manufactureDPO);
                            manufacture.NameManufacture = manufactureUp.NameManufacture;
                            manufacture.CountryId = manufactureUp.CountryId;
                            sparePartsStoreContext.Update(manufacture);// вносим данные в бд
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkListManufacture.Transmit(); // вызываем событие, чтобы полчить данные для изменения в БД
                }
            }

            WorkingWithData.ClearMemoryAfterFrame();
            pageListManufacture = new PageListManufacture(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListManufacture);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataManufacture(object sender, EventAggregator e)
        {
            // получаем данные для удаления
            pageListManufacture.EventDataSelectedManufactureItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Manufacture> manufacturies = sparePartsStoreContext.Manufactures.ToList(); // получаем список из БД
                    ManufactureDPO manufactureDPO = (ManufactureDPO)args.Value; // получили выбранные данные из таблицы
                    // находим в списке БД элемент для удаления
                    Manufacture manufacture = manufacturies.FirstOrDefault(carModel => carModel.ManufactureId == manufactureDPO.ManufactureId);
                    if (manufacture != null)
                    {
                        sparePartsStoreContext.Manufactures.Remove(manufacture);
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListManufacture.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListManufacture.TransmitData();
        }

        #endregion

        // страна - запчасти
        #region AutoParts

        // кнопка запуска страницы - марки авто
        PageListAutoparts pageListAutoparts; // объект класса отображения списка запчастей
        private RelayCommand _btn_Details { get; set; }
        public RelayCommand Btn_Details
        {
            get
            {
                return _btn_Details ??
                    (_btn_Details = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListAutoparts = new PageListAutoparts();
                        MainFrame.NavigationService.Navigate(pageListAutoparts);
                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных запчасти
        PageWorkDetail pageWorkDetail;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditAutoparts; // если true - значит добавлять, если false - значит редактировать
        
        // запуск страницы добавления запчасти
        private void LaunchPageAddAutoparts(object sender, EventAggregator e)
        {
            pageWorkDetail = new PageWorkDetail(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkDetail);
            pageWorkDetail.RenameButtonAutopart.Content = "Добавить"; // измененяем кнопку
            // поднимаем флаг, что мы добавляем данные
            addOrEditAutoparts = true;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
            // добавлем данные в ComBox
            pageWorkDetail.DataReceptionAdd();
        }

        // запуск страницы редактирования запчасти
        private void LaunchPageEditAutoparts(object sender, EventAggregator e)
        {
            pageWorkDetail = new PageWorkDetail(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkDetail); // запуск страницы
            pageWorkDetail.RenameButtonAutopart.Content = "Редактировать"; // измененяем кнопку
            // поднимаем флаг, что мы редактируем данные
            addOrEditAutoparts = false;
            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListAutoparts.EventDataSelectedAutopartItem += (sender, args) =>
            {
                AutopartDPO autopartDPO = (AutopartDPO)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                pageWorkDetail.DataReception(autopartDPO);
            };
            // вызываем событие для передачи данных
            pageListAutoparts.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataAutoparts(object sender, EventAggregator e)
        {
            if (addOrEditAutoparts) // если добавляем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Autopart> autoparts = sparePartsStoreContext.Autoparts.ToList(); // получаем список запчастей

                    // создаём экз для добавления данных
                    Autopart autopart = new Autopart();
                    pageWorkDetail.EventArgsAutopart += (sender, args) =>
                    {
                        AutopartDPO autopartDPO = (AutopartDPO)args.Value;
                        // преобразовываем AutopartDPO в Autopart
                        Autopart autopartCopy = autopart.CopyFromAutopartDPO(autopartDPO);

                        // переносим данные
                        autopart.NameAutopart = autopartCopy.NameAutopart;

                        // находим макисимальный номер запчасти и на основе него создаём новый номер + 1
                        int NumberAutopart;
                        if (autoparts.Any())
                        {
                            NumberAutopart = autoparts.Max(a => a.NumberAutopart);
                            if (NumberAutopart == null || NumberAutopart == 0)
                            {
                                NumberAutopart = 8000000;
                            }
                            else
                            {
                                NumberAutopart++;
                            }
                        }
                        else
                        {
                            NumberAutopart = 8000000;
                        }


                        autopart.NumberAutopart = NumberAutopart;
                        autopart.KnotId = autopartCopy.KnotId;
                        autopart.CarModelId = autopartCopy.CarModelId;
                        autopart.ManufactureId = autopartCopy.ManufactureId;
                        autopart.PriceSale = autopartCopy.PriceSale;
                        autopart.AvailableityStock = autopartCopy.AvailableityStock;
                        autopart.AccountId = autopartCopy.AccountId;

                        // проверяем под какой ролью вошёл пользователь, если поставщик, то статус заказа автоматически в обработке, а если администратор, то на его усмотрение
                        string role = authorizationViewModel.CheckingUserRole();
                        if(role == "Администратор")
                        {
                            autopart.ModerationStatus = autopartDPO.ModerationStatus;
                        }
                        else
                        {
                            autopart.ModerationStatus = "В обработке";
                        }

                        sparePartsStoreContext.Add(autopart); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkDetail.TransmitAdd();
                }
            }
            else // если редактируем данные
            {
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Autopart> autoparts = sparePartsStoreContext.Autoparts.ToList(); // получаем список запчастей

                    pageWorkDetail.EventArgsAutopart += (sender, args) =>
                    {
                        AutopartDPO autopartDPO = (AutopartDPO)args.Value;
                        // получаем объект из БД, чтобы внести в него изменения.
                        Autopart autopart = autoparts.FirstOrDefault(knot => knot.AutopartId == autopartDPO.AutopartId);
                        if(autopart != null)
                        {
                            // обновляем БД
                            Autopart autopartUP = new Autopart();
                            autopartUP = autopartUP.CopyFromAutopartDPO(autopartDPO);

                            autopart.NameAutopart = autopartUP.NameAutopart;
                            autopart.NumberAutopart = autopartUP.NumberAutopart;
                            autopart.KnotId = autopartUP.KnotId;
                            autopart.CarModelId = autopartUP.CarModelId;
                            autopart.ManufactureId = autopartUP.ManufactureId;
                            autopart.PriceSale = autopartUP.PriceSale;
                            autopart.AvailableityStock = autopartUP.AvailableityStock;
                            autopart.AccountId = autopartUP.AccountId;

                            // проверяем под какой ролью вошёл пользователь, если поставщик, то статус заказа автоматически в обработке, а если администратор, то на его усмотрение
                            string role = authorizationViewModel.CheckingUserRole();
                            if (role == "Администратор")
                            {
                                autopart.ModerationStatus = autopartDPO.ModerationStatus;
                            }
                            else
                            {
                                autopart.ModerationStatus = "В обработке";
                            }

                            sparePartsStoreContext.Update(autopart);// вносим данные в бд
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkDetail.TransmitAdd(); // вызываем событие, чтобы полчить данные для изменения в БД
                }
            }

            WorkingWithData.ClearMemoryAfterFrame();
            pageListAutoparts = new PageListAutoparts(); // обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListAutoparts);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataAutoparts(object sender, EventAggregator e)
        {
            // получаем данные для удаления
            pageListAutoparts.EventDataSelectedAutopartItem += (sender, args) =>
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Autopart> autoparts = sparePartsStoreContext.Autoparts.ToList(); // получаем список из БД
                    AutopartDPO autopartDPO = (AutopartDPO)args.Value;
                    // находим в списке БД элемент для удаления
                    Autopart autopart = autoparts.FirstOrDefault(carModel => carModel.AutopartId == autopartDPO.AutopartId);
                    if (autopart != null)
                    {
                        sparePartsStoreContext.Autoparts.Remove(autopart);
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListAutoparts.UpTable();
                    }
                }
            };
            // вызываем событие для передачи данных
            pageListAutoparts.TransmitData();
        }

        #endregion

        // страница - пользователи
        #region User

        // кнопка запуска страницы - марки авто
        PageListUsers pageListUsers; // объект класса отображения списка пользователей

        // переходим в меню настроек пользователей
        private RelayCommand _btn_UserSetting { get; set; }
        public RelayCommand Btn_UserSetting
        {
            get
            {
                return _btn_UserSetting ??
                    (_btn_UserSetting = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListUsers = new PageListUsers();
                        MainFrame.NavigationService.Navigate(pageListUsers);

                        // работа с меню 
                        typeMenu = false;
                        visibilitySetting = true;
                        selectedMenu();

                    }, (obj) => true));
            }
        }

        // переходим в основное меню
        private RelayCommand _btn_BasicMenu { get; set; }
        public RelayCommand Btn_BasicMenu
        {
            get
            {
                return _btn_BasicMenu ??
                    (_btn_BasicMenu = new RelayCommand(obj =>
                    {
                        // событие для очистка фреймов из памяти в PageMainHead
                        WorkingWithData.ClearMemoryAfterFrame();
                        pageListAutoparts = new PageListAutoparts();
                        MainFrame.NavigationService.Navigate(pageListAutoparts);

                        // работа с меню 
                        typeMenu = true;
                        visibilityMenu = true;
                        selectedMenu();

                    }, (obj) => true));
            }
        }

        // объект страницы для редактирования и добавления данных пользователя
        PageWorkUser pageWorkUser;
        // флаг, который нам сообщает, редактирует пользователь таблицу или добавлят новые данные
        bool addOrEditUser; // если true - значит добавлять, если false - значит редактировать

        // запуск страницы добавления пользователя
        private void LaunchPageAddUser(object sender, EventAggregator e)
        {
            pageWorkUser = new PageWorkUser(); // экз страницы для добавления

            MainFrame.NavigationService.Navigate(pageWorkUser);
            pageWorkUser.RenameButtonUser.Content = "Добавить"; // измененяем кнопку

            // поднимаем флаг, что мы добавляем данные
            addOrEditUser = true;
            // показываем, что было открыто не основное меню перед его скрытием
            typeMenu = false;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
            // добавлем данные в ComBox
            pageWorkUser.DataReceptionAdd();
        }

        // запуск страницы редактирования пользователя
        private void LaunchPageEditUser(object sender, EventAggregator e)
        {
            pageWorkUser = new PageWorkUser(); // экз страницы для редактирования

            MainFrame.NavigationService.Navigate(pageWorkUser); // запуск страницы
            pageWorkUser.RenameButtonUser.Content = "Редактировать"; // измененяем кнопку
            pageWorkUser.Password.Visibility = Visibility.Hidden;
            // поднимаем флаг, что мы редактируем данные
            addOrEditUser = false;
            // показываем, что было открыто не основное меню перед его скрытием
            typeMenu = false;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем выбранный данные для редактирования
            pageListUsers.EventDataSelectedUserItem += (sender, args) =>
            {
                Account accountDPO = (Account)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                pageWorkUser.DataReception(accountDPO);
            };
            // вызываем событие для передачи данных
            pageListUsers.TransmitData();
        }

        // редактируем или добавляем данные в таблицу
        private void WorkDataUser(object sender, EventAggregator e)
        {
            if (addOrEditUser) // если добавляем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Account> accounts = sparePartsStoreContext.Accounts.ToList(); // получаем список пользователей

                    // создаём экз для добавления данных
                    Account account = new Account();
                    pageWorkUser.EventArgsAccount += (sender, args) =>
                    {
                        Account accountNew = (Account)args.Value;

                        account.AccountLogin = accountNew.AccountLogin;
                        // зашифровываем полученный пароль
                        account.AccountPassword = PasswordHasher.HashPassword(accountNew.AccountPassword);
                        account.AccountRoleName = accountNew.AccountRoleName;
                        account.NameOrganization = accountNew.NameOrganization;
                        account.Inn = accountNew.Inn;
                        account.Ogrn = accountNew.Ogrn;
                        account.Ogrnip = accountNew.Ogrnip;
                        account.Kpp = accountNew.Kpp;

                        sparePartsStoreContext.Add(account); // вносим данные в бд
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд
                    };
                    pageWorkUser.Transmit();
                }
            }
            else // если редактируем данные
            {
                // подключаем БД
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Account> accounts = sparePartsStoreContext.Accounts.ToList(); // получаем список пользователей

                    // создаём экз для добавления данных
                    Account account = new Account();
                    pageWorkUser.EventArgsAccount += (sender, args) =>
                    {
                        Account accountUP = (Account)args.Value;
                        Account accountNew = accounts.FirstOrDefault(a => a.AccountId == accountUP.AccountId);

                        if(accountNew != null)
                        {
                            accountNew.AccountLogin = accountUP.AccountLogin;
                            // зашифровываем полученный пароль
                            accountNew.AccountPassword = PasswordHasher.HashPassword(accountUP.AccountPassword);
                            accountNew.AccountRoleName = accountUP.AccountRoleName;
                            accountNew.NameOrganization = accountUP.NameOrganization;
                            accountNew.Inn = accountUP.Inn;
                            accountNew.Ogrn = accountUP.Ogrn;
                            accountNew.Ogrnip = accountUP.Ogrnip;
                            accountNew.Kpp = accountUP.Kpp;

                            sparePartsStoreContext.Update(accountNew); // вносим данные в бд
                            sparePartsStoreContext.SaveChanges(); // сохраняем бд
                        }
                    };
                    pageWorkUser.Transmit();
                }
            }

            WorkingWithData.ClearMemoryAfterFrame();
            pageListUsers = new PageListUsers();// обновляем экз. класса
            MainFrame.NavigationService.Navigate(pageListUsers);
            selectedMenu(); // отображаем меню
        }

        // удаляем данные из таблицы
        private void DeleteDataUser(object sender, EventAggregator e)
        {
            // получаем данные для удаления
            pageListUsers.EventDataSelectedUserItem += (sender, args) =>
            {
                using (SparePartsStoreContext sparePartsStoreContext = new SparePartsStoreContext())
                {
                    List<Account> accounts = sparePartsStoreContext.Accounts.ToList(); // получаем список пользователей

                    Account account = (Account)args.Value;
                    // получили выбранные данные из таблицы
                    // находим в списке БД элемент для удаления
                    Account accNew = accounts.FirstOrDefault(a => a.AccountId == account.AccountId);
                    if(accNew != null)
                    {
                        sparePartsStoreContext.Accounts.Remove(accNew);
                        sparePartsStoreContext.SaveChanges(); // сохраняем бд

                        // обновляем список
                        pageListUsers.UpTable();
                    }
                }

            };
            // вызываем событие для передачи данных
            pageListUsers.TransmitData();
        }

        // переход на страницу "пользователи"

        private RelayCommand _btn_User {  get; set; }
        public RelayCommand Btn_User
        {
            get
            {
                return _btn_User ??
                    (_btn_Knot = new RelayCommand(obj =>
                    {



                    }, (obj) => true));
            }
        }

        // перереход на страницу "настройки"
        private RelayCommand _btn_Setting { get; set; }
        public RelayCommand Btn_Setting
        {
            get
            {
                return _btn_Setting ??
                    (_btn_Setting = new RelayCommand(obj =>
                    {



                    }, (obj) => true));
            }
        }

        #endregion

        // страница - аналоги
        #region Analog

        // запуск страницы аналоги
        // Страница аналогов
        PageListAnalogues pageListAnalogues;
        private void LaunchPageAddAnalog(object sender, EventAggregator e)
        {
            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            pageListAnalogues = new PageListAnalogues();
            MainFrame.NavigationService.Navigate(pageListAnalogues);

            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();

            // получаем данные для работы из события
            // получаем выбранный данные для редактирования
            pageListAutoparts.EventDataSelectedAutopartAnalogItem += (sender, args) =>
            {
                AutopartDPO autopartDPO = (AutopartDPO)args.Value; // получаем выбранные данные

                // передаём данные для редактирования (отображаем)
                //pageWorkDetail.DataReception(autopartDPO);
            };
            // вызываем событие для передачи данных
            pageListAutoparts.TransmitDataAnalog(); ;
        }

        #endregion

        // методы PageMainHead
        #region methodsPageMainHead

        // закрыть последнюю страницу
        private void CloseLastOnePage(object sender, EventAggregator e)
    {

        // закрываем страницу
        MainFrame.NavigationService.GoBack();
        // изменяем меню
        selectedMenu();

        // сборка мусора и освобождение неиспользуемых ресурсов
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    // скрываем основное меню и открываем меню настроек
    private void settingMenu()
    {
        IsBasicMenu = Visibility.Hidden; // выкл основное меню
        IsSettingMenu = Visibility.Visible; // вкл меню настроек
    }

    // скрываем меню настроек и открываем основное меню
    private void basicMenu()
    {
        IsSettingMenu = Visibility.Hidden; // выкл меню настроек
        IsBasicMenu = Visibility.Visible; // вкл основное меню
    }

    // переменная, которая показывает, было скрыто меню или нет
    private bool visibilityMenu = false; // меню скрыто

    // переменная, которая показывает, было открыто меню настроек польлзователей или нет
    private bool visibilitySetting = false; // меню скрыто

    // метод для скрытия или отображения меню, иконок при редактировании данных
    private void selectedMenu()
    {
        if (typeMenu) // если основное меню
        {
            if (visibilityMenu) // основное меню скрыто
            {
                IsSettingMenu = Visibility.Hidden; // выкл меню настроек
                IsBasicMenu = Visibility.Visible; // вкл основное меню

                IsMenu = Visibility.Visible; // включаем меню (любое)
                IsSettingVisible = Visibility.Visible; // включаем шестерёнку
                IsOutMenu = Visibility.Collapsed; // иконка переход на основное меню

                visibilityMenu = false; // меню не скрыто
            }
            else // основное меню не скрыто
            {
                IsMenu = Visibility.Collapsed; // скрываем меню (любое)
                IsSettingVisible = Visibility.Collapsed; // скрываем шестерёнку

                visibilityMenu = true; // меню скрыто
            }
        }
        else // если настройки
        {
            if (visibilitySetting) // открываем меню
            {
                IsBasicMenu = Visibility.Hidden; // выкл основное меню
                IsSettingMenu = Visibility.Visible; // вкл меню настроек

                IsMenu = Visibility.Visible; // включаем меню (любое)
                    
                IsOutMenu = Visibility.Visible; // иконка переход на основное меню
                IsSettingVisible = Visibility.Collapsed; // скрываем шестерёнку

                visibilitySetting = false; // меню было скрыто
            }
            else // закрываем меню
            {
                IsMenu = Visibility.Collapsed; // выключаем меню (любое)
                // иконка переход на основное меню
                IsOutMenu = Visibility.Collapsed;

                visibilitySetting = true; // меню было открыто
            }
        }
    }

    #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
