using System;
using System.Collections.Generic;
using CompiladorDeLenguaje.DataTypes;

namespace CompiladorDeLenguaje.Structures
{
    class StructFuncion : StructField
    {
        public string Identifier { get; set; }
        public List<StructVariableDeclare> Parameters { get; set; }
        public List<StructField> Code { get; set; }
        public List<StructVariableDeclare> LocalVariables { get; set; }
        public DATA_TYPE ReturnType { get; set; }
        public StructFuncion() : base(Struct_Type.Funcion)
        {
            Parameters = new List<StructVariableDeclare>();
            Code = new List<StructField>();
            LocalVariables = new List<StructVariableDeclare>();
        }
        public StructFuncion(string name, DATA_TYPE ReturnType) : base(Struct_Type.Funcion)
        {
            this.Identifier = name;
            this.Parameters = new List<StructVariableDeclare>();
            this.Code = new List<StructField>();
            this.ReturnType = ReturnType;
        }

        public override DataField runStructField()
        {
            foreach (var item in Code)
            {
                DataField return_data= item.runStructField();
                if (return_data.Identifier.Equals("000_RETURN")) return return_data;
            }
            return DataField.Null;
        }
    }
}
