using System;
using System.IO;
using System.Collections.Generic;

class testImageMenu
{
    static void Main()
    {
        //déclarations
        string ligne;

        if (File.Exists("imgMenu.txt"))
        {
            //création du flux de type texte pour lire des fichiers
            FileStream fsImageMenu = new FileStream("imgMenu.txt", FileMode.Open, FileAccess.Read);
            StreamReader srImageMenu = new StreamReader(fsImageMenu);

            //parcourir le flux
            while (!srImageMenu.EndOfStream)
            {
                //lecture d'une ligne
                ligne = srImageMenu.ReadLine();
                Console.WriteLine(ligne);
            }

            //fermeture du flux
            srImageMenu.Close();
        }
    }
}