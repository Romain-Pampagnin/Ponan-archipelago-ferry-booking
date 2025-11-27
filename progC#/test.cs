using System;
using System.IO;
using System.Collections.Generic;

class SAE11
{
    public struct Journee //déclaration de la structure pour stocker les horaires
    {
        public int numJour;
        public char jour;
        public Dictionary<string,string> horaires; 
        // pour chaque jour, les horaires sont stockés dans un dictionnaire dont la clé est l'horaire, et la valeur le nom du bateau qui fait la traversée

        //constructeur
        public Journee (string ligne)
        {
            //déclarations
            string [] uneLigne;
            char[] separateur = new char[] {';'};
            string [] unHoraire;
            char[] separateur2 = new char[] {'|'};
            Dictionary <string,string> heures = new Dictionary<string, string>();
            int i;

            uneLigne = ligne.Split(separateur);  //sépare un ligne en un tableau de strings
            numJour = int.Parse(uneLigne[0]);                  //le numéro du jour est la première case du tableau
            jour = uneLigne[1][0];                     //la lettre du jour est le 1er caractère de la 2ème case du tableau

            for (i = 2; i < uneLigne.Length; i++)   //pour les autres cases du tableau, boucle pour ajouter les entrées au dictionnaire
            {
                unHoraire = uneLigne[i].Split(separateur2);
                heures.Add(unHoraire[1],unHoraire[0]); 
                //unHoraire[1] est l'heure, unHoraire[0] est le nom du bateau dans le .csv, on inverse donc pour avoir l'heure en clé et le bateau en valeur       
            }

            //une fois la boucle terminée, le dictionnaire heures devient les horaires associés à la journée
            horaires = heures;
        }

    }
    public struct Tarif //structure pour stocker les tarifs
    {
        
    }
    static Dictionary<int, Journee> ajouterHorairesItineraire(string chemin)
    {
        //déclarations
        Dictionary<int, Journee> dicoMois = new Dictionary<int, Journee>();
        string ligne;
        Journee unJour;

        //vérification chemin
        if(File.Exists(chemin))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsHoraire = new FileStream(chemin, FileMode.Open, FileAccess.Read);
            StreamReader srHoraire = new StreamReader(fsHoraire);

            //parcourir le flux
            while (!srHoraire.EndOfStream)
            {
                //lecture d'une ligne
                ligne = srHoraire.ReadLine();
                unJour = new Journee(ligne);            //créer une nouvelle structure Journee à partir de la ligne lue
                dicoMois.Add(unJour.numJour,unJour);  //ajouter la Journee à l'indice qu'elle contient dans le dictionnaire
            }

            //fermeture du flux
            srHoraire.Close();
        }
        else
        {
            Console.WriteLine($"Le fichier {chemin} est introuvable, les horaires n'ont pas pu être chargés.");
        }
        return dicoMois; //renvoie un dictionnaire plein ou vide
    }
    static void affichageMenu(bool isLogoMenu, ref List <string> logoMenu)
    {
        if (isLogoMenu)
        {
            foreach(string ligne in logoMenu)
            {
                Console.WriteLine(ligne);
            }
        }
        else //si le fichier ascii du logo n'a pas pu être chargé, afficher un logo alternatif
        {
            Console.WriteLine(" ------------------------------------------------- ");
            Console.WriteLine("|                    CAN                          |");
            Console.WriteLine("|       Compagnie Alsacienne de Navigation        |");
            Console.WriteLine(" ------------------------------------------------- ");
            Console.WriteLine();
        }
    }   
    static void retourMenu()
    {
        Console.WriteLine("Appuyez sur une touche pour revenir au menu.");
        Console.ReadKey();  //attend que l'utilisateur appuie sur une touche, on peut stocker la valeur de la touche dans une variable
                            //var touche = Console.ReadKey();
    } 
    static void affichageHorairesItineraire(int idLigne, ref Dictionary<int,Dictionary<int,Journee>> horairesComplets, ref Dictionary<char,string> charTable)
    {
        //déclaration
        foreach (int x in horairesComplets[idLigne].Keys)
        {
            Console.Write($"{charTable[horairesComplets[idLigne][x].jour].ToString().PadRight(9)}");
            Console.Write($"{x.ToString().PadLeft(2)} : ");
            foreach(string heure in horairesComplets[idLigne][x].horaires.Keys)
            {
                Console.Write($"{heure}  ");
            }
            Console.Write("\n");
        }
    }
    static void Main()
    {
        //déclarations
        bool menu = true;
        bool isLogoMenu = false;
        int choix, choixAffichageLigne, i;
        Dictionary <int,string> itineraires = new Dictionary<int, string>(); //dictionnaire qui stocke les itinéraires
        List <string> logoMenu = new List<string>();
        Dictionary <int,Dictionary<int,Journee>> horairesComplets = new Dictionary<int,Dictionary<int,Journee>>(); //dictionnaire qui stocke tous les horaires pour tous les itinéraires
        Dictionary <int,Dictionary<string,Tarif>> tarifsComplets = newDictionary<int,Dictionary<string,Tarif>>();
        Dictionary <char,string> charTable = new Dictionary<char,string>();
        Dictionary <string,string> bateauTable = new Dictionary<string, string>();

        //dictionaire charTable
        charTable.Add('L',"lundi");
        charTable.Add('M',"mardi");
        charTable.Add('W',"mercredi");
        charTable.Add('J',"jeudi");
        charTable.Add('V',"vendredi");
        charTable.Add('S',"samedi");
        charTable.Add('D',"dimanche");

        //dictionnaire bateauTable
        bateauTable.Add("BN","Breizh Nevez");
        bateauTable.Add("V","Vindilis");
        bateauTable.Add("B","Bangor");
        bateauTable.Add("I","Ile de Groix");
        
        //dictionnaire itinéraires, chaque itinéraire a un ID unique
        itineraires.Add(1,"Lorient - Groix");
        itineraires.Add(2,"Groix - Lorient");
        itineraires.Add(3,"Quiberon - Le Palais");
        itineraires.Add(4,"Le Palais - Quiberon");

        //chargement des fichiers horaires
        if(File.Exists("GroixLorientNov25.csv") && File.Exists("LorientGroixNov25.csv") && File.Exists("LePalaisQuiberonNov25.csv") && File.Exists("QuiberonLePalaisNov25.csv"))
        {
            horairesComplets.Add(1,ajouterHorairesItineraire("LorientGroixNov25.csv"));
            horairesComplets.Add(2,ajouterHorairesItineraire("GroixLorientNov25.csv"));
            horairesComplets.Add(3,ajouterHorairesItineraire("QuiberonLePalaisNov25.csv"));
            horairesComplets.Add(4,ajouterHorairesItineraire("LePalaisQuiberonNov25.csv"));
        }
        else
        {
            Console.WriteLine("Erreur: fichier(s) horaires manquant(s).");
        }

        //chargement des fichiers tarifs
        if(File.Exists("tarifsGroix.csv") && File.Exists("tarifsBelleIle.csv"))
        {
            
        }
        else
        {
            Console.WriteLine("Erreur, fichier(s) tarifs manquant(s).");
        }



        //menu
        if (File.Exists("imgMenu.txt"))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsImageMenu = new FileStream("imgMenu.txt", FileMode.Open, FileAccess.Read);
            StreamReader srImageMenu = new StreamReader(fsImageMenu);
            isLogoMenu = true;

            //parcourir le flux
            while (!srImageMenu.EndOfStream)
            {
                //lecture d'une ligne et enregistrement dans une liste
                logoMenu.Add(srImageMenu.ReadLine());
            }

            //fermeture du flux
            srImageMenu.Close();
        }
        else
        {
            Console.WriteLine("Erreur: fichier ascii manquant."); //message d'erreur, superflu ?
        }
        while(menu == true)
        {
            Console.Clear();
            affichageMenu(isLogoMenu,ref logoMenu);            
            Console.WriteLine("         1. Afficher les lignes disponibles");
            Console.WriteLine("         2. Afficher les horaires");
            Console.WriteLine("         3. Afficher les tarifs");
            Console.WriteLine("         4. Effectuer une réservation");
            Console.WriteLine("         5. Quitter");
            choix = int.Parse(Console.ReadLine());

            switch (choix)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("Lignes opérées par la CAN:");
                        foreach(int x in itineraires.Keys) 
                        {
                            Console.WriteLine($"    > {x}: {itineraires[x]}");
                        }
                        Console.WriteLine();
                        retourMenu();
                        break;
                    }
                case 2:
                    {
                        Console.Clear();

                        //affichage des lignes
                        Console.WriteLine("Lignes opérées par la CAN:");
                        foreach(int x in itineraires.Keys) 
                        {
                            Console.WriteLine($"    > {x}: {itineraires[x]}");
                        }
                        Console.WriteLine();

                        //choix utilisateur
                        Console.WriteLine("De quelle ligne souhaitez-vous afficher les horaires ?");
                        choixAffichageLigne = int.Parse(Console.ReadLine());
                        
                        //affichage horaire
                        Console.Clear();
                        Console.WriteLine($"Ligne {itineraires[choixAffichageLigne]} :\n");
                        affichageHorairesItineraire(choixAffichageLigne, ref horairesComplets, ref charTable);
                        Console.WriteLine();
                        retourMenu();
                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        retourMenu();
                        break;    
                    }
                case 4:
                    {
                        Console.Clear();
                        retourMenu();
                        break;
                    }
                case 5:
                    {
                        Console.Clear();
                        menu = false;
                        break;
                    }
            }
        }
    }
}