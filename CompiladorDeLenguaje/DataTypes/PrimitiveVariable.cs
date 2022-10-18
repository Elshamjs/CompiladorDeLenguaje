using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
  
    class PrimitiveVariable<T> : DataField where T: struct
    {
        private T value;

        public PrimitiveVariable(T value, int location, string name, DATA_TYPE TYPE) : base(location, name, TYPE)
        {
            this.value = value;
        }

        public T Value { get => value; set => this.value = value; }
    }
}
