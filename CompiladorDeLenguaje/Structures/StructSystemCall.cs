using CompiladorDeLenguaje.DataTypes;
using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.Structures
{
    class StructSystemCall : StructField
    {
        public StructSystemCall(string system_call_name, List<DataField> parameters) : base(Struct_Type.System_Call)
        {
            this.system_call_name = system_call_name;
            this.parameters = parameters;
            switch (system_call_name)
            {
                case "SaltoLinea":
                    {
                        if (parameters.Count != 0)
                        {
                            throw new Exception("La funcion SaltoLinea() solo posee 0 parametros");
                        }
                        break;
                    }
                case "LimpiarConsola":
                    {
                        if (parameters.Count != 0)
                        {
                            throw new Exception("La funcion LimpiarConsola() solo posee 0 parametros");
                        }
                        break;
                    }
                case "Capturar":
                    {
                        if (parameters.Count != 1)
                        {
                            throw new Exception("La funcion Capturar() solo posee 1 parametro");
                        }
                        if (parameters[0].Identifier.Equals("000")) throw new Exception("Error de Ejecucion: La funcion Capturar() no acepta constantes o valores anonimos");
                        if (!(parameters[0].Type == DATA_TYPE.DECIMAL || parameters[0].Type == DATA_TYPE.ARRAY_CARACTER || parameters[0].Type == DATA_TYPE.ENTERO || parameters[0].Type == DATA_TYPE.CARACTER || parameters[0].Type == DATA_TYPE.BINARIO))
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion Capturar() acepta " + DataField.DataTypeToString(parameters[0].Type) + " en su parametro numero 1");
                        }
                        break;
                    }
                case "Mostrar":
                    {
                        if (parameters.Count != 1)
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion Mostrar() acepta " + parameters.Count + " parametro(s)");
                        }
                        if (!(parameters[0].Type == DATA_TYPE.DECIMAL || parameters[0].Type == DATA_TYPE.ARRAY_CARACTER || parameters[0].Type == DATA_TYPE.ENTERO || parameters[0].Type == DATA_TYPE.CARACTER || parameters[0].Type == DATA_TYPE.BINARIO))
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion Mostrar() acepta " + DataField.DataTypeToString(parameters[0].Type) + " en su parametro numero 1");
                        }
                        break;
                    }
                case "MostrarLn":
                    {
                        if (parameters.Count != 1)
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion MostrarLn() acepta " + parameters.Count + " parametro(s)");
                        }
                        if (!(parameters[0].Type == DATA_TYPE.DECIMAL || parameters[0].Type == DATA_TYPE.ARRAY_CARACTER || parameters[0].Type == DATA_TYPE.ENTERO || parameters[0].Type == DATA_TYPE.CARACTER || parameters[0].Type == DATA_TYPE.BINARIO))
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion MostrarLn() acepta " + DataField.DataTypeToString(parameters[0].Type) + " en su parametro numero 1");
                        }
                        break;
                    }
                case "Medir":
                    {
                        if (parameters.Count != 2)
                        {
                            throw new Exception("Error de Semantica: Ninguna sobrecarga de la funcion Mostrar() acepta " + parameters.Count + " parametro(s)");
                        }
                        if (parameters[1].Type != DATA_TYPE.ENTERO) throw new Exception($"Error de Semantica: Ninguna sobrecarga de la funcion Medir() corresponde con los parametros. Medir(Arreglo variable, Entero dimension_para_medir)");
                        if (!(DataField.IsBidimensional(parameters[0].Type) || DataField.IsUnidimensional(parameters[0].Type))) throw new Exception($"Error de Semantica: Ninguna sobrecarga de la funcion Medir() acepta un parametro {DataField.DataTypeToString(parameters[0].Type)}. Solo se pueden medir arreglos.");
                        break;
                    }

                default:
                    {
                        throw new Exception("Error de Semantica: La funcion del sistema " + system_call_name + " no existe");
                    }
            }
        }

        public string system_call_name { get; set; }
        public List<DataField> parameters { get; set; }

        public override DataField runStructField()
        {
            switch (system_call_name)
            {
                case "SaltoLinea":
                    {
                        if (parameters.Count != 0)
                        {
                            throw new Exception("La funcion SaltoLinea() solo posee 0 parametros");
                        }
                        Valentina.Window.syncAppendToOutput("\n");
                        return new DataField("000", DATA_TYPE.NULO);
                    }
                case "LimpiarConsola":
                    {
                        if (parameters.Count != 0)
                        {
                            throw new Exception("La funcion LimpiarConsola() solo posee 0 parametros");
                        }
                        Valentina.Window.syncClearOutput();
                        return new DataField("000", DATA_TYPE.NULO);
                    }
                case "Capturar":
                    {
                        Valentina.CaptureString = "";
                        if (parameters.Count != 1)
                        {
                            throw new Exception("La funcion Capturar() solo posee 1 parametro");
                        }
                        if (parameters[0].Identifier.Equals("000")) throw new Exception("Error de Ejecucion: La funcion Capturar() no acepta valores anonimos");
                        if (parameters[0].Type == DATA_TYPE.DECIMAL)
                        {
                            Valentina.Window.syncStartCapturingUserInput();
                            while (Valentina.CapturingUserInput)
                            {
                                Thread.Sleep(25);
                            }
                            double val = 0;
                            if (double.TryParse(Valentina.CaptureString, out val))
                            {
                                ((PrimitiveVariable<double>)parameters[0]).Value = val;
                            }
                            else
                            {
                                throw new Exception("Error de Ejecucion: No se puedo convertir la entrada a tipo de dato Decimal");
                            }
                        }
                        else if (parameters[0].Type == DATA_TYPE.ARRAY_CARACTER)
                        {
                            Valentina.Window.syncStartCapturingUserInput();
                            while (Valentina.CapturingUserInput)
                            {
                                Thread.Sleep(25);
                            }
                            DataField.appendStringToCharArray(((NonPrimitiveArray<char>)parameters[0]).Values, Valentina.CaptureString);
                        }
                        else if (parameters[0].Type == DATA_TYPE.ENTERO)
                        {
                            Valentina.Window.syncStartCapturingUserInput();
                            while (Valentina.CapturingUserInput)
                            {
                                Thread.Sleep(25);
                            }
                            int val = 0;
                            if (int.TryParse(Valentina.CaptureString, out val))
                            {
                                ((PrimitiveVariable<int>)parameters[0]).Value = val;
                            }
                            else
                            {
                                throw new Exception("Error de Ejecucion: No se puedo convertir la entrada a tipo de dato Entero");
                            }
                        }
                        else
                        {
                            throw new Exception("Error de Ejecucion: La funcion Capturar() no acepta ninguna variable de tipo " + DataField.DataTypeToString(parameters[0].Type));
                        }
                        return new DataField("000", DATA_TYPE.NULO);
                    }
                case "Mostrar":
                    {
                        if (parameters.Count != 1)
                        {
                            throw new Exception("La funcion Mostrar() solo posee 1 parametro");
                        }
                        switch(parameters[0].Type)
                        {
                            case DATA_TYPE.ARRAY_CARACTER:
                                {
                                    Valentina.Window.syncAppendToOutput(string.Join("", parseCaracterArrayToString((NonPrimitiveArray<char>)parameters[0])));
                                    break;
                                }
                            case DATA_TYPE.BINARIO:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<bool>)parameters[0]).Value ? "Verdadero" : "Falso");
                                    break;
                                }
                            case DATA_TYPE.DECIMAL:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<double>)parameters[0]).Value.ToString());
                                    break;
                                }
                            case DATA_TYPE.ENTERO:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<int>)parameters[0]).Value.ToString());
                                    break;
                                }
                            case DATA_TYPE.CARACTER:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<char>)parameters[0]).Value.ToString());
                                    break;
                                }
                        }
                        return new DataField("000", DATA_TYPE.NULO);
                    }
                case "MostrarLn":
                    {
                        if (parameters.Count != 1)
                        {
                            throw new Exception("La funcion MostrarLn() solo posee 1 parametro");
                        }
                        switch (parameters[0].Type)
                        {
                            case DATA_TYPE.ARRAY_CARACTER:
                                {
                                    Valentina.Window.syncAppendToOutput(string.Join("", parseCaracterArrayToString((NonPrimitiveArray<char>)parameters[0])) + "\n");
                                    break;
                                }
                            case DATA_TYPE.BINARIO:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<bool>)parameters[0]).Value ? "Verdadero" + "\n": "Falso" + "\n");
                                    break;
                                }
                            case DATA_TYPE.DECIMAL:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<double>)parameters[0]).Value.ToString() + "\n");
                                    break;
                                }
                            case DATA_TYPE.ENTERO:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<int>)parameters[0]).Value.ToString() + "\n");
                                    break;
                                }
                            case DATA_TYPE.CARACTER:
                                {
                                    Valentina.Window.syncAppendToOutput(((PrimitiveVariable<char>)parameters[0]).Value.ToString() + "\n");
                                    break;
                                }
                        }
                        return new DataField("000", DATA_TYPE.NULO);
                    }
                case "Medir":
                    {
                        if (parameters.Count != 2)
                        {
                            throw new Exception("La funcion MostrarLn() solo posee 2 parametros");
                        }
                        if (DataField.IsUnidimensional(parameters[0].Type))
                        {
                            if ((int)parameters[1].VariableValue != 0) throw new Exception($"Error de Ejecucion: Se intento obtener la dimension {(int)parameters[1].VariableValue} de la variable {parameters[1].Identifier}. Las dimensiones inician en 0 y sin signo");
                            return new PrimitiveVariable<int>(parameters[0].Columns, "000");
                        }
                        else
                        {
                            if (!((int)parameters[1].VariableValue == 0 || (int)parameters[1].VariableValue == 1)) throw new Exception($"Error de Ejecucion: Se intento obtener la dimension {(int)parameters[1].VariableValue} de la variable {parameters[1].Identifier}. Las dimensiones inician en 0 y sin signo");
                            if((int)parameters[1].VariableValue==0)
                            {
                                return new PrimitiveVariable<int>(parameters[0].Rows, "000");

                            }
                            else
                            {
                                return new PrimitiveVariable<int>(parameters[0].Columns, "000");

                            }
                        }
                    }
                default:
                    {
                        throw new Exception("Error de Semantica: La funcion del sistema " + system_call_name + " no existe");
                    }
            }
        }
        private string parseCaracterArrayToString(NonPrimitiveArray<char> char_array)
        {
            string ss= string.Empty;
            foreach (PrimitiveVariable<char> c in char_array.Values)
            {
                if (c.Value != '\0') ss+= c.Value;
            }
            return ss;
        }
    }
    
}
