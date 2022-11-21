using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.Structures.StructOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructValueAssignOperation : StructField
    {
        public StructValueAssignOperation(DataField variable, StructOperation operation) : base(Struct_Type.Value_Assign_Operation)
        {
            Variable = variable;
            Operation = operation;
        }

        public DataField Variable { get; set; }
        public StructOperation Operation { get; set; }

        public override DataField runStructField()
        {
            Variable.VariableValue= Operation.runStructField().VariableValue;
            return new DataField("000", DATA_TYPE.NULO);
        }
    }
}
