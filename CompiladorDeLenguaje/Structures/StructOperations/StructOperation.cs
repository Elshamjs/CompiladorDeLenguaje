using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures.StructOperations
{
    enum OperationType
    {
        Arithmetic, Compare, Logic
    }
    abstract class StructOperation : StructField
    {
        public StructOperation(OperationType operationType, DATA_TYPE dest_type) : base(Struct_Type.Operation)
        {
            OperationType = operationType;
            DestType = dest_type;
        }

        public OperationType OperationType { get; set; }
        public DATA_TYPE DestType { get; set; }

        public static object evaluateOperation(string operator_ss, object value1, object value2)
        {
            switch(operator_ss)
            {
                case "/":
                    {
                        return ((value1.GetType()==typeof(int) ? (int)value1 : (double) value1) / (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "*":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) * (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "+":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) + (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "-":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) - (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "<":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) < (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case ">":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) > (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "<=":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) <= (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case ">=":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) >= (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "==":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) == (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "!=":
                    {
                        return ((value1.GetType() == typeof(int) ? (int)value1 : (double)value1) != (value2.GetType() == typeof(int) ? (int)value2 : (double)value2));
                    }
                case "O":
                    {
                        return (bool)value1 || (bool)value2;
                    }
                case "Y":
                    {
                        return (bool)value1 && (bool)value2;
                    }
                default:
                    {
                        throw new Exception("Error de Ejecucion: No se pudo efectuar la operacion " + "(val" + operator_ss + "val)");
                    }
            }
        }
    }
}
