// json pour la gestion des variables globales
let variables = { 
    numeroReservation: null,
    nbPassagers: null,
    nbVehicules: null,
    total: 0,
};
// json pour les categories de passagers, avec un tableau qui stock nb de passagers dans cette categorie, le prix unique, et le prix total
let catPassager = {
    "bebe": [0, 0, 0],
    "enf417": [0, 0, 0],
    "jeu1825": [0, 0, 0],
    "adu26p": [0, 0, 0],
    "ancomp": [0, 0, 0],
};

// pareil
let catVehicule = {
    "trot": [0, 0, 0],
    "velo": [0, 0, 0],
    "velelec": [0, 0, 0],
    "cartand": [0, 0, 0],
    "mobil": [0, 0, 0],
    "moto": [0, 0, 0],
    "cat1": [0, 0, 0],
    "cat2": [0, 0, 0],
    "cat3": [0, 0, 0],
    "cat4": [0, 0, 0],
    "camp": [0, 0, 0],
};

//*********FONCTIONS (async car on appelle l'API donc on attend sa reponse ) ******************/
//La plupart des recuperations de variables avec getDocumentbyId se font sur des span avec des id dans le html


//fonction qui affiche les infos de la facture
let AffichageInfo = async function() {

    // recherche des parametres (le numero de resa) dans l'url grace au lien crée dans la page reservation.js
    let parametresUrl = new URLSearchParams(window.location.search);
    variables.numeroReservation = parametresUrl.get("numeroReservation");

    //fetch API classique vers la reservation
    let responseReservation = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}`);
    let dataReservation = await responseReservation.json();

    //changement dans les variables globales pour le nb de passagers et le nombre de vehicules, grace a l'api
    variables.nbPassagers = dataReservation.nbPassagers;
    variables.nbVehicules = dataReservation.nbVehicules;

    //get des element dans le dome html avec leurs ID
    let horodatage = document.getElementById("horodatage");
    let reservationId = document.getElementById("reservationId");
    let reservationNom = document.getElementById("reservationNom");
    let traversee = document.getElementById("traversee");
    let date = document.getElementById("dateTraversee");
    let heure = document.getElementById("heureDepart");
    let bateau = document.getElementById("bateau");

    // creation d'une date avec la fonction ci dessous ( pour l'horodatage de la facture )
    let dateDuJour = new Date().toLocaleDateString('fr-FR');

    // changement dans le HTML
    horodatage.innerHTML = dateDuJour;
    reservationId.innerHTML = dataReservation.id;
    reservationNom.innerHTML = dataReservation.nom;
    traversee.innerHTML = `${dataReservation.portDepart} - ${dataReservation.portArrivee}`;
    date.innerHTML = dataReservation.date; 
    heure.innerHTML = dataReservation.heure;
    bateau.innerHTML = dataReservation.bateau;
}

//fonction qui calcule les sommes de prix a afficher (pour les passagers)
let sommePassagers = async function() {

    let sommeTotalPassagers = 0;

    // on parcours tous les passagers
    for (let i = 1; i <= variables.nbPassagers; i++)
    {   
        //appel API avec le numero de resa et le numero du passager
        let responsePassager = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}/passager/${i}`);
        let dataPassager= await responsePassager.json();

        catPassager[dataPassager.code][0]++; // le nombre de passager augmente dans le json
        catPassager[dataPassager.code][1] = dataPassager.price; // le prix pour cette categorie est fixé
        catPassager[dataPassager.code][2] += dataPassager.price; // la somme pour cette categorie augmente
        sommeTotalPassagers += dataPassager.price; // la somme totale augmente
    }

    // on parcours le json catPassager
    for (const key in catPassager)
    {   
        // on recupere le tableau a modifier dans le html
        const tablePassagers = document.getElementById('tablePassagers');

        //si il y a + d'un passager dans cette categorie alors
        if (catPassager[key][0] > 0)
        {   
            //on ajoute une ligne au tableau
            const row = tablePassagers.insertRow();

            //et on affiche les infos
            row.insertCell(0).textContent = key;
            row.insertCell(1).textContent = catPassager[key][0]
            row.insertCell(2).textContent = catPassager[key][1].toFixed(1)
            row.insertCell(3).textContent = catPassager[key][2].toFixed(1)
        }
    }

    //et on modifie le total dans le html aussi
    const totalPassagers = document.getElementById("totalPassagers");
    totalPassagers.innerHTML = sommeTotalPassagers.toFixed(1);

    // et le total (passagers + vehicules) augmente
    variables.total += sommeTotalPassagers;

}

// fonction similaire a la precedente
let sommeVehicules = async function(){
    
    let sommeTotalVehicule = 0;

    for (let i = 1; i <= variables.nbVehicules; i++)
    {
        let responseVehicule = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}/vehicule/${i}`);
        let dataVehicule= await responseVehicule.json();

        catVehicule[dataVehicule.code][0] += dataVehicule.quantite;
        catVehicule[dataVehicule.code][1] = dataVehicule.prix;
        catVehicule[dataVehicule.code][2] += dataVehicule.prix * dataVehicule.quantite; //changement par rapport aux passagers car il peut y avoir plusieurs quantités donc on multiplie le prix
        sommeTotalVehicule += dataVehicule.prix * dataVehicule.quantite;
    }

    for (const key in catVehicule)
    {   
    
        const tableVehicules = document.getElementById('tableVehicules');

        if (catVehicule[key][0] > 0)
        {
            const row = tableVehicules.insertRow();
            row.insertCell(0).textContent = key;
            row.insertCell(1).textContent = catVehicule[key][0]
            row.insertCell(2).textContent = catVehicule[key][1].toFixed(1)
            row.insertCell(3).textContent = catVehicule[key][2].toFixed(1)
        }
    }

    const totalVehicules = document.getElementById("totalVehicules");
    totalVehicules.innerHTML = sommeTotalVehicule.toFixed(1);

    variables.total += sommeTotalVehicule;
}

//fonction Main (async aussi car on attend que les fonctions qu'on appelles se terminent)
let Main = async function() {
    
    await AffichageInfo(); // on fait le premier affichage
    await sommePassagers(); // puis la somme et l'affichage des passagers
    await sommeVehicules(); // pareil pour les vehicules

    const totalPrix = document.getElementById("totalPrix");
    totalPrix.innerHTML = variables.total.toFixed(1); // et on modifie le html du prix total (vehicule + passagers)
}

Main();


