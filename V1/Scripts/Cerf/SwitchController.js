#pragma strict

var cameraMain : Camera;
var cameraCombat : Camera;
var cameraTop : Camera;
var perso : GameObject;
var perso2 : GameObject;
var pilier : GameObject;
var scriptArrivee : TP_2;
var scriptArrivee2 : Mvt;
//var scriptThirdPersonController : RPG_Controller;
//var scriptThirdPersonAnimation : RPG_Animation;

function OnTriggerEnter (col : Collider) {
	if(col.gameObject.name == "SHA_ThirdPerson")
    {
        print("entreeCOL");
        perso.SetActive(true);
        cameraCombat.active = true;
        cameraTop.active = true;
        pilier.SetActive(true);
        scriptArrivee = GameObject.Find("Collider_Chemin_milieu1").gameObject.GetComponent(TP_2);
        scriptArrivee.enabled = true;
        scriptArrivee2 = GameObject.Find("Arrivee1").gameObject.GetComponent(Mvt);
        scriptArrivee2.enabled = true;
        perso2.SetActive(false);
        cameraMain.active = false;
        
        //GameObject.Find("Main Camera").SetActive(false);
        //GameObject.Find("Camera_Cerf").active = false;
        //GameObject.Find("Camera_TOP").SetActive(false);
		//scriptThirdPersonController = GameObject.Find("SHA").gameObject.GetComponent(RPG_Controller);
        //scriptThirdPersonController.enabled = false;
        
        //scriptThirdPersonAnimation = GameObject.Find("SHA").gameObject.GetComponentInChildren(RPG_Animation);
        //scriptThirdPersonAnimation.enabled = false;
        //cameraMain.enabled = false;
        //cameraCerf.enabled = true;
        //cameraTop.enabled = true;
        print("done");
    }
	
}
