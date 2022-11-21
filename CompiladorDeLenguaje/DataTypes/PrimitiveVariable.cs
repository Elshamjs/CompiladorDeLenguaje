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
        public T Value { get => (T) this.private_value; set => this.private_value = (object) value; }
        public ArrayIndex<T> ArrayIndex { get; set; } 
        public MultiArrayIndex<T> MultiArrayIndex { get; set; }
        public PrimitiveVariable(T value, string name, DATA_TYPE TYPE) : base(name, TYPE)
        {
            ArrayIndex = new ArrayIndex<T>() { IsArray = false };
            MultiArrayIndex= new MultiArrayIndex<T> {IsMultiArray = false };
            this.Value = value;
        }

        public PrimitiveVariable(T value, string name) : base(name, DATA_TYPE.NULO)
        {
            ArrayIndex = new ArrayIndex<T>() { IsArray = false };
            MultiArrayIndex = new MultiArrayIndex<T> { IsMultiArray = false };
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
