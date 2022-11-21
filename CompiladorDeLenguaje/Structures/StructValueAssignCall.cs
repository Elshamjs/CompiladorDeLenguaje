using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructValueAssignCall : StructField
    {
        public StructValueAssignCall(DataField variable, StructFuncionCall funcionCall) : base(Struct_Type.Value_Assign_Call)
        {
            Variable = variable;
            FuncionCall = funcionCall;
        }

        public DataField Variable { get; set; }
        public StructFuncionCall FuncionCall { get; set; }

        public override DataField runStructField()
        {
            Variable.VariableValue= FuncionCall.runStructField().VariableValue;
            return DataField.Null;
        }
    }
}
