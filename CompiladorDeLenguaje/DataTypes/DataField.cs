using CompiladorDeLenguaje.LanguageEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.DataTypes
{
    struct ArrayIndex<T> where T : struct
    {
        public NonPrimitiveArray<T> Array { get; set; }
        public bool IsArray { get; set; }
        public int index { get; set; }
    }
    struct MultiArrayIndex<T> where T : struct
    {
        public bool IsMultiArray { get; set; }
        public NonPrimitiveMultiArray<T> MultiArray { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
    public enum DATA_TYPE
    {
        ENTERO,
        BINARIO,
        DECIMAL,
        CARACTER,
        ARRAY_ENTERO,
        ARRAY_BINARIO,
        ARRAY_DECIMAL,
        ARRAY_CARACTER,
        BIARRAY_ENTERO,
        BIARRAY_BINARIO,
        BIARRAY_DECIMAL,
        BIARRAY_CARACTER,
        NULO
    }
    class DataField
    {
        public string _identifier { get; set; }
        public string Identifier { get=> _identifier; set=> setIdentifier(value); }
        public void setIdentifier(string ss)
        {
            _identifier = ss;
            switch (this.Type)
            {
                case DATA_TYPE.ARRAY_ENTERO: { ((NonPrimitiveArray<int>)this).renameValue(ss); break; }
                case DATA_TYPE.ARRAY_BINARIO: { ((NonPrimitiveArray<bool>)this).renameValue(ss); break; }
                case DATA_TYPE.ARRAY_DECIMAL: { ((NonPrimitiveArray<double>)this).renameValue(ss); break; }
                case DATA_TYPE.ARRAY_CARACTER: { ((NonPrimitiveArray<char>)this).renameValue(ss); break; }
                case DATA_TYPE.BIARRAY_ENTERO: { ((NonPrimitiveMultiArray<int>)this).renameValue(ss); break; }
                case DATA_TYPE.BIARRAY_BINARIO: { ((NonPrimitiveMultiArray<bool>)this).renameValue(ss); break; }
                case DATA_TYPE.BIARRAY_DECIMAL: { ((NonPrimitiveMultiArray<double>)this).renameValue(ss); break; }
                case DATA_TYPE.BIARRAY_CARACTER: { ((NonPrimitiveMultiArray<char>)this).renameValue(ss); break; }
                case DATA_TYPE.NULO: break;
            }
        }
        public DATA_TYPE Type { get; set; }
        protected object private_value;
        public object VariableValue { get => private_value; set => setValue(value); }
        private void setValue(object setvalue)
        {
            switch (this.Type)
            {
                case DATA_TYPE.ARRAY_ENTERO: { ((NonPrimitiveArray<int>)this).Values = (PrimitiveVariable<int>[])setvalue; break; }
                case DATA_TYPE.ARRAY_BINARIO: { ((NonPrimitiveArray<bool>)this).Values = (PrimitiveVariable<bool>[])setvalue; break; }
                case DATA_TYPE.ARRAY_DECIMAL: { ((NonPrimitiveArray<double>)this).Values = (PrimitiveVariable<double>[])setvalue; break; }
                case DATA_TYPE.ARRAY_CARACTER: { ((NonPrimitiveArray<char>)this).Values = (PrimitiveVariable<char>[])setvalue; break; }
                case DATA_TYPE.BIARRAY_ENTERO: { ((NonPrimitiveMultiArray<int>)this).Values = (PrimitiveVariable<int>[,])setvalue; break; }
                case DATA_TYPE.BIARRAY_BINARIO: { ((NonPrimitiveMultiArray<bool>)this).Values = (PrimitiveVariable<bool>[,])setvalue; break; }
                case DATA_TYPE.BIARRAY_DECIMAL: { ((NonPrimitiveMultiArray<double>)this).Values = (PrimitiveVariable<double>[,])setvalue; break; }
                case DATA_TYPE.BIARRAY_CARACTER: { ((NonPrimitiveMultiArray<char>)this).Values = (PrimitiveVariable<char>[,])setvalue; break; }
                case DATA_TYPE.NULO: break;
                default: { private_value= setvalue; break; }
            }
        }
        public static readonly DataField Null = new DataField(null,"000", DATA_TYPE.NULO);
        public DataField() { }
        public DataField(string name, DATA_TYPE type)
        {
            this._identifier = name;
            this.Type = type;
        }
        public DataField(object value, string name, DATA_TYPE type)
        {
            this._identifier = name;
            this.Type = type;
            this.VariableValue = value;
        }
        static public string DataTypeToString(DATA_TYPE type)
        {
            switch (type)
            {
                case DATA_TYPE.BINARIO          :             { return "Binario"      ; }
                case DATA_TYPE.ENTERO           :              { return "Entero"       ; }
                case DATA_TYPE.DECIMAL          :             { return "Decimal"      ; }
                case DATA_TYPE.CARACTER         :            { return "Caracter"     ; }
                case DATA_TYPE.ARRAY_ENTERO     :        { return "Entero[]"     ; }
                case DATA_TYPE.ARRAY_BINARIO    :       { return "Binario[]"    ; }
                case DATA_TYPE.ARRAY_DECIMAL    :       { return "Decimal[]"    ; }
                case DATA_TYPE.ARRAY_CARACTER   :      { return "Caracter[]"   ; }
                case DATA_TYPE.BIARRAY_ENTERO   :      { return "Entero[][]"   ; }
                case DATA_TYPE.BIARRAY_BINARIO  :     { return "Binario[][]"  ; }
                case DATA_TYPE.BIARRAY_DECIMAL  :     { return "Decimal[][]"  ; }
                case DATA_TYPE.BIARRAY_CARACTER :    { return "Caracter[][]" ; }
                case DATA_TYPE.NULO:                { return "Nulo"         ; }
                default: { throw new Exception("Engine Error: No se pudo reconocer el tipo de dato, talves no este implementado aun en Valentina"); }
            }
        }
        static public DATA_TYPE StringToDataType(string type)
        {
            switch (type)
            {
                case "Binario"      : { return DATA_TYPE.BINARIO            ; }
                case "Entero"       : { return DATA_TYPE.ENTERO             ; }
                case "Decimal"      : { return DATA_TYPE.DECIMAL            ; }
                case "Caracter"     : { return DATA_TYPE.CARACTER           ; }
                case "Entero[]"     : { return DATA_TYPE.ARRAY_ENTERO     ; }
                case "Binario[]"    : { return DATA_TYPE.ARRAY_BINARIO    ; }
                case "Decimal[]"    : { return DATA_TYPE.ARRAY_DECIMAL    ; }
                case "Caracter[]"   : { return DATA_TYPE.ARRAY_CARACTER   ; }
                case "Entero[][]"   : { return DATA_TYPE.BIARRAY_ENTERO   ; }
                case "Binario[][]"  : { return DATA_TYPE.BIARRAY_BINARIO  ; }
                case "Decimal[][]"  : { return DATA_TYPE.BIARRAY_DECIMAL  ; }
                case "Caracter[][]" : { return DATA_TYPE.BIARRAY_CARACTER ; }
                case "Nulo"         : { return DATA_TYPE.NULO            ; }
                default: { throw new Exception("Error de Sintaxis: El tipo de especificado no es reconocible por el compilador"); }
            }
        }
        public static PrimitiveVariable<char>[] stringToCharArray(string ss)
        {
            PrimitiveVariable<char>[] array = new PrimitiveVariable<char>[ss.Length];
            for (int i = 0; i < ss.Length; i++)
            {
                array[i].Value= ss[i];
            }
            return array;
                
        }
        public static DataField getIndexFromArray(string identifier)
        {
            string name = identifier.Where(c => char.IsLetter(c) || c=='_').ToString();
            int index_ss = int.Parse(identifier.Where(c => char.IsDigit(c)).ToString());
            DataField field = null;
            Valentina.findVariable(ref field, Valentina.LocalDeclarations, name, false);
            if (field == null) throw new Exception("Error de Ejecucion: La Variable " + name + " no esta correctamente inicializada y su valor es Nulo");
            try
            {
                switch (field.Type)
                {
                    case DATA_TYPE.ARRAY_ENTERO:
                        {
                            return ((NonPrimitiveArray<int>)field).Values[index_ss];
                        }
                    case DATA_TYPE.ARRAY_CARACTER:
                        {
                            return ((NonPrimitiveArray<char>)field).Values[index_ss];
                        }
                    case DATA_TYPE.ARRAY_BINARIO:
                        {
                            return ((NonPrimitiveArray<bool>)field).Values[index_ss];
                        }
                    case DATA_TYPE.ARRAY_DECIMAL:
                        {
                            return ((NonPrimitiveArray<double>)field).Values[index_ss];
                        }
                    default:
                        {
                            throw new Exception("Error de Ejecucion: La variable " + name + " no podido ser leida");
                        }
                }
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error de Ejecucion: Se ha intentado acceder a Nulo, la variable" + name + " aun no habia sido inicializada o se le asigno nulo");
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Error de Ejecucion: La variable " + name + " no tiene un elemento con indice [" + index_ss+"]");
            }
            
        }
        public static DataField getValueFromIdentifier(DataField Variable)
        {
            if (Regex.IsMatch(Variable.Identifier, "(\\[[0-9]+\\])(\\[[0-9]+\\])"))
            {
                return getIndexFromMultiArray(Variable.Identifier);
            }
            else if (Regex.IsMatch(Variable.Identifier, "(\\[[0-9]+\\])"))
            {
                return getIndexFromArray(Variable.Identifier);
            }
            else
            {
                return Variable;
            }
        }
        public static DataField getIndexFromMultiArray(string identifier)
        {
            string name = identifier.Where(c => char.IsLetter(c) || c == '_').ToString(); //hola[3][2] => ]2[]3[
            int row = int.Parse(identifier.Where(c => !char.IsLetter(c)).SkipWhile(c => c != ']').Where(c => char.IsDigit(c)).ToString());
            int column = int.Parse(identifier.Where(c => !char.IsLetter(c)).Reverse().SkipWhile(c => c != '[').Where(c => char.IsDigit(c)).Reverse().ToString());
            DataField field = null;
            Valentina.findVariable(ref field, Valentina.LocalDeclarations, name, false);
            if (field == null) throw new Exception("Error de Ejecucion: La Variable " + name + " no se pudo leer");
            try
            {
                switch (field.Type)
                {
                    case DATA_TYPE.BIARRAY_ENTERO:
                        {
                            return ((NonPrimitiveMultiArray<int>)field).Values[row, column];
                        }
                    case DATA_TYPE.BIARRAY_CARACTER:
                        {
                            return ((NonPrimitiveMultiArray<char>)field).Values[row,column];
                        }
                    case DATA_TYPE.BIARRAY_BINARIO:
                        {
                            return ((NonPrimitiveMultiArray<bool>)field).Values[row, column];
                        }
                    case DATA_TYPE.BIARRAY_DECIMAL:
                        {
                            return ((NonPrimitiveMultiArray<double>)field).Values[row, column];
                        }
                    default:
                        {
                            throw new Exception("Error de Ejecucion: La variable " + name + " no podido ser leida");
                        }
                }
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error de Ejecucion: Se ha intentado acceder a Nulo, la variable" + name + " aun no habia sido inicializada o se le asigno nulo");
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Error de Ejecucion: La variable " + name + " no tiene un elemento con indice [" + row + "][" +column+ "]");
            }

        }
        public static void appendStringToCharArray(PrimitiveVariable<char>[] array, string ss)
        {
            if (array.Length < ss.Length) throw new Exception("Error de Ejecucion: La variable de destino es de menor tamaño que la variable fuente");
            for (int i = 0; i < ss.Length; i++)
            {
                array[i].Value = ss[i];
            }
        }
        public bool isArray()
        {
            switch(Type)
            {
                case DATA_TYPE.ARRAY_ENTERO:    { return true;}
                case DATA_TYPE.ARRAY_BINARIO:   { return true;}
                case DATA_TYPE.ARRAY_DECIMAL:   { return true;}
                case DATA_TYPE.ARRAY_CARACTER:  { return true; }
            }
            return false;
        }
        public bool isBidimensionalArray()
        {
            switch (Type)
            {
                case DATA_TYPE.BIARRAY_ENTERO: { return true; }
                case DATA_TYPE.BIARRAY_BINARIO: { return true; }
                case DATA_TYPE.BIARRAY_DECIMAL: { return true; }
                case DATA_TYPE.BIARRAY_CARACTER: { return true; }
            }
            return false;
        }
    }

}
