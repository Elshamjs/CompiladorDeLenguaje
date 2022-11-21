using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructPara : StructField
    {
        public StructPara(StructVariableDeclare iterator, int to) : base(Struct_Type.Para)
        {
            this.iterator = iterator;
            To = to;
            Code = new List<StructField>();
        }

        public StructVariableDeclare iterator { get; set; }
        public int To { get; set; }
        public List<StructField> Code { get; set; }

        public override DataField runStructField()
        {
            throw new NotImplementedException();
        }
    }
}
