using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    class NonPrimitiveArray<T> : DataField where T : struct
    {
        public PrimitiveVariable<T>[] Values { get=> (PrimitiveVariable<T>[])this.VariableValue; set=> renameValue(value); }
        public int Size { get; set; }
        public NonPrimitiveArray(int size, T[] values_array, DATA_TYPE type, string identifier) : base(identifier, type)
        {
            Size = size;
            private_value= new PrimitiveVariable<T>[Valentina.MemorySize];
            for(int i= 0; i< Valentina.MemorySize; i++)
            {
                if(i<values_array.Length) Values[i] = new PrimitiveVariable<T>(values_array[i], identifier + "[" + i + "]");
                else Values[i] = new PrimitiveVariable<T>(new T(), identifier + "[" + i + "]");
            }
        }
        public NonPrimitiveArray(int size, DATA_TYPE type, string identifier) : base(identifier, type)
        {
            Size= size;
            private_value = new PrimitiveVariable<T>[Valentina.MemorySize];
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                Values[i] = new PrimitiveVariable<T>(new T(), identifier + "[" + i + "]");
            }
        }
        public NonPrimitiveArray(DATA_TYPE type, string identifier) : base(identifier, type)
        {
            Size = 0;
            private_value = new PrimitiveVariable<T>[Valentina.MemorySize];
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                Values[i] = new PrimitiveVariable<T>(new T(), identifier + "[" + i + "]");
            }
        }
        public void renameValue(string ss)
        {
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                Values[i]._identifier = ss + "[" + i + "]";
            }
        }
        private void renameValue(PrimitiveVariable<T>[] values)
        {
            Size = values.Length;
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                if (i < values.Length) Values[i].Value = values[i].Value;
                else Values[i].Value = new T();
            }
        }
    }
}
