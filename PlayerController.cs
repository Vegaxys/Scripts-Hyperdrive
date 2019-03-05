using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public Transform camerA, sol, HUD;
	public Rigidbody rigid;
	public Image lifeBar;

	public float playerPosX, sensibilité, speed, life;

	public Vector3 offset;
	public GameObject roadPrefab, laserPrefab;
	public Transform lastRoad;
	public int placement, maxSpeed;
	float duration;
	bool isUsed, isUsedJump;

	void Start(){
		maxSpeed = 80;
		placement = (int)transform.position.z + 150;
	}

	void FixedUpdate () {
		LifeManage ();
		GetMouvements ();
		Mouvement ();
		PlaceRoad ();
		Rotation ();
		PlaceHolderMainPosition ();
	}
	void LifeManage(){
		if (life < 3) {
			life += Time.deltaTime / 2;
		}
		lifeBar.transform.localScale = new Vector3 (Mathf.Lerp (lifeBar.transform.localScale.x, life / 3, 0.2f), 1, 1);
	}

	void GetMouvements(){
		if (Input.GetAxisRaw("Horizontal") == 1 ) {
			if (isUsed == false) {
				isUsed = true;
				if (Input.GetKey (KeyCode.LeftShift)) {
					playerPosX = 2;
					duration = 0.11f;
				} else {
					playerPosX++;
					duration = 0.15f;
				}
				if (playerPosX > 2) {
					playerPosX--;
				}
			}
		}
		if (Input.GetAxisRaw("Horizontal") == -1 ) {
			if (isUsed == false) {
				isUsed = true;
				if (Input.GetKey (KeyCode.LeftShift)) {
					playerPosX = -2;
					duration = 0.11f;
				} else {
					playerPosX--;
					duration = 0.15f;
				}
				if (playerPosX < -2) {
					playerPosX++;
				}
			}
		}
		if (Input.GetAxisRaw("Horizontal") == 0 ) {
			isUsed = false;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			Laser ();
		}
	}
	void PlaceHolderMainPosition(){
		if (transform.position.z + 150 > placement) {
			placement += 10;
		}
	}
	void Rotation(){
		Quaternion rot = Quaternion.Euler (0, 0, Input.GetAxisRaw ("Horizontal") * -40);
		transform.rotation = Quaternion.Lerp (transform.rotation, rot, 0.1f);
	}
	void Mouvement(){
		Vector3 pos = new Vector3 (playerPosX * 4.1f, 1, transform.position.z);
		transform.position = Vector3.Lerp (transform.position, pos, duration);
		if (rigid.velocity.z < maxSpeed) {
			rigid.AddForce (Vector3.forward * speed, ForceMode.Acceleration);
		}
		Vector3 cameraPos = new Vector3 (transform.position.x / 1.5f, transform.position.y, transform.position.z);
		camerA.transform.position = new Vector3 (camerA.transform.position.x, camerA.transform.position.y, transform.position.z);
		camerA.position = Vector3.Lerp (camerA.position, cameraPos, 0.15f);

		sol.position = new Vector3 (0, -20.2f, transform.position.z);
	}
	void PlaceRoad(){
		if (Vector3.Distance (transform.position, lastRoad.position) < 100) {
			GameObject road = Instantiate (roadPrefab, new Vector3 (lastRoad.position.x, lastRoad.position.y, lastRoad.position.z + 50), Quaternion.identity);
			Destroy (road, 6);
			lastRoad = road.transform;
		}
	}
	void Laser(){
		Vector3 pos = new Vector3 (0.6f, 0, 0);
		for (int i = -1; i < 2; i+=2) {
			GameObject laser = Instantiate (laserPrefab, transform.position + (pos * i), Quaternion.identity);
			laser.GetComponent<Rigidbody> ().AddForce (Vector3.forward * 300, ForceMode.Impulse);
			Destroy (laser, 3);
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Obstacle") {
			camerA.GetChild (0).GetComponent<CameraShake> ().ShakeCamera (0.2f, 0.6f);
			HUD.GetComponent<Animator> ().SetTrigger ("Damage");
			life--;
		}
		if (other.tag == "Arch" && GameObject.Find ("System").GetComponent<GenerateObstacle> ().timeBoss == 5) {
			GameObject.Find ("System").GetComponent<MapGeneration> ().typeDecor = 6;
			GameObject.Find ("System").GetComponent<GenerateObstacle> ().timeBoss = 0;
		}
	}
}
