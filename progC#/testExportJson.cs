using System;
using System.IO;
using System.Collections.Generic;

class testExportJson
{
    static void Main()
    {
        //déclarations
        string chemin = "../exports/essai.json";

        StreamWriter sw = new StreamWriter(chemin);
        sw.WriteLine("essai");
        sw.Close();

    }   
}
