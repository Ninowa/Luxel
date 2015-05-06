#pragma strict

var perso : GameObject;
var chemin : GameObject;
var chemin2 : GameObject;

function clicked()
{
	
	//ANIM_AVT_TP
	//perso.animation.Play();
	//yield WaitForSeconds(4);
	
	print ("recu");
	perso.SendMessage("Sha_TP2", SendMessageOptions.DontRequireReceiver); // envoie la fonction Sha_TP à l'objet.
	print("done");
	chemin.SetActive(true); // acctive le chemin pour le déplacement suivant
	chemin2.SetActive(true); // idem
	gameObject.SetActive(false); // désactive le chemin utilisé précédemment
	
	//chemin.SetActive(false); // désactive le chemin qui permet d'aller sur l'endroit où on se trouve
	
	
}