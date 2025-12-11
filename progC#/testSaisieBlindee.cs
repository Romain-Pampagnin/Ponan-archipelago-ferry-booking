using System;
using System.IO;
using System.Collections.Generic;

class testSaisieBlindee
{
    static int VerifSaisie(char[] choixPossibles, string question) //retourne -1 en cas d'erreur, sinon l'indice du char saisi
    {
        //déclarations
        int sortie = -1;
        int i = 0;
        bool trouve = false;
        string entree;
        //choixPossibles {1,2,3,4,x,X}

        while(sortie == -1)
        {
            //affichage de la question
            Console.WriteLine(question);
            entree = Console.ReadLine();

            if(entree.Length == 1)
            {
                while(i < choixPossibles.Length && !trouve) //boucle pour vérifier 
                {
                    if(choixPossibles[i] == entree[0])
                    {
                        trouve = true;
                    }
                    else
                    {
                        i ++;
                    }
                }

                if(trouve)
                {
                    sortie = i +1;
                }
            }
        }
        return sortie;
    }
    static void Main()
    {
        //déclaration
        char[]choixPossibles = new char[5]{'1','2','3','4','x'};
        int choix;
        

        //appel de fonction
        string question = "Choisissez dans la liste : 1,2,3,4 ou tapez x";
        choix = VerifSaisie(choixPossibles, question);
        Console.WriteLine($"Choix {choix}");

        
    }

}