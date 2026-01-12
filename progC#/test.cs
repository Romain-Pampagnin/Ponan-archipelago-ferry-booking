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
    public struct Traversee //structure Traversée et son contructeur
    {
        public string nom;
        public int idLiaison;
        public string date;
        public string heure;
        public string horodatage;

        //constructeur
        public Traversee(string nm, int idL, string dt, string hr, string horodat)
        {
            nom = nm;
            idLiaison = idL;
            date = dt;
            heure = hr;
            horodatage = horodat;
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
    static int ConfirmYN (string question)
    {
        //déclarations
        char saisie;
        string saisie2;
        int retour = -1;

        while(retour == -1)
        {
            Console.Write($"{question}");
            saisie = Console.ReadKey().KeyChar;     //stocke le char de la touche dans saisie
            saisie2 = saisie.ToString().ToUpper();  //converti saisie en un string, puis applique la méthode ToUpper
            if(saisie2.Length == 1 && saisie2[0] == 'Y')
            {
                retour = 1;
            }
            else if(saisie2.Length == 1 && saisie2[0] == 'N')
            {
                retour = 0;
            }
            else
            {
                Console.Write("\n");
            }
        }
        Console.WriteLine();
        return retour;
    }
    static int VerifSaisieChar(char[] choixPossibles, string question) //retourne forcément l'indice du char saisi (indice en fonction de la liste fournie)
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
    static int VerifSaisieString(string[] choixPossibles, string question) //retourne forcément l'indice du char saisi (indice en fonction de la liste fournie)
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
            if(entree.Length <= 2 /*&& entree.GetType() == n.GetType()*/)
            {
                i = 0;
                while(i < choixPossibles.Length && !trouve) //boucle pour vérifier 
                {
                    if(choixPossibles[i] == entree)
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
        choix = VerifSaisieChar(choixMenu, questionMenu);
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
    static void ChargementDicoCatVehicules (ref Dictionary <string,string> catVehicules)
    {
        //déclarations
        string ligne;
        char[] separateur = new char[] {';'};
        string[] categorie;
        
        //chargement du fichier catVehicules
        if(File.Exists("../ressources/catVehicules.csv"))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsCatVehic = new FileStream("../ressources/catVehicules.csv", FileMode.Open, FileAccess.Read);
            StreamReader srCatVehic = new StreamReader(fsCatVehic);

            //parcourir le flux
            while (!srCatVehic.EndOfStream)
            {
                //lecture d'une ligne
                ligne = srCatVehic.ReadLine();
                categorie = ligne.Split(separateur);
                catVehicules.Add(categorie[0],categorie[1]);
            }

            //fermeture du flux
            srCatVehic.Close();

            //test affichage du dico
            // foreach(string cle in catVehicules.Keys)
            // {
            //     Console.WriteLine($"clé : {cle}, valeur : {catVehicules[cle]}");
            // }
            // Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Erreur: fichier catVehicules.csv manquant.");
        }
    }
    static int CreerResa(Dictionary <int,string> itineraires, ref Dictionary<int,Dictionary<int,Journee>> horairesComplets, ref Dictionary<char,string> charTable, Dictionary<string,string> catVehicules, ref Dictionary <int,Dictionary<string,Tarif>> tarifsComplets)
    {
        //déclarations
        int choixLigne, choixJour, choixLigneRetour, choixJourRetour;
        int retour = 0;
        string horodatage;
        List <Passager> passagers = new List<Passager>();
        List <Vehicule> vehicules = new List<Vehicule>();

        //initialisation de la liste des véhicules
        foreach (string categorie in catVehicules.Keys)
        {
            Vehicule nouveauVehicule = new Vehicule(categorie, 0);
            vehicules.Add(nouveauVehicule);
        }
        
        TitreEncadre("EFFECTUER UNE RESERVATION");
        affichageLignes(itineraires);
        Console.WriteLine();
        string questionChoix1 = "Choissisez le numéro de la ligne sur laquelle vous voulez réserver : ";
        char[]choixPossiblesQ1 = new char[4] {'1','2','3','4'};
        choixLigne = VerifSaisieChar(choixPossiblesQ1, questionChoix1);
        
        //sélection automatique de la ligne retour
        choixLigneRetour = 0;
        if(choixLigne == 1)
        {
            choixLigneRetour = 2;
        }
        else if(choixLigne == 2)
        {
            choixLigneRetour = 1;
        }
        else if(choixLigne == 3)
        {
            choixLigneRetour = 4;
        }
        else if(choixLigne == 4)
        {
            choixLigneRetour = 3;
        }

        //afficher la ligne sélectionnée et ses horaires
        Console.Clear();
        TitreEncadre("EFFECTUER UNE RESERVATION");
        Console.WriteLine($"Ligne {itineraires[choixLigne]} :");
        Console.WriteLine();
        affichageHorairesItineraire(choixLigne, ref horairesComplets, ref charTable);
        Console.WriteLine();
        string questionChoix2 = "Choisissez le numéro du jour de votre traversée : ";
        string[]choixPossiblesQ2 = new string[30] {"1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","21","22","23","24","25","26","27","28","29","30"};
        choixJour = VerifSaisieString(choixPossiblesQ2, questionChoix2);

        //basculer le jour vers un string
        string choixJourString;
        if(choixJour < 10)
        {
            choixJourString = "2025-11-0" + choixJour.ToString();
        }
        else
        {
            choixJourString = "2025-11-" + choixJour.ToString();
        }
        

        //afficher la ligne et le jour choisi pour choix horaire
        Console.Clear();
        TitreEncadre("EFFECTUER UNE RESERVATION");
        Console.WriteLine($"Ligne {itineraires[choixLigne]} :");
        Console.WriteLine();
        Console.WriteLine($"Journée du {choixJour}/11 :");
        Console.WriteLine();
        int numJour = 0;
        
        //choix de l'horaire
        foreach(KeyValuePair <string,string> heure in horairesComplets[choixLigne][choixJour].horaires)
        {
            numJour ++;
            Console.WriteLine($"    > {numJour} : {heure.Key}");
        }
        Console.WriteLine();
        
        string questionChoix3 = "Choissisez l'horaire de votre trajet : ";
        char[]choixPossiblesQ3 = new char[numJour];
        for (int i = 0; i < numJour; i++)
        {
            choixPossiblesQ3[i] = (char)('0' + i + 1);
        }
        int choixHoraire = VerifSaisieChar(choixPossiblesQ3, questionChoix3);

        //récupération de l'horaire sous format string
        string choixHoraireString = "";
        int y = 1;
        foreach (string cle in horairesComplets[choixLigne][choixJour].horaires.Keys)
        {
            if(y == choixHoraire)
            {
                choixHoraireString = cle;
            }
            y++;
        }
        
        //confirmation choix traversée date heure
        Console.Clear();
        TitreEncadre("EFFECTUER UNE RESERVATION");
        Console.WriteLine($"Vous avez choisi un trajet sur la ligne {itineraires[choixLigne]}, le {choixJour}/11 à {choixHoraireString}.");
        Console.WriteLine();
        if(ConfirmYN("Confirmer [y/n] ? ") == 1)
        {
            Console.WriteLine("Réservation confirmée.");
            horodatage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //HH pour avoir l'heure en format 24, MM pour mois, mm pour minutes
            Console.WriteLine();
            Console.WriteLine("Appuyez sur une touche pour l'enregistrement des passagers.");
            Console.ReadKey();
            
            Console.Clear();
            TitreEncadre("EFFECTUER UNE RESERVATION");
            Console.WriteLine($"Réservation pour la ligne {itineraires[choixLigne]}, le {choixJour}/11 à {choixHoraireString}.");
            Console.WriteLine();

            //enregistrement des passagers
            passagers.Add(CreerPassager(ref passagers));
            Console.WriteLine();

            bool passagersFinis = false;
            while(!passagersFinis)
            {
                if(ConfirmYN("Ajouter un autre passager [y/n] ? ") == 1)
                {
                    Console.Clear();
                    passagers.Add(CreerPassager(ref passagers)); 
                    Console.WriteLine();
                }
                else
                {
                    passagersFinis = true;
                }
            }

            //récapitulatif des passagers saisis
            Console.Clear();
            TitreEncadre("EFFECTUER UNE RESERVATION");
            Console.WriteLine($"Réservation pour la ligne {itineraires[choixLigne]}, le {choixJour}/11 à {choixHoraireString}.");
            Console.WriteLine($"Vous avez saisi {passagers.Count} passager(s) :");
            Console.WriteLine(" _______________________________________________ ");
            Console.WriteLine("| Nom             | Prénom          | Catégorie |");
            for(int i = 0; i < passagers.Count; i ++)
            {
                Console.WriteLine($"| {passagers[i].nom.PadRight(15)} | {passagers[i].prenom.PadRight(15)} | {passagers[i].codeCategorie.PadRight(9)} |");
            }
            Console.WriteLine("|_______________________________________________|");
            Console.WriteLine();
            Console.WriteLine();

            //enregistrement des véhicules
            if(ConfirmYN("Voulez-vous enregistrer un ou des véhicules [y/n] ? ") == 1)
            { 
                CreerVehicule(ref catVehicules, ref vehicules);
            }
            
            //purger les catégories de véhicules avec une quantité nulle
            int vehicCount = vehicules.Count;
            for (int i = 0; i < vehicCount; i++)
            {
                if(vehicules[i].quantite == 0)
                {
                    vehicules.RemoveAt(i);
                    vehicCount = vehicCount -1;
                    i = i-1;
                }
            }

            Console.Clear();
            TitreEncadre("EFFECTUER UNE RESERVATION");
            Console.WriteLine("Récapitulatif de votre réservation (aller simple)");
            Console.WriteLine();
            AffichageRecapitulatifTraversee(itineraires, choixLigne, choixJour, choixHoraireString, passagers, vehicules, catVehicules, ref tarifsComplets);

            if(ConfirmYN("Confirmez la réservation [y/n] ? ") == 1)
            {
                retour = 1;
                Console.WriteLine("Réservation confirmée, ");
                Console.WriteLine();

                //demander pour réservation retour
                if(ConfirmYN("Souhaitez-vous réserver un voyage retour avec les mêmes paramêtres [y/n] ? ") == 1)
                {
                    
                    //saisie date et horaire par l'utilisateur
                    Console.Clear();
                    TitreEncadre("EFFECTUER UNE RESERVATION (retour)");
                    Console.WriteLine($"Ligne {itineraires[choixLigneRetour]} :");
                    Console.WriteLine();
                    affichageHorairesItineraire(choixLigneRetour, ref horairesComplets, ref charTable);
                    Console.WriteLine();

                    //création du tableau des réponses valides en prenant en compte le fait que la date de retour ne peut pas être inféreure ou égale à la date de l'aller
                    string questionChoix4 = "Choisissez le numéro du jour de votre traversée retour : ";

                    List <string> listeChoixRetour = new List<string>(); //je passe par une liste
                    for(int i = choixJour +1; i <= 30; i++ )
                    {
                        listeChoixRetour.Add(i.ToString()); //à laquelle j'ajoute les string une par une
                    }
                    string[]choixPossiblesQ4 = listeChoixRetour.ToArray();    //je crée un tableau de string à partir de ma liste de string (peu élégant)
                    choixJourRetour = VerifSaisieString(choixPossiblesQ4, questionChoix4);
                    choixJourRetour = choixJourRetour + choixJour;              //incrémente le résultat pour que l'entier soit égal à la valeur inscrite en chaîne 

                    //basculer le jour vers un string
                    string choixJourRetourString;
                    if(choixJourRetour < 10)
                    {
                        choixJourRetourString = "2025-11-0" + choixJourRetour.ToString();
                    }
                    else
                    {
                        choixJourRetourString = "2025-11-" + choixJourRetour.ToString();
                    }

                    //afficher la ligne et le jour choisi pour choix horaire
                    Console.Clear();
                    TitreEncadre("EFFECTUER UNE RESERVATION (retour)");
                    Console.WriteLine($"Ligne {itineraires[choixLigneRetour]} :");
                    Console.WriteLine();
                    Console.WriteLine($"Journée du {choixJourRetour}/11 :");
                    Console.WriteLine();
                    int numJourRetour = 0;
                    
                    //choix de l'horaire
                    foreach(KeyValuePair <string,string> heure in horairesComplets[choixLigneRetour][choixJourRetour].horaires)
                    {
                        numJourRetour ++;
                        Console.WriteLine($"    > {numJourRetour} : {heure.Key}");
                    }
                    Console.WriteLine();
                    
                    string questionChoix5 = "Choissisez l'horaire de votre trajet : ";
                    char[]choixPossiblesQ5 = new char[numJourRetour];
                    for (int i = 0; i < numJourRetour; i++)
                    {
                        choixPossiblesQ5[i] = (char)('0' + i + 1);
                    }
                    int choixHoraireRetour = VerifSaisieChar(choixPossiblesQ5, questionChoix5);

                    //récupération de l'horaire sous format string
                    string choixHoraireRetourString = "";
                    int z = 1;
                    foreach (string cle in horairesComplets[choixLigneRetour][choixJourRetour].horaires.Keys)
                    {
                        if(z == choixHoraireRetour)
                        {
                            choixHoraireRetourString = cle;
                        }
                        z++;
                    }

                    Console.Clear();
                    TitreEncadre("EFFECTUER UNE RESERVATION");
                    Console.WriteLine("Récapitulatif de votre réservation (aller-retour)");
                    Console.WriteLine();
                    Console.WriteLine("Aller :");
                    AffichageRecapitulatifTraversee(itineraires, choixLigne, choixJour, choixHoraireString, passagers, vehicules, catVehicules, ref tarifsComplets);
                    Console.WriteLine("Retour :");
                    AffichageRecapitulatifTraversee(itineraires, choixLigneRetour, choixJourRetour, choixHoraireRetourString, passagers, vehicules, catVehicules, ref tarifsComplets);

                    //création des deux réservations
                    string nomResa = "Groupe_" + passagers[0].nom;
                    string nomResaRetour = "Groupe_" + passagers[0].nom + "_Retour";

                    Reservation reservationAller = new Reservation(nomResa, choixLigne, choixJourString, choixHoraireString, horodatage, passagers, vehicules);
                    Reservation reservationRetour = new Reservation(nomResaRetour, choixLigneRetour, choixJourRetourString, choixHoraireRetourString, horodatage, passagers, vehicules);

                    //export dans un seul JSON
                    List<Reservation> listeAR = new List<Reservation>();
                    listeAR.Add(reservationAller);
                    listeAR.Add(reservationRetour);
                    ExportJson(listeAR);
                }
                else
                {
                    //donner le nom du premier passager à la réservation
                    string nomResa = "Groupe_" + passagers[0].nom;

                    //créer la structure réservation
                    Reservation reservation = new Reservation(nomResa, choixLigne, choixJourString, choixHoraireString, horodatage, passagers, vehicules);
                    List <Reservation> listeAllerSimple = new List<Reservation>();
                    listeAllerSimple.Add(reservation);
                    ExportJson(listeAllerSimple);
                }

                Console.WriteLine("Merci pour votre commande, la CAN vous souhaite d'excellentes traversées sur nos lignes.");
                Console.WriteLine();

            }
            else
            {
                Console.WriteLine("Réservation annulée.");
                Console.WriteLine();
            }

            
        }
        else
        {
            Console.WriteLine("Réservation annulée.");
            Console.WriteLine();
        }
        return retour;
    }
    static void AffichageRecapitulatifTraversee(Dictionary <int,string> itineraires, int choixLigne, int choixJour, string choixHoraireString, List<Passager> passagers, List<Vehicule> vehicules,Dictionary<string,string> catVehicules, ref Dictionary <int,Dictionary<string,Tarif>> tarifsComplets)
    {
        //déclarations
        float totalPrixPassagers, totalPrixVehicules;

        //récapitulatif total
        //trajet
        Console.WriteLine("TRAJET :");
        Console.WriteLine($"Réservation pour la ligne {itineraires[choixLigne]}, le {choixJour}/11 à {choixHoraireString}.");
        Console.WriteLine();

        //passagers
        totalPrixPassagers = 0;
        totalPrixVehicules = 0;
        Console.WriteLine("PASSAGER(S) :");
        Console.WriteLine(" __________________________________________________________ ");
        Console.WriteLine("| Nom             | Prénom          | Catégorie | Prix     |");
        for(int i = 0; i < passagers.Count; i ++)
        {
            Console.WriteLine($"| {passagers[i].nom.PadRight(15)} | {passagers[i].prenom.PadRight(15)} | {passagers[i].codeCategorie.PadRight(9)} | {tarifsComplets[choixLigne][passagers[i].codeCategorie].tarifCategorie.ToString().PadRight(8)} |");
            totalPrixPassagers = totalPrixPassagers + tarifsComplets[choixLigne][passagers[i].codeCategorie].tarifCategorie;
        }
        Console.WriteLine("|__________________________________________________________|");
        Console.WriteLine($"Sous-total passagers : {totalPrixPassagers} €");
        Console.WriteLine();

        //véhicules si il y en a
        if(vehicules.Count != 0)
        {
            Console.WriteLine("VEHICULE(S) :");
            Console.WriteLine(" _______________________________________________________________________________________ ");
            Console.WriteLine("| Description                                  | Nombre | Prix unitaire | Prix total    |");
            foreach (Vehicule vehicule in vehicules)
            {
                Console.WriteLine($"| {catVehicules[vehicule.codeCategorie].PadRight(44)} | {vehicule.quantite.ToString().PadRight(6)} | {tarifsComplets[choixLigne][vehicule.codeCategorie].tarifCategorie.ToString().PadRight(13)} | {(vehicule.quantite * tarifsComplets[choixLigne][vehicule.codeCategorie].tarifCategorie).ToString().PadRight(13)} |");
                totalPrixVehicules = totalPrixVehicules + vehicule.quantite * tarifsComplets[choixLigne][vehicule.codeCategorie].tarifCategorie;
            }
            Console.WriteLine("|_______________________________________________________________________________________|");
            Console.WriteLine($"Sous-total véhicules : {totalPrixVehicules} €");
            Console.WriteLine();
            Console.WriteLine($"Total : {totalPrixPassagers + totalPrixVehicules} €");
            Console.WriteLine();
        }
    }
    static void ExportJson(List<Reservation> listeResas)
    {
        //déclarations
        int nbResa = 0;

        //création du fichier JSON
        string chemin = "../exports/"+ listeResas[0].nom + "_" + listeResas[0].horodatage + ".json";
        if(!File.Exists(chemin))
        {
            StreamWriter sw = new StreamWriter($"../exports/"+ listeResas[0].nom + "_" + listeResas[0].horodatage + ".json");
            sw.WriteLine("[");
            foreach(Reservation resa in listeResas)
            {
                nbResa++;
                
                sw.WriteLine("  {");
                sw.WriteLine("      \"reservation\": {");
                sw.WriteLine($"         \"nom\": \"{resa.nom}\",");
                sw.WriteLine($"         \"idLiaison\": {resa.idLiaison},");
                sw.WriteLine($"         \"date\": \"{resa.date}\",");
                sw.WriteLine($"         \"heure\": \"{resa.heure}\",");
                sw.WriteLine($"         \"horodatage\": \"{resa.horodatage}\"");
                sw.WriteLine("          },");
                sw.WriteLine("      \"passagers\": [");
                for(int i = 0; i < resa.passagers.Count -1; i++)
                {
                    sw.WriteLine("      {");
                    sw.WriteLine($"             \"nom\": \"{resa.passagers[i].nom}\",");
                    sw.WriteLine($"             \"prenom\": \"{resa.passagers[i].prenom}\",");
                    sw.WriteLine($"             \"codeCategorie\": \"{resa.passagers[i].codeCategorie}\"");
                    sw.WriteLine("      },");
                }
                //pour le dernier passager et éviter la virgule en trop
                sw.WriteLine("      {");
                sw.WriteLine($"             \"nom\": \"{resa.passagers[resa.passagers.Count -1].nom}\",");
                sw.WriteLine($"             \"prenom\": \"{resa.passagers[resa.passagers.Count -1].prenom}\",");
                sw.WriteLine($"             \"codeCategorie\": \"{resa.passagers[resa.passagers.Count -1].codeCategorie}\"");
                sw.WriteLine("      }");
                sw.WriteLine("     ],");

                sw.WriteLine("      \"vehicules\": [");
                if(resa.vehicules.Count !=0)
                {
                    for(int i = 0; i < resa.vehicules.Count -1; i++)
                    {
                        sw.WriteLine("      {");
                        sw.WriteLine($"             \"codeCategorie\": \"{resa.vehicules[i].codeCategorie}\",");
                        sw.WriteLine($"             \"quantite\": {resa.vehicules[i].quantite}");
                        sw.WriteLine("      },");
                    }

                    //pour le dernier véhicule et éviter la virgule de trop
                    sw.WriteLine("      {");
                    sw.WriteLine($"             \"codeCategorie\": \"{resa.vehicules[resa.vehicules.Count -1].codeCategorie}\",");
                    sw.WriteLine($"             \"quantite\": {resa.vehicules[resa.vehicules.Count -1].quantite}");
                    sw.WriteLine("      }");

                }
                sw.WriteLine("     ]");
                sw.WriteLine("  }");
                
                if(nbResa < listeResas.Count)
                {
                    sw.WriteLine(",");
                } 
            }
            sw.WriteLine("]");
            sw.Close();
        }
        else
        {
            
        }
    }
    static Passager CreerPassager(ref List <Passager> passagersList)
    {
        //déclarations
        string nom, prenom;
        string codeCat = "";
        int age;
        int numPassager = passagersList.Count +1;

        //affichage des passagers déjà saisis
        Console.Clear();
        TitreEncadre("EFFECTUER UNE RESERVATION");
        Console.WriteLine(" _______________________________________________ ");
        Console.WriteLine("| Nom             | Prénom          | Catégorie |");
        for(int i = 0; i < passagersList.Count; i ++)
        {
            Console.WriteLine($"| {passagersList[i].nom.PadRight(15)} | {passagersList[i].prenom.PadRight(15)} | {passagersList[i].codeCategorie.PadRight(9)} |");
        }
        Console.WriteLine("|_______________________________________________|");
        Console.WriteLine();

        //saisie du passager en cours
        Console.WriteLine($"Passager {numPassager} :");
        if(passagersList.Count == 0) //si le nombre de passagers dans la liste est égal à zéro, il faut d'abord saisir un passager humain
        {
            Console.Write("Saisissez le nom du passager : ");
            nom = Console.ReadLine();
            Console.Write("Saisissez le prénom du passager : ");
            prenom = Console.ReadLine();
            Console.Write("Saisissez l'age du passager : ");
            age = int.Parse(Console.ReadLine());

            //appliquer la catégorie au passager en fonction de son age
            if(age < 4)
            {
                codeCat = "bebe";
            }
            else if(age >= 4 && age < 18)
            {
                codeCat = "enf417";
            }
            else if(age >= 18  && age <= 25)
            {
                codeCat = "jeu1825";
            }
            else if(age > 25)
            {
                codeCat = "adu26p";
            } 
        }
        else //une fois au moins un passager humain enregistré, on peut saisir des passagers animaux ou humains
        {
            if(ConfirmYN("Le passager est un animal de compagnie [y/n] ? ") == 1)
            {
                Console.Write("Saisissez le nom de l'animal : ");
                prenom = Console.ReadLine();
                nom = "Animal";
                codeCat = "ancomp";
            }
            else
            {
                Console.Write("Saisissez le nom du passager : ");
                nom = Console.ReadLine();
                Console.Write("Saisissez le prénom du passager : ");
                prenom = Console.ReadLine();
                Console.Write("Saisissez l'age du passager : ");
                age = int.Parse(Console.ReadLine());

                //appliquer la catégorie au passager en fonction de son age
                if(age < 4)
                {
                    codeCat = "bebe";
                }
                else if(age >= 4 && age < 18)
                {
                    codeCat = "enf417";
                }
                else if(age >= 18  && age <= 25)
                {
                    codeCat = "jeu1825";
                }
                else if(age > 25)
                {
                    codeCat = "adu26p";
                } 
            }
        }
        Passager passager = new Passager(nom, prenom, codeCat);
        return passager;
    }
    static void CreerVehicule(ref Dictionary <string,string> catVehicules, ref List <Vehicule> vehicules)
    {
        //boucle
        bool vehiculesFinis = false;
        while(!vehiculesFinis)
        {
            //affichage
            int i = 0;
            Console.Clear();
            TitreEncadre("EFFECTUER UNE RESERVATION");
            Console.WriteLine("Ajout de véhicule(s) : ");
            Console.WriteLine();
            Console.WriteLine(" ___________________________________________________________________ ");
            Console.WriteLine("| Catégorie | Description                                  | Nombre |");
            foreach (Vehicule vehicule in vehicules)
            {
                i ++;
                Console.WriteLine($"| {i.ToString().PadRight(9)} | {catVehicules[vehicule.codeCategorie].PadRight(44)} | {vehicule.quantite.ToString().PadRight(6)} |");
            }
            Console.WriteLine("|___________________________________________________________________|");
            Console.WriteLine();

            //saisie
            string questionVehic = "Tappez la catégorie à ajouter, ou tappez x pour confirmer : ";
            string[]choixPossiblesVehic = new string[13] {"1","2","3","4","5","6","7","8","9","10","11","x","X"};
            int choixVehicule = VerifSaisieString(choixPossiblesVehic, questionVehic);

            if(choixVehicule == 12 || choixVehicule == 13) //si l'utilisateur tappe x ou X
            {
                vehiculesFinis = true; //on sort de la boucle d'ajout des véhicules
            }
            else
            {
                //on incrémente la catégorie sélectionnée
                int compte = vehicules[choixVehicule -1].quantite;  //on récupère la quantité de la catégorie sélectionnée
                compte ++;                                          //on l'incrémente
                vehicules[choixVehicule -1] = new Vehicule(vehicules[choixVehicule -1].codeCategorie,compte); //on remplace le véhicule sélectionné par un nouveau véhicule
                //de même codeCatégorie, avec la quantité qui vient d'être incrémentée
            }
        }
    }
    static void TitreEncadre(string phrase)
    {
        //déclarations
        int longueur;

        longueur = phrase.Length;

        Console.Write(" ___");
        for(int i = 0; i < longueur; i++)
        {
            Console.Write("_");
        }
        Console.Write("___ \n");
        Console.Write("|   ");
        Console.Write($"{phrase}");
        Console.Write("   |\n");
        Console.Write("|___");
        for(int i = 0; i < longueur; i++)
        {
            Console.Write("_");
        }
        Console.Write("___|\n");
        Console.WriteLine();
    }
    static void Main()
    {
        //déclarations menu et affichage
        bool menu = true;
        bool isLogoMenu = false;
        int choix = 0; 
        int choixAffichageLigne, choixAffichageTarif;
        Dictionary <int,string> itineraires = new Dictionary<int, string>(); //dictionnaire qui stocke les itinéraires
        List <string> logoMenu = new List<string>();
        Dictionary <char,string> charTable = new Dictionary<char,string>();
        Dictionary <string,string> bateauTable = new Dictionary<string, string>();

        //création et chargement des dictionnaires horaires, tarifs et catVéhicules
        Dictionary <int,Dictionary<int,Journee>> horairesComplets = new Dictionary<int,Dictionary<int,Journee>>(); //dictionnaire qui stocke tous les horaires pour tous les itinéraires
        Dictionary <int,Dictionary<string,Tarif>> tarifsComplets = new Dictionary<int,Dictionary<string,Tarif>>();
        ChargementDicoHoraires(ref horairesComplets);
        ChargementDicoTarifs(ref tarifsComplets);
        Dictionary <string, string> catVehicules = new Dictionary<string, string>();
        ChargementDicoCatVehicules(ref catVehicules);

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
                        TitreEncadre("AFFICHAGE DES LIGNES");
                        affichageLignes(itineraires);
                        retourMenu();
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        TitreEncadre("AFFICHAGE DES HORAIRES");
                        affichageLignes(itineraires);

                        //choix utilisateur
                        string questionChoixLigne ="De quelle ligne souhaitez-vous afficher les horaires ? ";
                        char[]choixPossiblesLigne = new char[4] {'1','2','3','4'};
                        choixAffichageLigne = VerifSaisieChar(choixPossiblesLigne, questionChoixLigne);
                        
                        //affichage horaire
                        Console.Clear();
                        TitreEncadre("AFFICHAGE DES HORAIRES");
                        Console.WriteLine($"Ligne {itineraires[choixAffichageLigne]} :\n");
                        affichageHorairesItineraire(choixAffichageLigne, ref horairesComplets, ref charTable);
                        Console.WriteLine();
                        retourMenu();
                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        TitreEncadre("AFFICHAGE DES TARIFS");
                        affichageLignes(itineraires);

                        //choix utilisateur
                        string questionAffichageTarif ="De quelle ligne souhaitez-vous afficher les tarifs ? ";
                        char[]choixPossiblesAffichageTarif = new char[4] {'1','2','3','4'};
                        choixAffichageTarif = VerifSaisieChar(choixPossiblesAffichageTarif, questionAffichageTarif);

                        //affichage tarifs
                        Console.Clear();
                        TitreEncadre("AFFICHAGE DES TARIFS");
                        Console.WriteLine($"Ligne {itineraires[choixAffichageTarif]} :\n");
                        AffichageTarifs(choixAffichageTarif, tarifsComplets);
                        Console.WriteLine();
                        retourMenu();
                        break;    
                    }
                case 4:
                    {
                        Console.Clear();

                        CreerResa(itineraires, ref horairesComplets, ref charTable, catVehicules, ref tarifsComplets);
                        

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