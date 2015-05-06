// ce script fait en sorte que la caméra recherche son objectif (Player2) et le suive.

using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	public GameObject target;

	void LateUpdate () { // cette fonction est mieux que Update dans certains cas! Elle attend la fin du déplacement du Player2.
		transform.LookAt(target.transform); // Suit la position du Player2.
	}
}
