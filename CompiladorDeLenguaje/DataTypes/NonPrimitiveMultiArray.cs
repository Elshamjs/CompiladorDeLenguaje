using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    class NonPrimitiveMultiArray<T> : DataField where T: struct
    {
        private int columns_size;
        private int rows_size;
        private T[][] values;

        public NonPrimitiveMultiArray(int columns_size, int rows_size, T[][] values, DATA_TYPE type, int location, string name) : base(location, name, type)
        {
            this.columns_size = columns_size;
            this.rows_size = rows_size;
            this.values = values;
        }

        public int Columns_size { get => columns_size; set => columns_size = value; }
        public int Rows_size { get => rows_size; set => rows_size = value; }
        public T[][] Values { get => values; set => values = value; }
    }
}
