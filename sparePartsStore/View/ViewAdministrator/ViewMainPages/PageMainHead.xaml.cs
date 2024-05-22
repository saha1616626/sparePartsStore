﻿using sparePartsStore.Helper;
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

namespace sparePartsStore.View.ViewAdministrator.ViewMainPages
{
    /// <summary>
    /// Interaction logic for PageMainHead.xaml
    /// </summary>
    public partial class PageMainHead : Page
    {
        public PageMainHead()
        {
            InitializeComponent();

            // тестово запускаем страницу посик запчастей
            //administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListManufacture.xaml", UriKind.Relative));
            //administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListAnalogues.xaml", UriKind.Relative));

            // запуск страницы
            startPage();

            // подписываемся на событие запуска страницы добавления марки авто
            WorkingWithData.launchPageAddCarBrand += Add_CarBrands;
            // подписываемся на событие запуска страницы списка марок после закрытия страницы добавления
            WorkingWithData.closePageAddCarBrand += LaunchCarBrand;

            // подписываемя на событие запуска страницы редактирования марки
            WorkingWithData.launchPageEditCarBrand += Edit_CarBrand;
        }

        // анимация включена
        public void AnimationOn(Button nameButton)
        {
            nameButton.Background = Brushes.Wheat;
        }
        // анимация выключена
        public void AnimationOff(Button nameButton)
        {
            nameButton.ClearValue(Button.ForegroundProperty);
            nameButton.ClearValue(Button.BackgroundProperty);
        }

        // начальная страница при запуске экрана 
        void startPage()
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            // автоматичесое отображение страницы при входе в учетную запись адинистратора
            administratorFrame.Navigate(new Uri("/View/ViewSearchSpareParts.xaml", UriKind.Relative));
        }

        // страница поиска запчастей
        private void Btn_SearchParts(object sender, RoutedEventArgs e)
        {
            startPage();
        }

        // марка
        private void Btn_carBrand(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            // автоматичесое отображение страницы при входе в учетную запись адинистратора
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCarBrands.xaml", UriKind.Relative));
        }

        // модель
        private void Btn_carModel(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCarModels.xaml", UriKind.Relative));
        }

        // агрегат
        private void Btn_Unit(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListUnit.xaml", UriKind.Relative));
        }

        // узел
        private void Btn_Knot(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListKnot.xaml", UriKind.Relative));
        }

        // запчасть
        private void Btn_Details(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListAutoparts.xaml", UriKind.Relative));
        }

        // страна
        private void Btn_Country(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCountry.xaml", UriKind.Relative));
        }

        // произовдитель
        private void Btn_Manufacture(object sender, RoutedEventArgs e)
        {
            // открываем основное меню
            basicMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListManufacture.xaml", UriKind.Relative));
        }

        // очистка памяти
        private void ClearMemoryAfterFrame()
        {
            // закрываем предыдущий фрейм
            administratorFrame.NavigationService?.RemoveBackEntry();
            administratorFrame.Content = null;

            // очистка визуальных элементов
            this.Resources.Clear();

            // очистка всех привязанных элементов
            BindingOperations.ClearAllBindings(this);

            // сборка мусора и освобождение неиспользуемых ресурсов
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // скрываем основное меню и открываем меню настроек
        private void settingMenu()
        {
            _basicMenu.Visibility = Visibility.Hidden; // выкл основное меню
            _settingMenu.Visibility = Visibility.Visible; // вкл меню настроек
        }

        // скрываем меню настроек и открываем основное меню
        private void basicMenu()
        {
            _settingMenu.Visibility = Visibility.Hidden; // выкл меню настроек
            _basicMenu.Visibility = Visibility.Visible; // вкл основное меню
        }

        // пользователи
        private void Btn_User(object sender, RoutedEventArgs e)
        {
            // открываем меню настроек
            settingMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewSettingRole/PageListUsers.xaml", UriKind.Relative));
        }

        // роли
        private void Btn_Role(object sender, RoutedEventArgs e)
        {
            // открываем меню настроек
            settingMenu();
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewSettingRole/PageListRole.xaml", UriKind.Relative));

        }

        // настройки
        private void Btn_Setting(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_UserSetting(object sender, RoutedEventArgs e)
        {
            // скрываем кнопку шестерёнка и отображаем кнопку меню
            _btnSetting.Visibility = Visibility.Hidden;
            _btnOutMenu.Visibility = Visibility.Visible;

            // открываем меню настроек
            settingMenu();

            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewSettingRole/PageListUsers.xaml", UriKind.Relative));
        }

        // кнопка перехода на основное меню
        private void Btn_BasicMenu(object sender, RoutedEventArgs e)
        {
            // скрываем кнопку меню и отображаем кнопку шестерёнок
            _btnSetting.Visibility = Visibility.Visible;
            _btnOutMenu.Visibility = Visibility.Hidden;

            // открываем основеное меню
            basicMenu();

            // запуск первоначального меню
            startPage();
        }

        #region WorkListBrand
        // запуск страницы добавить марку авто
        public void Add_CarBrands(object sender, EventAggregator e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            _menu.Visibility = Visibility.Collapsed; // отключаем меню основное
            _btnSetting.Visibility= Visibility.Collapsed; // отключаем кнопку настроек
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorking/PageWorkListBrand.xaml", UriKind.Relative));
        }

        // запуск страницы списка марок. после выхода.
        public void LaunchCarBrand(object sender, EventAggregator e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            _menu.Visibility = Visibility.Visible; // включаем меню основное
            _btnSetting.Visibility = Visibility.Visible; // включаем кнопку настроек
            // автоматичесое отображение страницы при входе в учетную запись адинистратора
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCarBrands.xaml", UriKind.Relative));
        }

        // запуск страницы для редактирования
        public void Edit_CarBrand(object sender, EventAggregator e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            _menu.Visibility = Visibility.Collapsed; // отключаем меню основное
            _btnSetting.Visibility = Visibility.Collapsed; // отключаем кнопку настроек
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorking/PageWorkListBrand.xaml", UriKind.Relative));

            // задержка, что PageWorkListBrand успела открыться
            Task.Run(async () =>
            {
                await Task.Delay(1); // Ждем завершения задержки
                Application.Current.Dispatcher.Invoke(() =>
                {
                    // изменяем название кнопки
                    WorkingWithData.RenameButtonBrand();
                });
            });
        }
        #endregion
    }
}
