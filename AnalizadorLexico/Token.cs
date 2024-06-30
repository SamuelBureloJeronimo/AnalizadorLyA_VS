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
        private String component;

        public Token(String lexema, String component)
        {
            this.lexema = lexema;
            this.component = component;
        }

        public String getLex()
        {
            return this.lexema;
        }

        public String getComp()
        {
            return this.component;
        }

    }
}
