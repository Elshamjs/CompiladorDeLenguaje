using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    enum Struct_Type
    {
        Funcion, Codigo, Global, Mientras, Para, Si, Value_Assing, Variable_Declare, Funcion_Call, Operation, Value_Assign_Call, System_Call, Value_Assign_Operation, Return, Control
    }
    abstract class StructField
    {
        public StructField(Struct_Type STRUCT_TYPE)
        {
            this.Struct_Type = STRUCT_TYPE;
        }
        abstract public DataField runStructField();

        public Struct_Type Struct_Type { get; set; }  
        
    }
}
