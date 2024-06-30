using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    public class AnalizadorLexico
    {
        public List<Derivaciones> Separaciones;
        public OperacionesLR op = new OperacionesLR();
        public String analisisLexico = "";
        public String salidaConsola = "";
        public List<String> solucionFinal(List<Derivaciones> Separaciones)
        {
            salidaConsola += "\n\n*** Se eliminan cadenas vacías ***";
            Console.WriteLine("== Eliminar las cadenas vacías ==");
            int rftg = 0;
            for (int i = 0; i < Separaciones.Count - 1; i++)
            {
                if (Separaciones[i].content[0].Equals("Ɛ") && Separaciones[i].content.Count == 1)
                {
                    if (i == 0)
                    {
                        salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[i].content);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i].content));
                        Separaciones.RemoveAt(i);
                        salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[i+1].content);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i+1].content));
                        Separaciones.RemoveAt(i);
                        rftg = 0;
                    }
                    else
                    {
                        salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[i].content);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i].content));
                        Separaciones.RemoveAt(i);
                        rftg = 0;
                        i--;
                    }

                }
                Console.WriteLine(Separaciones[i].content[0]);
                if (Separaciones[0].content[0].Equals("|") || Separaciones[0].content[0].Equals("."))
                {
                    Separaciones.RemoveAt(0);
                }
                //Si se repite el signo en el siguiente
                if (Separaciones[i].content[0].Equals("|"))
                {
                    if (rftg > 0)
                    {
                        salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[i].content);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i].content));
                        Separaciones.RemoveAt(i);
                        i--;
                        rftg = 1;
                    }
                    else
                    {
                        rftg++;
                    }
                }
                else if (Separaciones[i].content[0].Equals("."))
                {
                    if (rftg > 0)
                    {
                        salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[i].content);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i].content));
                        Separaciones.RemoveAt(i);
                        i--;
                        rftg = 1;
                    }
                    else
                    {
                        rftg++;
                    }
                }
                else
                {
                    rftg = 0;
                }
            }
            if (Separaciones[Separaciones.Count - 1].content[0].Equals("Ɛ") && Separaciones[Separaciones.Count - 1].content.Count == 1)
            {

                salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content);
                Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content));
                Separaciones.RemoveAt(Separaciones.Count - 1);

                salidaConsola += "\nSe removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content);
                Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content));
                Separaciones.RemoveAt(Separaciones.Count - 1);
            }

            Console.WriteLine("=== CASI FINAL ===\n");
            Console.WriteLine("LISTO PARA SU RESOLUCIÓN");
            salidaConsola += "\n\n==============================";
            salidaConsola += "\nLISTO PARA SU RESOLUCIÓN";
            for (int i = 0; i < Separaciones.Count; i++)
            {
                salidaConsola += "\n"+String.Join(", ", Separaciones[i].content);
                Console.WriteLine(String.Join(", ",Separaciones[i].content));
            }
            salidaConsola += "\n==========================\n";
            Console.WriteLine("\n=========================");
            int vuelta = 0;
            if (Separaciones.Count != 1)
            {
                while (true)
                {
                    for (int i = vuelta; i < Separaciones.Count; i++)
                    {
                        Console.WriteLine("Vuelta: " + vuelta);
                        if (Separaciones[i].content[0].Equals("."))
                        {

                            Console.WriteLine("RESOLVER[Concatenación]:\n");
                            salidaConsola += "\nRESOLVER[Concatenación]:\n";

                            salidaConsola += "--> Cadena 1:" + String.Join(", ", Separaciones[i - 1].content) + "\n";
                            Console.WriteLine("--> Cadena 1:" + String.Join(", ", Separaciones[i - 1].content));

                            Console.WriteLine("--> Cadena 2: " + String.Join(", ", Separaciones[i + 1].content));
                            salidaConsola += "--> Cadena 2: " + String.Join(", ", Separaciones[i + 1].content) + "\n";

                            Separaciones[i + 1].content = op.concatenarCadenas(Separaciones[i - 1].content, Separaciones[i + 1].content);
                            Console.WriteLine("CONCATENAR: " + String.Join(", ", Separaciones[i + 1].content));
                            vuelta = i + 2;
                            Console.WriteLine("=======");


                            break;
                        }
                        else if (Separaciones[i].content[0].Equals("|"))
                        {

                            Console.WriteLine("=======");
                            salidaConsola += "\nRESOLVER[Unión]:\n";

                            Console.WriteLine("Cad1:" + String.Join(", ", Separaciones[i - 1].content));
                            salidaConsola += "--> Cadena 1:" + String.Join(", ", Separaciones[i - 1].content) + "\n";

                            Console.WriteLine("Cad2: " + String.Join(", ", Separaciones[i + 1].content));
                            salidaConsola += "--> Cadena 2: " + String.Join(", ", Separaciones[i + 1].content) + "\n";

                            Separaciones[i + 1].content = op.unirCadenas(Separaciones[i - 1].content, Separaciones[i + 1].content);
                            Console.WriteLine("RESULTADO: " + String.Join(", ", Separaciones[i + 1].content));
                            vuelta = i + 2;
                            Console.WriteLine("=======");

                            break;
                        }
                    }
                    if (vuelta >= Separaciones.Count)
                    {
                        break;
                    }
                }
            }
            salidaConsola += "\n\n*** CADENA RESUELTA ***";
            Console.WriteLine("Resultado:");
            Console.WriteLine(String.Join(", ", Separaciones[Separaciones.Count - 1].content));

            return Separaciones[Separaciones.Count - 1].content;
        }
        public List<Derivaciones> IdentificarOrden(List<Token> tokensAnalizados)
        {
            for (int i = 0; i < tokensAnalizados.Count; i++)
            {
                //Juntar una exponenciacion cuando la base es un Identificador
                if (tokensAnalizados[i].getComp().Equals("Identificador") || tokensAnalizados[i].getLex().Equals("Ɛ"))
                {
                    if ((i + 1) < tokensAnalizados.Count)
                    {
                        if (tokensAnalizados[i + 1].getLex().Substring(0, 1).Equals("^"))
                        {
                            salidaConsola += "2do Orden:   " + tokensAnalizados[i].getLex() + tokensAnalizados[i + 1].getLex()+"\n";
                            Console.WriteLine("** 2do Orden: " + tokensAnalizados[i].getLex() + tokensAnalizados[i + 1].getLex());
                            List<String> lst = op.resolver2doOrden(tokensAnalizados[i].getLex(), tokensAnalizados[i + 1].getLex().Substring(1));
                            Derivaciones dr2 = new Derivaciones();
                            dr2.add(lst, 4);
                            Separaciones.Add(dr2);
                        }
                        else
                        {
                            salidaConsola += "3er Orden:   " + tokensAnalizados[i].getLex() + "\n";
                            Console.WriteLine("*** 3er Orden: " + tokensAnalizados[i].getLex());
                            List<String> lst = new List<String>();
                            if (tokensAnalizados[i].getLex().Equals("Ɛ"))
                            {
                                lst.Add(tokensAnalizados[i].getLex());
                                Derivaciones dr2 = new Derivaciones();
                                dr2.add(lst, 3);
                                Separaciones.Add(dr2);
                            }
                            else
                            {
                                lst.Add(tokensAnalizados[i].getLex());
                                Derivaciones dr2 = new Derivaciones();
                                dr2.add(lst, 3);
                                Separaciones.Add(dr2);
                            }
                        }
                    }
                    else
                    {
                        salidaConsola += "3er Orden:   " + tokensAnalizados[i].getLex()+"\n";
                        Console.WriteLine("*** 3er Orden: " + tokensAnalizados[i].getLex());
                        List<String> lst = new List<String>();
                        if (tokensAnalizados[i].getLex().Equals("Ɛ"))
                        {
                            lst.Add(tokensAnalizados[i].getLex());
                            Derivaciones dr2 = new Derivaciones();
                            dr2.add(lst, 3);
                            Separaciones.Add(dr2);
                        }
                        else
                        {
                            lst.Add(tokensAnalizados[i].getLex());
                            Derivaciones dr2 = new Derivaciones();
                            dr2.add(lst, 3);
                            Separaciones.Add(dr2);
                        }
                    }
                }
                //Juntar una exponenciacion cuando la base es un parentesis
                else if (tokensAnalizados[i].getLex().Equals("("))
                {
                    int ind = SearchIndex(tokensAnalizados, 1, 0, i + 1);
                    List<string> arrlt = new List<string>();
                    List<Token> othr = new List<Token>();
                    for (int j = i; j <= ind; j++)
                    {
                        arrlt.Add(tokensAnalizados[j].getLex());
                        othr.Add(tokensAnalizados[j]);
                    }
                    salidaConsola += "1er Orden:   " + String.Join("", arrlt)+"\n";
                    Console.WriteLine("* 1er Orden: " + string.Join(", ", arrlt));
                    if (arrlt[arrlt.Count - 1].Substring(0, 1).Equals("^"))
                    {
                        List<Derivaciones> dr = op.resolver1erOrden(othr, arrlt[arrlt.Count - 1].Substring(1));
                        Separaciones.Add(dr[0]);
                    }
                    else
                    {
                        List<Derivaciones> dr = op.resolver1erOrden(othr, "");
                        for (int j = 0; j < dr.Count; j++)
                        {
                            Separaciones.Add(dr[j]);
                        }
                    }
                    //Separaciones.Add(dr2);
                    i = ind;
                }
                else if (tokensAnalizados[i].getLex().Equals("."))
                {
                    Derivaciones dr2 = new Derivaciones();
                    List<String> lst = new List<String>();
                    lst.Add(".");
                    dr2.add(lst, 4);
                    Separaciones.Add(dr2);
                    salidaConsola += "4to Orden:   .\n";
                    Console.WriteLine("**** 4to Orden: .");
                }
                else if (tokensAnalizados[i].getLex().Equals("|"))
                {
                    Derivaciones dr2 = new Derivaciones();
                    List<String> lst = new List<String>();
                    lst.Add("|");
                    dr2.add(lst, 4);
                    Separaciones.Add(dr2);
                    salidaConsola += "4to Orden:   |\n";
                    Console.WriteLine("**** 4to Orden: |");
                }
            }
            Console.WriteLine("=== Final ===\n");
            salidaConsola += "\n\nORDEN RESUELTO -->\n";
            Console.WriteLine("LISTO PARA SU RESOLUCIÓN");
            foreach (Derivaciones derivacion in Separaciones)
            {
                salidaConsola += String.Join(", ", derivacion.content)+"\n";
                Console.WriteLine(String.Join(", ", derivacion.content));
            }
            Console.WriteLine("\n");
            return Separaciones;
        }
        private int SearchIndex(List<Token> tokensAnalizados, int p_a, int p_c, int index)
        {
            if (p_a == p_c)
            {
                // Cuando ya se cerraron todos los paréntesis se apertura y de cierre
                // Verifica si tiene un exponente al final
                if (index < tokensAnalizados.Count)
                {
                    if (tokensAnalizados[index].getLex().Substring(0, 1).Equals("^", StringComparison.OrdinalIgnoreCase))
                    {
                        return index;
                    }
                }
                return index - 1;
            }
            else if (tokensAnalizados[index].getLex().Equals("("))
            {
                // Cuando se encuentra un "("
                return SearchIndex(tokensAnalizados, p_a + 1, p_c, index + 1);
            }
            else if (tokensAnalizados[index].getLex().Equals(")"))
            {
                // Cuando se encuentra un ")"
                return SearchIndex(tokensAnalizados, p_a, p_c + 1, index + 1);
            }
            else
            {
                return SearchIndex(tokensAnalizados, p_a, p_c, index + 1);
            }
        }
        




        public List<Token> AnalyzeString(string cadena)
        {
            //Almacena la cadena convertida en Tokens
            List<Token> stringConverted = new List<Token>();

            //Bucle que recorre el tamaño de la cadena
            while (cadena.Length > 0)
            {
                //Obtiene el primer caracter de la cadena
                string c = cadena.Substring(0, 1);

                //Verifica si el caracter es un IDENTIFICADOR
                if (Regex.IsMatch(c, @"^[0-9]*$") || Regex.IsMatch(c, @"^[A-Z]*$") || Regex.IsMatch(c, @"^[a-z]*$"))
                {
                    //Busca si el siguiente caracter es un identificador tambien
                    string ident = c + SearchNextAZ_09(cadena.Substring(1));
                    //Inicializa un nuevo token y lo agrega como identificador
                    stringConverted.Add(new Token(ident, "Identificador"));
                    //Si se encontró más identificadores es necesario eliminar la parte ya leida
                    cadena = cadena.Substring(ident.Length);
                    continue;
                }

                // Verifica de el caracter es un OPERADOR
                if (cadena.Length > 1 && c.Equals("^"))
                {
                    //Obtiene el valor del exponente
                    string expType = cadena.Substring(1, 1);

                    //Valida si es una cerradura de Kleene o una Positiva
                    if (expType.Equals("*") || expType.Equals("+")) {
                        //Inicializa un nuevo token y lo agrega como operador
                        stringConverted.Add(new Token(cadena.Substring(0, 2), "Operador"));
                        /* Verifica si aun quedan carateres que analizar...
                         * De ser asi valida si el siguiente signo es otro '*'
                         */
                        if (cadena.Length > 2 && cadena.Substring(2, 1).Equals("*"))
                            cadena = cadena.Substring(3);//Recorre la cadena en 3 posiciones
                        else
                            cadena = cadena.Substring(2);//Recorre la cadena en 2 posiciones
                        continue;
                    }

                    //Valida si es una exponenciación
                    if (Regex.IsMatch(expType, @"^[0-9]*$")) {
                        //Busca si el siguiente caracter es un identificador tambien
                        string ident = c + SearchNextAZ_09(cadena.Substring(1));
                        //Inicializa un nuevo token y lo agrega como Operador
                        stringConverted.Add(new Token(ident, "Operador"));
                        //Recorre la cadena segun el tamaño de ident
                        cadena = cadena.Substring(ident.Length);
                        continue;
                    }
                    //Si no es ninguna de las anteriores entonces tiene un error de sintaxis
                    Console.WriteLine("Error de Sinstaxis");
                    continue;
                }
                //Valida los tokens sencillos
                if (c.Equals("(") || c.Equals(")")) {
                    stringConverted.Add(new Token(c, "Agrupador"));
                    cadena = cadena.Substring(1);
                }
                else if (c.Equals("|") || c.Equals(".")) {
                    stringConverted.Add(new Token(c, "Operador"));
                    cadena = cadena.Substring(1);
                }
                else if (c.Equals("Ɛ")) {
                    stringConverted.Add(new Token("Ɛ", "Constante"));
                    cadena = cadena.Substring(1);
                }
                else {   
                    stringConverted.Add(new Token(c, "No identificado"));
                    cadena = cadena.Substring(1);
                }
            }
            Console.WriteLine(leerAnalisis(stringConverted));
            return stringConverted;
        }

        /* 
         * Nivel 1: Resolver Union y concatenacion
         * Nivel 2: Resolver Exponenciación, kleen y cerradura positiva
         * Nivel 3: Resolver Agrupaciones
         * 
         * Con arboles de derivaciones ascendente y desendente, ahi esta la clave
         * 
         * Paso 1: Crear un método para resolver las concatenaciones y uniones
         * Paso 2: Crear un método para resolver las exponenciaciones
         * Paso 3: Crear un método para eliminar los parentesis
         *      Caso 1: Puede quedar un arreglo: [],[],[]...
         *      Caso 2: Puede quedar varios arreglos en uniones: ([],[]...|[],[]...|[],[]...) 
         */


        public string leerAnalisis(List<Token> analisis)
        {
            string final = "======= ANÁLISIS LEXICO =======\n";
            foreach (Token token in analisis)
            {
                final += "\n<"+token.getComp()+">"+"    "+token.getLex();
            }
            return final;
        }






        private string SearchNextAZ_09(string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return "";

            string c = cadena.Substring(0, 1);
            if (string.IsNullOrEmpty(cadena))
                return "";
            if (Regex.IsMatch(c, @"^[0-9]*$") || Regex.IsMatch(c, @"^[A-Z]*$") || Regex.IsMatch(c, @"^[a-z]*$"))
                return c + SearchNextAZ_09(cadena.Substring(1));
            
            return "";
            
        }

    }
}
