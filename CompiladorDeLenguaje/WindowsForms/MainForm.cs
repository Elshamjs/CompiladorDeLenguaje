using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private int TabSize = 4;
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
        private string example_hello_world = "Global:\r\n{\r\n    Entero aux1=0;\r\n    Entero aux2=0;\r\n    Entero aux3=0;\r\n}\r\nFuncion Entero fib(Entero n, Binario print)\r\n{\r\n    Entero ret=0;\r\n    Si((n>0)) Hace\r\n    {\r\n        Si((n<2)) Hace\r\n        {\r\n            $ret=1;\r\n        }\r\n        Sino\r\n        {\r\n            $aux3= (n-1);\r\n            $aux1= #fib(aux3, Falso);\r\n            $aux3= (n-2);\r\n            $aux2= #fib(aux3, Falso);\r\n        }\r\n        Si(print) Hace\r\n        {\r\n            $aux3= (n-1);\r\n            #fib(aux3, Verdadero);\r\n            #Mostrar(ret);\r\n        }\r\n    }\r\n    Retorna ret;\r\n}\r\nCodigo:\r\n{\r\n    #fib(20, Verdadero);\r\n    #SaltoLinea();\r\n}";
        public MainForm()
        {
            InitializeComponent();
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
            Valentina.Input.KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.KeyCode == Keys.Tab)
                {
                    int po = Valentina.Input.SelectionStart;
                    Valentina.Input.Text = Valentina.Input.Text.Insert(Valentina.Input.SelectionStart, new String(' ', TabSize));
                    Valentina.Input.Select(po + TabSize, 0);
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

    }
}
