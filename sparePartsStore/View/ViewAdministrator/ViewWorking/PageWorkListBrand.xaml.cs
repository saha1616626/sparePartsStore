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

namespace sparePartsStore.View.ViewAdministrator.ViewWorking
{
    /// <summary>
    /// Interaction logic for PageWorkListBrand.xaml
    /// </summary>
    public partial class PageWorkListBrand : Page
    {

        public PageWorkListBrand()
        {
            InitializeComponent();

            // подписка на событие, если мы редактируем данные
            WorkingWithData.renameButtonBrand += nameButtonAdd;
        }

        // обновление названия кнопки в зависимости от действия edit or add
        private void nameButtonAdd(object sender, EventAggregator e)
        {
            RenameButtonBrand.Content = "Редактировать"; 
        }
    }
}
