using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using sparePartsStore.ViewModel;
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

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkDetail.xaml
    /// </summary>
    public partial class PageWorkDetail : Page
    {
        private readonly ListAutopartViewModel _listAutopartViewModel; // объект класса
        private Storyboard _focusAnimation; // анимация подсветки
        public PageWorkDetail()
        {
            InitializeComponent();

            // получаем экз ListAutopartViewModel
            _listAutopartViewModel = (ListAutopartViewModel)this.Resources["ListAutopartViewModel"];

            // анимация при неккорректном вводе данных в поля. подключаем ресурс
            _focusAnimation = (Storyboard)FindResource("FocusAnimation"); // анимация исчезает через n интервал времени
        }

        // переменная, которая хранит переданные данные из MainHeadViewModel для редактирования
        private AutopartDPO getAutopartDPOs = new AutopartDPO();

        // получаем данные из MainHeadViewModel для вывода на экран и последущего редактирования
        public void DataReception(AutopartDPO autopartDPO)
        {
            getAutopartDPOs = autopartDPO; // сохраняем данные
            // передаём данные в поля
            this.CbCountry.ItemsSource = _listAutopartViewModel.GetCountryOnComboBox(); // получаем список для ComBox
            CbCountry.Text = autopartDPO.NameCountry;

            this.CbManufacture.ItemsSource = _listAutopartViewModel.GetManufactureOnComboBox();
            this.CbCarBrand.ItemsSource = _listAutopartViewModel.GetCarBrandOnComboBox();
            this.CbCarModel.ItemsSource = _listAutopartViewModel.GetCarModelOnComboBox();
            this.CbUnit.ItemsSource = _listAutopartViewModel.GetUnitOnComboBox();
            this.CbKnot.ItemsSource = _listAutopartViewModel.GetKnotOnComboBox();
            this.CbModerationStatus.ItemsSource = _listAutopartViewModel.GetModerationStatusOnComboBox();

            NameAutopart.Text = autopartDPO.NameAutopart.ToString();
        }


    }
}
