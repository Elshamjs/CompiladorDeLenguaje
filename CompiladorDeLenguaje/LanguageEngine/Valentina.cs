using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.Structures;
using CompiladorDeLenguaje.Structures.StructOperations;
using CompiladorDeLenguaje.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CompiladorDeLenguaje.LanguageEngine
{
    static class Valentina
    {
        static public int MemorySize = 100;
        static public IEnumerable<string> logic_operators = new List<string>() {"Y", "O", "(No)" };/**/
        static public IEnumerable<string> compare_operators = new List<string>() { "<", ">", ">=", "<=", "==", "!=" };/**/
        static public IEnumerable<string> key_comments = new List<string>() { "\\~", "~\\" };/**/
        static public IEnumerable<string> arithmetic_operators = new List<string>() { "/", "*", "+", "-" };/**/
        static public IEnumerable<string> data_types = new List<string>() { "Entero", "Decimal", "Binario", "Caracter", "Nulo" };/**/
        static public IEnumerable<string> round_brackets = new List<string>() { "(", ")" };/**/
        static public IEnumerable<string> curly_brackets = new List<string>() { "{", "}" };/**/
        static public IEnumerable<string> key_character = new List<string>() { "#", "$", "?", "¿", "=", ";", "\"", ","};/**/
        static public IEnumerable<string> square_brackets = new List<string>() { "[", "]" };/**/
        static public IEnumerable<string> boolvalues_brackets = new List<string>() { "Verdadero", "Falso" };/**/
        static public IEnumerable<string> key_word = new List<string>() { "Mientras", "Para", "Si", "Retorna", "Funcion", "Global:", "Codigo:", "Sino", "Hasta", "Sale", "Continua", "Hace", "Asigna", "SoloSi" };/**/
        static public IEnumerable<string> system_fuctions = new List<string>() { "Mostrar", "Capturar", "LimpiarConsola", "SaltoLinea", "MostrarLn" };/**/
        static public List<StructVariableDeclare> LocalDeclarations = new List<StructVariableDeclare>();
        static public Regex regex_word = new Regex("^[a-zA-Z_]");
        static public Regex regex_numeric = new Regex("^[0-9.]");
        static public Regex regex_string = new Regex("^\".*\"$");
        static public Regex regex_char = new Regex("^'.+'$");
        static public StructFuncion CurrentFuncion = null;
        static public bool IsCodePaused = true;
        static public bool CodeCompiled = false;
        static public bool ProgramRunning = false;
        static public string CaptureString = "";
        static public bool CapturingUserInput= false;
        static public RichTextBox Output=null;
        static public RichTextBox Input=null;
        static public MainForm Window;
        static public List<Lexeme> extracted_lexemes = null;
        static public bool User_Writing=false;
        static public string User_Input = string.Empty;
        static public StructGlobal GlobalDeclareEngine= new StructGlobal();
        static public List<StructField> CodigoDeclareEngine = new List<StructField>();
        static public List<StructFuncion> FuncionDeclareEngine= new List<StructFuncion>();



        static public void compileProgram()
        {
            try
            {
                if (CodeCompiled) throw new Exception("El programa ya ha sido compilado, presione ejecutar para inciar la ejecucion del programa.");
                if (Output == null) throw new Exception("Engine Error: OutputNotSetException");
                if (Window == null) throw new Exception("Engine Error: WindowNotSetException");
                Output.Clear();
                startSyntacticAnalysis();
                startSemanticAnalysis();
                CodeCompiled = true;
                MessageBox.Show("Codigo Compilado con exito");
            }
            catch (NullReferenceException)
            {
                Valentina.Window.syncAppendToOutput("\nEl programa no se pudo compilar...");
                Window.ProgramFails(new Exception("Error de Compilacion: Alguna variable No-Primitiva no se ha inicializado y se intento acceder a sus valores desde Nulo"));
                ProgramRunning = false;
                CodeCompiled = false;
                extracted_lexemes = null;
                CodigoDeclareEngine.Clear();
                GlobalDeclareEngine = new StructGlobal();
                FuncionDeclareEngine.Clear();
                Valentina.Window.syncReadOnlyInput(false);
            }
            catch (Exception ex)
            {
                Valentina.Window.syncAppendToOutput("\nEl programa no se pudo compilar...");
                Window.ProgramFails(ex);
                ProgramRunning = false;
                CodeCompiled = false;
                extracted_lexemes = null;
                CodigoDeclareEngine.Clear();
                GlobalDeclareEngine = new StructGlobal();
                FuncionDeclareEngine.Clear();
                Valentina.Window.syncReadOnlyInput(false);
            }
        }
        static public void runProgram()
        {
            if (!CodeCompiled) throw new Exception("Engine Error: CodeWithoutComplilationException");
            ProgramRunning = true;
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    Valentina.Window.syncReadOnlyInput(true);
                    ExecuteCodeBlock(CodigoDeclareEngine);
                    Valentina.Window.syncAppendToOutput("\nEl programa a finalizado sin errores...");
                }
                catch(NullReferenceException)
                {
                    Valentina.Window.syncAppendToOutput("\nEl programa a finalizado con errores...");
                    Window.ProgramFails(new Exception("Error de Ejecucion: Alguna variable No-Primitiva no se ha inicializado. Se ha intentado acceder a Nulo desde una variable"));
                }
                catch(Exception ex)
                {
                    Valentina.Window.syncAppendToOutput("\nEl programa a finalizado con errores...");
                    Window.ProgramFails(ex);
                }
                ProgramRunning = false;
                CodeCompiled = false;
                extracted_lexemes = null;
                CodigoDeclareEngine.Clear();
                GlobalDeclareEngine = new StructGlobal();
                FuncionDeclareEngine.Clear();
                Valentina.Window.syncReadOnlyInput(false);
            })).Start();
        }
        static private StructVariableDeclare getVariableDeclare(ref int i, List<StructVariableDeclare> src_local_declares)
        {
            Lexeme this_lexeme= extracted_lexemes[i];
            DataField start_value = null;
            DataField variable_declare = null;
            StructVariableDeclare aux = null;
            string identifier = string.Empty;
            bool negative = false;
            variable_declare = getDataType(extracted_lexemes, ref i, "000");
            i++; this_lexeme = extracted_lexemes[i];
            if (this_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Sintaxis: Un identificador procede despues del tipo de dato para declarar variables");
            identifier = this_lexeme.Text;
            i++; this_lexeme = extracted_lexemes[i];
            if (this_lexeme.Text.Equals("="))
            {
                i++; this_lexeme = extracted_lexemes[i];
                if (this_lexeme.Text.Equals("-")) { negative = true; i++; this_lexeme = extracted_lexemes[i]; }
                else negative = false;
                start_value = getVariableValueFromLexeme(extracted_lexemes, ref i, src_local_declares);
                if (!start_value.Identifier.Equals("000") && negative) throw new Exception("Error de Semantica: Solo las constantes pueden tener signo negativo");
                if ((!(start_value.Type == DATA_TYPE.ENTERO || start_value.Type == DATA_TYPE.DECIMAL) && negative)) throw new Exception("Error de Semantica: Unicamente los numeros pueden tener signo negativo");
                if (variable_declare.Type != start_value.Type) throw new Exception("Error de Semantica: Asignacion invalida, los tipos de dato deben ser el mismo");
                if ((start_value.Type == DATA_TYPE.ENTERO || start_value.Type == DATA_TYPE.DECIMAL) && negative)
                {
                    if (start_value.Type == DATA_TYPE.ENTERO)
                    {
                        ((PrimitiveVariable<int>)start_value).Value = ((PrimitiveVariable<int>)start_value).Value * -1;
                    }
                    else
                    {
                        ((PrimitiveVariable<double>)start_value).Value = ((PrimitiveVariable<double>)start_value).Value * -1;
                    }
                }
                aux = new StructVariableDeclare(variable_declare, identifier, start_value);
                aux.Variable.Identifier = identifier;
                i++; this_lexeme = extracted_lexemes[i];
                if (!this_lexeme.Text.Equals(";")) throw new Exception("Error de Sintaxis: Terminacion de linea falta despues de declaracion de variable");
                i++; this_lexeme = extracted_lexemes[i];
            }
            else
            {
                if (!this_lexeme.Text.Equals(";")) throw new Exception("Error de Sintaxis: Terminacion de linea falta despues de declaracion de variable");
                i++; this_lexeme = extracted_lexemes[i];
                aux = new StructVariableDeclare(variable_declare, identifier, start_value);
                aux.Variable.Identifier = identifier;
            }
            return aux;
        }

        static public void findVariable(ref DataField dest, List<StructVariableDeclare> src_declares,  string identifier, bool validate)
        {
            foreach (StructVariableDeclare declare in src_declares)
            {
                if (identifier.Equals(declare.Identifier))
                {
                    dest = declare.Variable;
                    break;
                }
            }
            if (dest == null)
            {
                foreach (StructVariableDeclare declare in GlobalDeclareEngine.structVariableDeclares)
                {
                    if (identifier.Equals(declare.Identifier))
                    {
                        dest = declare.Variable;
                        break;
                    }
                }
                if (dest == null)
                {
                    if (!validate) return;
                    throw new Exception("Error de Semantica: La variable " + identifier + " no existe en el contexto actual o es una variable local de otro bloque de codigo");
                }
            }
        }
        static public DataField getVariableValueFromLexeme(List<Lexeme> lexemes, ref int i, List<StructVariableDeclare> src_declares)
        {
            Lexeme current_lexeme = lexemes[i];
            switch(current_lexeme.WhatKind)
            {
                case LexemeKind.BooleanValue:
                    {
                        return new PrimitiveVariable<bool>(current_lexeme.Text.Equals("Verdadero"), "000", DATA_TYPE.BINARIO);
                    }
                case LexemeKind.Numeric:
                    {
                        if (current_lexeme.Text.Contains(".")) //es un decimal
                        {
                            double val;
                            if(double.TryParse(current_lexeme.Text, out val))
                            {
                                return new PrimitiveVariable<double>(val, "000", DATA_TYPE.DECIMAL);
                            }
                            else
                            {
                                throw new Exception("Error de Sintaxis: El formato de tipo Decimal no es correcto");
                            }
                        }
                        else //es un entero-
                        {
                            int val;
                            if (int.TryParse(current_lexeme.Text, out val))
                            {
                                return new PrimitiveVariable<int>(val, "000", DATA_TYPE.ENTERO);
                            }
                            else
                            {
                                throw new Exception("Error de Sintaxis: El formato de tipo Entero no es correcto");
                            }
                        }
                    }
                case LexemeKind.String:
                    {
                        return new NonPrimitiveArray<char>(current_lexeme.Text.Length-2, current_lexeme.Text.Replace("\"", string.Empty).ToCharArray(), DATA_TYPE.ARRAY_CARACTER, "000");
                    }
                case LexemeKind.Char:
                    {
                        char val;
                        if(char.TryParse(current_lexeme.Text.Replace("'", string.Empty), out val))
                        {
                            return new PrimitiveVariable<char>(val, "000", DATA_TYPE.CARACTER);
                        }
                        else
                        {
                            throw new Exception("Error de Sintaxis: El formato de tipo Caracter no es correcto");
                        }
                    }
                case LexemeKind.DataType:
                    {
                        string data_type = string.Empty;
                        data_type = current_lexeme.Text;
                        string index_ss = string.Empty;
                        int column = 0;
                        int row = 0;
                        i++; current_lexeme = lexemes[i];
                        for (int x = i; (lexemes[x].WhatKind == LexemeKind.Numeric || lexemes[x].WhatKind == LexemeKind.SquareBrackets); x++)
                        {
                            index_ss += lexemes[x].Text;
                        }
                        if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])(\\[[0-9]+\\])"))
                        {
                            row = int.Parse(lexemes[i + 1].Text);
                            column = int.Parse(lexemes[i + 4].Text);
                            i += 5; current_lexeme = lexemes[i];
                            switch(data_type)
                            {
                                case "Entero":
                                    {
                                        return new NonPrimitiveMultiArray<int>(row, column, DATA_TYPE.BIARRAY_ENTERO, "000");
                                    }
                                case "Decimal":
                                    {
                                        return new NonPrimitiveMultiArray<double>(row, column, DATA_TYPE.BIARRAY_DECIMAL, "000");
                                    }
                                case "Caracter":
                                    {
                                        return new NonPrimitiveMultiArray<char>(row, column, DATA_TYPE.BIARRAY_CARACTER, "000");
                                    }
                                case "Binario":
                                    {
                                        return new NonPrimitiveMultiArray<bool>(row, column, DATA_TYPE.BIARRAY_BINARIO, "000");
                                    }
                                default:
                                    {
                                        throw new Exception("Error de Sintaxis: No se reconoce el tipo de dato que se utilizo para iniciar un arreglo bidimensional");
                                    }
                            }
                        }
                        else if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])"))
                        {
                            column = int.Parse(lexemes[i + 1].Text);
                            i += 2; current_lexeme = lexemes[i];
                            switch (data_type)
                            {
                                case "Entero":
                                    {
                                        return new NonPrimitiveArray<int>(column, DATA_TYPE.ARRAY_ENTERO, "000");
                                    }
                                case "Decimal":
                                    {
                                        return new NonPrimitiveArray<double>(column, DATA_TYPE.ARRAY_DECIMAL, "000");
                                    }
                                case "Caracter":
                                    {
                                        return new NonPrimitiveArray<char>(column, DATA_TYPE.ARRAY_CARACTER, "000");
                                    }
                                case "Binario":
                                    {
                                        return new NonPrimitiveArray<bool>(column, DATA_TYPE.ARRAY_BINARIO, "000");
                                    }
                                default:
                                    {
                                        throw new Exception("Error de Sintaxis: No se reconoce el tipo de dato que se utilizo para iniciar un arreglo bidimensional");
                                    }
                            }
                        }
                        else
                        {
                            throw new Exception("Error de Sintaxis: No se pudo reconocer el constructor del arreglo");
                        }
                    }
                case LexemeKind.Identifier:
                    {
                        DataField var = null;
                        findVariable(ref var, src_declares, current_lexeme.Text, true);
                        i++; current_lexeme = lexemes[i];
                        string index_ss = string.Empty;
                        int column = 0;
                        int row = 0;
                        for (int x = i; (lexemes[x].WhatKind == LexemeKind.Numeric || lexemes[x].WhatKind == LexemeKind.SquareBrackets); x++)
                        {
                            index_ss += lexemes[x].Text;
                        }
                        if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])(\\[[0-9]+\\])"))
                        {
                            row = int.Parse(lexemes[i + 1].Text);
                            column = int.Parse(lexemes[i + 4].Text);
                            i += 5; current_lexeme = lexemes[i];
                            try
                            {
                                switch (var.Type)
                                {
                                    case DATA_TYPE.BIARRAY_BINARIO:
                                        {
                                            return ((NonPrimitiveMultiArray<bool>)var).Values[row, column];
                                        }
                                    case DATA_TYPE.BIARRAY_CARACTER:
                                        {
                                            return ((NonPrimitiveMultiArray<char>)var).Values[row, column];
                                        }
                                    case DATA_TYPE.BIARRAY_ENTERO:
                                        {
                                            return ((NonPrimitiveMultiArray<int>)var).Values[row, column];
                                        }
                                    case DATA_TYPE.BIARRAY_DECIMAL:
                                        {
                                            return ((NonPrimitiveMultiArray<double>)var).Values[row, column];
                                        }
                                    default:
                                        {
                                            throw new Exception("Error de Semantica: La variable " + var.Identifier + " no posee indices");
                                        }
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new Exception("Error de Semantica: Los indices especificados para el arreglo bidimensional " + var.Identifier + " son erroneos");
                            }
                        }
                        else if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])"))
                        {
                            column = int.Parse(lexemes[i + 1].Text);
                            i += 2; current_lexeme = lexemes[i];
                            try
                            {
                                switch (var.Type)
                                {
                                    case DATA_TYPE.ARRAY_BINARIO:
                                        {
                                            return ((NonPrimitiveArray<bool>)var).Values[column];
                                        }
                                    case DATA_TYPE.ARRAY_CARACTER:
                                        {
                                            return ((NonPrimitiveArray<char>)var).Values[column];
                                        }
                                    case DATA_TYPE.ARRAY_ENTERO:
                                        {
                                            return ((NonPrimitiveArray<int>)var).Values[column];
                                        }
                                    case DATA_TYPE.ARRAY_DECIMAL:
                                        {
                                            return ((NonPrimitiveArray<double>)var).Values[column];
                                        }
                                    default:
                                        {
                                            throw new Exception("Error de Semantica: La variable " + var.Identifier + " no posee indices");
                                        }
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                throw new Exception("Error de Semantica: Los indices especificados para el arreglo " + var.Identifier + " son erroneos");
                            }
                        }
                        i--;
                        return var;
                    }
                default:
                    {
                        throw new Exception("Error de Sintaxis: El compilador no pudo reconocer el tipo de dato especificado");
                    }
            }
        }
        static public List<StructField> getRunCode(List<StructField> dest_code, List<Lexeme> lexemes, ref int i, List<StructVariableDeclare> _src_local_variables)
        {
            List<StructVariableDeclare> src_local_variables= new List<StructVariableDeclare>(_src_local_variables);
            i++; Lexeme current_lexeme = lexemes[i];//{
            if (current_lexeme.WhatKind == LexemeKind.CurlyBrackets) 
            {
                i++; current_lexeme = lexemes[i];//#
                while (current_lexeme.WhatKind != LexemeKind.CurlyBrackets)
                {
                    if (current_lexeme.WhatKind == LexemeKind.KeyCharacter)
                    {
                        if (current_lexeme.Text.Equals("#"))
                        {
                            i++; current_lexeme = lexemes[i];//#
                            if (current_lexeme.WhatKind == LexemeKind.SystemFuctions)
                            {
                                string Funcion_name= current_lexeme.Text;
                                i++; current_lexeme = lexemes[i];//#
                                if (current_lexeme.WhatKind == LexemeKind.RoundBrackets)
                                {
                                    i++; current_lexeme = lexemes[i];//#
                                    List<DataField> parameters = new List<DataField>();
                                    while (current_lexeme.WhatKind != LexemeKind.RoundBrackets)
                                    {
                                        if (current_lexeme.WhatKind == LexemeKind.Identifier)
                                        {
                                            DataField found_variable = null;
                                            findVariable(ref found_variable, src_local_variables, current_lexeme.Text, true);
                                            int column = 0;
                                            int row = 0;
                                            i++; current_lexeme = lexemes[i];
                                            string index_ss = string.Empty;
                                            for(int x= i; (lexemes[x].WhatKind == LexemeKind.Numeric || lexemes[x].WhatKind == LexemeKind.SquareBrackets); x++)
                                            {
                                                index_ss += lexemes[x].Text;
                                            }
                                            if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])(\\[[0-9]+\\])"))
                                            {
                                                row = int.Parse(lexemes[i + 1].Text);
                                                column = int.Parse(lexemes[i + 4].Text);
                                                try
                                                {
                                                    switch(found_variable.Type)
                                                    {
                                                        case DATA_TYPE.BIARRAY_BINARIO:
                                                            {
                                                                parameters.Add(((NonPrimitiveMultiArray<bool>)found_variable).Values[row, column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.BIARRAY_CARACTER:
                                                            {
                                                                parameters.Add(((NonPrimitiveMultiArray<char>)found_variable).Values[row, column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.BIARRAY_ENTERO:
                                                            {
                                                                parameters.Add(((NonPrimitiveMultiArray<int>)found_variable).Values[row, column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.BIARRAY_DECIMAL:
                                                            {
                                                                parameters.Add(((NonPrimitiveMultiArray<double>)found_variable).Values[row, column]);
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                throw new Exception("Error de Semantica: La variable " + found_variable.Identifier + " no posee indices");
                                                            }
                                                    }
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                    throw new Exception("Error de Semantica: Los indices especificados para el arreglo bidimensional " + found_variable.Identifier + " son erroneos");
                                                }
                                                i += 6; current_lexeme = lexemes[i];
                                            }
                                            else if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])"))
                                            {
                                                column = int.Parse(lexemes[i + 1].Text);
                                                try
                                                {
                                                    switch (found_variable.Type)
                                                    {
                                                        case DATA_TYPE.ARRAY_BINARIO:
                                                            {
                                                                parameters.Add(((NonPrimitiveArray<bool>)found_variable).Values[column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.ARRAY_CARACTER:
                                                            {
                                                                parameters.Add(((NonPrimitiveArray<char>)found_variable).Values[column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.ARRAY_ENTERO:
                                                            {
                                                                parameters.Add(((NonPrimitiveArray<int>)found_variable).Values[column]);
                                                                break;
                                                            }
                                                        case DATA_TYPE.ARRAY_DECIMAL:
                                                            {
                                                                parameters.Add(((NonPrimitiveArray<double>)found_variable).Values[column]);
                                                                break;
                                                            }
                                                        default:
                                                            {
                                                                throw new Exception("Error de Semantica: La variable " + found_variable.Identifier + " no posee indices");
                                                            }
                                                    }
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                    throw new Exception("Error de Semantica: Los indices especificados para el arreglo " + found_variable.Identifier + " son erroneos");
                                                }
                                                i += 3; current_lexeme = lexemes[i];
                                            }
                                            else
                                            {
                                                parameters.Add(found_variable);
                                            }
                                        }
                                        else if (current_lexeme.WhatKind == LexemeKind.String)
                                        {
                                            parameters.Add(new NonPrimitiveArray<char>(current_lexeme.Text.Count() - 2, current_lexeme.Text.Replace("\"", String.Empty).ToArray(), DATA_TYPE.ARRAY_CARACTER, "000"));
                                            i++; current_lexeme = lexemes[i];
                                        }
                                        else if (current_lexeme.WhatKind == LexemeKind.Numeric)
                                        {
                                            if (current_lexeme.Text.Contains("."))
                                            {
                                                parameters.Add(new PrimitiveVariable<double>(double.Parse(current_lexeme.Text), "000", DATA_TYPE.DECIMAL));
                                            }
                                            else
                                            {
                                                parameters.Add(new PrimitiveVariable<int>(int.Parse(current_lexeme.Text), "000", DATA_TYPE.ENTERO));
                                            }
                                            i++; current_lexeme = lexemes[i];
                                        }
                                        else
                                        {
                                            throw new Exception("Error de sintaxis en funcion" + Funcion_name);
                                        }
                                        if (current_lexeme.Text.Equals(","))
                                        {
                                            i++; current_lexeme = lexemes[i];//#
                                        }
                                        else if (current_lexeme.Text.Equals(")")) break;
                                        else
                                        {
                                            throw new Exception("Error de Sintaxis: Los parametros deben estar separados por comas");
                                        }
                                    }
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text.Equals(";"))
                                    {
                                        switch(Funcion_name)
                                        {
                                            case "Mostrar":
                                                {
                                                    dest_code.Add(new StructSystemCall("Mostrar", parameters));
                                                    break;
                                                }
                                            case "Capturar":
                                                {
                                                    dest_code.Add(new StructSystemCall("Capturar", parameters));
                                                    break;
                                                }
                                            case "LimpiarConsola":
                                                {
                                                    dest_code.Add(new StructSystemCall("LimpiarConsola", parameters));
                                                    break;
                                                }
                                            case "SaltoLinea":
                                                {
                                                    dest_code.Add(new StructSystemCall("SaltoLinea", parameters));
                                                    break;
                                                }
                                            case "MostrarLn":
                                                {
                                                    dest_code.Add(new StructSystemCall("MostrarLn", parameters));
                                                    break;
                                                }
                                            default:
                                                {
                                                    throw new Exception("Error de Sintaxis: La funcion " + Funcion_name + " no existe");
                                                }
                                        }
                                        i++; current_lexeme = lexemes[i];//#
                                    }
                                    else
                                    {
                                        throw new Exception("Error de Sintaxis: Los finales de linea deben acabar en ; ");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error de sintaxis en funcion " + Funcion_name);
                                }
                            }
                            if(current_lexeme.WhatKind==LexemeKind.Identifier)
                            {
                                string func_name = current_lexeme.Text;
                                i++; current_lexeme = lexemes[i];
                                if (!current_lexeme.Text.Equals("(")) throw new Exception("Error de Sintaxis: Los parametros de la funcion inician con (");
                                i++; current_lexeme = lexemes[i];
                                List<DataField> data_params = new List<DataField>();
                                while (!current_lexeme.Text.Equals(")"))
                                {
                                    data_params.Add(getVariableValueFromLexeme(lexemes, ref i, src_local_variables));
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text.Equals(","))
                                    {
                                        i++; current_lexeme = lexemes[i];
                                    }
                                }
                                i++; current_lexeme = lexemes[i];
                                if (!current_lexeme.Text.Equals(";")) throw new Exception("Error de Sintaxis: Final de Linea invalido.");
                                List<DATA_TYPE> types_params = new List<DATA_TYPE>();
                                foreach (var item in data_params)
                                {
                                    types_params.Add(item.Type);
                                }
                                StructFuncion aux= findFuncion(func_name, types_params);
                                if (aux == null) throw new Exception("Error de Semantica: La funcion llamada " + func_name + " no existe o ninguna de sus sobrecargas corresponde con los parametros ingresados");
                                dest_code.Add(new StructFuncionCall(aux, data_params));
                                i++; current_lexeme = lexemes[i];
                            }
                        }
                        else if (current_lexeme.Text.Equals("$"))
                        {
                            dest_code.Add(findStructVariableAssign(lexemes, ref i, src_local_variables, ref current_lexeme));
                        }
                        else
                        {
                            throw new Exception("Error de Sintaxis: Inicio de linea invalida");
                        }
                    }
                    else if (current_lexeme.WhatKind == LexemeKind.DataType)
                    {
                        StructVariableDeclare declare = getVariableDeclare(ref i, src_local_variables);
                        if (declare == null)
                        {
                            throw new Exception("Error de Sintaxis: La sintaxis en la declaracion de variables locales no es correcta");
                        }
                        else
                        {
                            DataField dataField = null; //Lo siguiente es para saber si la variable ya ha sido creada antes
                            findVariable(ref dataField, src_local_variables, declare.Identifier, false);
                            if(dataField!=null) throw new Exception("Error de Semantica: La variable " + declare.Identifier + " ya ha sido declarada, no se puede volver a declarar de nuevo");
                            src_local_variables.Add(declare);
                            dest_code.Add(declare);
                            current_lexeme = lexemes[i];
                        }
                    }
                    else if(current_lexeme.WhatKind == LexemeKind.KeyWord)
                    {
                        switch(current_lexeme.Text)
                        {
                            case "Retorna":
                                {
                                    if (CurrentFuncion == null) throw new Exception("Error de Semantica: No se puede utilizar la palabra clave Retorna si no se encuentra dentro de una funcion");
                                    i++; current_lexeme = lexemes[i];
                                    DataField return_data = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                    if (return_data.Type != CurrentFuncion.ReturnType) throw new Exception("Error de Semantica: El funcion " + CurrentFuncion.Identifier + " solo puede devolver variables de tipo " + DataField.DataTypeToString(CurrentFuncion.ReturnType));
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals(";")) throw new Exception("Error de Sintaxis: El final de linea debe acabar con \";\"");
                                    dest_code.Add(new StructReturn(return_data));
                                    i++; current_lexeme = lexemes[i];
                                    break;
                                }
                            case "Si": //Si((a Y B)) Hace {} Sino {} || Si(a) Hace {} 
                                {
                                    StructSi si_struct = new StructSi();
                                    DataField variable1;
                                    DataField variable2;
                                    bool not=false;
                                    string logic_operator= string.Empty;
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals("(")) throw new Exception("Error de Sintaxis: Despues de la palabra clave \"Si\" procede su condicion. Ej. Si(a)");
                                    i++; current_lexeme = lexemes[i];
                                    if(current_lexeme.Text.Equals("(No)"))
                                    {
                                        not= true;
                                        i++; current_lexeme = lexemes[i];
                                    }
                                    if (current_lexeme.Text.Equals("("))
                                    {
                                        i++; current_lexeme = lexemes[i];
                                        variable1 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (!(variable1.Type == DATA_TYPE.BINARIO || variable1.Type == DATA_TYPE.ENTERO || variable1.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: El cuerpo de un Si solo puede tener variables de tipo Binario, Entero o Decimal");
                                        i++; current_lexeme = lexemes[i];
                                        logic_operator = current_lexeme.Text;
                                        if (!(current_lexeme.WhatKind == LexemeKind.CompareOperator || logic_operator=="Y" || logic_operator=="O")) throw new Exception("Error de Sintaxis: El cuerpo de un Si solo puede tener operadores logicos");
                                        i++; current_lexeme = lexemes[i];
                                        variable2 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (!(variable2.Type == DATA_TYPE.BINARIO || variable2.Type == DATA_TYPE.ENTERO || variable2.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: El cuerpo de un Si solo puede tener variables de tipo Binario, Entero o Decimal");
                                        i++; current_lexeme = lexemes[i];
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: La operacion logica del cuerpo del Si debe cerrar con \")\"");
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: El cuerpo del Si debe cerrar con \")\"");
                                        i++; current_lexeme = lexemes[i];
                                        if((variable1.Type == DATA_TYPE.ENTERO || variable1.Type == DATA_TYPE.DECIMAL) && (variable2.Type == DATA_TYPE.ENTERO || variable2.Type == DATA_TYPE.DECIMAL))
                                        {
                                            si_struct.ConditionCompareOperation = new StructCompareOperation(variable1, variable2, logic_operator, not);
                                        }
                                        else if(variable1.Type == DATA_TYPE.BINARIO && variable2.Type == DATA_TYPE.BINARIO)
                                        {
                                            si_struct.ConditionLogicOperation = new StructLogicOperation((PrimitiveVariable<bool>)variable1, (PrimitiveVariable<bool>)variable2, logic_operator, not);
                                        }
                                        else
                                        {
                                            throw new Exception("Error de Sintaxis: La Operacion del cuerpo del Si es invalida");
                                        }
                                    }
                                    else
                                    {
                                        variable1= getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (variable1.Type != DATA_TYPE.BINARIO) throw new Exception("Error de Semantica: El cuerpo de un Si solo puede tener valores Binario o una operacion que devuelva Binario. Ej Si((a<b))");
                                        i++; current_lexeme = lexemes[i];
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: El cuerpo del Si debe cerrar con \")\"");
                                        si_struct.ConditionSimple = (PrimitiveVariable<bool>)variable1;
                                    }
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals("Hace")) throw new Exception("Error de Sintaxis: El cuerpo del Si debe abrir el bloque de codigo con la palabra reservada Hace");
                                    getRunCode(si_struct.Code, lexemes, ref i, src_local_variables);
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text.Equals("Sino"))
                                    {
                                        getRunCode(si_struct.ElseCode, lexemes, ref i, src_local_variables);
                                        i++; current_lexeme = lexemes[i];
                                    }
                                    current_lexeme = lexemes[i];
                                    dest_code.Add(si_struct);
                                    break;
                                }
                            case "Mientras": //Mientras() Hace {}
                                {
                                    DataField variable1;
                                    DataField variable2;
                                    string compare_operator;
                                    StructMientras struct_mientras= new StructMientras(null, null);
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "(") throw new Exception("Error Sintaxis: Cuerpo de Mientras invalido");
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text.Equals("("))
                                    {
                                        i++; current_lexeme = lexemes[i];
                                        variable1 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (!(variable1.Type == DATA_TYPE.ENTERO || variable1.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: La operacion de comparacion del cuerpo de un Mientras solo puede tener variables de tipo Entero o Decimal");
                                        i++; current_lexeme = lexemes[i];
                                        compare_operator = current_lexeme.Text;
                                        if (current_lexeme.WhatKind!=LexemeKind.CompareOperator) throw new Exception("Error de Sintaxis: El cuerpo de un Mientras solo puede tener operadores logicos");
                                        i++; current_lexeme = lexemes[i];
                                        variable2 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (!(variable1.Type == DATA_TYPE.ENTERO || variable1.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: La operacion de comparacion del cuerpo de un Mientras solo puede tener variables de tipo Entero o Decimal");
                                        i++; current_lexeme = lexemes[i];
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: La operacion comparativa del cuerpo del Mientras debe cerrar con \")\"");
                                        i++; current_lexeme = lexemes[i];
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: El cuerpo del Mientras debe cerrar con \")\"");
                                        i++; current_lexeme = lexemes[i];
                                        struct_mientras.ConditionOperation = new StructCompareOperation(variable1, variable2, compare_operator);
                                    }
                                    else
                                    {
                                        variable1 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables);
                                        if (variable1.Type != DATA_TYPE.BINARIO) throw new Exception("Error de Semantica: El cuerpo de un Mientras solo puede tener valores Binario");
                                        i++; current_lexeme = lexemes[i];
                                        if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: El cuerpo del Si debe cerrar con \")\"");
                                        struct_mientras.Condition = (PrimitiveVariable<bool>)variable1;
                                        i++; current_lexeme = lexemes[i];
                                    }
                                    if (current_lexeme.Text != "Hace") throw new Exception("Error de Sintaxis: Palabra reservada Hace falta despues de cuerpo de Mientras");
                                    getRunCode(struct_mientras.Code, lexemes, ref i, src_local_variables);
                                    i++; current_lexeme = lexemes[i];
                                    dest_code.Add(struct_mientras);
                                    break;
                                }
                            case "Sale":
                                {
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != ";") throw new Exception("Error de Compilacion: Falta el caracter clave \";\" despues de palabra clave Sale");
                                    dest_code.Add(new StructControl(ControlType.Break));
                                    i++; current_lexeme = lexemes[i];
                                    break;
                                }
                            case "Continua":
                                {
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != ";") throw new Exception("Error de Compilacion: Falta el caracter clave \";\" despues de palabra clave Continua");
                                    dest_code.Add(new StructControl(ControlType.Break));
                                    i++; current_lexeme = lexemes[i];
                                    break;
                                }
                            case "Para": //Para(Entero i= 0;SoloSi((i<10)); Asigna $i=(a-2);) Hace {}
                                {
                                    StructPara struct_para = new StructPara();
                                    StructVariableDeclare declare;
                                    StructField asign_struct;
                                    DataField solosi_var1;
                                    DataField solosi_var2;
                                    string compare_operator;
                                    List<StructVariableDeclare> declare_list= new List<StructVariableDeclare>(src_local_variables);
                                    StructCompareOperation compare_operation;
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "(") throw new Exception("Error Sintaxis: Cuerpo de Para invalido, se esperaba \"(\"");
                                    i++; current_lexeme = lexemes[i];
                                    declare = getVariableDeclare(ref i, src_local_variables);
                                    declare_list.Add(declare);
                                    current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "SoloSi") throw new Exception(@"Error de Sintaxis: Falta palabra reservada SoloSi dentro de cuerpo de Para");
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "(") throw new Exception("Error Sintaxis: Cuerpo de Para invalido");
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals("(")) throw new Exception(@"Error de Sintaxis: Se esperaba ""("" despues de palabra reservada SoloSi dentro del cuerpo del Para");
                                    i++; current_lexeme = lexemes[i];
                                    solosi_var1 = getVariableValueFromLexeme(lexemes, ref i, declare_list);
                                    if (!(solosi_var1.Type == DATA_TYPE.ENTERO || solosi_var1.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: La operacion de comparacion del cuerpo de un Para solo puede tener variables de tipo Entero o Decimal");
                                    i++; current_lexeme = lexemes[i];
                                    compare_operator = current_lexeme.Text;
                                    if (current_lexeme.WhatKind != LexemeKind.CompareOperator) throw new Exception("Error de Sintaxis: El cuerpo de un Para solo puede tener operadores logicos");
                                    i++; current_lexeme = lexemes[i];
                                    solosi_var2 = getVariableValueFromLexeme(lexemes, ref i, declare_list);
                                    if (!(solosi_var1.Type == DATA_TYPE.ENTERO || solosi_var1.Type == DATA_TYPE.DECIMAL)) throw new Exception("Error de Semantica: La operacion de comparacion del cuerpo de un Para solo puede tener variables de tipo Entero o Decimal");
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: La operacion comparativa del cuerpo del Para debe cerrar con \")\"");
                                    i++; current_lexeme = lexemes[i];
                                    if (!current_lexeme.Text.Equals(")")) throw new Exception("Error de Sintaxis: El cuerpo del Para debe cerrar con \")\"");
                                    compare_operation = new StructCompareOperation(solosi_var1, solosi_var2, compare_operator);
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != ";") throw new Exception(@"Error de Sintaxis: Se esperaba "";"" dentro del cuerpo del Para");
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "Asigna") throw new Exception(@"Error de Sintaxis: Se espera palabra reservada ""Asigna"" detro de cuerpo de Para");
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "$") throw new Exception(@"Error de Sintaxis: Se esperaba ""$"" antes de asignacion dentro del cuerpo del Para");
                                    asign_struct = findStructVariableAssign(lexemes, ref i, declare_list, ref current_lexeme);
                                    if (current_lexeme.Text != ")") throw new Exception(@"Error de Sintaxis: Se espera "")"" en el cierre de los parametros de Para");
                                    i++; current_lexeme = lexemes[i];
                                    if (current_lexeme.Text != "Hace") throw new Exception(@"Error de Sintaxis: Se espera palabra reservada Hace antes de bloque de codigo");
                                    getRunCode(struct_para.Code, lexemes, ref i, declare_list);
                                    struct_para.StructAssign = asign_struct;
                                    struct_para.iterator = declare;
                                    struct_para.CompareOperation = compare_operation;
                                    dest_code.Add(struct_para);
                                    i++; current_lexeme = lexemes[i];
                                    break;
                                }
                            default:
                                {
                                    throw new Exception($"Error de Compilacion: No se pudo reconocer la palabra {current_lexeme.Text} dentro del contexto actual");
                                }
                        }
                    }
                    else
                    {
                        throw new Exception("Error de Sintaxis: Inicio de linea invalida");
                    }
                }
            }
            else
            {
                throw new Exception("Error de Sintaxis: Inicio de bloque de codigo invalido");
            }
            return dest_code;
        }

        static private StructField findStructVariableAssign(List<Lexeme> lexemes, ref int i, List<StructVariableDeclare> src_local_variables, ref Lexeme current_lexeme)
        {
            StructField ret = null;
            i++; current_lexeme = lexemes[i];
            DataField found_variable = null;
            findVariable(ref found_variable, src_local_variables, current_lexeme.Text, true);
            i++; current_lexeme = lexemes[i];
            int column = 0;
            int row = 0;
            string index_ss = string.Empty;
            for (int x = i; (lexemes[x].WhatKind == LexemeKind.Numeric || lexemes[x].WhatKind == LexemeKind.SquareBrackets); x++)
            {
                index_ss += lexemes[x].Text;
            }
            if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])(\\[[0-9]+\\])"))
            {
                if (!lexemes[i + 6].Text.Equals("=")) throw new Exception("Error de Sintaxis: Signo de asignacion despues de variable faltante");
                row = int.Parse(lexemes[i + 1].Text);
                column = int.Parse(lexemes[i + 4].Text);
                try
                {
                    switch (found_variable.Type)
                    {
                        case DATA_TYPE.BIARRAY_BINARIO:
                            {
                                found_variable = ((NonPrimitiveMultiArray<bool>)found_variable).Values[row, column];
                                break;
                            }
                        case DATA_TYPE.BIARRAY_CARACTER:
                            {
                                found_variable = ((NonPrimitiveMultiArray<char>)found_variable).Values[row, column];
                                break;
                            }
                        case DATA_TYPE.BIARRAY_ENTERO:
                            {
                                found_variable = ((NonPrimitiveMultiArray<int>)found_variable).Values[row, column];
                                break;
                            }
                        case DATA_TYPE.BIARRAY_DECIMAL:
                            {
                                found_variable = ((NonPrimitiveMultiArray<double>)found_variable).Values[row, column];
                                break;
                            }
                        default:
                            {
                                throw new Exception("Error de Semantica: La variable " + found_variable.Identifier + " no posee indices dobles");
                            }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("Error de Semantica: Los indices especificados para el arreglo bidimensional " + found_variable.Identifier + " son erroneos");
                }
                i += 7; current_lexeme = lexemes[i];
            }
            else if (Regex.IsMatch(index_ss, "(\\[[0-9]+\\])"))
            {
                if (!lexemes[i + 3].Text.Equals("=")) throw new Exception("Error de Sintaxis: Signo de asignacion despues de variable faltante");
                column = int.Parse(lexemes[i + 1].Text);
                try
                {
                    switch (found_variable.Type)
                    {
                        case DATA_TYPE.ARRAY_BINARIO:
                            {
                                found_variable = ((NonPrimitiveArray<bool>)found_variable).Values[column];
                                break;
                            }
                        case DATA_TYPE.ARRAY_CARACTER:
                            {
                                found_variable = ((NonPrimitiveArray<char>)found_variable).Values[column];
                                break;
                            }
                        case DATA_TYPE.ARRAY_ENTERO:
                            {
                                found_variable = ((NonPrimitiveArray<int>)found_variable).Values[column];
                                break;
                            }
                        case DATA_TYPE.ARRAY_DECIMAL:
                            {
                                found_variable = ((NonPrimitiveArray<double>)found_variable).Values[column];
                                break;
                            }
                        default:
                            {
                                throw new Exception("Error de Semantica: La variable " + found_variable.Identifier + " no posee indices");
                            }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new Exception("Error de Semantica: Los indices especificados para el arreglo " + found_variable.Identifier + " son erroneos");
                }
                i += 4; current_lexeme = lexemes[i];
            }
            else
            {
                if (!current_lexeme.Text.Equals("=")) throw new Exception("Error de Sintaxis: Signo de asignacion despues de variable faltante");
                i++; current_lexeme = lexemes[i];
            }
            string value = string.Empty;
            int negative = 1;
            bool not = false;
            if (current_lexeme.Text.Equals("-"))
            {
                negative = -1;
                i++; current_lexeme = lexemes[i];
            }
            else if (current_lexeme.Text.Equals("(No)"))
            {
                not = true;
                i++; current_lexeme = lexemes[i];
            }
            if (current_lexeme.Text.Equals("(")) //ENCONTRAR ASIGNACION DE OPERACION INICIA
            {
                if (negative == -1) throw new Exception("Error de Sintaxis: No se puede usar signo negativo para efectuar una operacion, solo el operador logico (No)");
                i++; current_lexeme = lexemes[i];
                if (current_lexeme.Text.Equals("-")) //PODRIA SER NEGATIVO
                {
                    negative = -1;
                    i++; current_lexeme = lexemes[i];
                }
                DataField operand1 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables); //OPERANDO IZQUIERDO
                if (negative == -1 && !operand1.Identifier.Equals("000")) throw new Exception("Error de Sintaxis: Solo las constantes pueden tener signo, las variables no");
                i++; current_lexeme = lexemes[i];
                string operator_ss = current_lexeme.Text;
                switch (current_lexeme.WhatKind)
                {
                    case LexemeKind.ArithmeticOperator:
                        {
                            if (!(found_variable.Type == DATA_TYPE.ENTERO || found_variable.Type == DATA_TYPE.DECIMAL)) throw new Exception("Una operacion aritmetica solo puede asignarse a una variable Entera o Decimal");
                            i++; current_lexeme = lexemes[i];
                            int negative2 = 1;
                            if (current_lexeme.Text.Equals("-")) //PODRIA SER NEGATIVO
                            {
                                negative2 = -1;
                                i++; current_lexeme = lexemes[i];
                            }
                            DataField operand2 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables); //OPERANDO DERECHO
                            if (negative2 == -1 && !operand2.Identifier.Equals("000")) throw new Exception("Error de Sintaxis: Solo las constantes pueden tener signo, las variables no");
                            i++; current_lexeme = lexemes[i];
                            if (current_lexeme.Text.Equals(")")) //FINAL DE LA OPERACION, SE REVISA SINTAXIS Y SEMANTICA
                            {
                                if (!((operand1.Type == DATA_TYPE.ENTERO || operand1.Type == DATA_TYPE.DECIMAL) && (operand2.Type == DATA_TYPE.ENTERO || operand2.Type == DATA_TYPE.DECIMAL))) throw new Exception("Error de Sintaxis: En una operacion aritmerica los operando solo pueden ser Entero o Decimal");
                                if (operand1.Type == DATA_TYPE.ENTERO && operand2.Type == DATA_TYPE.DECIMAL)
                                {
                                    ((PrimitiveVariable<int>)operand1).Value *= negative;//permite cambiar el signo del valor si es necesario
                                    ((PrimitiveVariable<double>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructArithmeticOperation((PrimitiveVariable<int>)operand1, (PrimitiveVariable<double>)operand2, operator_ss, found_variable.Type));
                                }
                                else if (operand1.Type == DATA_TYPE.DECIMAL && operand2.Type == DATA_TYPE.ENTERO)
                                {
                                    ((PrimitiveVariable<double>)operand1).Value *= negative;
                                    ((PrimitiveVariable<int>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructArithmeticOperation((PrimitiveVariable<double>)operand1, (PrimitiveVariable<int>)operand2, operator_ss, found_variable.Type));
                                }
                                else if (operand1.Type == DATA_TYPE.DECIMAL && operand2.Type == DATA_TYPE.DECIMAL)
                                {
                                    ((PrimitiveVariable<double>)operand1).Value *= negative;
                                    ((PrimitiveVariable<double>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructArithmeticOperation((PrimitiveVariable<double>)operand1, (PrimitiveVariable<double>)operand2, operator_ss, found_variable.Type));
                                }
                                else if (operand1.Type == DATA_TYPE.ENTERO && operand2.Type == DATA_TYPE.ENTERO)
                                {
                                    ((PrimitiveVariable<int>)operand1).Value *= negative;
                                    ((PrimitiveVariable<int>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructArithmeticOperation((PrimitiveVariable<int>)operand1, (PrimitiveVariable<int>)operand2, operator_ss, found_variable.Type));
                                }
                            }
                            else
                            {
                                throw new Exception("Error de Sintaxis: Las operaciones deben finalizar con )");
                            }
                            break;
                        }
                    case LexemeKind.LogicOperator:
                        {
                            if (found_variable.Type != DATA_TYPE.BINARIO) throw new Exception("Una operacion logica solo puede ser asignada a una variable Binaria");
                            i++; current_lexeme = lexemes[i];
                            DataField operand2 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables); //OPERANDO DERECHO
                            i++; current_lexeme = lexemes[i];
                            if (current_lexeme.Text.Equals(")")) //FINAL DE LA OPERACION, SE REVISA SINTAXIS Y SEMANTICA
                            {
                                if (!(operand1.Type == DATA_TYPE.BINARIO && operand2.Type == DATA_TYPE.BINARIO)) throw new Exception("Error de Sintaxis: En una operacion logica los operando solo pueden ser Binario");
                                ret = new StructValueAssignOperation(found_variable, new StructLogicOperation((PrimitiveVariable<bool>)operand1, (PrimitiveVariable<bool>)operand2, operator_ss, not));
                            }
                            else
                            {
                                throw new Exception("Error de Sintaxis: Las operaciones deben finalizar con )");
                            }
                            break;
                        }
                    case LexemeKind.CompareOperator: //
                        {
                            if (found_variable.Type != DATA_TYPE.BINARIO) throw new Exception("Una operacion comparativa solo puede ser asignada a una variable Binaria");
                            i++; current_lexeme = lexemes[i];
                            int negative2 = 1;
                            if (current_lexeme.Text.Equals("-")) //PODRIA SER NEGATIVO
                            {
                                negative2 = -1;
                                i++; current_lexeme = lexemes[i];
                            }
                            DataField operand2 = getVariableValueFromLexeme(lexemes, ref i, src_local_variables); //OPERANDO DERECHO
                            i++; current_lexeme = lexemes[i];
                            if (current_lexeme.Text.Equals(")")) //FINAL DE LA OPERACION, SE REVISA SINTAXIS Y SEMANTICA
                            {
                                if (!((operand1.Type == DATA_TYPE.ENTERO || operand1.Type == DATA_TYPE.DECIMAL) && (operand2.Type == DATA_TYPE.ENTERO || operand2.Type == DATA_TYPE.DECIMAL))) throw new Exception("Error de Sintaxis: En una operacion aritmerica los operando solo pueden ser Entero o Decimal");
                                if (operand1.Type == DATA_TYPE.ENTERO && operand2.Type == DATA_TYPE.DECIMAL)
                                {
                                    ((PrimitiveVariable<int>)operand1).Value *= negative;//permite cambiar el signo del valor si es necesario
                                    ((PrimitiveVariable<double>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructCompareOperation((PrimitiveVariable<int>)operand1, (PrimitiveVariable<double>)operand2, operator_ss));
                                }
                                else if (operand1.Type == DATA_TYPE.DECIMAL && operand2.Type == DATA_TYPE.ENTERO)
                                {
                                    ((PrimitiveVariable<double>)operand1).Value *= negative;
                                    ((PrimitiveVariable<int>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructCompareOperation((PrimitiveVariable<double>)operand1, (PrimitiveVariable<int>)operand2, operator_ss));
                                }
                                else if (operand1.Type == DATA_TYPE.DECIMAL && operand2.Type == DATA_TYPE.DECIMAL)
                                {
                                    ((PrimitiveVariable<double>)operand1).Value *= negative;
                                    ((PrimitiveVariable<double>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructCompareOperation((PrimitiveVariable<double>)operand1, (PrimitiveVariable<double>)operand2, operator_ss));
                                }
                                else if (operand1.Type == DATA_TYPE.ENTERO && operand2.Type == DATA_TYPE.ENTERO)
                                {
                                    ((PrimitiveVariable<int>)operand1).Value *= negative;
                                    ((PrimitiveVariable<int>)operand2).Value *= negative2;
                                    ret = new StructValueAssignOperation(found_variable, new StructCompareOperation((PrimitiveVariable<int>)operand1, (PrimitiveVariable<int>)operand2, operator_ss));
                                }
                            }
                            else
                            {
                                throw new Exception("Error de Sintaxis: Las operaciones deben finalizar con )");
                            }
                            break;
                        }
                    default:
                        {
                            throw new Exception("Error de Sintaxis: La operacion no tiene un formato correcto");
                        }
                } //ENCONTRAR ASIGNACION DE OPERACION TERMINA
                i++; current_lexeme = lexemes[i];
                if (current_lexeme.Text.Equals(";"))
                {
                    i++; current_lexeme = lexemes[i];
                }
                else
                {
                    throw new Exception("Error de Sintaxis: Los finales de linea deben acabar con ;");
                }

            }
            else
            {
                value += current_lexeme.Text;
                switch (current_lexeme.WhatKind)
                {
                    case LexemeKind.String:
                        {
                            if (negative == -1) throw new Exception("Error de Sintaxis: El signo de negativo no se permite con arreglos de caracteres");
                            if (found_variable.Type == DATA_TYPE.ARRAY_CARACTER)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    ret = new StructValueAssign(found_variable, new NonPrimitiveArray<char>(value.Length - 2, value.Replace("\"", string.Empty).ToArray(), DATA_TYPE.ARRAY_CARACTER, "000"));
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ;");
                                }
                                i++; current_lexeme = lexemes[i];
                            }
                            else
                            {
                                throw new Exception("Error de Semantica: Los tipos de dato no coinciden");
                            }
                            break;
                        }
                    case LexemeKind.Numeric:
                        {
                            if (negative == -1) value = "-" + value;
                            if (found_variable.Type == DATA_TYPE.ENTERO)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    int val = 0;
                                    if (int.TryParse(value, out val))
                                    {
                                        ret = new StructValueAssign(found_variable, new PrimitiveVariable<int>(val, "000", DATA_TYPE.ENTERO));
                                    }
                                    else
                                    {
                                        throw new Exception("Error de Sintaxis: No se pudo leer el formato del numero.");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ; o el valor este mal escrito.");
                                }
                                i++; current_lexeme = lexemes[i];
                            }
                            else if (found_variable.Type == DATA_TYPE.DECIMAL)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    double val = 0;
                                    if (double.TryParse(value, out val))
                                    {
                                        ret = new StructValueAssign(found_variable, new PrimitiveVariable<double>(val, "000", DATA_TYPE.DECIMAL));
                                    }
                                    else
                                    {
                                        throw new Exception("Error de Sintaxis: No se pudo leer el formato del numero.");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ;");
                                }
                                i++; current_lexeme = lexemes[i];
                            }
                            else
                            {
                                throw new Exception("Error de Semantica: Los tipos de dato no coinciden");
                            }
                            break;
                        }
                    case LexemeKind.BooleanValue:
                        {
                            if (negative == -1) throw new Exception("Error de Sintaxis: El valor de asignacion no se permite con valores Binario");
                            if (found_variable.Type == DATA_TYPE.BINARIO)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    ret = new StructValueAssign(found_variable, new PrimitiveVariable<bool>(value.Equals("Verdadero"), "000", DATA_TYPE.BINARIO));
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ;");
                                }
                                i++; current_lexeme = lexemes[i];
                            }
                            else
                            {
                                throw new Exception("Error de Semantica: Los tipos de dato no coinciden");
                            }
                            break;
                        }
                    case LexemeKind.Identifier:
                        {
                            if (negative == -1) throw new Exception("Error de Sintaxis: El valor de asignacion negativo para variables no es posible en valores que no sean operaciones");
                            DataField found_variable_value = null;
                            findVariable(ref found_variable_value, src_local_variables, current_lexeme.Text, true);
                            if (found_variable_value.Type == found_variable.Type)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    ret = new StructValueAssign(found_variable, found_variable_value);
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ;");
                                }
                                i++; current_lexeme = lexemes[i];
                            }
                            else
                            {
                                throw new Exception("Error de Semantica: Los tipos de dato no coinciden");
                            }
                            break;
                        }
                    case LexemeKind.Char:
                        {
                            if (negative == -1) throw new Exception("Error de Sintaxis: El valor de asignacion no es reconocible");
                            if (found_variable.Type == DATA_TYPE.CARACTER)
                            {
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(";"))
                                {
                                    ret = new StructValueAssign(found_variable, new PrimitiveVariable<char>(char.Parse(value.Replace("'", string.Empty)), "000", DATA_TYPE.CARACTER));
                                }
                                else
                                {
                                    throw new Exception("Error de Sintaxis: Final de linea invalido, talves haga falta un ;");
                                }
                            }
                            else
                            {
                                throw new Exception("Error de Semantica: Los tipos de dato no coinciden");
                            }
                            i++; current_lexeme = lexemes[i];
                            break;
                        }
                    case LexemeKind.KeyCharacter:
                        {
                            if (!current_lexeme.Text.Equals("#")) throw new Exception("Error de Compilacion: No se logro desifrar el valor de asignacion a la variable " + found_variable.Identifier);
                            if (negative == -1) throw new Exception("Error de Compilacion: Una llamada a funcion no puede tener signo negativo");
                            i++; current_lexeme = lexemes[i];
                            if (current_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Compilacion: No se pudo comprender el identificador de la funcion despues de \"#\"");

                            string func_name = current_lexeme.Text;
                            i++; current_lexeme = lexemes[i];
                            if (!current_lexeme.Text.Equals("(")) throw new Exception("Error de Sintaxis: Los parametros de la funcion inician con (");
                            i++; current_lexeme = lexemes[i];
                            List<DataField> data_params = new List<DataField>();
                            while (!current_lexeme.Text.Equals(")"))
                            {
                                data_params.Add(getVariableValueFromLexeme(lexemes, ref i, src_local_variables));
                                i++; current_lexeme = lexemes[i];
                                if (current_lexeme.Text.Equals(","))
                                {
                                    i++; current_lexeme = lexemes[i];
                                }
                            }
                            i++; current_lexeme = lexemes[i];
                            if (!current_lexeme.Text.Equals(";")) throw new Exception("Error de Sintaxis: Final de Linea invalido.");
                            List<DATA_TYPE> types_params = new List<DATA_TYPE>();
                            foreach (var item in data_params)
                            {
                                types_params.Add(item.Type);
                            }
                            StructFuncion aux = findFuncion(func_name, types_params);
                            if (aux == null) throw new Exception("Error de Semantica: La funcion llamada " + func_name + " no existe o ninguna de sus sobrecargas corresponde con los parametros ingresados");
                            if (found_variable.Type != aux.ReturnType) throw new Exception("Error de Semantica: El tipo de dato devuelto por " + func_name + " " + DataField.DataTypeToString(aux.ReturnType) + " no es el mismo de la variable " + found_variable.Identifier + " " + DataField.DataTypeToString(found_variable.Type));
                            ret = new StructValueAssignCall(found_variable, new StructFuncionCall(aux, data_params));
                            i++; current_lexeme = lexemes[i];
                            break;
                        }
                    default:
                        {
                            throw new Exception("Error de Sintaxis: El valor de asignacion no es reconocible");
                        }
                }
            }
            return ret;
        }

        static private void ExecuteCodeBlock(List<StructField> code_block)
        {
            code_block.ForEach(current_struct =>
            {
                current_struct.runStructField();
            });
        }

        static public bool startSemanticAnalysis()
        {
            if (extracted_lexemes == null) throw new Exception("Engine Error: SyntacticAnalysisNotUInitializedException");
            List<StructField> result = new List<StructField>();
            bool globaltag_complete = false;
            bool codigotag_complete = false;
            for (int i = 0; i < extracted_lexemes.Count; i++)
            {
                Lexeme this_lexeme = extracted_lexemes[i];
                if (this_lexeme.WhatKind == LexemeKind.KeyWord)
                {
                    switch (this_lexeme.Text)
                    {
                        case "Global:":
                            {
                                if (extracted_lexemes[i + 1].WhatKind == LexemeKind.CurlyBrackets)
                                {
                                    i+=2; this_lexeme = extracted_lexemes[i];
                                    while(!this_lexeme.Text.Equals("}"))
                                    {
                                        if (this_lexeme.WhatKind != LexemeKind.DataType) throw new Exception("Error de Sintaxis: Inicio de linea invalido en la etiqueta Global:");
                                        StructVariableDeclare declare = getVariableDeclare(ref i, new List<StructVariableDeclare>(){ });
                                        DataField field = null;
                                        findVariable(ref field, new List<StructVariableDeclare>() { }, declare.Identifier, false);
                                        if(field!=null) throw new Exception("Error de Semantica: La variable " + declare.Identifier + " ya ha sido declarada, no se puede volver a declarar de nuevo");
                                        declare.runStructField();
                                        GlobalDeclareEngine.structVariableDeclares.Add(declare);
                                        this_lexeme = extracted_lexemes[i];
                                    }
                                    globaltag_complete = true;
                                    break;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        case "Codigo:":
                            {
                                List<StructVariableDeclare> local_variables = new List<StructVariableDeclare>();
                                getRunCode(CodigoDeclareEngine, extracted_lexemes, ref i, local_variables);
                                codigotag_complete = true;
                                break;
                            }
                        case "Funcion": //Funcion Entero hola(Entero a, Entero b) 
                            {
                                StructFuncion funcion_declare = new StructFuncion();
                                i++; this_lexeme = extracted_lexemes[i];
                                funcion_declare.ReturnType = getDataType(extracted_lexemes, ref i, "000").Type;
                                i++; this_lexeme = extracted_lexemes[i];
                                if (this_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Sintaxis: Se debe especificar un identificador para cada funcion");
                                funcion_declare.Identifier = this_lexeme.Text;
                                i++; this_lexeme = extracted_lexemes[i];
                                if (!this_lexeme.Text.Equals("(")) throw new Exception("Error de Sintaxis: Los parametros de la funcion inician con (");
                                i++; this_lexeme = extracted_lexemes[i];
                                DataField param_var;
                                string param_name="";
                                while (!this_lexeme.Text.Equals(")"))
                                {
                                    if (this_lexeme.WhatKind != LexemeKind.DataType) throw new Exception("Error de Sintaxis: El compilador no pudo reconocer el tipo de dato del parametro de la funcion");
                                    param_var= getDataType(extracted_lexemes, ref i, param_name);
                                    if (param_var.Type == DATA_TYPE.NULO) throw new Exception("Error de Semantica: Una variable declarada Nulo no es permitido");
                                    i++; this_lexeme = extracted_lexemes[i];
                                    if (this_lexeme.WhatKind != LexemeKind.Identifier) throw new Exception("Error de Sintaxis: El compilador no pudo reconocer el identificador del parametro de la funcion");
                                    param_name= this_lexeme.Text;
                                    param_var.Identifier = param_name;
                                    if (GlobalDeclareEngine.structVariableDeclares.Where(entry => entry.Identifier.Equals(param_name)).Any()) throw new Exception("Error de Semantica: La variable " + param_name + " ya esta declarada como global, no se puede usar como parametro");
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
                                if(findFuncion(funcion_declare.Identifier, funcion_declare.Parameters)!=null) throw new Exception("La funcion " + funcion_declare.Identifier + " ya a sido declarada con esos mismos parametros");
                                FuncionDeclareEngine.Add(funcion_declare);
                                CurrentFuncion = funcion_declare;
                                getRunCode(funcion_declare.Code, extracted_lexemes, ref i, funcion_declare.LocalVariables);
                                CurrentFuncion = null;
                                break;
                            }
                        default:
                            {
                                throw new Exception("Error de Sintaxis: Inicio de linea erroneo. Es probable que se haya escrito mal una palabra reservada");
                            }
                    }
                }
                else
                {
                    throw new Exception("Error de Sintaxis: Inicio de linea invalido. Talvez se haya omitido alguna seccion de codigo como \"Codigo:\" o \"Global:\"");
                }
            }
            return (globaltag_complete && codigotag_complete);
        }
        static public StructFuncion findFuncion(string identifier, List<StructVariableDeclare> fparams)
        {
            foreach(StructFuncion funcion in FuncionDeclareEngine)
            {
                if(identifier.Equals(funcion.Identifier))
                {
                    if(fparams.Count==funcion.Parameters.Count)
                    {
                        bool find = true;
                        for (int i = 0; i < fparams.Count; i++)
                        {
                            if (fparams[i].Variable.Type != funcion.Parameters[i].Variable.Type)
                            {
                                find = false;
                                break;
                            }
                        }
                        if(find)
                        {
                            return funcion;
                        }
                    }
                }
            }
            return null;
        }
        static public StructFuncion findFuncion(string identifier, List<DATA_TYPE> fparams)
        {
            foreach (StructFuncion funcion in FuncionDeclareEngine)
            {
                if (identifier.Equals(funcion.Identifier))
                {
                    if (fparams.Count == funcion.Parameters.Count)
                    {
                        bool find = true;
                        for (int i = 0; i < fparams.Count; i++)
                        {
                            if (fparams[i] != funcion.Parameters[i].Variable.Type)
                            {
                                find = false;
                                break;
                            }
                        }
                        if (find)
                        {
                            return funcion;
                        }
                    }
                }
            }
            return null;
        }
        static public DataField getDataType(List<Lexeme> lexemes, ref int i, string identifier)
        {
            Lexeme current_lexeme;
            current_lexeme = lexemes[i];
            string index_ss = string.Empty;
            for (int x = i+1; (lexemes[x].WhatKind == LexemeKind.SquareBrackets); x++)
            {
                index_ss += lexemes[x].Text;
            }
            if (string.IsNullOrEmpty(index_ss))
            {
                switch(current_lexeme.Text)
                {
                    case "Entero":      return new PrimitiveVariable<int>(0, identifier);
                    case "Decimal":     return new PrimitiveVariable<double>(0f, identifier);
                    case "Binario":     return new PrimitiveVariable<bool>(false, identifier);
                    case "Caracter":    return new PrimitiveVariable<char>((char) 0, identifier);
                    case "Nulo":        return DataField.Null;
                    default:            throw new Exception("Error de Sintaxis: El tipo de dato especificado no existe en Valentina");
                }
            }
            else if(Regex.IsMatch(index_ss, "^\\[\\]\\[\\]$"))
            {
                i += 4;
                switch(current_lexeme.Text)
                {
                    case "Entero":      return new NonPrimitiveMultiArray<int>(DATA_TYPE.BIARRAY_ENTERO, identifier);
                    case "Decimal":     return new NonPrimitiveMultiArray<double>(DATA_TYPE.BIARRAY_DECIMAL, identifier);
                    case "Binario":     return new NonPrimitiveMultiArray<bool>(DATA_TYPE.BIARRAY_BINARIO, identifier);
                    case "Caracter":    return new NonPrimitiveMultiArray<char>(DATA_TYPE.BIARRAY_CARACTER, identifier);
                    default:            throw new Exception("Error de Sintaxis: El tipo de dato especificado no existe en Valentina");
                }
            }
            else if (Regex.IsMatch(index_ss, "^\\[\\]$"))
            {
                i += 2;
                switch (current_lexeme.Text)
                {
                    case "Entero": return new NonPrimitiveArray<int>(DATA_TYPE.ARRAY_ENTERO, identifier);
                    case "Decimal": return new NonPrimitiveArray<double>(DATA_TYPE.ARRAY_DECIMAL, identifier);
                    case "Binario": return new NonPrimitiveArray<bool>(DATA_TYPE.ARRAY_BINARIO, identifier);
                    case "Caracter": return new NonPrimitiveArray<char>(DATA_TYPE.ARRAY_CARACTER, identifier);
                    default: throw new Exception("Error de Sintaxis: El tipo de dato especificado no existe en Valentina");
                }
            }
            else
            {
                throw new Exception("Error de Sintaxis: El tipo de dato no ha podido ser reconocido por el compilador");
            }
        }

        static public void startSyntacticAnalysis()
        {
            if (Input == null) throw new Exception("Engine Error: InputNotSetException");
            List<string> strings = getAll(Input.Text);
            List<Lexeme> result = new List<Lexeme>();
            for(int i= 0; i < strings.Count; i++)
            {
                if(i==126)
                {
                    Console.WriteLine("a");
                }
                string this_ss = strings[i];
                if (arithmetic_operators.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.ArithmeticOperator));
                }
                else if(boolvalues_brackets.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.BooleanValue));
                }
                else if (data_types.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.DataType));
                }
                else if (curly_brackets.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.CurlyBrackets));
                }
                else if (this_ss.Equals("Y") || this_ss.Equals("O"))
                {
                    result.Add(new Lexeme(strings[i], LexemeKind.LogicOperator));
                }
                else if ((strings[i] + (i + 1 < strings.Count ? strings[i + 1] : "") + (i + 2 < strings.Count ? strings[i + 2] : "")).Equals("(No)"))
                {
                    result.Add(new Lexeme(strings[i] + strings[i + 1] + strings[i + 2], LexemeKind.LogicOperator));
                    i += 2;
                }
                else if (round_brackets.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.RoundBrackets));
                }
                else if (square_brackets.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.SquareBrackets));
                }
                else if (compare_operators.Contains(strings[i] + strings[i + 1]))
                {
                    result.Add(new Lexeme(strings[i] + strings[i + 1], LexemeKind.CompareOperator));
                    i++;
                }
                else if (compare_operators.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.CompareOperator));
                }
                else if (key_character.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.KeyCharacter));
                }
                else if (system_fuctions.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.SystemFuctions));
                }
                else if (key_word.Contains(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.KeyWord));
                }
                else if (key_comments.Contains(strings[i] + strings[i + 1]))
                {
                    result.Add(new Lexeme(strings[i] + strings[i + 1], LexemeKind.KeyComments));
                    i++;
                }
                else if (regex_word.IsMatch(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.Identifier));
                }
                else if (regex_numeric.IsMatch(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.Numeric));
                }
                else if (regex_string.IsMatch(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.String));
                }
                else if (regex_char.IsMatch(this_ss))
                {
                    result.Add(new Lexeme(this_ss, LexemeKind.Char));
                }
                else
                {
                    throw new Exception("Sintaxis Error: El codigo contiene algun elemento que no pertenece a la sintaxis de Valentina");
                }
            }
            extracted_lexemes = result;
            return;
        }

        static public List<string> getAll(string code)
        {
            List<string> result = new List<string>();
            string ss = string.Empty;
            bool flag_writing_word = false;
            bool flag_writing_number = false;
            bool flag_writing_string = false;
            bool flag_writing_char=false;
            code.ToList().ForEach(character =>
            {
                if(character=='\'')
                {
                    if (flag_writing_char)
                    {
                        flag_writing_char = false;
                        ss += character;
                        result.Add(ss);
                        ss = string.Empty;
                        return;
                    }
                    else
                    {
                        flag_writing_char = true;
                        ss += character;
                        return;
                    }
                }
                if (flag_writing_char)
                {
                    ss += character;
                    return;
                }
                if (character=='"')
                {
                    if(flag_writing_string)
                    {
                        flag_writing_string=false;
                        ss += character;
                        result.Add(ss);
                        ss = string.Empty;
                        return;
                    }
                    else
                    {
                        flag_writing_string=true;
                        ss += character;
                        return;
                    }
                }
                if(flag_writing_string)
                {
                    ss += character;
                    return;
                }
                if(char.IsLetter(character) || character==':' || character=='_')
                {
                    if (flag_writing_word)
                    {
                        ss += character;
                    }
                    else
                    {
                        flag_writing_word=true;
                        ss += character;
                    }
                }
                else if(char.IsDigit(character) || character=='.')
                {
                    if(flag_writing_number)
                    {
                        ss += character;
                    }
                    else
                    {
                        flag_writing_number = true;
                        ss += character;
                    }
                }
                else
                {
                    if((flag_writing_number || flag_writing_word))
                    {
                        flag_writing_number=false;
                        flag_writing_word=false;
                        flag_writing_char=false;
                        flag_writing_string = false;
                        result.Add(ss);
                        if(!char.IsWhiteSpace(character) && character!='\n' && character!='\t') result.Add("" + character);
                        ss = string.Empty;
                    }
                    else if(!char.IsWhiteSpace(character))
                    {
                        result.Add("" + character);
                    }
                }
            });
            return result;
        }
    }
}
