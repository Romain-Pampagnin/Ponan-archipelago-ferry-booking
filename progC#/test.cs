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
        public string codeTarif;
        public string descTarif;
        public float tarifCategorie;

        //constructeur
        public Tarif (string ligne)
        {
            //déclarations
            string [] uneLigne;
            char[] separateur = new char[] {';'};

            uneLigne = ligne.Split(separateur);
            codeTarif = uneLigne[0];
            descTarif = uneLigne[1];
            tarifCategorie = float.Parse(uneLigne[2]);
        }
    }

    public struct Passager //structure Passager et son constructeur
    {
        public string nom;
        public string prenom;
        public string codeCategorie;

        //constructeur
        public Passager (string nm, string pn, string cdCtg)
        {
            nom = nm;
            prenom = pn;
            codeCategorie = cdCtg;
        }
    }

    public struct Vehicule //structure Vehicule et son constructeur
    {
        public string codeCategorie;
        public int quantite;

        //constructeur
       public Vehicule (string cdCtg, int qt)
        {
            codeCategorie = cdCtg;
            quantite = qt;
        }
    }

    public struct Reservation
    {
        public string nom;
        public int idLiaison;
        public string date;
        public string heure;
        public string horodatage;
        public List<Passager> passagers;
        public List<Vehicule> vehicules;

        //constructeur
        public Reservation (string nm, int idL, string dt, string hr, string horo, List<Passager> psg, List<Vehicule> vhcl)
        {
            nom = nm;
            idLiaison = idL;
            date = dt;
            heure = hr;
            horodatage = horo;
            passagers = psg;
            vehicules = vhcl;
        }
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
    static Dictionary<string,Tarif> AjouterTarifsItineraire(string chemin)
    {
        //déclarations
        Dictionary<string, Tarif> dicoTarifs = new Dictionary<string, Tarif>();
        string ligne;
        Tarif unTarif;

        //vérification chemin
        if(File.Exists(chemin))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsTarifs = new FileStream(chemin, FileMode.Open, FileAccess.Read);
            StreamReader srTarifs = new StreamReader(fsTarifs);

            //parcourir le flux
            while (!srTarifs.EndOfStream)
            {
                //lecture d'une ligne
                ligne = srTarifs.ReadLine();
                unTarif = new Tarif(ligne);            //créer une nouvelle structure Tarif à partir de la ligne lue
                dicoTarifs.Add(unTarif.codeTarif, unTarif);  //ajouter le Tarif à l'indice dans le dictionnaire avec pour clé son code
            }

            //fermeture du flux
            srTarifs.Close();
        }
        else
        {
            Console.WriteLine($"Le fichier {chemin} est introuvable, les horaires n'ont pas pu être chargés.");
        }
        return dicoTarifs; //renvoie un dictionnaire plein ou vide
    }
    static void AffichageLogoMenu(bool isLogoMenu, ref List <string> logoMenu)
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
    static void affichageLignes (Dictionary <int,string> itineraires)
    {
        Console.WriteLine("Lignes opérées par la CAN:");
        foreach(int x in itineraires.Keys) 
        {
            Console.WriteLine($"    > {x}: {itineraires[x]}");
        }
        Console.WriteLine();
    }
   static int VerifSaisie(char[] choixPossibles, string question) //retourne forcément l'indice du char saisi (indice en fonction de la liste fournie)
    {
        //déclarations
        int sortie = -1;
        int i;
        bool trouve = false;
        string entree;

        while(sortie == -1)
        {
            //affichage de la question
            Console.Write(question);
            entree = Console.ReadLine();

            if(entree.Length == 1)
            {
                i = 0;
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

    static void AffichageMenu(ref int choix)
    {
        //affichage
        Console.WriteLine("         1. Afficher les lignes disponibles");
        Console.WriteLine("         2. Afficher les horaires");
        Console.WriteLine("         3. Afficher les tarifs");
        Console.WriteLine("         4. Effectuer une réservation");
        Console.WriteLine("         5. Quitter");
        Console.WriteLine();
        
        //choix user
        string questionMenu ="Sélectionnez une option : ";
        char[]choixMenu = new char[5] {'1','2','3','4','5'};
        choix = VerifSaisie(choixMenu, questionMenu);
    }
    static void AffichageTarifs(int idLigne, Dictionary<int,Dictionary<string,Tarif>> tarifsComplets)
    {
        foreach(string code in tarifsComplets[idLigne].Keys)
        {
            Console.WriteLine($"{tarifsComplets[idLigne][code].descTarif.PadRight(45)} : {tarifsComplets[idLigne][code].tarifCategorie.ToString().PadLeft(6)} €");
        }
    }
    static void ChargementDicoTarifs(ref Dictionary<int,Dictionary<string,Tarif>> tarifsComplets)
    {
        //chargement des fichiers tarifs
        if(File.Exists("../ressources/tarifsGroix.csv") && File.Exists("../ressources/tarifsBelleIle.csv"))
        {
            tarifsComplets.Add(1,AjouterTarifsItineraire("../ressources/tarifsGroix.csv"));
            tarifsComplets.Add(2,AjouterTarifsItineraire("../ressources/tarifsGroix.csv"));
            tarifsComplets.Add(3,AjouterTarifsItineraire("../ressources/tarifsBelleIle.csv"));
            tarifsComplets.Add(4,AjouterTarifsItineraire("../ressources/tarifsBelleIle.csv"));
        }
        else
        {
            Console.WriteLine("Erreur, fichier(s) tarifs manquant(s).");
        }
    }

    static void ChargementDicoHoraires(ref Dictionary <int,Dictionary<int,Journee>> horairesComplets)
    {
        //chargement des fichiers horaires
        if(File.Exists("../ressources/GroixLorientNov25.csv") && File.Exists("../ressources/LorientGroixNov25.csv") && File.Exists("../ressources/LePalaisQuiberonNov25.csv") && File.Exists("../ressources/QuiberonLePalaisNov25.csv"))
        {
            horairesComplets.Add(1,ajouterHorairesItineraire("../ressources/LorientGroixNov25.csv"));
            horairesComplets.Add(2,ajouterHorairesItineraire("../ressources/GroixLorientNov25.csv"));
            horairesComplets.Add(3,ajouterHorairesItineraire("../ressources/QuiberonLePalaisNov25.csv"));
            horairesComplets.Add(4,ajouterHorairesItineraire("../ressources/LePalaisQuiberonNov25.csv"));
        }
        else
        {
            Console.WriteLine("Erreur: fichier(s) horaires manquant(s).");
        }
    }
    static void Reservation()
    {
        
    }
    static void Main()
    {
        //déclarations menu et affichage
        bool menu = true;
        bool isLogoMenu = false;
        int choix = 0; 
        int choixAffichageLigne, choixAffichageTarif, i;
        Dictionary <int,string> itineraires = new Dictionary<int, string>(); //dictionnaire qui stocke les itinéraires
        List <string> logoMenu = new List<string>();
        Dictionary <char,string> charTable = new Dictionary<char,string>();
        Dictionary <string,string> bateauTable = new Dictionary<string, string>();

        //création et chargement des dictionnaires horaires et tarifs
        Dictionary <int,Dictionary<int,Journee>> horairesComplets = new Dictionary<int,Dictionary<int,Journee>>(); //dictionnaire qui stocke tous les horaires pour tous les itinéraires
        Dictionary <int,Dictionary<string,Tarif>> tarifsComplets = new Dictionary<int,Dictionary<string,Tarif>>();
        ChargementDicoHoraires(ref horairesComplets);
        ChargementDicoTarifs(ref tarifsComplets);

        //déclarations choix user réservation
        bool confirmChoixResaLigne = false;
        string verifChoixResaLigne;
        int choixResaLigne = 1;

        int choixResaDate;
        string choixResaHeure;

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

        //menu
        if (File.Exists("../ressources/imgMenu.txt"))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsImageMenu = new FileStream("../ressources/imgMenu.txt", FileMode.Open, FileAccess.Read);
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
            AffichageLogoMenu(isLogoMenu,ref logoMenu);            
            AffichageMenu(ref choix);
            switch (choix)
            {
                case 1:
                    {
                        Console.Clear();
                        affichageLignes(itineraires);
                        retourMenu();
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        affichageLignes(itineraires);

                        //choix utilisateur
                        string questionChoixLigne ="De quelle ligne souhaitez-vous afficher les horaires ? ";
                        char[]choixPossiblesLigne = new char[4] {'1','2','3','4'};
                        choixAffichageLigne = VerifSaisie(choixPossiblesLigne, questionChoixLigne);
                        
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
                        affichageLignes(itineraires);

                        //choix utilisateur
                        string questionAffichageTarif ="De quelle ligne souhaitez-vous afficher les tarifs ? ";
                        char[]choixPossiblesAffichageTarif = new char[4] {'1','2','3','4'};
                        choixAffichageTarif = VerifSaisie(choixPossiblesAffichageTarif, questionAffichageTarif);

                        //affichage tarifs
                        Console.Clear();
                        Console.WriteLine($"Ligne {itineraires[choixAffichageTarif]} :\n");
                        AffichageTarifs(choixAffichageTarif, tarifsComplets);
                        Console.WriteLine();
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