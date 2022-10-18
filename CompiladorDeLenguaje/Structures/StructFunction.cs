using System.Collections.Generic;
using CompiladorDeLenguaje.DataTypes;

namespace CompiladorDeLenguaje.Structures
{
    class StructFunction
    {
        private string name;
        private List<DataField> parameters;
        private List<DataField> variables;
        private int location;
        private string code;
        private DATA_TYPE TYPE;

        public StructFunction(string name, List<DataField> parameters, List<DataField> variables, int location, string code, DATA_TYPE TYPE)
        {
            this.name = name;
            this.parameters = parameters;
            this.variables = variables;
            this.location = location;
            this.code = code;
            this.TYPE = TYPE;
        }

        public string Name { get => name; set => name = value; }
        public int Location { get => location; set => location = value; }
        public string Code { get => code; set => code = value; }
        public DATA_TYPE TYPE1 { get => TYPE; set => TYPE = value; }
        internal List<DataField> Parameters { get => parameters; set => parameters = value; }
        internal List<DataField> Variables { get => variables; set => variables = value; }
    }
}
