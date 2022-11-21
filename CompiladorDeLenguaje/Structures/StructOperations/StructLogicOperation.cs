using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures.StructOperations
{
    class StructLogicOperation : StructOperation
    {
        public StructLogicOperation(PrimitiveVariable<bool> value1, PrimitiveVariable<bool> value2, string operandSS, bool not) : base(OperationType.Logic, DATA_TYPE.BINARIO)
        {
            Value1 = value1;
            Value2 = value2;
            OperandSS = operandSS;
            Not = not;
        }

        public PrimitiveVariable<bool> Value1 { get; set; }
        public PrimitiveVariable<bool> Value2 { get; set; }
        public bool Not { get; set; }
        public string OperandSS { get; set; }

        public override DataField runStructField()
        {
            return new PrimitiveVariable<bool>((Not ? !(bool)StructOperation.evaluateOperation(OperandSS, Value1.VariableValue, Value2.VariableValue) : (bool)StructOperation.evaluateOperation(OperandSS, Value1.VariableValue, Value2.VariableValue)), "000");
        }
    }
}
