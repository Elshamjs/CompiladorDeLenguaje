using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    class BooleanVariable : DataField
    {
        private bool value;

        public BooleanVariable(bool value, DATA_TYPE type, int location, string name) : base(location, name, type)
        {
            this.value = value;
        }

        public bool Value { get => value; set => this.value = value; }
    }
}
