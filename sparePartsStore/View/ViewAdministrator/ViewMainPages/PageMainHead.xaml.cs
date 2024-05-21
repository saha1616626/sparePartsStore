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
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            // автоматичесое отображение страницы при входе в учетную запись адинистратора
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCarBrands.xaml", UriKind.Relative));
        }

        // модель
        private void Btn_carModel(object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCarModels.xaml", UriKind.Relative));
        }

        // агрегат
        private void Btn_Unit(object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListUnit.xaml", UriKind.Relative));
        }

        // узел
        private void Btn_Knot(object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListKnot.xaml", UriKind.Relative));
        }

        // запчасть
        private void Btn_Details(object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListAutoparts.xaml", UriKind.Relative));
        }

        // страна
        private void Btn_Country (object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewWorkingWithData/PageListCountry.xaml", UriKind.Relative));
        }

        // произовдитель
        private void Btn_Manufacture(object sender, RoutedEventArgs e)
        {
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

        // фрейм настройка пользователей
        private void Btn_UserSetting(object sender, RoutedEventArgs e)
        {
            // очистка фреймов памяти
            ClearMemoryAfterFrame();
            administratorFrame.Navigate(new Uri("/View/ViewAdministrator/ViewSettingRole/PageListUsers.xaml", UriKind.Relative));
        }
    }
}
