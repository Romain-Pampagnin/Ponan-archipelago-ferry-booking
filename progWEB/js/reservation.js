// json pour la gestion des variables globales
let variables = {
    numeroReservation: null,
    carteActuelle: "passager",
    numeroPassager: 1,
    numeroVehicule: null,
    nbPassagers: null,
    nbVehicules: null,
};

//*********FONCTIONS (async car on appelle l'API donc on attend sa reponse ) ******************/
//La plupart des recuperations de variables dans le dome html avec getDocumentbyId se font sur des span avec des id

//Fonction de l'affichage general (infos qui sont affichés sur les deux cartes)
let Affichage = async function(){
	
	//fetch API classique vers la reservation avec le numero de resa
	let responseReservation = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}`);
    let dataReservation = await responseReservation.json();
	
	//recuperation dans le html grace aux id
	let depart = document.getElementById("depart");
	let arrivee = document.getElementById("arrivee");
	let date = document.getElementById("date");
	let heure = document.getElementById("heure");
	let bateau = document.getElementById("bateau");
	let numReservation = document.getElementById("numReservation");
	let nomReservation = document.getElementById("nomReservation");

	//modification du html
	depart.innerHTML = dataReservation.portDepart;
	arrivee.innerHTML = dataReservation.portArrivee
	date.innerHTML = dataReservation.date;
	heure.innerHTML = dataReservation.heure;
	bateau.innerHTML = dataReservation.bateau;
	numReservation.innerHTML = dataReservation.id;
	nomReservation.innerHTML = dataReservation.nom;
}

//fonction pour affiché les infos propres aux passagers 
let AffichagePassager = async function(){

	//on change le json de variables globales pour dire que la carte actuellement affichée est la carte passager
	variables.carteActuelle = "passager";

	//recuperation html
	let infoPassagers = document.getElementById("infoPassagers");
	let infoVehicules = document.getElementById("infoVehicules");

	// on ajoute la class hidden à la div infoPassagers et on la retire à infoVehicules
	// la class hidden cache cette div en css
	// on affiche donc que la carte passager dans ce cas ci
	infoPassagers.classList.remove("hidden");
	infoVehicules.classList.add("hidden");
	
	//fetch API avec numero de resa et numero de passager
	let responsePassager = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}/passager/${variables.numeroPassager}`);
    let dataPassager = await responsePassager.json();
	
	//recuperation html
	let nomPassager = document.getElementById("nomPassager");
	let prenomPassager = document.getElementById("prenomPassager");
	let catPassager = document.getElementById("catPassager");
	let prixPassager = document.getElementById("prixPassager");

	//modification html
	nomPassager.innerHTML = dataPassager.nom;
	prenomPassager.innerHTML = dataPassager.prenom;
	catPassager.innerHTML = dataPassager.libelleCategorie;
	prixPassager.innerHTML = dataPassager.price;
}

//fonction pour afficher les infos propres aux passagers, exactement la meme que la precedente mais pour afficher la carte passagers
let AffichageVehicule = async function(){
	
	variables.carteActuelle = "vehicule"

	let infoPassagers = document.getElementById("infoPassagers");
	let infoVehicules = document.getElementById("infoVehicules");

	infoVehicules.classList.remove("hidden");
	infoPassagers.classList.add("hidden");

	let responseVehicule = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}/vehicule/${variables.numeroVehicule}`);
    let dataVehicule = await responseVehicule.json();
	
	let catVehicule = document.getElementById("catVehicule");
	let nombreVehicule = document.getElementById("nombreVehicule");
	let prixVehicule = document.getElementById("prixVehicule");

	catVehicule.innerHTML = dataVehicule.libelle;
	nombreVehicule.innerHTML = dataVehicule.quantite;
	prixVehicule.innerHTML = dataVehicule.prix;
}

// fonction qui change le numero de passager ou de vehicule en fonction d'un bouton et qui affiche ce passager ou vehicule
let Precedent = function(){
	
	// pour les passagers
	if (variables.carteActuelle == "passager")
	{	
		// si on est deja sur le premier
		if (variables.numeroPassager == 1)
		{
			return; // on return 
		}
		
		//sinon, on decremente
		variables.numeroPassager--;

		// et on affiche le passager precedent
		AffichagePassager(variables.numeroPassager);
	}

	// pour les véhicules
	else
	{	
		//pareil
		if (variables.numeroVehicule == 1)
		{
			return;
		}
		
		variables.numeroVehicule--;

		AffichageVehicule(variables.numeroVehicule);
	}
}

let Suivant = function(){
	
	//fonction similaire a la precedente
	if (variables.carteActuelle == "passager")
	{	
		//si on est sur le dernier
		if (variables.numeroPassager == variables.nbPassagers)
		{
			return; // on return
		}
		
		//sinon on incremente
		variables.numeroPassager++;

		// et on affiche le suivant
		AffichagePassager(variables.numeroPassager);
	}

	// pour les vehicules
	else
	{
		if (variables.numeroVehicule == variables.nbVehicules)
		{
			return;
		}
		
		variables.numeroVehicule++;

		AffichageVehicule(variables.numeroVehicule);
	}
}

//fonction Main (async aussi car on attend que les fonctions qu'on appelle se terminent)
let Main = async function () {
	
	//recuperation du numero de reservation dans l'URL grace a la method get employée dans le formulaire de la page submitReservation.html
	let parametresUrl = new URLSearchParams(window.location.search);
	variables.numeroReservation = parametresUrl.get("numeroReservation");

	// on genere un lien vers la page facture.html avec le numero de resa dans l'url pour toujours y avoir acces meme en changeant de page
	let factureLink = document.getElementById("factureLink");
	factureLink.innerHTML = `<li><a href=facture.html?numeroReservation=${variables.numeroReservation}>Facture</a></li>`

	//fetch API avec le numero de reservation
	let response = await fetch(`https://can.iutrs.unistra.fr/api/reservation/${variables.numeroReservation}`);
    let data = await response.json();
	
	//changement dans les variables globales grace à l'API
	variables.nbPassagers = data.nbPassagers;
	variables.nbVehicules = data.nbVehicules;

	// si le nb de vehicules est > 0
	if (variables.nbVehicules > 0)
	{	
		//alors le vehicule actuel est initialisé à 1
		variables.numeroVehicule = 1;
	}

	else 
	{	
		//sinon 0
		variables.numeroVehicule = 0;
	}

	// on fait un premier affichage en arrivant sur le site (avec la carte du premier passager)
	await Affichage();
	await AffichagePassager(1);

	// recuperation html des buttons de choix de carte
	let buttonPassager = document.getElementById("buttonPassager");
	let buttonVehicule = document.getElementById("buttonVehicule");

	// addEventListener pour detecter le clic
	buttonPassager.addEventListener("click", async () => await AffichagePassager(1)); //aficher le premier passager si clic sur le bouton passager
	buttonVehicule.addEventListener("click", async () => { 
		if (variables.numeroVehicule == 0) 
		{
			alert("il n'y a pas de vehicule pour cette reservation") // si pas de vehicule alors alert
		} 
		else 
		{ 
			await AffichageVehicule(1) // sinon on affiche le premier vehicule
		} 
		} )
	
	//recuperation html des boutons precedent et suivant
	let buttonPrecedent = document.getElementById("buttonPrecedent");
	let buttonSuivant = document.getElementById("buttonSuivant");
	
	//detection du clic et appel de la fonction 
	buttonPrecedent.addEventListener("click",Precedent);
	buttonSuivant.addEventListener("click",Suivant);
}

Main();