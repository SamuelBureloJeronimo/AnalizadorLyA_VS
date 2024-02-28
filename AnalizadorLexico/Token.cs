using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    public class Token
    {
        private String lexema;
        private String categ;

        public Token(String lexema, String categ)
        {
            this.lexema = lexema;
            this.categ = categ;
        }

        public String getLex()
        {
            return this.lexema;
        }

        public String getCateg()
        {
            return this.categ;
        }

    }
}
