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
            salida1.Text = String.Join(", ",solucionFinal);
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
            entrada.Text = entrada.Text + "Ɛ";
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            entrada.Text = entrada.Text + "^*";
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            entrada.Text = entrada.Text + "^+";
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            entrada.Text = entrada.Text + "^";
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            entrada.Text = entrada.Text + "|";
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            entrada.Text = entrada.Text + ".";
        }
    }
}
