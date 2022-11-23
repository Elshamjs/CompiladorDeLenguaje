using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructFuncionCall : StructField
    {
        public StructFuncionCall(StructFuncion funcion, List<DataField> ValueParameters) : base(Struct_Type.Funcion_Call)
        {
            Funcion = funcion;
            this.ValueParameters = ValueParameters;
        }

        public StructFuncion Funcion { get; set; }
        public List<DataField> ValueParameters { get; set; }

        public override DataField runStructField()
        {
            Funcion = reMakeAndCopyFuncion(Funcion.Block);
            for (int i = 0; i < ValueParameters.Count; i++)
            {
                Funcion.Parameters[i].Variable.VariableValue= ValueParameters[i].VariableValue;
                Funcion.Parameters[i].Variable.Columns = ValueParameters[i].Columns;
                Funcion.Parameters[i].Variable.Rows = ValueParameters[i].Rows;
                Funcion.Parameters[i].Variable.IsInitialized= true;
            }
            return Funcion.runStructField(); 
        }

        public StructFuncion reMakeAndCopyFuncion(LexemeBlock src_block)
        {
            List<Lexeme> extracted_lexemes = src_block.Lexemes;
            int i = src_block.BlockStart;
            Lexeme this_lexeme = extracted_lexemes[i];
            StructFuncion funcion_declare = new StructFuncion();
            i++; this_lexeme = extracted_lexemes[i];
            funcion_declare.ReturnType = Valentina.getDataType(extracted_lexemes, ref i, "000").Type;
            i++; this_lexeme = extracted_lexemes[i];
            if (this_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Sintaxis: Se debe especificar un identificador para cada funcion");
            funcion_declare.Identifier = this_lexeme.Text;
            i++; this_lexeme = extracted_lexemes[i];
            if (!this_lexeme.Text.Equals("(")) throw new Exception("Error de Sintaxis: Los parametros de la funcion inician con (");
            i++; this_lexeme = extracted_lexemes[i];
            DataField param_var;
            string param_name = "";
            while (!this_lexeme.Text.Equals(")"))
            {
                if (this_lexeme.WhatKind != LexemeKind.DataType) throw new Exception("Error de Sintaxis: El compilador no pudo reconocer el tipo de dato del parametro de la funcion");
                param_var = Valentina.getDataType(extracted_lexemes, ref i, param_name);
                if (param_var.Type == DATA_TYPE.NULO) throw new Exception("Error de Semantica: Una variable declarada Nulo no es permitido");
                i++; this_lexeme = extracted_lexemes[i];
                if (this_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Sintaxis: El compilador no pudo reconocer el identificador del parametro de la funcion");
                param_name = this_lexeme.Text;
                param_var.Identifier = param_name;
                if (Valentina.GlobalDeclareEngine.structVariableDeclares.Where(entry => entry.Identifier.Equals(param_name)).Any()) throw new Exception("Error de Semantica: La variable " + param_name + " ya esta declarada como global, no se puede usar como parametro");
                i++; this_lexeme = extracted_lexemes[i];
                if (this_lexeme.Text.Equals(","))
                {
                    i++; this_lexeme = extracted_lexemes[i];
                }
                StructVariableDeclare _param = new StructVariableDeclare(param_var, param_name);
                funcion_declare.Parameters.Add(_param);
                funcion_declare.LocalVariables.Add(_param);
            }
            i++; this_lexeme = extracted_lexemes[i];
            if (!this_lexeme.Text.Equals("{")) throw new Exception("Error de Sintaxis: Despues de una funcion se necesita especificar el codigo con \"{\"");
            i--;
            funcion_declare.Block = src_block;
            Valentina.CurrentFuncion = funcion_declare;
            Valentina.getRunCode(funcion_declare.Code, extracted_lexemes, ref i, funcion_declare.LocalVariables);
            Valentina.CurrentFuncion = null;
            return funcion_declare;
        }
    }
}
