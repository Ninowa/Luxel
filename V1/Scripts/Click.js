#pragma strict

var camera_principale : Camera;
var camera_secondaire : Camera;

    function Update () {
    	if(Input.GetKeyDown("escape")) {//When a key is pressed down it see if it was the escape key if it was it will execute the code
    	Application.Quit(); // Quits the game
    	}
    }

function OnGUI(){
	var e : Event = Event.current;

	if (e.button == 0 && e.type == EventType.MouseDown){

	//var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var ray = camera_principale.ScreenPointToRay(Input.mousePosition);
	var ray2 = camera_secondaire.ScreenPointToRay(Input.mousePosition);

	var hit : RaycastHit;

	if (Physics.Raycast(ray, hit)){
		//print("Clic gauche, " + hit.transform.name + " touché");
		GameObject.Find(hit.transform.name).SendMessage("clicked", SendMessageOptions.DontRequireReceiver);
		//print("message envoye");
		}
		
	else if	(Physics.Raycast(ray2, hit)){
		//print("Clic gauche, " + hit.transform.name + " touché");
		GameObject.Find(hit.transform.name).SendMessage("clicked", SendMessageOptions.DontRequireReceiver);
		//print("message envoye");
		}
	}
}