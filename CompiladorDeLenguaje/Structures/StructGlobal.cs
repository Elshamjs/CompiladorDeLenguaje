using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructGlobal : StructField
    {
        public StructGlobal() : base(Struct_Type.Global)
        {
            this.structVariableDeclares = new List<StructVariableDeclare>();
        }

        public List<StructVariableDeclare> structVariableDeclares { get; set; }

        public override DataField runStructField()
        {
            throw new NotImplementedException();
        }
    }
}
