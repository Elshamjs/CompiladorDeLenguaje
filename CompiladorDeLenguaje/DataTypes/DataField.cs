using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    public enum DATA_TYPE
    {
        INT,
        BOOL,
        FLOAT,
        CHAR,
    }
    public enum ARITHMETIC_OPERATOR
    {
        ADD,
        SUBSTRACT,
        MULTIPLY,
        DIVIDE
    }
    public enum LOGIC_OPERATOR
    {
        AND,
        OR
    }
    abstract class DataField
    {
        private int location;
        private string name;
        private DATA_TYPE type;

        protected DataField(int location, string name, DATA_TYPE type)
        {
            this.location = location;
            this.name = name;
            this.type = type;
        }

        public int Location { get => location;}
        public string Name { get => name;  }
        public DATA_TYPE Type { get => type; }
    }
}
