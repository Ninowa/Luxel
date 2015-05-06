// ce script fait en sorte que la caméra soit au-dessus de son objectif (SHA) et le suive.

using UnityEngine;
using System.Collections;

public class DungeonCamera : MonoBehaviour {
	public GameObject target; // crée une variable qui sera SHA.
	public float damping = 1; // crée une variable pour le step
	Vector3 offset; // crée une variable qui va garder la distance entre le joueur et la caméra.

	void Start () {
		offset = transform.position - target.transform.position; // On assigne la première valeur de la distance entre SHA et la caméra.
	}

	void LateUpdate () {
		//Vector3 NouvellePosition = target.transform.position + offset;
		//transform.position = NouvellePosition; // on sauvergarde la position pour la prochain déplacement.

		Vector3 desiredPosition = target.transform.position + offset; // crée une variable où on assigne à chaque fois la nouvelle position de SHA.
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping); // bouge la caméra en fonction des positions et du step
		transform.position = position; // on sauvergarde la position pour la prochain déplacement.

		transform.LookAt(target.transform.position); // fait en sorte que la caméra suive le joueur.
	}
}