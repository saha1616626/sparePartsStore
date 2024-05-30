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

        // Производитель
        #region Manufacture

        // Событие запуска страницы добавления прозиводителя из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddManufacture; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddManufacture() // вызываем в PageListManufacture
        {
            launchPageAddManufacture?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования прозиводителя из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageEditManufacture; // подписываемся в MainHeadViewModel
        public static void LaunchPageEditManufacture() // вызываем в PageListManufacture
        {
            launchPageEditManufacture?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования производителя
        public static event EventHandler<EventAggregator> saveDataCreateOrEditManufacture; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditManufacture() // вызываем в PageWorkManufacture
        {
            saveDataCreateOrEditManufacture?.Invoke(null, new EventAggregator());
        }

        // событие на удаление производителя
        public static event EventHandler<EventAggregator> saveDataDeleteManufacture; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteManufacture() // вызываем в PageListCountry
        {
            saveDataDeleteManufacture?.Invoke(null, new EventAggregator());
        }

        #endregion

        // запчасть
        #region Autopart

        // событие запуск страницы добавления аналогов из MainHeadViewModel
        public static event EventHandler<EventAggregator> analog; // подписываемся в MainHeadViewModel
        public static void Analog() // вызываем в PageListAutoparts
        {
            analog?.Invoke(null, new EventAggregator());
        }

        // Событие запуска страницы добавления запчасти из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageAddAutoparts; // подписываемся в MainHeadViewModel
        public static void LaunchPageAddAutoparts() // вызываем в PageListAutoparts
        {
            launchPageAddAutoparts?.Invoke(null, new EventAggregator());
        }

        // событие запуска страницы редактирования запчасти из MainHeadViewModel
        public static event EventHandler<EventAggregator> launchPageEditAutoparts; // подписываемся в MainHeadViewModel
        public static void LaunchPageEditAutoparts() // вызываем в PageListAutoparts
        {
            launchPageEditAutoparts?.Invoke(null, new EventAggregator());
        }

        // событие изменение БД после добавления или редактирования запчасти
        public static event EventHandler<EventAggregator> saveDataCreateOrEditAutoparts; // подписываемся в MainHeadViewModel
        public static void SaveDataCreateOrEditAutoparts() // вызываем в PageWorkAutoparts
        {
            saveDataCreateOrEditAutoparts?.Invoke(null, new EventAggregator());
        }

        // событие на удаление запчасти
        public static event EventHandler<EventAggregator> saveDataDeleteAutoparts; // подписываемся в MainHeadViewModel
        public static void SaveDataDeleteAutoparts() // вызываем в PageListAutoparts
        {
            saveDataDeleteAutoparts?.Invoke(null, new EventAggregator());
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

        // передача из ViewAuthorization объектов текстового поля
        public static event EventHandler<EventAggregator> updatePropertyViewAuthorization; // подписываемся в ViewAuthorizationViewAuthorization
        public static void UpdatePropertyViewAuthorization() // вызываем в AuthorizationViewModel
        {
            updatePropertyViewAuthorization?.Invoke(null, new EventAggregator());
        }

        // вход пользователя (авторизация)
        public static event EventHandler<EventAggregator> userLogin; // подписываемся в MainWindow
        public static void UserLogin() // вызываем в AuthorizationViewModel
        {
            userLogin?.Invoke(null, new EventAggregator());
        }

        #endregion        
    }
}
