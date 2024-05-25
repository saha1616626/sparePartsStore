using sparePartsStore.Helper;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using sparePartsStore.ViewModel;
using sparePartsStore.View.ViewAdministrator.ViewMainPages;
using sparePartsStore.Model;

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkListBrand.xaml
    /// </summary>
    public partial class PageWorkListBrand : Page
    {
        private readonly ListCarBrandViewModel _listCarBrandViewModel; // здесь храним экз. класса ListCarBrandViewModel
        public PageWorkListBrand()
        {
            InitializeComponent();
        }

        // закрываем страницу 
        private void ClosePageAddOrDeleteCarBrands(object sender, EventArgs e)
        {
            // вызываем события для закрытия страницы
            WorkingWithData.ClosePage();
        }

        // сохраняем данные после изменениня или добавления данных в БД
        private void SaveData(object sender, EventArgs e)
        {
            // обрабатываем логику полей перед добавлением или изменением

            // вызываем событие для сохранения данных в классе MainHeadViewModel
            WorkingWithData.SaveDataCreateOrEditCarBrands();
        }


        // событие передачи данных из текстового поля
        public event EventHandler<MyEventArgsObject> MyEventArgsObject;
        protected virtual void TransmitData(CarBrand value)
        {
            MyEventArgsObject?.Invoke(this, new MyEventArgsObject { Value = value});
        }
        // метод передачи данных из TextBox в MainHeadViewModel
        public void Transmit()
        {
            CarBrand carBrand = new CarBrand();
            carBrand.NameCarBrand = nameBrand.Text.Trim();
            TransmitData(carBrand); // передали название марки авто
        }
    }
}
