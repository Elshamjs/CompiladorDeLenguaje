﻿using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    enum ControlType
    {
        Continue,
        Break
    }
    class StructControl : StructField
    {
        public ControlType ControlType { get; set; }
        public StructControl(ControlType control_type) : base(Struct_Type.Control)
        {
            ControlType = control_type;
        }

        public override DataField runStructField()
        {
            switch(ControlType)
            {
                case ControlType.Continue:
                    {
                        return new DataField(null, "000_CONTINUE", DATA_TYPE.NULO);
                    }
                case ControlType.Break:
                    {
                        return new DataField(null, "000_BREAK", DATA_TYPE.NULO);
                    }
            }
            throw new Exception("Error de Ejecucion: No se pudo resolver una intruccion de control");
        }
    }
}
