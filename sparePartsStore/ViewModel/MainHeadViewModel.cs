using sparePartsStore.Helper;
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
        private Visibility _isSettingMenu = Visibility.Collapsed;
        public Visibility IsSettingMenu
        {
            get { return _isSettingMenu; }
            set { _isSettingMenu = value; OnPropertyChanged(nameof(IsSettingMenu)); }
        }

        #endregion

        public MainHeadViewModel()
        {
            // подписываемя на событие запуска страницы добавления марки авто
            WorkingWithData.launchPageAddCarBrand += LaunchPageAddCarBrand;
        }

        // запуск страницы - поиск запчастей
        #region launchWorkPage

        // кнопка запуска страницы - поиск запчастей
        private RelayCommand _btn_SearchParts {  get; set; }
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
                        PageListCarBrands pageListCarBrands = new PageListCarBrands();
                        MainFrame.Navigate(pageListCarBrands);
                    }, (obj) => true));
            }
        }

        // запуск страницы - добавить марку авто
        private void LaunchPageAddCarBrand(object sender, EventAggregator e)
        {
            PageWorkListBrand pageWorkListBrand = new PageWorkListBrand();
            MainFrame.Navigate(pageWorkListBrand);

            // показываем, что было открыто основное меню перед его скрытием
            typeMenu = true;
            // скрываем шестерёнку и основное меню, чтобы нельзя было перемещаться между страницами
            selectedMenu();
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

        #endregion

        // переменная, которая показывает, было скрыто меню или нет
        private bool visibilityMenu = false; // меню скрыто

        // метод для скрытия или отображения меню
        private void selectedMenu()
        {
            if (typeMenu) // если основное меню
            {
                if(visibilityMenu) // основное меню скрыто
                {
                    IsBasicMenu = Visibility.Visible;

                    visibilityMenu = false; // меню не скрыто
                }
                else // основное меню не скрыто
                {
                    IsBasicMenu = Visibility.Collapsed;

                    visibilityMenu = true; // меню скрыто
                }
            }
            else // если настройки
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
