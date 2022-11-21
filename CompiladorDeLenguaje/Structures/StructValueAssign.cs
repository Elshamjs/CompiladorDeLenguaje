using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructValueAssign : StructField
    {
        public StructValueAssign(DataField variable, DataField value) : base(Struct_Type.Value_Assing)
        {
            this.Variable = variable;
            this.Value = value;
        }


        public DataField Variable { get; set; }
        public DataField Value { get; set; }

        public override DataField runStructField()
        {
            switch (Variable.Type)
            {
                case DATA_TYPE.BINARIO: { ((PrimitiveVariable<bool>)Variable).Value = ((PrimitiveVariable<bool>)Value).Value; break; }
                case DATA_TYPE.ENTERO: { ((PrimitiveVariable<int>)Variable).Value = ((PrimitiveVariable<int>)Value).Value; break; }
                case DATA_TYPE.DECIMAL: { ((PrimitiveVariable<double>)Variable).Value = ((PrimitiveVariable<double>)Value).Value; break; }
                case DATA_TYPE.CARACTER: { ((PrimitiveVariable<char>)Variable).Value = ((PrimitiveVariable<char>)Value).Value; break; }
                case DATA_TYPE.ARRAY_ENTERO: { ((NonPrimitiveArray<int>)Variable).Values = ((NonPrimitiveArray<int>)Value).Values; break; }
                case DATA_TYPE.ARRAY_BINARIO: { ((NonPrimitiveArray<bool>)Variable).Values = ((NonPrimitiveArray<bool>)Value).Values; break; }
                case DATA_TYPE.ARRAY_DECIMAL: { ((NonPrimitiveArray<double>)Variable).Values = ((NonPrimitiveArray<double>)Value).Values; break; }
                case DATA_TYPE.ARRAY_CARACTER: { ((NonPrimitiveArray<char>)Variable).Values = ((NonPrimitiveArray<char>)Value).Values; break; }
                case DATA_TYPE.BIARRAY_ENTERO: { ((NonPrimitiveMultiArray<int>)Variable).Values = ((NonPrimitiveMultiArray<int>)Value).Values; break; }
                case DATA_TYPE.BIARRAY_BINARIO: { ((NonPrimitiveMultiArray<bool>)Variable).Values = ((NonPrimitiveMultiArray<bool>)Value).Values; break; }
                case DATA_TYPE.BIARRAY_DECIMAL: { ((NonPrimitiveMultiArray<double>)Variable).Values = ((NonPrimitiveMultiArray<double>)Value).Values; break; }
                case DATA_TYPE.BIARRAY_CARACTER: { ((NonPrimitiveMultiArray<char>)Variable).Values = ((NonPrimitiveMultiArray<char>)Value).Values; break; }
            }
            return new DataField("000", DATA_TYPE.NULO);
        }
    }
}
