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
        // события запуска страницы добавления марки автомобиля
        public static event EventHandler<EventAggregator> launchPageAddCarBrand; // подписываемся в PageMainHead
        public static void LaunchPageAddCarBrand() // вызываем событие в ListCarBrandViewModel
        {
            launchPageAddCarBrand?.Invoke(null, new EventAggregator());
        }

        // событие выхода на страницу списка марок после на жатия на кнопку выход в меню
        public static event EventHandler<EventAggregator> closePageAddCarBrand; // подписываемся в PageMainHead
        public static void ClosePageAddCarBrand() // вызываем событие в ListCarBrandViewModel
        {
            closePageAddCarBrand?.Invoke(null, new EventAggregator());
        }

        // событие входа на страницу редактирования данных списка марок
        public static event EventHandler<EventAggregator> launchPageEditCarBrand; // подписываемя в PageMainHead
        public static void LaunchPageEditCarBrand() // вызвываем в ListCarBrandViewModel
        {
            launchPageEditCarBrand?.Invoke(null, new EventAggregator());
        }

        // событие на смену названия кнопки "Редактировать" на странице PageWorkListBrand
        public static event EventHandler<EventAggregator> renameButtonBrand; // подписываемя на странице PageWorkListBrand
        public static void RenameButtonBrand() // вызывается событие в PageMainHead
        {
            renameButtonBrand?.Invoke(null, new EventAggregator());
        }
    }
}
