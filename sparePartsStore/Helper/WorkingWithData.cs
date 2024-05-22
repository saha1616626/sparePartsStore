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
    }
}
