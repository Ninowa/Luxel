#pragma strict

var perso : GameObject;
var chemin : GameObject;
var chemin2 : GameObject;

function clicked()
{
	//ANIM_AVT_TP
	//perso.animation.Play();
	//yield WaitForSeconds(4);
	
	perso.SendMessage("Sha_TP3", SendMessageOptions.DontRequireReceiver); // envoie la fonction Sha_TP à l'objet.
	chemin.SetActive(true); // active le chemin pour le déplacement suivant
	chemin2.SetActive(false); // désactive un des chemins de la position antérieure
	gameObject.SetActive(false); // désactive le chemin utilisé précédemment
}