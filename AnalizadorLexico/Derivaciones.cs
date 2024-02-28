using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    public class Derivaciones
    {
        public List<String> content = new List<String>();
        public int orden;

        public void add(List<String> content, int orden)
        {
            this.content = content;
            this.orden = orden;
        }
    }
}
