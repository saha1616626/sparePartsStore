using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.Helper
{
    // класс определеня событий
    public static class WorkingWithData
    {
        // марки авто
        #region CarBrand

        // события запуска страницы добавления марки автомобиля из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddCarBrand; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddCarBrand() // вызываем событие в ListCarBrandViewModel
        {
            launchPageAddCarBrand?.Invoke(null, new EventAggregator());
        }

        // события запуска страницы редакирования марки автомобиля из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageEditCarBrand; // подписываемя в MainHeadViewModel
        public static void LaunchPageEditCarBrand() // вызвываем в ListCarBrandViewModel
        {
            launchPageEditCarBrand?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования данных марок авто
        public static event EventHandler<EventAggregator> saveDataCreateOrEditCarBrands; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditCarBrands() // вызываем в PageWorkListBrand
        {
            saveDataCreateOrEditCarBrands?.Invoke(null, new EventAggregator());
        }

        // событие на удаление данных марок авто
        public static event EventHandler<EventAggregator> saveDataDeleteCarBrands; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteCarBrands() // вызываем в PageWorkListBrand
        {
            saveDataDeleteCarBrands?.Invoke(null, new EventAggregator());
        }

        #endregion

        // модели авто
        #region CarModel

        // событие запуска страницы добавления марки авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchinPageAddCarModel; // подписываемся в MainHeadViewModel
        public static void LaunchinPageAddCarModel() // вызываем в PageListCarModels
        {
            launchinPageAddCarModel?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования марки авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchpageEditCarModel; // подписываемся в MainHeadViewModel
        public static void LaunchpageEditCarModel() // вызываем в PageListCarModels
        {
            launchpageEditCarModel?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования данных моделей авто
        public static event EventHandler<EventAggregator> saveDataCreateOrEditCarModels; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditCarModels() // вызываем в PageWorkListModel
        {
            saveDataCreateOrEditCarModels?.Invoke(null, new EventAggregator());
        }
        
        // событие на удаление данных моделей авто
        public static event EventHandler<EventAggregator> saveDataDeleteCarModels; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteCarModels() // вызваем в PageListCarModels
        {
            saveDataDeleteCarModels?.Invoke(null, new EventAggregator());
        }

        #endregion

        // общие события
        #region GeneralEvent

        // событие выхода из страницы на предидущую страницу 
        public static event EventHandler<EventAggregator> closePage; // подписываемся MainHeadViewModel
        public static void ClosePage() // вызываем событие в PageWorkListBrand
        {
            closePage?.Invoke(null, new EventAggregator());
        }


        // событие на очистку данных страницы PageMainHead
        public static event EventHandler<EventAggregator> clearMemoryAfterFrame; // подписываемся в PageMainHead
        public static void ClearMemoryAfterFrame() // вызываем событие в MainHeadViewModel
        {
            clearMemoryAfterFrame?.Invoke(null, new EventAggregator());
        }

        #endregion        
    }
}
