﻿using sparePartsStore.Model.DPO;
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

namespace sparePartsStore.View.ViewAdministrator.ViewWorkingWithData
{
    /// <summary>
    /// Interaction logic for PageListAnalogues.xaml
    /// </summary>
    public partial class PageListAnalogues : Page
    {
        private readonly ListAnaloguesViewModel _listAnaloguesViewModel; // объект класса
        public PageListAnalogues()
        {
            InitializeComponent();

            // получаем экз ListAutopartViewModel
            _listAnaloguesViewModel = (ListAnaloguesViewModel)this.Resources["ListAnaloguesViewModel"];
        }

        // получаем данные запчасти, к котрой нужно подобрать аналог
        public void DataReception(AutopartDPO autopartDPO)
        {
            _listAnaloguesViewModel.AutopartDPO = autopartDPO; // передаём данные для 
        }
    }
}
