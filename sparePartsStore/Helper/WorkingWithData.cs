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

        // агрегат авто
        #region Unit

        // событие запуска страницы добавления агрегата авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddUnit; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddUnit() // вызываем в PageListUnit
        {
            launchPageAddUnit?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования агрегатов авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchpageEditUnit; // подписываемся в MainHeadViewModel
        public static void LaunchpageEditUnit() // вызываем в PageListUnit
        {
            launchpageEditUnit?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования агрегатов авто
        public static event EventHandler<EventAggregator> saveDataCreateOrEditUnit; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditUnit() // вызываем в PageWorkUnit
        {
            saveDataCreateOrEditUnit?.Invoke(null, new EventAggregator());
        }

        // событие на удаление данных агрегатов авто
        public static event EventHandler<EventAggregator> saveDataDeleteUnit; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteUnit() // вызываем в PageListUnit
        {
            saveDataDeleteUnit?.Invoke(null, new EventAggregator());
        }

        #endregion

        // узел авто
        #region Knot

        // Событие запуска страницы добавления узла авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddKnot; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddKnot() // вызываем в PageListKnot
        {
            launchPageAddKnot?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования узлов авто из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchpageEditKnot; // подписываемся в MainHeadViewModel
        public static void LaunchpageEditKnot() // вызываем в PageListKnot
        {
            launchpageEditKnot?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования узлов авто
        public static event EventHandler<EventAggregator> saveDataCreateOrEditKnot; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditKnot() // вызываем в PageListKnot
        {
            saveDataCreateOrEditKnot?.Invoke(null, new EventAggregator());
        }

        // событие на удаление данных узлов авто
        public static event EventHandler<EventAggregator> saveDataDeleteKnot; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteKnot() // вызываем в PageListKnot
        {
            saveDataDeleteKnot?.Invoke(null, new EventAggregator());
        }

        #endregion

        // страна
        #region CountryCountry

        // Событие запуска страницы добавления страны из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddCountry; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddCountry() // вызываем в PageListCountry
        {
            launchPageAddCountry?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования страны из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchpageEditCountry; // подписываемся в MainHeadViewModel
        public static void LaunchpageEditCountry() // вызываем в PageListCountry
        {
            launchpageEditCountry?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования страны
        public static event EventHandler<EventAggregator> saveDataCreateOrEditCountry; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditCountry() // вызываем в PageWorkCountry
        {
            saveDataCreateOrEditCountry?.Invoke(null, new EventAggregator());
        }

        // событие на удаление данных стран
        public static event EventHandler<EventAggregator> saveDataDeleteCountry; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteCountry() // вызываем в PageListCountry
        {
            saveDataDeleteCountry?.Invoke(null, new EventAggregator());
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

        // событие на 

        #endregion        
    }
}
