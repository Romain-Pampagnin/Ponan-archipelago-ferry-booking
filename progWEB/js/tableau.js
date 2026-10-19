let affichage = async function (){
    
    let parametresUrl = new URLSearchParams(window.location.search);
	let liaison = parametresUrl.get("liaison");
    let date = parametresUrl.get("date")

    let response = await fetch(`https://can.iutrs.unistra.fr/api/liaison/${liaison}/remplissage/${date}`);
    let data = await response.json();

    let infos = document.getElementById("infos");

    let tauxRempliPassagers;
    let tauxRempliVehicules;
    let i = 1;

    for (let key in data)
    {   
        tauxRempliPassagers = data[key].nbReservationPassagers / data[key].capacitePassagers * 100;
        tauxRempliVehicules = data[key].nbReservationVoitures / data[key].capaciteVoitures * 100;

        tauxRempliPassagers = tauxRempliPassagers.toFixed(1);
        tauxRempliVehicules = tauxRempliVehicules.toFixed(1);

        infos.innerHTML += `
                    <article>
                    <p>Heure :<span id="heure${i}"></span></p>
                    <p>Capacité des passagers :<span id="capacitePassagers${i}"></span></p>
                    <p>Nombre de reservation de passagers :<span id="nbPassagers${i}"></span></p>
                    <p>Taux de remplissage des passagers (en %) :<span id="rempliPassagers${i}"></span></p>
                    <p>Capacité des voitures :<span id="capaciteVoitures${i}"></span></p>
                    <p>Nombre de reservation de voitures :<span id="nbVoitures${i}"></span></p>
                    <p>Taux de remplissage des voitures (en %) :<span id="rempliVehicules${i}"></span></p>
                    </article>
                    `;

        let heure = document.getElementById(`heure${i}`);
        let capacitePassagers = document.getElementById(`capacitePassagers${i}`);
        let nbPassagers = document.getElementById(`nbPassagers${i}`);
        let rempliPassagers = document.getElementById(`rempliPassagers${i}`);
        let capaciteVoitures = document.getElementById(`capaciteVoitures${i}`);
        let nbVoitures = document.getElementById(`nbVoitures${i}`);
        let rempliVehicules = document.getElementById(`rempliVehicules${i}`);

        heure.innerHTML = data[key].heure;
        capacitePassagers.innerHTML = data[key].capacitePassagers;
        nbPassagers.innerHTML = data[key].nbReservationPassagers;
        rempliPassagers.innerHTML = tauxRempliPassagers;
        capaciteVoitures.innerHTML = data[key].capaciteVoitures;
        nbVoitures.innerHTML = data[key].nbReservationVoitures;
        rempliVehicules.innerHTML = tauxRempliVehicules;

        if (tauxRempliPassagers <= 50) 
        {
            rempliPassagers.style.backgroundColor="green";
        }

        else if (tauxRempliPassagers > 50 && tauxRempliPassagers <= 75)
        {
            rempliPassagers.style.backgroundColor="orange";
        }

        else if (tauxRempliPassagers > 75 && tauxRempliPassagers <= 100)
        {
            rempliPassagers.style.backgroundColor="red";
        }

        else {
            rempliPassagers.style.backgroundColor="rgb(183, 255, 0)";
        }

        if (tauxRempliVehicules <= 50) 
        {
            rempliVehicules.style.backgroundColor="green";
        }

        else if (tauxRempliVehicules > 50 && tauxRempliVehicules <= 75)
        {
            rempliVehicules.style.backgroundColor="orange";
        }

        else if (tauxRempliVehicules > 75 && tauxRempliVehicules <= 100)
        {
            rempliVehicules.style.backgroundColor="red";
        }

        else {
            rempliVehicules.style.backgroundColor="rgb(183, 255, 0)";
        }

        i++;
    
    
    }
    

}

affichage();