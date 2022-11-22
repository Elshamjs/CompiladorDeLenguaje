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
        public StructValueAssignCall(DataField variable, StructFuncionCall funcionCall, StructSystemCall funcionSysCall = null) : base(Struct_Type.Value_Assign_Call)
        {
            Variable = variable;
            FuncionCall = funcionCall;
            FuncionSysCall = funcionSysCall;
        }

        public DataField Variable { get; set; }
        public StructFuncionCall FuncionCall { get; set; }
        public StructSystemCall FuncionSysCall { get; set; }

        public override DataField runStructField()
        {
            if(FuncionCall!=null)
            {
                Variable.VariableValue = FuncionCall.runStructField().VariableValue;
            }
            else
            {
                Variable.VariableValue = FuncionSysCall.runStructField().VariableValue;
            }
            return DataField.Null;
        }
    }
}
