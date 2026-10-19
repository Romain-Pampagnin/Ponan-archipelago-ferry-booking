let Main = async function () {
    
    //on parcours toutes les liaisons
    for (let i = 1; i <= 4; i++)
    {   
        //on appelle l'api
        let response = await fetch(`https://can.iutrs.unistra.fr/api/liaison/${i}/chiffreAffaire`)
        let data = await response.json();

        //et on modifie le html avec les reponses de l'API
        let caPassagers = document.getElementById(`caPassagers${i}`);
        let caVehicules = document.getElementById(`caVehicules${i}`);
        let nbPassagers = document.getElementById(`nbPassagers${i}`);
        let nbVehicules = document.getElementById(`nbVehicules${i}`);

        caPassagers.innerHTML = data.passagers.chiffreAffaire.toFixed(1);
        caVehicules.innerHTML = data.vehicules.chiffreAffaire.toFixed(1);
        nbPassagers.innerHTML = data.passagers.nombre.toFixed(1);
        nbVehicules.innerHTML = data.vehicules.quantite.toFixed(1);
    }

}

Main();
