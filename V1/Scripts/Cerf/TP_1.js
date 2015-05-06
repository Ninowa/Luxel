#pragma strict

var perso : GameObject;
var chemin : GameObject; // récupère un chemin
var chemin2 : GameObject; // idem

function clicked()
{
	//print("envoie_TP1");
	//script.GetComponent("Tp_2").active = false;
	//GameObject.Find("Chemin_milieu3").GetComponent(Tp_2);enabled = false;
	
	//ANIM_AVT_TP
	//perso.animation.Play();
	//yield WaitForSeconds(4);
	
	perso.SendMessage("Sha_TP", SendMessageOptions.DontRequireReceiver); // fait le TP
	chemin.SetActive(true); // active le chemin pour le déplacement suivant
	chemin2.SetActive(false); // désactive un des chemins de la position antérieure
	gameObject.SetActive(false); // désactive le chemin utilisé précédemment
	
}

