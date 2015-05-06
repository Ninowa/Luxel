// ce script fait en sorte que la caméra soit au-dessus de son objectif (SHA) et le suive tout en pouvant faire une rotation.

using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public GameObject target; // crée une variable qui sera SHA.
	public float damping = 1; // crée une variable qui permet d'avoir un fluidité (step)
	Vector3 offset; // crée une variable qui va garder la distance entre le joueur et la caméra.
	
	void Start () {
		offset = target.transform.position - transform.position; // On assigne la première valeur de la distance entre SHA et la caméra.
	}

	void LateUpdate () {
		// permet de garder l'offset (distance SHA/Caméra) correct.
		float desiredAngle = target.transform.eulerAngles.y; // crée une variable qui aura l'angle pour voir SHA.
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0); // utilise la variable précédente et la transforme en une rotation.

		transform.position = target.transform.position - (rotation * offset); // permet d'avoir une distance entre SHA et la caméra qui change en fonction de la rotation.

		transform.LookAt(target.transform); // suit SHA.
	}
}