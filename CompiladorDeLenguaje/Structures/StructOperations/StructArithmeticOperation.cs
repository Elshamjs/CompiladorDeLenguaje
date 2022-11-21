using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures.StructOperations
{
    enum ArithmeticOperands
    {
        DoubleInt,
        IntDouble,
        DoubleDouble,
        IntInt,
    }
    class StructArithmeticOperation : StructOperation
    {
        public StructArithmeticOperation(PrimitiveVariable<int> Value1, PrimitiveVariable<int> Value2, string operatorSS, DATA_TYPE dest_type) : base(OperationType.Arithmetic, dest_type)
        {
            Value1Int = Value1;
            Value2Int = Value2;
            OperatorSS = operatorSS;
            ArithmeticOperands = ArithmeticOperands.IntInt;
        }
        public StructArithmeticOperation(PrimitiveVariable<double> Value1, PrimitiveVariable<int> Value2, string operatorSS, DATA_TYPE dest_type) : base(OperationType.Arithmetic, dest_type)
        {
            Value1Double = Value1;
            Value2Int = Value2;
            OperatorSS = operatorSS;
            ArithmeticOperands = ArithmeticOperands.DoubleInt;
        }

        public StructArithmeticOperation(PrimitiveVariable<int> Value1, PrimitiveVariable<double> Value2, string operatorSS, DATA_TYPE dest_type) : base(OperationType.Arithmetic, dest_type)
        {
            Value1Int = Value1;
            Value2Double = Value2;
            OperatorSS = operatorSS;
            ArithmeticOperands = ArithmeticOperands.IntDouble;
        }
        public StructArithmeticOperation(PrimitiveVariable<double> Value1, PrimitiveVariable<double> Value2, string operatorSS, DATA_TYPE dest_type) : base(OperationType.Arithmetic, dest_type)
        {
            Value1Double = Value1;
            Value2Double = Value2;
            OperatorSS = operatorSS;
            ArithmeticOperands = ArithmeticOperands.DoubleDouble;
        }

        public PrimitiveVariable<int> Value1Int { get; set; }
        public PrimitiveVariable<int> Value2Int { get; set; }
        public PrimitiveVariable<double> Value1Double { get; set; }
        public PrimitiveVariable<double> Value2Double { get; set; }

        public ArithmeticOperands ArithmeticOperands { get; set; }
        public string OperatorSS {get; set; }


        public override DataField runStructField()
        {
            switch(this.ArithmeticOperands)
            {
                case ArithmeticOperands.DoubleDouble:
                    {
                        if(this.DestType==DATA_TYPE.ENTERO)
                        {
                            return new PrimitiveVariable<int>((int)(double)StructOperation.evaluateOperation(OperatorSS, Value1Double.VariableValue, Value2Double.VariableValue), "000");
                        }
                        else
                        {
                            return new PrimitiveVariable<double>((double)StructOperation.evaluateOperation(OperatorSS, Value1Double.VariableValue, Value2Double.VariableValue), "000");
                        }
                    }
                case ArithmeticOperands.IntDouble:
                    {
                        if (this.DestType == DATA_TYPE.ENTERO)
                        {
                            return new PrimitiveVariable<int>((int)(double)StructOperation.evaluateOperation(OperatorSS, Value1Int.VariableValue, Value2Double.VariableValue), "000");
                        }
                        else
                        {
                            return new PrimitiveVariable<double>((double)StructOperation.evaluateOperation(OperatorSS, Value1Int.VariableValue, Value2Double.VariableValue), "000");
                        }
                    }
                case ArithmeticOperands.IntInt:
                    {
                        if (this.DestType == DATA_TYPE.ENTERO)
                        {
                            return new PrimitiveVariable<int>((int)(double)StructOperation.evaluateOperation(OperatorSS, Value1Int.VariableValue, Value2Int.VariableValue), "000");
                        }
                        else
                        {
                            return new PrimitiveVariable<double>((double)StructOperation.evaluateOperation(OperatorSS, Value1Int.VariableValue, Value2Int.VariableValue), "000");
                        }
                    }
                case ArithmeticOperands.DoubleInt:
                    {
                        if (this.DestType == DATA_TYPE.ENTERO)
                        {
                            return new PrimitiveVariable<int>((int)(double)StructOperation.evaluateOperation(OperatorSS, Value1Double.VariableValue, Value2Int.VariableValue), "000");
                        }
                        else
                        {
                            return new PrimitiveVariable<double>((double)StructOperation.evaluateOperation(OperatorSS, Value1Double.VariableValue, Value2Int.VariableValue), "000");
                        }
                    }
                default:
                    {
                        throw new Exception("Error de Ejecucion: No se pudo realizar la convercion de la operacion aritmetica a el tipo de variable de destino");
                    }
            }
        }
    }
}
