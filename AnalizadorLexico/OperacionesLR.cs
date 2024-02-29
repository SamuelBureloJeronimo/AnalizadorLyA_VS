using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    public class OperacionesLR
    {
        public List<string> concatenarCadenas(List<string> cad1, List<string> cad2)
        {
            List<string> result = new List<string>();
            if (cad1.Count == 0)
            {
                return cad2;
            }
            else if (cad2.Count == 0)
            {
                return cad1;
            }
            foreach (var c1 in cad1)
            {
                foreach (var c2 in cad2)
                {
                    if (c1.Equals("Ɛ") && c2.Equals("Ɛ"))
                    {
                        result.Add("Ɛ");
                    }
                    else if (c1.Equals("Ɛ") && !c2.Equals("Ɛ"))
                    {
                        result.Add(c2);
                    }
                    else if (!c1.Equals("Ɛ") && c2.Equals("Ɛ"))
                    {
                        result.Add(c1);
                    }
                    else
                    {
                        result.Add(c1 + c2);
                    }
                }
            }
            return result;
        }

        public List<string> unirCadenas(List<string> cad1, List<string> cad2)
        {
            for (int i = 0; i < cad2.Count; i++)
            {
                cad1.Add(cad2[i]);
            }
            return cad1;
        }


        public List<Derivaciones> resolver1erOrden(List<Token> arr, string exp)
        {
            // Elimina los paréntesis de inicio y del final
            arr.RemoveAt(0);
            arr.RemoveAt(arr.Count - 1);
            AnalizadorLexico anl = new AnalizadorLexico();
            List<Derivaciones> result = anl.IdentificarOrden(arr);

            if (!exp.Equals(""))
            {
                Console.WriteLine("Namas Expongo y ya. de DRIVARCION:");
                int vuelta = 0;
                int uniones = 0;
                while (true)
                {
                    for (int i = vuelta; i < result.Count; i++)
                    {
                        Console.WriteLine("Entro al For.");
                        Console.WriteLine(String.Join(", ", result[i].content));
                        if (result[i].content[0].Equals("|"))
                        {
                            Console.WriteLine("Cad1" + String.Join(", ", result[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", result[i + 1].content));
                            result[i + 1].content = unirCadenas(result[i - 1].content, result[i + 1].content);
                            Console.WriteLine("UNION: " + String.Join(", ", result[i + 1].content));
                            vuelta += i + 2;
                            uniones++;
                            break;
                        }
                        else if (result[i].content[0].Equals("."))
                        {
                            Console.WriteLine("Cad1" + String.Join(", ", result[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", result[i + 1].content));
                            result[i + 1].content = concatenarCadenas(result[i - 1].content, result[i + 1].content);
                            Console.WriteLine("CONCATENAR: " + String.Join(", ", result[i + 1].content));
                            vuelta += i + 2;
                            break;
                        }
                    }
                    if (vuelta >= result.Count)
                    {
                        break;
                    }
                }
                if (uniones == 1)
                {
                    Derivaciones der = new Derivaciones();
                    der.content = exp2doOrdenUnion(result[0].content, exp);
                    result[0] = der;
                    Console.WriteLine("Es una union");
                    Console.WriteLine("RESULTADO:" + String.Join(", ", der.content));
                    return result;
                }
                else
                {
                    Derivaciones der = new Derivaciones();
                    der.content = resolver2doOrden(result[result.Count - 1].content, exp);
                    result[0] = der;
                    Console.WriteLine("No es una union");
                    Console.WriteLine("RESULTADO:" + String.Join(", ", der.content));
                    return result;
                }
            }
            else
            {
                if (result.Count == 1)
                {
                    Console.WriteLine("Resultado de DRIVARCION:");
                    Console.WriteLine(String.Join(", ", result[0].content));
                    return result;
                }
                int rftg = 0;
                for (int i = 0; i < result.Count - 1; i++)
                {
                    Console.WriteLine(result[i].content[0]);
                    // Si se repite el signo en el siguiente
                    if (result[i].content[0].Equals("|"))
                    {
                        if (rftg > 0)
                        {
                            Console.WriteLine("Se removio:" + String.Join(", ", result[i].content));
                            result.RemoveAt(i);
                            i--;
                            rftg = 1;
                        }
                        else
                        {
                            rftg++;
                        }
                    }
                    else if (result[i].content[0].Equals("."))
                    {
                        if (rftg > 0)
                        {
                            Console.WriteLine("Se removio:" + String.Join(", ", result[i].content));
                            result.RemoveAt(i);
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
                Console.WriteLine("=========================");
                foreach (var res in result)
                {
                    Console.WriteLine(String.Join(", ", res.content));
                }
                Console.WriteLine("=========================");
                int vuelta = 0;
                while (true)
                {
                    for (int i = vuelta; i < result.Count; i++)
                    {
                        Console.WriteLine("Entro al For.");
                        Console.WriteLine(String.Join(", ", result[i].content));
                        if (result[i].content[0].Equals("|"))
                        {
                            Console.WriteLine("Cad1" + String.Join(", ", result[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", result[i + 1].content));
                            result[i + 1].content = unirCadenas(result[i - 1].content, result[i + 1].content);
                            Console.WriteLine("UNION: " + String.Join(", ", result[i + 1].content));
                            vuelta += i + 2;
                            break;
                        }
                        else if (result[i].content[0].Equals("."))
                        {
                            Console.WriteLine("Cad1" + String.Join(", ", result[i - 1].content));
                            Console.WriteLine("Cad2: " + String.Join(", ", result[i + 1].content));
                            result[i + 1].content = concatenarCadenas(result[i - 1].content, result[i + 1].content);
                            Console.WriteLine("CONCATENAR: " + String.Join(", ", result[i + 1].content));
                            vuelta += i + 2;
                            break;
                        }
                    }
                    if (vuelta >= result.Count)
                    {
                        break;
                    }
                }
                for (int i = 0; i < result.Count - 1; i++)
                {
                    result.RemoveAt(i);
                }
                return result;
            }
        }

        public List<string> exp2doOrdenUnion(List<string> idents, string tipo)
        {
            Console.WriteLine("Esta es una OP U");
            List<string> tmp = resolver2doOrden(idents[0], tipo);
            List<string> tmp2 = resolver2doOrden(idents[1], tipo);
            tmp.Add(idents[0] + idents[1]);
            tmp2.Add(idents[1] + idents[0]);

            tmp.Add(idents[0] + tmp[tmp.Count - 1]);
            tmp2.Add(idents[1] + tmp2[tmp2.Count - 1]);

            tmp.Add(tmp[tmp.Count - 2] + idents[0]);
            tmp2.Add(tmp2[tmp2.Count - 2] + idents[1]);

            tmp.Add(idents[0] + tmp2[1]);
            tmp2.Add(idents[1] + tmp[1]);

            Console.WriteLine("Idents: " + string.Join(", ", idents));
            Console.WriteLine("Temp: " + string.Join(", ", tmp));
            Console.WriteLine("Temp2: " + string.Join(", ", tmp2));

            foreach (string str in tmp2)
            {
                tmp.Add(str);
            }

            return tmp;
        }

        public List<string> resolver2doOrden(List<string> idents, string tipo)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < idents.Count; i++)
            {
                List<String> tmp = resolver2doOrden(idents[i], tipo);
                for (int j = 0; j < tmp.Count; j++)
                {
                    result.Add(tmp[j]);
                }
            }
            return result;
        }

        public List<string> resolver2doOrden(string ident, string tipo)
        {
            int MAX_ELEM = 3;
            List<string> result = new List<string>();
            string sum = "";
            if (ident.Equals("Ɛ", StringComparison.OrdinalIgnoreCase))
            {
                result.Add("Ɛ");
                //TIPO: Cerradura de Kleene
            }
            else if (tipo.Equals("*", StringComparison.OrdinalIgnoreCase))
            {
                result.Add("Ɛ");
                for (int i = 0; i < MAX_ELEM; i++)
                {
                    sum += ident;
                    result.Add(sum);
                }

                //TIPO: Cerradura Positiva
            }
            else if (tipo.Equals("+", StringComparison.OrdinalIgnoreCase))
            {
                for (int i = 0; i < MAX_ELEM; i++)
                {
                    sum += ident;
                    result.Add(sum);
                }

                //TIPO: Exponenciación
            }
            else if (Regex.IsMatch(tipo, "[0-9]*"))
            {
                result.Add("Ɛ");
                for (int i = 0; i < int.Parse(tipo); i++)
                {
                    sum += ident;
                    result.Add(sum);
                }
            }
            return result;
        }


    }
}
