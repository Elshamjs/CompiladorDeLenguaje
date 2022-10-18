using CompiladorDeLenguaje.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompiladorDeLenguaje.LanguageEngine
{
    static class Valentina
    {
        static public bool IsCodePaused = true;
        static public RichTextBox Output;
        static public bool User_Writing=false;
        static public string User_Input = string.Empty;
        static public KeyEventHandler Event_Write_Output = (object sender, KeyEventArgs e) =>
        {
            if(User_Writing)
            {

            }
        };
        static public void MostrarFunction(string text)
        {

            Output.Text = Output + text;
        }

        static public void LeerFunction(NonPrimitiveArray<char>  variable)
        {
            IsCodePaused = true;
            User_Writing = true;
            Output.KeyDown += Event_Write_Output;
        }

        static public void compileCode()
        {
            new Thread(new ThreadStart(() =>
            {
                string code = Output.Text;
                int code_end = code.Length;
                int code_cursor = 0;
                string word = String.Empty; 
                while (code_cursor <= code_end)
                {
                    word.
                    if (code[code_cursor] == ' ') continue;
                    switch(code[code_cursor])
                    {
                        case ''
                    }
                }
            })).Start();
        }

        static public void runCode()
        {
            new Thread(new ThreadStart(() =>
            {

            })).Start();
        }

        static private List<string> getAllWords(string code)
        {
            string word = string.Empty;
            List<Word> word_list = new List<Word>();
            for(int i=0; i<code.Length; i++)
            {
                if((code[i] >= 40 && code[i] <= 47) || (code[i] >= 59 && code[i] <= 62))
                {
                    if(!string.IsNullOrEmpty(word)) word_list.Add(new Word(word) { IsNameWord = true });
                    switch(code[i])
                    {
                        case '+': word_list.Add(new Word("" + code[i]) { IsArithmeticOperator = true }); break; 
                        case '-': word_list.Add(new Word("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '/': word_list.Add(new Word("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '*': word_list.Add(new Word("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '&': word_list.Add(new Word("" + code[i]) { IsArithmeticOperator = true }); break;
                    }
                    word = "";
                    continue;
                }
                if(code[i]=='-')
                {
                    word_list.Add("" + code[i]);
                    if(code[i+1]>=48 && code[i+1]<=57)
                    {
                        string num = string.Empty;
                        int new_i = i+1;
                        while(code[new_i] >= 48 && code[new_i] <= 57)
                        {
                            num += code[new_i];
                            new_i++;
                        }

                    }
                    else
                    {

                    }
                }
                if ((code[i] == ' ' || code[i] == '\n') && String.IsNullOrEmpty(word))
                {
                    continue;
                }
                if (code[i] == ' ' || code[i] == '\n')
                {
                    word_list.Add(new Word(word) { IsNameWord = true });
                    word = "";
                    continue;
                }
                word += code[i];
            }
        }
    }
}
