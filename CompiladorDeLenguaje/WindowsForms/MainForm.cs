using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompiladorDeLenguaje.LanguageEngine;

namespace CompiladorDeLenguaje.WindowsForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Valentina.Output = txt_output;
        }

        private void compileCodeClick(object sender, EventArgs e)
        {
            Valentina.compileCode();
        }

        private void runCodeClick(object sender, EventArgs e)
        {
        }
    }
}
