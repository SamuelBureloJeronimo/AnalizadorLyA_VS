using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<String> solucionFinal(List<Derivaciones> Separaciones)
        {
            Console.WriteLine("== Eliminar las cadenas vacías ==");
            int rftg = 0;
            for (int i = 0; i < Separaciones.Count - 1; i++)
            {
                if (Separaciones[i].content[0].Equals("Ɛ") && Separaciones[i].content.Count == 1)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i].content));
                        Separaciones.RemoveAt(i);
                        Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[i+1].content));
                        Separaciones.RemoveAt(i);
                        rftg = 0;
                    }
                    else
                    {
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
                Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content));
                Separaciones.RemoveAt(Separaciones.Count - 1);
                Console.WriteLine("Se removio:" + String.Join(", ", Separaciones[Separaciones.Count - 1].content));
                Separaciones.RemoveAt(Separaciones.Count - 1);
            }

            Console.WriteLine("=== CASI FINAL XD ===\n");
            Console.WriteLine("LISTO PARA SU RESOLUCIÓN");
            for (int i = 0; i < Separaciones.Count; i++)
            {
                Console.WriteLine(String.Join(", ",Separaciones[i].content));
            }
            Console.WriteLine("\n");
            Console.WriteLine("=========================");
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
                            Console.WriteLine("=======");
                            Console.WriteLine("Cad1:" + String.Join(", ", Separaciones[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", Separaciones[i + 1].content));
                            Separaciones[i + 1].content = op.concatenarCadenas(Separaciones[i - 1].content, Separaciones[i + 1].content);
                            Console.WriteLine("CONCATENAR: " + String.Join(", ", Separaciones[i + 1].content));
                            vuelta = i + 2;
                            Console.WriteLine("=======");
                            break;
                        }
                        else if (Separaciones[i].content[0].Equals("|"))
                        {
                            Console.WriteLine("=======");
                            Console.WriteLine("Cad1:" + String.Join(", ", Separaciones[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", Separaciones[i + 1].content));
                            Separaciones[i + 1].content = op.unirCadenas(Separaciones[i - 1].content, Separaciones[i + 1].content);
                            Console.WriteLine("UNIR: " + String.Join(", ", Separaciones[i + 1].content));
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

            Console.WriteLine("Resultado:");
            Console.WriteLine(String.Join(", ", Separaciones[Separaciones.Count - 1].content));

            Console.WriteLine("\n\nFinal");

            return Separaciones[Separaciones.Count - 1].content;
        }
        public List<Derivaciones> IdentificarOrden(List<Token> tokensAnalizados)
        {
            Separaciones = new List<Derivaciones>();
            Console.WriteLine("\n==== Indentificar el orden de resolución de la ecuación ====\n");

            //eliminarCadenaVacia();
            Console.WriteLine("=== Inicio ===");

            for (int i = 0; i < tokensAnalizados.Count; i++)
            {
                //Juntar una exponenciacion cuando la base es un Identificador
                if (tokensAnalizados[i].getCateg().Equals("Identificador") || tokensAnalizados[i].getLex().Equals("Ɛ"))
                {
                    if ((i + 1) < tokensAnalizados.Count)
                    {
                        if (tokensAnalizados[i + 1].getLex().Substring(0, 1).Equals("^"))
                        {
                            Console.WriteLine("** 2do Orden: " + tokensAnalizados[i].getLex() + tokensAnalizados[i + 1].getLex());
                            List<String> lst = op.resolver2doOrden(tokensAnalizados[i].getLex(), tokensAnalizados[i + 1].getLex().Substring(1));
                            Derivaciones dr2 = new Derivaciones();
                            dr2.add(lst, 4);
                            Separaciones.Add(dr2);
                        }
                        else
                        {
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
                    Console.WriteLine("**** 4to Orden: .");
                }
                else if (tokensAnalizados[i].getLex().Equals("|"))
                {
                    Derivaciones dr2 = new Derivaciones();
                    List<String> lst = new List<String>();
                    lst.Add("|");
                    dr2.add(lst, 4);
                    Separaciones.Add(dr2);
                    Console.WriteLine("**** 4to Orden: |");
                }
            }
            Console.WriteLine("=== Final ===\n");
            Console.WriteLine("LISTO PARA SU RESOLUCIÓN");
            foreach (Derivaciones derivacion in Separaciones)
            {
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
        public List<Token> AnalizarCadena(string cadena)
        {
            List<Token> tokensAnalizados = new List<Token>();
            analisisLexico = "==== ANALIZADOR LÉXICO ====\n";
            Console.WriteLine("==== ANALIZADOR LÉXICO By Samuel Burelos Jerónimo ====\n");

            while (cadena.Length > 0)
            {
                string c = cadena.Substring(0, 1);
                if (Regex.IsMatch(c, @"^[0-9]*$") || Regex.IsMatch(c, @"^[A-Z]*$") || Regex.IsMatch(c, @"^[a-z]*$"))
                {
                    string ident = c + SearchNextAZ_09(cadena.Substring(1));
                    analisisLexico += "\n<Identificador>       " + ident;
                    Console.WriteLine("<Identificador>       " + ident);
                    tokensAnalizados.Add(new Token(ident, "Identificador"));
                    cadena = cadena.Substring(ident.Length);
                }
                else
                {
                    if (cadena.Length > 1 && c.Equals("^"))
                    {
                        string lt = cadena.Substring(1, 1);
                        if (lt.Equals("*") || lt.Equals("+"))
                        {
                            analisisLexico += "\n<Operador>           " + cadena.Substring(0, 2);
                            Console.WriteLine("<Operador>           " + cadena.Substring(0, 2));
                            tokensAnalizados.Add(new Token(cadena.Substring(0, 2), "Operador"));
                            if (cadena.Length > 2 && cadena.Substring(2, 1).Equals("*"))
                                cadena = cadena.Substring(3);
                            else
                                cadena = cadena.Substring(2);
                        }
                        else if (Regex.IsMatch(lt, @"^[0-9]*$"))
                        {
                            string ident = c + SearchNextAZ_09(cadena.Substring(1));
                            Console.WriteLine("<Operador>           " + ident);
                            analisisLexico += "\n<Operador>           " + ident;
                            tokensAnalizados.Add(new Token(ident, "Operador"));
                            cadena = cadena.Substring(ident.Length);
                        }
                        else
                        {
                            Console.WriteLine("Error de Sinstaxis");
                        }
                    }
                    else if (c.Equals("(") || c.Equals(")"))
                    {
                        tokensAnalizados.Add(new Token(c, "Agrupador"));
                        analisisLexico += "\n<Agrupador>           " + c;
                        Console.WriteLine("<Agrupador>          " + c);
                        cadena = cadena.Substring(1);
                    }
                    else if (c.Equals("|") || c.Equals("."))
                    {
                        tokensAnalizados.Add(new Token(c, "Operador"));
                        analisisLexico += "\n<Operador>           " + c;
                        Console.WriteLine("<Operador>           " + c);
                        cadena = cadena.Substring(1);
                    }
                    else if (c.Equals("Ɛ"))
                    {
                        tokensAnalizados.Add(new Token("Ɛ", "Constante"));
                        Console.WriteLine("<Constante>          " + c);
                        analisisLexico += "\n<Constante>           " + c;
                        cadena = cadena.Substring(1);
                    }
                    else
                    {
                        tokensAnalizados.Add(new Token(c, "No identificado"));
                        Console.WriteLine("\n<DESCONOCIDO>        " + c);
                        analisisLexico += "<Desconocido>           " + c;
                        cadena = cadena.Substring(1);
                    }
                }
            }
            return tokensAnalizados;
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
