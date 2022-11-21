using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructFuncionCall : StructField
    {
        public StructFuncionCall(StructFuncion funcion, List<DataField> ValueParameters) : base(Struct_Type.Funcion_Call)
        {
            Funcion = funcion;
            this.ValueParameters = ValueParameters;
        }

        public StructFuncion Funcion { get; set; }
        public List<DataField> ValueParameters { get; set; }

        public override DataField runStructField()
        {
            for (int i = 0; i < ValueParameters.Count; i++)
            {
                Funcion.Parameters[i].Variable.VariableValue= ValueParameters[i].VariableValue;
            }
            return Funcion.runStructField(); 
        }
    }
}
