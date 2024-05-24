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

        public MainHeadViewModel(Frame frame)
        {
            _mainFrame = frame; // ссылка на фрейм из PageMainHead

            // запуск страницы при открытии PageMainHead
            //startMainPage();
        }

        public MainHeadViewModel() { }

        // запуск страниц по кнопкам или автозапуск
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
                        startMainPage();
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

            //NavigationManager.StartFrame = _mainFrame;
            //MainFrame.Navigate(viewSearchSpareParts);
        }

        #endregion



        // запуск страницы марка авто
        public void PageListCarBrandsBtn()
        {
            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            // экз. класс страницы марок авто
            PageListCarBrands pageListCarBrands = new PageListCarBrands();
            MainFrame.Navigate(pageListCarBrands);
        }

        // запуск страницы поиска запчастей
        public void PageListSearchPartsBtn()
        {
            // событие для очистка фреймов из памяти в PageMainHead
            WorkingWithData.ClearMemoryAfterFrame();
            // экз. класс страницы марок авто
            ViewSearchSpareParts viewSearchSpareParts = new ViewSearchSpareParts();
            MainFrame.Navigate(viewSearchSpareParts);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
