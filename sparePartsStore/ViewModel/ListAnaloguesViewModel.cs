using sparePartsStore.Model.DPO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sparePartsStore.ViewModel
{
    public class ListAnaloguesViewModel
    {
        public ListAnaloguesViewModel()
        {

        }

        // хранит выбранные данные для подбора аналогов
        public AutopartDPO AutopartDPO { get; set; }
    }
}
