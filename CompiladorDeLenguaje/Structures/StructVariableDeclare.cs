using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructVariableDeclare : StructField
    {
        public StructVariableDeclare(DataField variable, string identifier, DataField startValue) : base(Struct_Type.Variable_Declare)
        {
            Identifier = identifier;
            Variable = variable;
            StartValue = startValue;
            IsParam = false;
        }

        public StructVariableDeclare(DataField variable, string identifier) : base(Struct_Type.Variable_Declare)
        {
            IsParam = false;
            Identifier = identifier;
            Variable = variable;
            StartValue = null;
        }

        public StructVariableDeclare(DATA_TYPE data_type, string identifier) : base(Struct_Type.Variable_Declare)
        {
            IsParam = false;
            Identifier = identifier;
            Variable = new DataField(identifier, data_type);
            StartValue = DataField.Null;
        }

        public DataField Variable { get; set; }
        public DataField StartValue { get; set; }
        public string Identifier { get; set; }
        public bool IsParam { get; set; }

        public void MatchSizes()
        {
            if(StartValue != null)
            {
                Variable.Columns = StartValue.Columns;
                Variable.Rows = StartValue.Rows;
            }
        }

        public override DataField runStructField()
        {
            MatchSizes();
            if(StartValue == null) return new DataField("000", DATA_TYPE.NULO);
            switch (Variable.Type)
            {
                case DATA_TYPE.BINARIO: { ((PrimitiveVariable<bool>)Variable).Value = ((PrimitiveVariable<bool>)StartValue).Value; break; }
                case DATA_TYPE.ENTERO: { ((PrimitiveVariable<int>)Variable).Value = ((PrimitiveVariable<int>)StartValue).Value; break; }
                case DATA_TYPE.DECIMAL: { ((PrimitiveVariable<double>)Variable).Value = ((PrimitiveVariable<double>)StartValue).Value; break; }
                case DATA_TYPE.CARACTER: { ((PrimitiveVariable<char>)Variable).Value = ((PrimitiveVariable<char>)StartValue).Value; break; }
                case DATA_TYPE.ARRAY_ENTERO: { ((NonPrimitiveArray<int>)Variable).Values = ((NonPrimitiveArray<int>)StartValue).Values; break; }
                case DATA_TYPE.ARRAY_BINARIO: { ((NonPrimitiveArray<bool>)Variable).Values = ((NonPrimitiveArray<bool>)StartValue).Values; break; }
                case DATA_TYPE.ARRAY_DECIMAL: { ((NonPrimitiveArray<double>)Variable).Values = ((NonPrimitiveArray<double>)StartValue).Values; break; }
                case DATA_TYPE.ARRAY_CARACTER: { ((NonPrimitiveArray<char>)Variable).Values = ((NonPrimitiveArray<char>)StartValue).Values; break; }
                case DATA_TYPE.BIARRAY_ENTERO: { ((NonPrimitiveMultiArray<int>)Variable).Values = ((NonPrimitiveMultiArray<int>)StartValue).Values;break; }
                case DATA_TYPE.BIARRAY_BINARIO: { ((NonPrimitiveMultiArray<bool>)Variable).Values = ((NonPrimitiveMultiArray<bool>)StartValue).Values; break; }
                case DATA_TYPE.BIARRAY_DECIMAL: { ((NonPrimitiveMultiArray<double>)Variable).Values = ((NonPrimitiveMultiArray<double>)StartValue).Values; break; }
                case DATA_TYPE.BIARRAY_CARACTER: { ((NonPrimitiveMultiArray<char>)Variable).Values = ((NonPrimitiveMultiArray<char>)StartValue).Values;  break; }
            }
            return new DataField("000", DATA_TYPE.NULO);
        }
    }
}
