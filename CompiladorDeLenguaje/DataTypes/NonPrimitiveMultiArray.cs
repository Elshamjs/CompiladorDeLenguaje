using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CompiladorDeLenguaje.DataTypes
{
    class NonPrimitiveMultiArray<T> : DataField where T: struct
    {
        public PrimitiveVariable<T>[,] Values { get => (PrimitiveVariable<T>[,])this.VariableValue; set => renameValue(value); }

        public NonPrimitiveMultiArray(int rows_size, int columns_size, T[,] values_array, DATA_TYPE type, string identifier) : base( identifier, type)
        {
            Rows = rows_size;
            Columns = columns_size;

            private_value = new PrimitiveVariable<T>[Valentina.MemorySize, Valentina.MemorySize];
            for(int i =0; i< Valentina.MemorySize; i++)
            {
                for(int t=0; t< Valentina.MemorySize; t++)
                {
                    if(i<values_array.GetLength(0) && t<values_array.GetLength(1)) Values[i, t] = new PrimitiveVariable<T>(values_array[i,t], identifier + "[" + i + "]" + "[" + t + "]");
                    else Values[i, t] = new PrimitiveVariable<T>(new T(), identifier + "[" + i + "]" + "[" + t + "]");
                }
            }
        }
        public NonPrimitiveMultiArray(DATA_TYPE type, string identifier) : base(identifier, type)
        {
            Rows = 0;
            Columns = 0;
            private_value = new PrimitiveVariable<T>[Valentina.MemorySize, Valentina.MemorySize];
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                for (int t = 0; t < Valentina.MemorySize; t++)
                {
                    Values[i, t] = new PrimitiveVariable<T>(new T(), identifier + "[" + i + "]" + "[" + t + "]");
                }
            }
        }

        public NonPrimitiveMultiArray(int rows_size, int columns_size, DATA_TYPE type, string name) : base(name, type)
        {
            Rows = rows_size;
            Columns = columns_size;
            private_value = new PrimitiveVariable<T>[Valentina.MemorySize, Valentina.MemorySize];
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                for (int t = 0; t < Valentina.MemorySize; t++)
                {
                    Values[i, t] = new PrimitiveVariable<T>(new T(), Identifier + "[" + i + "]" + "[" + t + "]");
                }
            }
        }
        public void Validate(int r, int c)
        {
            if (!IsInitialized) throw new Exception($"Error de Ejecucion: Referencia a Nulo en la variable {Identifier}. La variable {Identifier} era Nulo y no se pudo acceder");
            if (Rows < r || Columns < c || r < 0 || c < 0) throw new Exception($"Error de Ejecucion: Se detecto un especificacion de indices de la variable {Identifier} erronea. El tamaño de {Identifier} no lo permite.");
        }

        public void renameValue(string ss)
        {
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                for (int t = 0; t < Valentina.MemorySize; t++)
                {
                    Values[i, t]._identifier = ss + "[" + i + "]" + "[" + t + "]";
                }
            }
        }

        private object renameValue(PrimitiveVariable<T>[,] values)
        {
            for (int i = 0; i < Valentina.MemorySize; i++)
            {
                for (int t = 0; t < Valentina.MemorySize; t++)
                {
                    if (i < values.GetLength(0) && t < values.GetLength(1)) Values[i, t].Value = values[i, t].Value;
                    else Values[i, t].Value = new T();
                }
            }
            return values;
        }

    }
}
