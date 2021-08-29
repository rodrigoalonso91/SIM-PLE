using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_PLE_2._0
{
    class Walker
    {
        public string FullName { get; private set; }
        public int ObjSim { get; private set; }
        public int ObjSellOut { get; private set; }
        public int Volumen { get; private set; }
        public int Gift40percent { get; private set; }

        public Walker(string Fullname, int ObjSim, int ObjSellOut, int Volumen, int Gift40percent)
        {
            this.FullName = Fullname;
            this.ObjSim = ObjSim;
            this.ObjSellOut = ObjSellOut;
            this.Volumen = Volumen;
            this.Gift40percent = Gift40percent;
        }

    }
}
