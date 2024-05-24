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
        public MainHeadViewModel()
        {

            // подписываемя на событие запуска страницы добавления марок авто
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

        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
