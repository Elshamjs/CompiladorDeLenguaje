using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompiladorDeLenguaje.LanguageEngine;
using CompiladorDeLenguaje.Structures;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CompiladorDeLenguaje.WindowsForms
{

    public partial class MainForm : Form
    {
        private Regex regex = new Regex("^\\s*(Caracter)(\\[[0-9]+\\])(\\[[0-9]+\\])\\s+[a-zA-Z_]+\\s*;$");
        static private int input_capture_cont= 0;
        static private KeyPressEventHandler CapturingEvent= (object s, KeyPressEventArgs eve) =>
        {
            if (eve.KeyChar == 13)
            {
                Valentina.Output.KeyPress -= CapturingEvent;
                Valentina.Output.ReadOnly = true;
                Valentina.CapturingUserInput = false;
                eve.Handled = true;
                return;
            }
            if(eve.KeyChar == 8)
            {
                if(input_capture_cont<=0)
                {
                    eve.Handled = true;
                    return;
                }
                else
                {
                    input_capture_cont--;
                    Valentina.CaptureString= Valentina.CaptureString.Remove(Valentina.CaptureString.Length - 1);
                    Valentina.Output.Text=Valentina.Output.Text.Remove(Valentina.Output.Text.Length - 1);
                    Valentina.Output.Select(Valentina.Output.Text.Length, 0);
                    eve.Handled = true;
                    return;
                }
            }
            if(eve.KeyChar >= ' ' && eve.KeyChar <= '~')
            {
                Valentina.CaptureString += eve.KeyChar;
                input_capture_cont++;
                Valentina.Output.Text += eve.KeyChar;
                Valentina.Output.Select(Valentina.Output.Text.Length, 0);
            }
            eve.Handled=true;
        };
        private string example_hello_world = "\r\n\r\nGlobal:{}\r\nCodigo:\r\n{\r\n    #MostrarLn(\"Hola Mundo\");\r\n}";
        public MainForm()
        {
            InitializeComponent();
            this.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WindowsForms\img", @"ICON.ico"));
            Valentina.Window = this;
            Valentina.Output = txt_output;
            Valentina.Input = txt_input;
            Valentina.Output.KeyDown += (object sender, KeyEventArgs e) =>
            {
                if(e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
                
            };

            /*Valentina.Input.KeyUp += (object sender, KeyEventArgs e) =>
            {
                Valentina.Output.Text = regex.IsMatch(Valentina.Input.Text).ToString();
            };*/
            Valentina.Input.Text = example_hello_world;

        }

        

        private void compileCodeClick(object sender, EventArgs e)
        {
            if (!Valentina.ProgramRunning)
            {
                Valentina.compileProgram();
            }
            else
            {
                MessageBox.Show("El programa ya se esta ejecutando");
            }
        }
        public void syncStartCapturingUserInput()
        {
            Valentina.CapturingUserInput = true;
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(syncStartCapturingUserInput));
                return;
            }
            txt_output.ReadOnly = false;
            txt_output.KeyPress += CapturingEvent;
        }

        public void syncClearOutput()
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(syncClearOutput));
                return;
            }
            txt_output.Text = "";
        }
        public void syncAppendToOutput(string value)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(syncAppendToOutput), new object[] { value });
                return;
            }
            txt_output.Text += value;
        }
        public void syncReadOnlyOutput(bool val)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<bool>(syncReadOnlyOutput), new object[] { val });
                return;
            }
            txt_output.ReadOnly = val;
        }
        public void syncReadOnlyInput(bool val)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<bool>(syncReadOnlyInput), new object[] { val });
                return;
            }
            txt_input.ReadOnly = val;
        }
        public void ProgramFails(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        private void runCodeClick(object sender, EventArgs e)
        {
            try
            {
                if (!Valentina.ProgramRunning)
                {
                    Valentina.runProgram();
                }
                else
                {
                    MessageBox.Show("El programa ya se esta ejecutando");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    \r\n}\r\n\r\nCodigo:\r\n{\r\n    \r\n}";
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global: \\~ Aca se declara las variables globales del codigo ~\\\r\n{\r\n    \\~ Estas son todas las formas posibles de inicializar variables en VALENTINA~\\ \\~\r\n    Entero aa;\r\n    Entero int=10;\r\n    Entero eint=-10;\r\n    Decimal float= 0.5;\r\n    Decimal efloat=-0.5;\r\n    Binario bool= Falso; \r\n    Caracter char= 'A';\r\n    Caracter[] buffer= Caracter[100];\r\n    Entero[] intarray = Entero[100];\r\n    Decimal[] floatarray= Decimal[100];\r\n    Binario[] boolarray=  Binario[100]; \r\n    Caracter[][] bibuffer=Caracter[10][10];\r\n    Entero[][] biintarray=Entero[10][10];\r\n    Decimal[][] bifloatarray=Decimal[10][10];\r\n    Binario[][] biboolarray=Binario[10][10];\r\n    Caracter[] hola= \"Hola Mundo\"; ~\\\r\n}\r\n \\~ De esta forma se declara una funcion, despues de la palabra reservada funcion va el tipo de dato, luego el nombre y los parametros\r\nFuncion Entero hola(Caracter[] hola_ss)\r\n{\r\n    Entero a= 0;\r\n    Retorna a;\r\n    #MostrarLn(hola_ss);\r\n}\r\n~\\\r\nCodigo: \\~Siempre debes declarar la etiqueta \"Codigo:\" en tu programa, aca inicia el funcionamiento del mismo~\\\r\n{\r\n  \\~Entero a= -2;\r\n    #MostrarLn(a);\r\n    Decimal b= -0.4;\r\n    #MostrarLn(b);\r\n    Caracter c= 'b';      Estas son algunas forma de asginar variables\r\n    #MostrarLn(c);        En VALENTINA solo se puede por el momento en la version actual hacer operaciones de unicamente dos\r\n    Binario d= Falso;     operando, los operadores logicos son: Y O (No)\r\n    #MostrarLn(d);        Los Operadores comparadores son: < > <= >= == !=\r\n    #hola(hola);          Y los operadores aritmeticos son: + - * /\r\n~\\\r\n    \\~ En VALENTINA existen diversas funcion propias del lenguaje como en muchos otros lenguajes, por ejemplo Capturar,\r\n    para producir datos de entrada, o las diferentes formas de salida como son: SaltoLinea(), Mostrar(variable), MostrarLn(variable).\r\n    Otras utiles son Medir(Arreglo, Entero), esta funcion te permite saber el tamaño del arreglo~\\\r\n\r\n    \\~ Estas son la estructuras de control de Valentina, tambien si te sientes valiente puedes probar a utilizar recursividad~\\\r\n    \\~ Para(Entero i=0; SoloSi((i<max)); Asigna $i=(i+1); ) Hace {} ~\\\r\n    \\~ Si((lim>0)) Hace {} ~\\\r\n    \\~ Mientras((cont<=10)) Hace ~\\\r\n    \r\n    #Mostrar(\"Hola Mundo\"); \\~ Por ultimo pero no menos importante recuerda acabar todas las lineas con \";\" ~\\\r\n    \r\n   \\~ PRESIONA EL BOTON DE COMPILAR Y LUEGO EL DE EJECUTAR ~\\\r\n}";
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:{}\r\n\r\nFuncion Entero fib(Entero x)\r\n{\r\n    Si((x==0)) Hace\r\n    {\r\n        Retorna 0;\r\n    }\r\n    Si((x==1)) Hace\r\n    {\r\n        Retorna 1;\r\n    }\r\n    Entero a;\r\n    Entero b;\r\n    Entero ret;\r\n\r\n    $a= (x-1);\r\n    $b= (x-2);\r\n\r\n    $a= #fib(a);\r\n    $b= #fib(b);\r\n\r\n    $ret=(a+b);\r\n\r\n    Retorna ret;\r\n}\r\n\r\nCodigo:\r\n{\r\n    Entero result;\r\n    $result= #fib(20);\r\n    #MostrarLn(result);\r\n}";
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            txt_input.Text = "\r\nGlobal:\r\n{\r\n    Entero aa;\r\n    Entero int=10;\r\n    Entero eint=-10;\r\n    Decimal float= 0.5;\r\n    Decimal efloat=-0.5;\r\n    Binario bool= Falso; \r\n    Caracter char= 'A';\r\n    Caracter[] buffer= Caracter[100];\r\n    Entero[] intarray = Entero[100];\r\n    Decimal[] floatarray= Decimal[100];\r\n    Binario[] boolarray=  Binario[100]; \r\n    Caracter[][] bibuffer=Caracter[10][10];\r\n    Entero[][] biintarray=Entero[10][10];\r\n    Decimal[][] bifloatarray=Decimal[10][10];\r\n    Binario[][] biboolarray=Binario[10][10];\r\n    Caracter[] hola= \"Hola Mundo\"; \r\n}\r\nFuncion Entero sum(Entero num, Entero cont)\r\n{\r\n    $bool= (cont==11);\r\n    Si(bool) Hace\r\n    {\r\n        Retorna num;\r\n    }\r\n    Sino\r\n    {\r\n        $num=(cont+num);\r\n        $cont=(cont+1);\r\n        $int=#sum(num, cont);\r\n        Retorna int;\r\n    }\r\n}\r\nCodigo:\r\n{\r\n    Entero x= 0;\r\n    $x=#sum(0, 1);\r\n    #MostrarLn(x);\r\n}\r\n";
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    Entero eint=-10;\r\n    Decimal float= 0.5;\r\n    Binario bool= Falso; \r\n    Caracter char= 'A';\r\n}\r\nCodigo:\r\n{\r\n    Entero sum= 0;\r\n    Entero cont=1;\r\n    Mientras((cont<=10)) Hace\r\n    {\r\n        #MostrarLn(cont);\r\n        Si((cont==5)) Hace\r\n        {\r\n            #Mostrar(\"Mientras acabo en: \");\r\n            #MostrarLn(cont);\r\n            Sale;\r\n        }\r\n        Si((cont==10)) Hace\r\n        {\r\n            #Mostrar(\"Se brinco el \");\r\n            #MostrarLn(cont);\r\n            Continua;\r\n        }\r\n        $sum=(sum+cont);\r\n        $cont= (cont+1);\r\n    }\r\n    #Mostrar(\"Resultado Final: \");\r\n    #MostrarLn(sum);\r\n}";
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    Entero size1=0;\r\n    Entero size2=0;\r\n    Caracter[][] hola= Caracter[2][2];\r\n}\r\nCodigo:\r\n{\r\n    $hola[0][0]= 'H';\r\n    $hola[0][1]= 'O';\r\n    $hola[1][0]= 'L';\r\n    $hola[1][1]= 'A';\r\n    $size1= #Medir(hola, 0);\r\n    Para(Entero i=0; SoloSi((i<size1)); Asigna $i=(i+1);) Hace\r\n    {\r\n        $size2= #Medir(hola, 1);\r\n        Para(Entero t=0; SoloSi((t<size2)); Asigna $t=(t+1);) Hace\r\n        {\r\n            #Mostrar(\"[\");\r\n\t\t#Mostrar(hola[i][t]);\r\n            #Mostrar(\"]\");\r\n        }\r\n        #SaltoLinea();\r\n    }\r\n}";
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    Caracter[] INFO= \"Este programa imprime la sucesion de fibonacci desde 1 hasta N, siendo este ultimo un numero ingresado por el usuario\";\r\n    Entero aux= 1;\r\n    Entero fib=0;\r\n    Entero lim;\r\n}\r\nCodigo:\r\n{\r\n    #MostrarLn(INFO);\r\n    #SaltoLinea();\r\n    #SaltoLinea();\r\n    #Mostrar(\"Ingrese un numero para la sucesion de fibonacci: \");\r\n    #Capturar(lim);\r\n    Si((lim>0)) Hace\r\n    {\r\n        Para(Entero init =1; SoloSi((init<=lim)); Asigna $init=(init+1);) Hace\r\n        {\r\n            #Mostrar(\"[\");\r\n            #Mostrar(fib);\r\n            #Mostrar(\"] \");\r\n            $aux= (aux+fib);\r\n            $fib=(aux-fib);\r\n        }\r\n    }\r\n    Sino\r\n    {\r\n        #MostrarLn(\"El numero debe ser mayor a cero!!\");\r\n\r\n    }\r\n    #SaltoLinea();\r\n}\r\n";
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    Entero eint=-10;\r\n    Decimal float= 0.5;\r\n    Binario bool= Falso; \r\n    Caracter char= 'A';\r\n}\r\nCodigo:\r\n{\r\n    Entero sum= 0;\r\n    Para(Entero i= 100; SoloSi((i<=1000)); Asigna $i=(i+100);) Hace \r\n    {\r\n        $sum= (sum+i);        \r\n    }\r\n    #MostrarLn(sum);\r\n}";
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:\r\n{\r\n    Caracter[] hola;\r\n}\r\nFuncion Caracter[] unir_txt(Caracter[] sx, Caracter[] sy)\r\n{\r\n    Caracter[] buffer= Caracter[50];\r\n    Entero buffer_cont=0;\r\n    Entero max;\r\n    $max= #Medir(sx, 0);\r\n    Para(Entero i=0; SoloSi((i<max)); Asigna $i=(i+1); ) Hace\r\n    {\r\n        $buffer[buffer_cont]=sx[i];\r\n        $buffer_cont=(buffer_cont+1);\r\n    }\r\n    $max= #Medir(sy, 0);\r\n    Para(Entero i=0; SoloSi((i<max)); Asigna $i=(i+1); ) Hace\r\n    {\r\n        $buffer[buffer_cont]=sy[i];\r\n        $buffer_cont=(buffer_cont+1);\r\n    }\r\n    Retorna buffer;\r\n}\r\nCodigo:\r\n{\r\n    Caracter[] txta= \"Hola \";\r\n    Caracter[] txtb= \"Mundo\";\r\n    $hola= #unir_txt(txta, txtb);\r\n    #MostrarLn(hola);\r\n}";
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            txt_input.Text = "Global:{}\r\n\r\nCodigo:\r\n{\r\n    Si(Verdadero) Hace\r\n    {\r\n        Entero prueba=0;\r\n    }\r\n    $prueba=1;\r\n}";
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            txt_input.Text = @"\~
                                               Natalia Rojas Morales
                                            Luis Diego Jimenez Sanchez
                                        Universidad Nacional de Costa Rica 
                                        EIF400: Paradigmas de Programación 
                                               Josías Chaves Murillo 
                                             22 de noviembre del 2022
~\";
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
