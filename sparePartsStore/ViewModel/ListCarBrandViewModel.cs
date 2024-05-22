using sparePartsStore.Helper;
using sparePartsStore.View.ViewAdministrator.ViewWorking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.ViewModel
{
    // класс для работы с маркамо авто
    public class ListCarBrandViewModel : INotifyPropertyChanged
    {
        // открывам страницу добавления данных "марка авто"
        #region openPageAddCarBrand
        private RelayCommand _add_CarBrands;
        public RelayCommand Add_CarBrands
        {
            get
            {
                return _add_CarBrands ??
                    (_add_CarBrands = new RelayCommand(obj =>
                    {

                        //  очищаем фрейм и запускаем страницу добавления данных
                        WorkingWithData.LaunchPageAddCarBrand(); // вызываем событие запуска страницы 

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
