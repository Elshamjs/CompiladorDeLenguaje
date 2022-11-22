using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorDeLenguaje.LanguageEngine
{
    class LexemeBlock
    {
        public List<Lexeme> Lexemes { get; set; }
        public int BlockStart { get; set; }
        public int BlockEnd { get; set; }
        public string Block_Name { get; set; }

        public LexemeBlock(List<Lexeme> lexemes, int blockStart, int blockEnd)
        {
            Lexemes = lexemes;
            BlockStart = blockStart;
            BlockEnd = blockEnd;
        }
        public LexemeBlock(List<Lexeme> lexemes)
        {
            Lexemes = lexemes;
        }
    }
}
