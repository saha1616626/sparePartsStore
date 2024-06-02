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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sparePartsStore.View
{
    /// <summary>
    /// Interaction logic for ViewSearchSpareParts.xaml
    /// </summary>
    public partial class ViewSearchSpareParts : Page
    {
        private readonly ListSearchSpareParts _listSearchSpareParts; // объект класса
        public ViewSearchSpareParts()
        {
            InitializeComponent();

            // получаем экз ListSearchSpareParts
            _listSearchSpareParts = (ListSearchSpareParts)this.Resources["ListSearchSpareParts"];
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // событие на ввод данных в текстовое поле поиска производителя
        private void TextBoxNameAutoParts(object sender, TextChangedEventArgs e)
        {
            // получаем текст из поля при изменении данных (поиска)
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // приводим к формату string
                string nameAutoParts = textBox.Text;

                _listSearchSpareParts.HandlerTextBoxChanged(nameAutoParts);
            }
        }
    }
}
