using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.LanguageEngine
{
    class Lexeme
    {
        public Lexeme(string text, bool isKeyWord, bool isNameWord, bool isLogicOperator, bool isArithmeticOperator, bool isKeyCharacter, bool isRoundBracket, bool isCurlyBracket)
        {
            Text = text;
            IsKeyWord = isKeyWord;
            IsNameWord = isNameWord;
            IsLogicOperator = isLogicOperator;
            IsArithmeticOperator = isArithmeticOperator;
            IsKeyCharacter = isKeyCharacter;
            IsRoundBracket = isRoundBracket;
            IsCurlyBracket = isCurlyBracket;
        }

        public string Text { get; set; }
        public bool IsKeyWord { get; set; }
        public bool IsNameWord { get; set; }
        public bool IsLogicOperator { get; set; }
        public bool IsArithmeticOperator { get; set; }
        public bool IsKeyCharacter { get; set; }
        public bool IsRoundBracket { get; set; }
        public bool IsCurlyBracket { get; set; }
    }
}
