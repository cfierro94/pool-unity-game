using UnityEngine;
using System.Collections;

public class HandPlacer : MonoBehaviour {

	public Controller controller;

	private bool handling;
	private Plane plane;
	private Ray ray;
	private float zMin, zMax;

	// Use this for initialization
	void Start () {
		handling = false;
		plane = new Plane (new Vector3 (0, 1, 0), new Vector3(0.6f,36,-0.6f));
	}
	
	// Update is called once per frame
	void Update () {
		if (handling) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			float dist;
			if(plane.Raycast(ray, out dist)){
				transform.position = ray.GetPoint(dist);
				//Debug.Log(transform.position);
			}
			if(Input.GetButtonUp("Fire1") && canPlace() && GUIUtility.hotControl == 0){
				controller.onHandPlace(transform.position);
				handling = false;
				transform.position = new Vector3(0,0,0);
			}
		}
	}

	bool canPlace(){
		float x = transform.position.x;
		float z = transform.position.z;
		if (z < zMin || z > zMax || x < -18.5 || x > 18.5)
			return false;
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if(hit.collider.gameObject.CompareTag("Balls"))
				return false;
			else
				return true;
		}
		return false;
	}

	public void startHandling(int pos){
		setZRange (pos);
		handling = true;
	}

	// 0: izquierda, 1: medio, 2: derecha
	void setZRange(int pos){
		zMin = -37 + pos * 74 / 3;
		zMax = -37 + (pos + 1) * 74 / 3;
	}

}
