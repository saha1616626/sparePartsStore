using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sparePartsStore.ViewModel
{
    // вспомогательный класс "агрегат"
    public class ListUnitViewModel : INotifyPropertyChanged
    {
        // конструктор
        public ListUnitViewModel()
        {
            //чтение данных из БД
            ListUnitRead = GetListUnitBD();

            //вывод данных в таблицу
            ListUnit = LoadUnit();
        }

        // выбранные данные таблицы
        private Unit _selectedUnit { get; set; }
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedUnit != null; } // если нажто поле в таблице
            set 
            { 
                _isWorkButtonEnable = value; 
                OnPropertyChanged(nameof(IsWorkButtonEnable)); 
            }
        }

        // коллекция считанная из БД 
        public ObservableCollection<Unit> ListUnitRead { get; set; } = new ObservableCollection<Unit>();

        // коллекция отображения агрегатов авто на экране
        private ObservableCollection<Unit> _listUnit { get; set; }
        public ObservableCollection<Unit> ListUnit
        {
            get { return _listUnit; }
            set { _listUnit = value; OnPropertyChanged(nameof(ListUnit)); }
        }

        // метод получения списка агрегатов авто для вывода на экран. Преобразование ListUnitRead в ListUnit
        private ObservableCollection<Unit> LoadUnit()
        {
            if(ListUnitRead != null)
            {
                // копируем список ListUnitRead в список ListUnit
                ListUnit = ListUnitRead;
            }

            return ListUnit;
        }

        // метод для получения коллекции моделей авто из БД
        private ObservableCollection<Unit> GetListUnitBD()
        {
            try
            {
                ObservableCollection<Unit> listUnit = new ObservableCollection<Unit>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Unit> unit = context.Units.ToList(); // получаем список агрегатов
                    if(unit != null)
                    {
                        // копируем данные в список из БД
                        foreach(var u in unit)
                        {
                            Unit unitUp = new Unit();
                            unitUp.UnitId = u.UnitId;
                            unitUp.NameUnit = u.NameUnit;
                            // добавляем в список
                            listUnit.Add(unitUp);
                        }
                    }
                }

                return listUnit;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаем полученные выбранные данные из таблицы
        public Unit TransmitionUnit()
        {
            Unit unit = new Unit();
            unit = (Unit)SelectedUnit;
            return unit;
        }

        // переменная, которая хранит текущие данные в поле у PageWorkUnit
        public TextBox NameUnitInput { get; set; }

        // проверяем, есть ли совпадение данных перед добавлением
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Unit> units = context.Units.ToList(); // получаем список агрегатов
                noCoincidence = !units.Any(num => num.NameUnit.ToLower().Contains(NameUnitInput.Text.ToLower()));
            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(Unit unit)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Unit> units = context.Units.ToList(); // получаем список агрегатов
                foreach (Unit u in units)
                {
                    if(u.UnitId == unit.UnitId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                    {
                        continue;
                    }

                    if(u.NameUnit.ToLower().Trim() == NameUnitInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                    {
                        noCoincidence = true;
                    }
                }
            }

            return noCoincidence;
        }

        // свойство поля для ввода названия агрегата авто
        private TextBox _outNameUnit;
        public TextBox OutNameUnit
        {
            get { return _outNameUnit; }
            set
            {
                _outNameUnit = value;
                OnPropertyChanged(nameof(OutNameUnit));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListUnitRead = GetListUnitBD();

            //вывод данных в таблицу
            ListUnit = LoadUnit();
        }

        // список для фильтров таблицы
        public ObservableCollection<Unit> ListSearch { get; set; } = new ObservableCollection<Unit>();
        // поиск агрегата авто
        public void HandlerTextBoxChanged(string nameUnit)
        {
            if(nameUnit.Trim() != "")
            {
                ListSearch = GetListUnitBD(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resUnit = ListSearch.Where(num => num.NameUnit.ToLower().Contains(nameUnit.ToLower())).ToList();

                // выведет всё, если ничего не найдено
                //if (resUnit.Count > 0)
                //{
                //    ListUnit.Clear(); // очищаем список отображения данных в таблице
                //    // вносим актуальные данные основного списка
                //    foreach(var unit in resUnit)
                //    {
                //        Unit un = new Unit();
                //        un = unit;
                //        ListUnit.Add(un); // добавляем новые данные в список (с учетом фильтра)
                //    }
                //}

                ListUnit.Clear(); // очищаем список отображения данных в таблице
                                  // вносим актуальные данные основного списка
                foreach (var unit in resUnit)
                {
                    Unit un = new Unit();
                    un = unit;
                    ListUnit.Add(un); // добавляем новые данные в список (с учетом фильтра)
                }
            }

            if(nameUnit.Trim() == "")
            {
                ListUnit.Clear(); // очищаем список отображения данных в таблице
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListUnitBD(); // получаем актуальные данные из БД
                foreach(var u in ListSearch)
                {
                    Unit unit = new Unit();
                    unit = u;
                    ListUnit.Add(unit);  // добавляем новые данные в список (без фильтра)
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
