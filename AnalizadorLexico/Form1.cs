using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
        AnalizadorLexico al = new AnalizadorLexico();

        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            List<String> solucionFinal = al.solucionFinal(al.IdentificarOrden(al.AnalizarCadena(entrada.Text)));
            salida1.Text = String.Join(", ",solucionFinal).Replace("Ɛ, ", "");
            salida2.Text = al.analisisLexico;
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            salida1.Text = "";
            salida2.Text = "";
            entrada.Text = "";
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, "Ɛ", 0);
            entrada.SelectionStart = index + 1;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, "^*", 0);
            entrada.SelectionStart = index + 2;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, "^+", 0);
            entrada.SelectionStart = index+2;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private String ingresar(int pos, String cad, String extr, int i)
        {
            if (pos == i)
                return extr+cad;
            return cad.Substring(0, 1) + ingresar(pos, cad.Substring(1), extr, i+1);
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, "^", 0);
            entrada.SelectionStart = index + 1;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, "|", 0);
            entrada.SelectionStart = index + 1;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            int index = entrada.SelectionStart;
            entrada.Text = ingresar(index, entrada.Text, ".", 0);
            entrada.SelectionStart = index + 1;
            entrada.SelectionLength = 0;
            entrada.Focus();
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            salida2.Text = "";
            String res = "";
            for (int i = cadenaInt.Text.Length; i > 0; i--)
            {
                res += "S = "+cadenaInt.Text.Substring(0, i)+"\n";
            }
            res += "S = Ɛ";
            salida1.Text = res;
        }

        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            salida2.Text = "";
            String res = "";
            for (int i = 0; i < cadenaInt.Text.Length; i++)
            {
                res += "S = " + cadenaInt.Text.Substring(i) + "\n";
            }
            res += "S = Ɛ";
            salida1.Text = res;
        }
    }
}
