using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using sparePartsStore.Helper;
using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.View;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
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

        // запуск страницы - узел авто
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

        // метод для скрытия или отображения меню, иконок при редактировании данных
        private void selectedMenu()
        {
            if (typeMenu) // если основное меню
            {
                if (visibilityMenu) // основное меню скрыто
                {
                    IsMenu = Visibility.Visible; // включаем меню (любое)
                    IsSettingVisible = Visibility.Visible; // включаем шестерёнку

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
