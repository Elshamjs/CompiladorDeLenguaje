using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.Structures.StructOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructMientras : StructField
    {
        public StructMientras(PrimitiveVariable<bool> condition, StructCompareOperation condition_operation) : base(Struct_Type.Mientras)
        {
            this.ConditionOperation = condition_operation;
            this.Condition = condition;
            this.Code = new List<StructField>();
        }

        public StructCompareOperation ConditionOperation { get; set; }
        public PrimitiveVariable<bool> Condition { get; set; }
        public List<StructField> Code { get; set; }

        public override DataField runStructField()
        {
            DataField ret;
            bool @break= false;
            if (ConditionOperation == null)
            {
                while ((bool)Condition.VariableValue)
                {
                    foreach (StructField f in Code)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingContinued) break;
                        if (ret.IsBeingBroken) { @break = true; break; }
                        if (ret.IsBeingReturned) { return ret; }
                    }
                    if (@break) break;
                }
            }
            else
            {
                while((bool)ConditionOperation.runStructField().VariableValue)
                {
                    foreach(StructField f in Code)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingContinued) break;
                        if (ret.IsBeingBroken) { @break = true; break; }
                        if (ret.IsBeingReturned) { return ret; }
                    }
                    if (@break) break;
                }
            }
            return DataField.Null;
        }
    }
}
