using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using sparePartsStore.Helper;
using sparePartsStore.Model;
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
                }
            }

            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            PageListCarBrands pageListCarBrands = new PageListCarBrands();
            MainFrame.NavigationService.Navigate(pageListCarBrands);
            selectedMenu(); // отображаем меню
        }

        #endregion

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

        // методы PageMainHead
        #region methodsPageMainHead

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
