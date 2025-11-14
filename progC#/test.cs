using System;
using System.IO;

class SAE11
{
    static void Main()
    {
        //déclarations
        bool menu = true;
        int choix, i;
        string[] Lignes = new string[4]{"Lorient - Groix","Groix - Lorient","Quiberon - Le Palais","Le Palais - Quiberon"};
        

        //menu
        while(menu == true)
        {
            Console.WriteLine(" ------------------------------------ ");
            Console.WriteLine("|              CAN                   |");
            Console.WriteLine("| Compagnie Alsacienne de Navigation |");
            Console.WriteLine(" ------------------------------------ ");
            Console.WriteLine();
            Console.WriteLine("1. Afficher les lignes disponibles");
            Console.WriteLine("2. Afficher les horaires");
            Console.WriteLine("3. Afficher les prix");
            Console.WriteLine("4. Effectuer une réservation");
            Console.WriteLine("5. Quitter");
            choix = int.Parse(Console.ReadLine());

            switch (choix)
            {
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("Lignes opérées par la CAN:");
                        for(i = 0; i < Lignes.Length; i++)
                        {
                            Console.WriteLine($"    > {i+1}: {Lignes[i]}");
                        }
                        Console.WriteLine();
                        break;
                    }
                case 2:
                    {
                        Console.Clear();
                        break;
                    }
                case 3:
                    {
                        Console.Clear();
                        break;    
                    }
                case 4:
                    {
                        Console.Clear();
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
