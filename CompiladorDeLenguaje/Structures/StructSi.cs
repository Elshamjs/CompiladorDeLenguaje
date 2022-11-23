using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.Structures.StructOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructSi : StructField
    {
        public StructSi(StructLogicOperation conditionOperation, PrimitiveVariable<bool> conditionSimple) : base(Struct_Type.Si)
        {
            ConditionLogicOperation = conditionOperation;
            ConditionSimple = conditionSimple;
            Code = new List<StructField>();
            ElseCode = new List<StructField>();
        }
        public StructSi() : base(Struct_Type.Si)
        {
            ConditionLogicOperation = null;
            ConditionSimple = null;
            Code = new List<StructField>();
            ElseCode = new List<StructField>();
        }

        public StructLogicOperation ConditionLogicOperation { get; set; }
        public StructCompareOperation ConditionCompareOperation { get; set; }
        public PrimitiveVariable<bool> ConditionSimple { get; set; }
        public List<StructField> Code { get; set; }
        public List<StructField> ElseCode { get; set; }

        public override DataField runStructField()
        {
            DataField ret;
            if (ConditionSimple!=null)
            {
                if ((bool)ConditionSimple.VariableValue)
                {
                    foreach (StructField f in Code)
                    {
                        ret= f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
                else 
                {
                    foreach (StructField f in ElseCode)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
            }
            else if(ConditionLogicOperation!=null)
            {
                if ((bool)ConditionLogicOperation.runStructField().VariableValue)
                {
                    foreach (StructField f in Code)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
                else
                {
                    foreach (StructField f in ElseCode)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
            }
            else
            {
                if ((bool)ConditionCompareOperation.runStructField().VariableValue)
                {
                    foreach (StructField f in Code)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
                else
                {
                    foreach (StructField f in ElseCode)
                    {
                        ret = f.runStructField();
                        if (ret.IsBeingReturned) return ret;
                        if (ret.IsBeingContinued) return ret;
                        if (ret.IsBeingBroken) return ret;
                    }
                }
            }
            return DataField.Null;
        }
    }
}
