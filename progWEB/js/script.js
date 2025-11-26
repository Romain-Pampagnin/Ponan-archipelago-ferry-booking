let liaison = { //declaration dictionnaire liaison
    1: ["Lorient", "Groix"],
    2: ["Grois", "Lorient"],
    3: ["Quiberon", "Le Palais"],
    4: ["Le Palais", "Quiberon"]
}

let addams = {
	"reservation": 
		{
			"nom": "Famille Addams",
			"idLiaison": 1,
			"date": "2025-11-01",
			"heure": "09:45",
			"horodatage": "2025-10-20 16:55:14",
			"navire": "Breizh Nevez"
		},
    "passagers": [
		{
			"nom": "Addams",
			"prenom": "Gomez",		
			"codeCategorie": "adu26p"
		},
		{
			"nom": "Addams",
			"prenom": "Morticia",		
			"codeCategorie": "adu26p"
		},
		{
			"nom": "Addams",
			"prenom": "Lurch",		
			"codeCategorie": "je1825"
		},
		{
			"nom": "Addams",
			"prenom": "Mercredi",		
			"codeCategorie": "enf417"
		},
		{
			"nom": "Addams",
			"prenom": "Pugsley",		
			"codeCategorie": "bebe"
		}
    ],

    "vehicules": [
		{
			"codeCategorie": "cat4",
			"quantite": 1
		},
		{
			"codeCategorie": "trot",
			"quantite": 2
		}	  
	] 
}

let gareDepart = document.getElementsByClassName("Depart");
let gareArrivee = document.getElementsByClassName("Arrivee");
let date = document.getElementsByClassName("Date");
let heure = document.getElementsByClassName("Heure");
let bateau = document.getElementsByClassName("Bateau");
let NumReservation = document.getElementsByClassName("NumReservation");
let nomReservation = document.getElementsByClassName("NomReservation");

let nomPassager = document.getElementById("NomPassager");
let prenomPassager = document.getElementById("PrenomPassager");
let categoriePassager = document.getElementById("CategoriePassager");

gareDepart[0].innerHTML += liaison[(addams.reservation.idLiaison)][0];
gareArrivee[0].innerHTML += liaison[(addams.reservation.idLiaison)][1];
date[0].innerHTML += addams.reservation.date;
heure[0].innerHTML += addams.reservation.heure;
bateau[0].innerHTML += addams.reservation.navire;


let nbPassagers = addams.passagers.length;
let numPassager = 0;

nomPassager.innerHTML = addams.passagers[numPassager].nom;
prenomPassager.innerHTML = addams.passagers[numPassager].prenom;
categoriePassager.innerHTML = addams.passagers[numPassager].codeCategorie;

let PassagerPrecedent = function(){
	if (numPassager == 0)
	{
		throw new Error("Pas de passager precedent");
    }
	
	numPassager--;

	nomPassager.innerHTML = addams.passagers[numPassager].nom;
	prenomPassager.innerHTML = addams.passagers[numPassager].prenom;
	categoriePassager.innerHTML = addams.passagers[numPassager].codeCategorie;

}

let PassagerSuivant = function(){
	if (numPassager == nbPassagers - 1)
	{
		throw new Error("Pas de passager Suivant");
    }
	
	numPassager++;

	nomPassager.innerHTML = addams.passagers[numPassager].nom;
	prenomPassager.innerHTML = addams.passagers[numPassager].prenom;
	categoriePassager.innerHTML = addams.passagers[numPassager].codeCategorie;
}

let buttonPrecedent = document.getElementById("precedent");
let buttonSuivant = document.getElementById("suivant");


buttonPrecedent.addEventListener("click", PassagerPrecedent);
buttonSuivant.addEventListener("click", PassagerSuivant);






// for(let elem of addams.passagers) {
//     p.innerHTML += elem.prenom + " ";
// 	p.innerHTML += elem.nom + " ";
// }
