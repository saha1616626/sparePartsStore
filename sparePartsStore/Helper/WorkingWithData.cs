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
    }
}
