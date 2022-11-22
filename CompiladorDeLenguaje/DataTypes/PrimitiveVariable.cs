using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    class PrimitiveVariable<T> : DataField where T: struct
    {
        public T Value { get => getValue(); set => setValue(value); }

        private void setValue(T value)
        {
            if (ArrayIndex == null)
            {
                this.private_value= (object) value;
            }
            else
            {
                if (DataField.IsBidimensional(ArrayIndex.Array.Type))
                {
                    ((NonPrimitiveMultiArray<T>)ArrayIndex.Array).Validate(ArrayIndex.Index1.Value, ArrayIndex.Index2.Value);
                    ((NonPrimitiveMultiArray<T>)ArrayIndex.Array).Values[ArrayIndex.Index1.Value, ArrayIndex.Index2.Value].Value= value;
                }
                else
                {
                    ((NonPrimitiveArray<T>)ArrayIndex.Array).Validate(ArrayIndex.Index1.Value);
                    ((NonPrimitiveArray<T>)ArrayIndex.Array).Values[ArrayIndex.Index1.Value].Value= value;

                }
            }
        }
        private T getValue()
        {
            if(ArrayIndex==null)
            {
                return (T)this.private_value;
            }
            else
            {
                if(DataField.IsBidimensional(ArrayIndex.Array.Type))
                {
                    ((NonPrimitiveMultiArray<T>)ArrayIndex.Array).Validate(ArrayIndex.Index1.Value, ArrayIndex.Index2.Value);
                    return ((NonPrimitiveMultiArray<T>)ArrayIndex.Array).Values[ArrayIndex.Index1.Value, ArrayIndex.Index2.Value].Value;
                }
                else
                {
                    ((NonPrimitiveArray<T>)ArrayIndex.Array).Validate(ArrayIndex.Index1.Value);
                    return ((NonPrimitiveArray<T>)ArrayIndex.Array).Values[ArrayIndex.Index1.Value].Value;

                }
            }
        }

        public ArrayIndex ArrayIndex { get; set; } = null;

        public PrimitiveVariable(T value, string name, DATA_TYPE TYPE) : base(name, TYPE)
        {
            this.Value = value;
        }

        public PrimitiveVariable(T value, string name) : base(name, DATA_TYPE.NULO)
        {
            this.Value = value;
            if(value.GetType() == typeof(int))
            {
                this.Type = DATA_TYPE.ENTERO;
            }
            else if (value.GetType() == typeof(double))
            {
                this.Type = DATA_TYPE.DECIMAL;
            }
            else if (value.GetType() == typeof(char))
            {
                this.Type = DATA_TYPE.CARACTER;
            }
            else if (value.GetType() == typeof(bool))
            {
                this.Type = DATA_TYPE.BINARIO;
            }
            else
            {
                throw new Exception("Engine Error: DataTypeNotImplementedException | " + name + " => " + value.GetType());
            }
        }
    }
}
