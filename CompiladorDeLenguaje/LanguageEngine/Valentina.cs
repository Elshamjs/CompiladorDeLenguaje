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


        /*
        Global:
        {
            Entero int=34;
            Decimal float;
            Binario bool;
            Caracter char;
            Caracter[100] buffer;
        }
        
        Funcion Entero contarUnidadesDosDigitos(Entero num)
        {
            Entero aux;
            $aux= #modulo(num, 10); // esto si 
            Retorna $aux;
        }

        Funcion principal() 
        {
            Entero numero_usuario;
            Entero unidades;
            #imprimir_holamundo();
            #Mostrar("Escribe un numero de dos digitos: ");
            #Capturar($numero_usuario);
            $unidades= #contarUnidadesDosDigitos($numero_usuario);
            Mostrar(unidades);

            /~ Si() Hace{} sino{} ~/ 
            /~ Mientras() Hace{} ~/
            /~ Para(Entero variable=0) SoloSi($variable<10) Asigna($variable++) Hace {} ~/
        }

        Funcion Nulo imprimir_holamundo()
        {
            $buffer= "Hola Mundo\n";
            $num= num2;  // esto si 
            $num= ¿num1 + num2?;  // esto si 
            Mostrar($buffer);
            Retorna Nulo;
        }

        Funcion Entero modulo(Entero num1, Entero num2)
        {
            Retorna ¿¿$num1 + $num ? + $num ?;
        }

        */

        static private List<string> getAllWords(string code)
        {
            string word = string.Empty;
            List<Lexeme> word_list = new List<Lexeme>();
            for(int i=0; i<code.Length; i++)
            {
                if((code[i] >= 40 && code[i] <= 47) || (code[i] >= 59 && code[i] <= 62))
                {
                    if(!string.IsNullOrEmpty(word)) word_list.Add(new Lexeme(word) { IsNameWord = true });
                    switch(code[i])
                    {
                        case '+': word_list.Add(new Lexeme("" + code[i]) { IsArithmeticOperator = true }); break; 
                        case '-': word_list.Add(new Lexeme("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '/': word_list.Add(new Lexeme("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '*': word_list.Add(new Lexeme("" + code[i]) { IsArithmeticOperator = true }); break;
                        case '&': word_list.Add(new Lexeme("" + code[i]) { IsArithmeticOperator = true }); break;
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
                    word_list.Add(new Lexeme(word) { IsNameWord = true });
                    word = "";
                    continue;
                }
                word += code[i];
            }
        }
    }
}
