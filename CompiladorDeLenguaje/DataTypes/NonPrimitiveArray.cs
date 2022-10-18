using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    class NonPrimitiveArray<T> : DataField where T : struct
    {
        private int size;
        private T[] values;

        public NonPrimitiveArray(int size, T[] values, DATA_TYPE type, int location, string name) : base(location, name, type)
        {
            this.size = size;
            this.values = values;
        }

        public int Size { get => size;  }
        public T[] Values { get => values; set => values = value; }
    }
}
