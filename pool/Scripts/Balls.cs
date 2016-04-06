using UnityEngine;
using System.Collections;

public class Balls : MonoBehaviour {

	private float posY;
	private bool resting;

	public Rigidbody rb;
	public int num;
	private Controller controller;

	// Use this for initialization
	void Start () {
		posY = transform.position.y;
		rb = GetComponent<Rigidbody> ();
		//Se ignoran los choques con el palo
		GameObject palo = GameObject.FindGameObjectsWithTag ("Palo")[0];
		Physics.IgnoreCollision (GetComponent<Collider> (), palo.GetComponent<Collider> ());

		controller = GameObject.FindGameObjectWithTag ("Controller").GetComponent<Controller> ();

		resting = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (resting) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.isKinematic = true;
			return;
		}
		if (transform.position.y > posY) {
			transform.position = new Vector3 (transform.position.x, posY, transform.position.z);
		} 
		else if (transform.position.y < posY-2) {
			Debug.Log("Se cayo la bola "+num);
			controller.newBallOut(num);
			transform.position = new Vector3(-58.4f,40f,num*2);
			resting = true;
		}
		if (rb.velocity.sqrMagnitude < 0.4) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		else if (rb.velocity.sqrMagnitude < 10) {
			rb.velocity = 0.9f*rb.velocity;
			rb.angularVelocity = 0.9f*rb.angularVelocity;
		}
	}
}
