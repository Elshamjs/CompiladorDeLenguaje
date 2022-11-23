using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.Structures.StructOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructPara : StructField
    {
        public StructPara() : base(Struct_Type.Para)
        {
            Code = new List<StructField>();
        }
        public StructVariableDeclare iterator { get; set; }
        public StructCompareOperation CompareOperation { get; set; }
        public StructField StructAssign { get; set; }
        public List<StructField> Code { get; set; }

        public override DataField runStructField()
        {
            DataField ret;
            bool @break=false;
            for (iterator.runStructField(); (bool)CompareOperation.runStructField().VariableValue; StructAssign.runStructField())
            {
                foreach (StructField f in Code)
                {
                    ret =f.runStructField();
                    if (ret.IsBeingReturned) return ret;
                    if (ret.IsBeingContinued) break;
                    if (ret.IsBeingBroken) { @break = true; break; }
                }
                if (@break) break;
            }
            return DataField.Null;
        }
    }
}
