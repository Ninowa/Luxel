#pragma strict

var destination : Transform; // crée une variable qui aura les coordonnées de la destination du TP.
var TP : int = 0;
var joueur : GameObject;

function Start () {
	joueur.animation.Play();
	yield WaitForSeconds(3);
	TP = 1;
}

function OnTriggerEnter(other : Collider){ // au moment de la collision avec un objet.
	print("OUI");
	if (TP == 1){
		print("OK");
		if (other.tag == "Player")// si l'objet s'appelle Player.
		{
			other.transform.position = destination.position; // on donne la nouvelle position au Player.
		}
	}
}