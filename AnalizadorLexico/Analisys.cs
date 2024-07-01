using System;
using System.Collections.Generic;
using System.Media;
using System.Text.RegularExpressions;

namespace AnalizadorLexico
{

    public class Analisys
    {

        /*
         *  ---- TABLE OF LEXICAL COMPONENTS ----
         *  
         * Concatenación		->	    .		->	Caracter .
         * Unión			    ->	    |		->	Caracter |
         * ParéntesisDeApertura ->      (       ->  Carácter (
         * ParéntesisDeCierre   ->      )       ->  Carácter )
         * CadenaVacía          ->      Ɛ       ->  Carácter Ɛ
         * CerraduraDeKleen 	->	    ^*      ->  Carácter ^ seguido de *
         * CerraduraPositiva 	->	    ^+      ->  Carácter ^ seguido de +
         * Exponenciación 		->	^123, ^6    ->  Carácter ^ seguido de más dígitos
         * Identificador 		->	letra, r    ->  Letra o digito seguido de más letras o digitos
         * 
         */
        public List<Token> lexical(string value)
        {
            //Stores the string converted to tokens
            List<Token> sequence = new List<Token>();

            //Loop that traverses the size of the string
            while (value.Length > 0)
            {
                //Gets the first character of the string
                string c = value.Substring(0, 1);

                /* ====== SECTION - THE LEXICAL COMPONENT IS SEARCHED ====== */

                //Validate if the character is an -> Concatenation
                if (c.Equals("."))
                {
                    sequence.Add(new Token(c, "Concatenación"));
                    value = value.Substring(1);
                    continue;
                }
                //Validate if the character is an -> Union
                if (c.Equals("|"))
                {
                    sequence.Add(new Token(c, "Unión"));
                    value = value.Substring(1);
                    continue;
                }
                //Validate if the character is an -> Opening Parenthesis
                if (c.Equals("("))
                {
                    sequence.Add(new Token(c, "Paréntesis De Apertura"));
                    value = value.Substring(1);
                    continue;
                }
                //Validate if the character is an -> Closing Parenthesis
                if (c.Equals(")"))
                {
                    sequence.Add(new Token(c, "Paréntesis De Cierre"));
                    value = value.Substring(1);
                    continue;
                }
                //Validate if the character is an -> Empty string
                if (c.Equals("Ɛ"))
                {
                    sequence.Add(new Token("Ɛ", "Cadena Vacía"));
                    value = value.Substring(1);
                    continue;
                }
                //Validate the exponent type
                if (c.Equals("^"))
                {
                    if (value.Length == 1)
                    {
                        sequence.Add(new Token(c, "No identificado"));
                        return sequence;
                    }
                    //Gets the value of exponent
                    string expType = value.Substring(1, 1);

                    //Validates whether it is a Kleene lock.
                    if (expType.Equals("*"))
                    {
                        //Inicializa un nuevo token y lo agrega como operador
                        sequence.Add(new Token(value.Substring(0, 2), "Cerradura de Kleen"));
                        /* Verifica si aun quedan carateres que analizar...
                         * De ser asi valida si el siguiente signo es otro '*'
                         */
                        if (value.Length > 2 && value.Substring(2, 1).Equals("*"))
                            value = value.Substring(3);//Recorre la cadena en 3 posiciones
                        else
                            value = value.Substring(2);//Recorre la cadena en 2 posiciones
                        continue;
                    }

                    //Validates whether it is a Kleene or Positive lock.
                    if (expType.Equals("+"))
                    {
                        //Inicializa un nuevo token y lo agrega como operador
                        sequence.Add(new Token(value.Substring(0, 2), "Cerradura positiva"));
                        value = value.Substring(2);//Recorre la cadena en 2 posiciones
                        continue;
                    }

                    //Validates whether it is a exponentiation
                    if (Regex.IsMatch(expType, @"^[0-9]*$"))
                    {
                        //Busca si el siguiente caracter es un identificador tambien
                        string ident = c + SearchNext_09(value.Substring(1));
                        //Inicializa un nuevo token y lo agrega como Operador
                        sequence.Add(new Token(ident, "Exponenciacion"));
                        //Recorre la cadena segun el tamaño de ident
                        value = value.Substring(ident.Length);
                        continue;
                    }
                }

                //Validate if the character is an -> IDENTIFIER
                if (Regex.IsMatch(c, @"^[0-9]*$") || Regex.IsMatch(c, @"^[A-Z]*$") || Regex.IsMatch(c, @"^[a-z]*$"))
                {
                    //Busca si el siguiente caracter es un identificador tambien
                    string ident = c + SearchNextAZ_09(value.Substring(1));
                    //Inicializa un nuevo token y lo agrega como identificador
                    sequence.Add(new Token(ident, "Identificador"));
                    //Si se encontró más identificadores es necesario eliminar la parte ya leida
                    value = value.Substring(ident.Length);
                    continue;
                }

                sequence.Add(new Token(c, "No identificado"));
                value = value.Substring(1);

            }
            return sequence;
        }
        
        
        private Boolean parenthesisResolve(List<Token> value){
            // Q_1 ->   [Identificador, Ɛ]    -> Q_4
            Boolean isUnion;
            do
            {
                isUnion = false;
                do
                {
                    isUnion = false;
                    value.RemoveAt(0);
                    if (value.Count == 0) return false;
                    //Q_4 - Validating
                    if (value[0].getComp().Equals("Identificador") || value[0].getLex().Equals("Ɛ"))
                    {
                        value.RemoveAt(0);
                        if (value.Count == 0) return false;
                        // Verifica si el siguiente valor es una union
                        if (value[0].getLex().Equals("|") && value[0] != null)
                            isUnion = true;
                        else
                            isUnion = false;
                    }
                    // ERROR DE SINTAXIS
                    else return false;

                    // Q_4 ->>LOOP>>    [Unión]   ->>LOOP>> Q_1
                } while (isUnion);

                // Q_4 ->    [Kleen, Positiva, Expon]   -> Q_5
                if (value[0].getLex().Equals("^*") || value[0].getLex().Equals("^+") || value[0].getComp().Equals("Exponenciacion"))
                {
                    value.RemoveAt(0);
                    if (value.Count == 0) return false;
                    // Verifica si el siguiente valor es una union
                    if (value[0].getLex().Equals("|") && value[0] != null)
                        isUnion = true;
                    else
                        isUnion = false;
                }
                // Q_5 ->>LOOP>>    [Unión]   ->>LOOP>> Q_1
            } while (isUnion);

            //Q_5 ->    [)]   -> Q_6
            if (value[0].getLex().Equals(")"))
            {
                value.RemoveAt(0);
                // Q_6 ->   [null]   -> -- ACEPTADA --
                if (value.Count == 0)
                    return true;
                // Q_6 ->   [Kleen, Positiva, Expon]   -> Q_7
                else if (value[0].getLex().Equals("^*") || value[0].getLex().Equals("^+") || value[0].getComp().Equals("Exponenciacion"))
                {
                    value.RemoveAt(0);
                    // Q_7 ->   [null]   -> -- ACEPTADA --
                    if (value.Count == 0)
                        return true;
                    //Q_7 ->    [Concatenación]   -> Q_0
                    else if (value[0].getLex().Equals("."))
                    {
                        value.RemoveAt(0);
                        return Syntactic(value);
                    }
                    else return false;

                }
                //Q_6 ->    [Concatenación]   -> Q_0
                else if (value[0].getLex().Equals("."))
                {
                    value.RemoveAt(0);
                    return Syntactic(value);
                }
                else return false;
            }
            return false;
        }
        
        
        public Boolean Syntactic(List<Token> value)
        {
            if (value.Count == 0) return false;
            // Q_0 ->   [(]   -> Q_1
            if (value[0].getLex().Equals("("))
                return parenthesisResolve(value);

            // Q_0 ->   [Identificador, Ɛ]    -> Q_2
            else if (value[0].getComp().Equals("Identificador") || value[0].getLex().Equals("Ɛ"))
            {
                value.RemoveAt(0);
                // Q_2 ->   [null]   -> -- ACEPTADA --
                if (value.Count == 0)
                    return true;
                // Q_2 ->   [Kleen, Positiva, Expon]  -> Q_3
                else if (value[0].getLex().Equals("^*") || value[0].getLex().Equals("^+") || value[0].getComp().Equals("Exponenciacion"))
                {
                    value.RemoveAt(0);
                    // Q_3 ->   [null]   -> -- ACEPTADA --
                    if (value.Count == 0)
                        return true;
                    //Q_3 ->    [Concatenación]   -> Q_0
                    else if (value[0].getLex().Equals("."))
                    {
                        value.RemoveAt(0);
                        if (value.Count == 0) return false;
                        return Syntactic(value);
                    }
                    else if (value[0].getLex().Equals(")"))
                    {
                        value.RemoveAt(0);
                        if (value.Count == 0) return false;
                        return Syntactic(value);
                    }
                    else return false;
                }
                //Q_2 ->    [Concatenación]   -> Q_0
                else if (value[0].getLex().Equals("."))
                {
                    value.RemoveAt(0);
                    if (value.Count == 0) return false;
                    return Syntactic(value);
                }
                else return false;
            }
        return false;
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

        private string SearchNext_09(string cadena)
        {
            if (string.IsNullOrEmpty(cadena))
                return "";

            string c = cadena.Substring(0, 1);
            if (string.IsNullOrEmpty(cadena))
                return "";
            if (Regex.IsMatch(c, @"^[0-9]*$"))
                return c + SearchNext_09(cadena.Substring(1));

            return "";

        }

        public string leerAnalisis(List<Token> analisis)
        {
            string final = "======= ANÁLISIS LEXICO =======\n";
            foreach (Token token in analisis)
            {
                final += "\n<" + token.getComp() + ">" + "    " + token.getLex();
            }
            return final;
        }

    }
}
