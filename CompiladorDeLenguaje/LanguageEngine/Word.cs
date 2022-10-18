using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.LanguageEngine
{
    class Word
    {
        public string Text { get => Text; set => Text = value; }
        public bool IsKeyWord { get => IsKeyWord; set => IsKeyWord = value; }
        public bool IsNameWord { get => IsNameWord; set => IsNameWord = value; }
        public bool IsLogicOperator { get => IsLogicOperator; set => IsLogicOperator = value; }
        public bool IsArithmeticOperator { get => IsArithmeticOperator; set => IsArithmeticOperator = value; }
        public bool IsKeyCharacter { get => IsKeyCharacter; set => IsKeyCharacter = value; }
        public bool IsBracket { get => IsBracket; set => IsBracket = value; }
        public Word(string text)
        {
            IsKeyWord = false;
            IsArithmeticOperator = false;
            IsKeyCharacter = false;
            IsNameWord = false;
            IsLogicOperator = false;
            Text = text;
        }
    }
}
