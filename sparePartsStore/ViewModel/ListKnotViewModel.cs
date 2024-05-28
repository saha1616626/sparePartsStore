using sparePartsStore.Model;
using sparePartsStore.Model.DPO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace sparePartsStore.ViewModel
{
    public class ListKnotViewModel : INotifyPropertyChanged
    {
        public ListKnotViewModel()
        {
            //чтение данных из БД
            ListKnotRead = GetListKnot();

            //вывод данных в таблицу
            ListKnotDPO = LoadKnotBD();
        }

        // выбранные данные из таблицы
        private KnotDPO _selectedKnot { get; set; }
        public KnotDPO SelectedKnot
        {
            get { return _selectedKnot; }
            set
            {
                _selectedKnot = value;
                OnPropertyChanged(nameof(SelectedKnot));
                OnPropertyChanged(nameof(IsWorkButtonEnable));
            }
        }

        // отображение кнопки редактировать или удалить при выборе данных в таблице
        private bool _isWorkButtonEnable;
        public bool IsWorkButtonEnable
        {
            get { return _selectedKnot != null; } // если нажто поле в таблице
            set { _isWorkButtonEnable = value; OnPropertyChanged(nameof(IsWorkButtonEnable)); }
        }

        // список отображения узлов авто на экране
        private ObservableCollection<KnotDPO> _listKnotDPO { get; set; }
        public ObservableCollection<KnotDPO> ListKnotDPO
        {
            get { return _listKnotDPO; }
            set { _listKnotDPO = value; OnPropertyChanged(nameof(ListKnotDPO)); }
        }

        // метод получения списка узлов авто для вывода на экран. Преобразование ListKnotRead в ListKnotDPO
        private ObservableCollection<KnotDPO> LoadKnotBD()
        {
            // создаём преобразованный список
            ObservableCollection<KnotDPO> knots = new ObservableCollection<KnotDPO>();

            // экз для преобразования данных
            KnotDPO knotDPO = new KnotDPO();

            if (ListKnotRead != null)
            {
                // проходимся по списку ListKnotRead для заполнения списка ListKnotDPO
                foreach (var knot in ListKnotRead.ToList())
                {
                    knots.Add(knotDPO.CopyFromKnot(knot)); // добавляем данные 
                }
            }

            return knots;
        }

        // коллекция считанная из БД
        public ObservableCollection<Knot> ListKnotRead { get; set; } = new ObservableCollection<Knot>();

        // метод для получения коллекции узлов авто
        private ObservableCollection<Knot> GetListKnot()
        {
            try
            {
                ObservableCollection<Knot> knots = new ObservableCollection<Knot>(); // список полученных данных из БД
                using (SparePartsStoreContext context = new SparePartsStoreContext())
                {
                    List<Knot> knotDB = context.Knots.ToList(); // получаем список узлов авто
                    if (knotDB != null)
                    {
                        // копируем данные в список из БД
                        foreach (var knotItem in knotDB)
                        {
                            Knot knotUP = new Knot();
                            knotUP.KnotId = knotItem.KnotId;
                            knotUP.UnitId = knotItem.UnitId;
                            knotUP.NameKnot = knotItem.NameKnot;
                            // добавляем в список
                            knots.Add(knotUP);
                        }
                    }
                }
                return knots;
                throw new Exception("Ошибка работы БД!");
            }
            catch (Exception ex)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show(ex.ToString(), "Внимание!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return null;
            }
        }

        // передаём получаенные выбранные данные из таблицы
        public KnotDPO TransmitionKnot()
        {
            KnotDPO knotDPO = new KnotDPO();
            knotDPO = (KnotDPO)SelectedKnot;
            return knotDPO;
        }

        // получаем данные для ComBox
        public List<Unit> GetUnitOnComboBox()
        {
            // список, котроый будет в себе хранить значения comBox
            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Unit> units = context.Units.ToList(); // получили список узлов авто из БД
                List<Unit> unitList = new List<Unit>(); // список для ComBox
                // записываем массив knotList в knotDPOs
                foreach (Unit knotItem in units)
                {
                    // добавляем данные в список
                    unitList.Add(knotItem);
                }
                // возвращаем массив обратно
                return unitList;
            }
        }

        // переменная, которая хранит текущие данные в поле у PageWorkKnot
        public TextBox NameKnotInput { get; set; }

        // проверяем, есть ли совпадения данных перед добавлением
        public bool CheckingForMatchDB()
        {
            bool noCoincidence = true; // по умолчанию нет совпадения

            Unit unit = new Unit();
            unit.UnitId = SelectedUnit.UnitId; // получаем ID узла

            // проводим проверку на уникальность узла в выбранном агрегате. так, как один и тот-же узел может быть в разынх агрегатах

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Knot> knots = context.Knots.ToList(); // получаем список  узлов авто
                List<Knot> newKnot = new List<Knot>();
                if (knots != null)
                {
                    newKnot = knots.Where(k => k.UnitId == unit.UnitId).ToList(); // получаем список узлом по выбранному агрегату в ComBox
                }

                // проверка наличия узла в определенном агрегате
                if (newKnot != null)
                {
                    noCoincidence = !newKnot.Any(num => num.NameKnot.ToLower().Contains(NameKnotInput.Text.ToLower()));
                }
            }

            return noCoincidence;
        }

        // если пользователь редактирует данные проверяем, что он не изменяет данные уже существующие в таблице
        public bool CheckingForMatchEditDB(KnotDPO knot)
        {
            bool noCoincidence = false; // по умолчанию совпадение не найдено

            Unit unit = new Unit();
            unit.UnitId = SelectedUnit.UnitId; // получаем ID узла

            using (SparePartsStoreContext context = new SparePartsStoreContext())
            {
                List<Knot> knots = context.Knots.ToList(); // получаем список узлов
                List<Knot> newKnot = new List<Knot>();
                if (knots != null)
                {
                    newKnot = knots.Where(k => k.UnitId == unit.UnitId).ToList(); // получаем список узлом по выбранному агрегату в ComBox
                }

                if(newKnot != null) // проверяем совпадение в определенном агрегате
                {
                    foreach (Knot k in newKnot)
                    {
                        if (k.KnotId == knot.KnotId) // если находим данные в списке БД, которые в текущий момент редактируем, то пропускаем
                        {
                            continue;
                        }

                        if (k.NameKnot.ToLower().Trim() == NameKnotInput.Text.ToLower().Trim()) // если нашли совпадение в таблице
                        {
                            noCoincidence = true;
                        }
                    }
                }
            }

            return noCoincidence;
        }

        // свойство поля для ввода названия узла авто
        private TextBox _outNameNot;
        public TextBox OutNameKnot
        {
            get { return _outNameNot; }
            set
            {
                _outNameNot = value;
                OnPropertyChanged(nameof(OutNameKnot));
            }
        }

        // обновляем отображение таблицы после удаления данных
        public void UpdateTabel()
        {
            //чтение данных из БД
            ListKnotRead = GetListKnot();

            //вывод данных в таблицу
            ListKnotDPO = LoadKnotBD();
        }

        // список для фильтров таблицы
        public ObservableCollection<Knot> ListSearch { get; set; } = new ObservableCollection<Knot>();

        // поиск узлов авто
        public void HandlerTextBoxChanged(string nameKnot)
        {
            if (nameKnot.Trim() != "")
            {
                ListSearch = GetListKnot(); // получаем актуальные данные из БД
                // создаём список с поиском по введенным данным в таблице
                var resKnot = ListSearch.Where(num => num.NameKnot.ToLower().Contains(nameKnot.ToLower())).ToList();
                
                // выведет всё, если ничего не найдено
                //if (resKnot.Count > 0)
                //{
                //    ListKnotDPO.Clear();// очищаем основной список
                //    // вносим актуальные данные основного списка
                //    foreach (var knotItem in resKnot)
                //    {
                //        KnotDPO knotDPO = new KnotDPO();
                //        knotDPO = knotDPO.CopyFromKnot(knotItem);
                //        ListKnotDPO.Add(knotDPO); // добавляем данные
                //    }
                //}

                ListKnotDPO.Clear();// очищаем основной список
                                    // вносим актуальные данные основного списка
                foreach (var knotItem in resKnot)
                {
                    KnotDPO knotDPO = new KnotDPO();
                    knotDPO = knotDPO.CopyFromKnot(knotItem);
                    ListKnotDPO.Add(knotDPO); // добавляем данные
                }
            }

            if (nameKnot.Trim() == "")
            {
                ListKnotDPO.Clear();// очищаем основной список
                ListSearch.Clear(); // очищаем доп список
                ListSearch = GetListKnot(); // получаем актуальные данные из БД
                foreach (var knotItem in ListSearch)
                {
                    KnotDPO knotDPO = new KnotDPO();
                    knotDPO = knotDPO.CopyFromKnot(knotItem);
                    ListKnotDPO.Add(knotDPO);
                }
            }
        }

        // comboBox
        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
