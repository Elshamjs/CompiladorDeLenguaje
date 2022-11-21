using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures.StructOperations
{
    class StructCompareOperation : StructOperation
    {
        public StructCompareOperation(DataField value1, DataField value2, string operatorSS, bool not = false) : base(OperationType.Compare, DATA_TYPE.BINARIO)
        {
            Value1 = value1;
            Value2 = value2;
            OperatorSS = operatorSS;
            Not= not;
        }
        public bool Not { get; set; }
        public DataField Value1 { get; set; }
        public DataField Value2 { get; set; }
        public string OperatorSS { get; set; }


        public override DataField runStructField()
        {
            return new PrimitiveVariable<bool>(Not ? !(bool)StructOperation.evaluateOperation(OperatorSS, Value1.VariableValue, Value2.VariableValue) : (bool)StructOperation.evaluateOperation(OperatorSS, Value1.VariableValue, Value2.VariableValue), "000");
        }
    }
}
