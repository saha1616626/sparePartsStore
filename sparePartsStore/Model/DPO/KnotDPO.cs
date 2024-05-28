using sparePartsStore.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.Model.DPO
{
    public class KnotDPO : INotifyPropertyChanged // узлы авто
    {
        public int KnotId { get; set; }

        private string _nameKnot {  get; set; }
        public string NameKnot
        {
            get { return _nameKnot; }
            set { _nameKnot = value;  OnPropertyChanged(nameof(NameKnot)); }
        }
        private string _nameUnit {  get; set; }
        public string NameUnit
        {
            get { return _nameUnit; }
            set { _nameUnit = value; OnPropertyChanged(nameof(NameUnit)); }
        }

        private int _unitId { get; set; }
        public int UnitId
        {
            get { return _unitId; }
            set { _unitId = value; OnPropertyChanged(nameof(UnitId)); }
        }

        // замена int UnitId на string NameUnit
        public KnotDPO CopyFromKnot(Knot knot)
        {
            KnotDPO knorDPO = new KnotDPO();
            ListUnitViewModel listUnitViewModel = new ListUnitViewModel();
            string nameUnit = string.Empty;
            foreach(var name in listUnitViewModel.ListUnit)
            {
                if(name.UnitId == knot.UnitId)
                {
                    nameUnit = name.NameUnit;
                    break;
                }
            }

            if(nameUnit != string.Empty)
            {
                knorDPO.KnotId = knot.KnotId;
                knorDPO.NameKnot = knot.NameKnot;
                knorDPO.NameUnit = nameUnit;
            }

            return knorDPO;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
