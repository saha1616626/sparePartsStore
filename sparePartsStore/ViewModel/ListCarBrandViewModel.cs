using sparePartsStore.Helper;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sparePartsStore.ViewModel
{
    // класс для работы с маркамо авто
    public class ListCarBrandViewModel : INotifyPropertyChanged
    {

        // свойства

        // свойства страницы марки
        #region Brand
        // поле названия марки
        private string? _textBrand {  get; set; }
        public string? TextBrand
        {
            get { return _textBrand; }
            set 
            {
                if (_textBrand != value)
                {
                    _textBrand = value;
                    OnPropertyChanged(nameof(TextBrand));
                }
            }
        }


        #endregion

        // открывам страницу добавления данных "марка авто"
        #region openPageAddCarBrand
        private RelayCommand _addCarBrands;
        public RelayCommand AddCarBrands
        {
            get
            {
                return _addCarBrands ??
                    (_addCarBrands = new RelayCommand(obj =>
                    {

                        //  очищаем фрейм и запускаем страницу добавления данных
                        WorkingWithData.LaunchPageAddCarBrand(); // вызываем событие запуска страницы 

                    }, (obj) => true));
            }
        }
        #endregion

        // открываем страницу для редактирования
        #region openPageEditCarBrand
        private RelayCommand _editCarBrand;
        public RelayCommand EditCarBrand
        {
            get
            {
                return _editCarBrand ??
                    (_editCarBrand = new RelayCommand(obj =>
                    {
                        // вызываем события для запуска страныцы редактирования марки авто
                        WorkingWithData.LaunchPageEditCarBrand();

                        // задержка, что PageWorkListBrand успе
                        Task.Run(async () =>
                        {
                            await Task.Delay(10); // Ждем завершения задержки
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                TextBrand = "Тест передачи данных для редактирования";

                            });
                        });


                    }, (obj) => true));
            }
        }

        #endregion

        // выход из страницы добавить или редактировать на страницу списка марок авто
        #region closePageAddOrDeleteCarBrand
        private RelayCommand _closePageAddOrDeleteCarBrands;
        public RelayCommand ClosePageAddOrDeleteCarBrands
        {
            get
            {
                return _closePageAddOrDeleteCarBrands ??
                    (_closePageAddOrDeleteCarBrands = new RelayCommand(obj =>
                    {
                        // закрываем страницу добавления товара
                        WorkingWithData.ClosePageAddCarBrand(); // вызов события закрытия страницы
                    }, (obj) => true));
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
