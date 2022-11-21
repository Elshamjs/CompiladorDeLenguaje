using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructReturn : StructField
    {
        public DataField ReturnVariable { get; set; }
        public StructReturn(DataField return_variable) : base(Struct_Type.Return)
        {
            this.ReturnVariable = return_variable;
        }
        public override DataField runStructField()
        {
            return new DataField(ReturnVariable.VariableValue, "000_RETURN", ReturnVariable.Type);
        }
    }
}
