using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.LanguageEngine
{
    enum LexemeKind
    {
        ArithmeticOperator,
        CompareOperator,
        DataType,
        RoundBrackets,
        CurlyBrackets,
        KeyComments,
        KeyCharacter,
        SquareBrackets,
        KeyWord,
        SystemFuctions,
        Identifier,
        LogicOperator,
        Numeric,
        String,
        Char,
        BooleanValue
    }
    class Lexeme
    {

        public string Text { get; set; }
        public LexemeKind WhatKind { get; set; } 
        public Lexeme(string text, LexemeKind Is)
        {  
            this.WhatKind = Is;
            Text = text;
        }

        public override string ToString()
        {
            switch (WhatKind)
            {
                case LexemeKind.ArithmeticOperator:     return "" + Text + " | IsArithmeticOperator";
                case LexemeKind.CompareOperator:        return "" + Text + " | IsCompareOperator";
                case LexemeKind.DataType:               return "" + Text + " | IsDataType";
                case LexemeKind.RoundBrackets:          return "" + Text + " | IsRoundBrackets";
                case LexemeKind.CurlyBrackets:          return "" + Text + " | IsCurlyBrackets";
                case LexemeKind.KeyComments:            return "" + Text + " | IsKeyComments";
                case LexemeKind.KeyCharacter:           return "" + Text + " | IsKeyCharacter";
                case LexemeKind.SquareBrackets:         return "" + Text + " | IsSquareBrackets";
                case LexemeKind.KeyWord:                return "" + Text + " | IsKeyWord";
                case LexemeKind.SystemFuctions:         return "" + Text + " | IsSystemFuctions";
                case LexemeKind.Identifier:             return "" + Text + " | IsIdentifier";
                case LexemeKind.LogicOperator:          return "" + Text + " | IsLogicOperator";
                case LexemeKind.Numeric:                return "" + Text + " | IsNumeric";
                case LexemeKind.String:                 return "" + Text + " | IsString";
                case LexemeKind.Char:                   return "" + Text + " | IsChar";
                case LexemeKind.BooleanValue:           return "" + Text + " | IsBooleanValue";
                default:                                throw new Exception("Error de Sintaxis: El lexema no se pudo reconocer");
            }
        }

    }
}
