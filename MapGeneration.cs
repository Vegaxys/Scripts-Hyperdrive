using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour {

	public GameObject[] decorPrefab;
	public Transform player;
	public PlayerController playerController;

	float DecorDistance = 7, DecorActual;
	public int typeDecor, epaisseur;
	public float  placement;

	void Start(){
		epaisseur = 13;
	}

	void FixedUpdate(){
		if (playerController.placement > placement) {
			TestDistance (typeDecor);
		}
	}
	void NextDecor(){
		int deco = Random.Range (2, 6);
		if (deco == typeDecor) {
			NextDecor ();
		} else {
			typeDecor = deco;
		}
	}
	//**********************************************************************
	//******************************//Foret\\*******************************
	//**********************************************************************
	void TestDistance(int type){
		if (type == 1) {
			NextDecor ();
		}
		if (type == 2) {
			epaisseur = 13;
			DecorDistance = 5;
			SpawnTree ();
			placement = playerController.placement + 2.5f;
		}
		if (type == 3) {
			epaisseur = 26;
			SpawnCube ();
			placement = playerController.placement + 5;
		}
		if (type == 4) {
			epaisseur = 50;
			SpawnCubeForest ();
			placement = playerController.placement + .5f;
		}
		if (type == 5) {
			epaisseur = 13;
			SpawnHexagones ();
			placement = playerController.placement + 2.5f;
		}
		//************Boss Decor
		if (type == 6) {
			epaisseur = 13;
			SpawnStyle01 ();
			placement = playerController.placement + 5;
		}
	}
	//**********************************************************************
	//*****************************//Style01\\******************************
	//**********************************************************************
	//de 13 à 8
	void SpawnStyle01(){
		GameObject decoL = Instantiate (decorPrefab [2], new Vector3 (-40, 3, placement), Quaternion.Euler(-80, 90, -90));
		decoL.transform.localScale = new Vector3 (-3, 3, 3);
		decoL.AddComponent<DestroyDecor> ();

		GameObject decoR = Instantiate (decorPrefab [2], new Vector3 (40, 3, placement), Quaternion.Euler(-100, 90, -90));
		decoR.transform.localScale = new Vector3 (3, 3, 3);
		decoR.AddComponent<DestroyDecor> ();
	}
	//**********************************************************************
	//******************************//Arbre\\*******************************
	//**********************************************************************
	void SpawnTree(){
		for (int i = 0; i < epaisseur; i++) {
			float randomL = Random.Range (0, 10);
			float randomR = Random.Range (0, 10);
			if (randomL < 6f) {
				GameObject decoL = Instantiate (decorPrefab [0], new Vector3 (-13 - (i * DecorDistance), -22/*0.8f*/, placement), Quaternion.identity);
				decoL.transform.localScale = new Vector3 (1,randomL / 5 + 1, 1);
				decoL.name = "Gauche";
				decoL.AddComponent<DestroyDecor> ();
			}
			if (randomR < 6f) {
				GameObject decoR = Instantiate (decorPrefab [0], new Vector3 (13 + (i * DecorDistance), -22/*0.8f*/, placement), Quaternion.identity);
				decoR.transform.localScale = new Vector3 (1,randomR / 5 + 1, 1);
				decoR.name = "Droite";
				decoR.AddComponent<DestroyDecor> ();
			}
		}
	}
	//**********************************************************************
	//******************************//House\\*******************************
	//**********************************************************************
	//de 13 à 8
	void SpawnHouse(){
		for (int i = 0; i < epaisseur; i++) {	//67 / 7
			float randomL = Random.Range (0, 10);
			float randomR = Random.Range (0, 10);
			if (randomL < 6f) {
				GameObject decoL = Instantiate(decorPrefab[0], new Vector3(-13 + (-i * 7), 0.5f, placement), Quaternion.identity);
				decoL.AddComponent<DestroyDecor> ();
				decoL.layer = 8;
				decoL.transform.localScale = new Vector3 (1,randomL / 5 + 1, 1);
				Destroy(decoL, 6);
			}
			if (randomR < 6f) {
				GameObject decoR = Instantiate(decorPrefab[0], new Vector3(13 + (i * 7), 0.5f, placement), Quaternion.identity);
				decoR.AddComponent<DestroyDecor> ();
				decoR.transform.localScale = new Vector3 (1,randomR / 5 + 1, 1);
				Destroy(decoR, 6);
			}
		}
	}
	//**********************************************************************
	//****************************//Hegaxones\\*****************************
	//**********************************************************************
	//de 13 à 8
	void SpawnHexagones(){
		for (int i = 0; i < epaisseur; i++) {	//67 / 7
			float randomL = Random.Range (0, 10);
			float randomR = Random.Range (0, 10);
			if (randomL < 6f) {
				GameObject decoL = Instantiate(decorPrefab[1], new Vector3(-17 + (-i * 5.6f), -17/*0.5f*/, placement), Quaternion.Euler(-90, 0, 0));
				decoL.AddComponent<DestroyDecor> ();
				decoL.transform.GetChild(0).localScale = new Vector3 (3, 3, randomL * 2 + 6);
				AssignColor (decoL.transform.GetChild(0).gameObject);
			}
			if (randomR < 6f) {
				GameObject decoR = Instantiate(decorPrefab[1], new Vector3(17 + (i * 5.6f), -17/*0.5f*/, placement), Quaternion.Euler(-90, 0, 0));
				decoR.AddComponent<DestroyDecor> ();
				decoR.transform.GetChild(0).localScale = new Vector3 (3, 3, randomL * 2 + 6);
				AssignColor (decoR.transform.GetChild(0).gameObject);
			}
		}
	}
	//**********************************************************************
	//***************************//Ville Cube\\*****************************
	//**********************************************************************
	//de 26 à 12
	void SpawnCube(){
		for (int i = 0; i < epaisseur; i++) {	//67 / 7
			GameObject cuby = GameObject.CreatePrimitive (PrimitiveType.Cube);
			Destroy (cuby.GetComponent<BoxCollider> ());
			cuby.AddComponent<DestroyDecor> ();
			cuby.AddComponent<PingPongScript> ();
			float randomL = Random.Range (0, 10);
			float randomR = Random.Range (0, 10);
			if (randomL < 6f) {
				GameObject decoL = Instantiate(cuby, new Vector3(-18 + (-i * 7), 0.5f, placement), Quaternion.identity);
				decoL.transform.rotation = Quaternion.Euler (Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
				AssignColor (decoL);
				decoL.transform.localScale = new Vector3 (6,6,6);
			}
			if (randomR < 6f) {
				GameObject decoR = Instantiate(cuby, new Vector3(18 + (i * 7), 0.5f, placement), Quaternion.identity);
				decoR.transform.rotation = Quaternion.Euler (Random.Range (0, 180), Random.Range (0, 180), Random.Range (0, 180));
				AssignColor (decoR);
				decoR.transform.localScale = new Vector3 (6,6,6);
			}
		}
	}
	//**********************************************************************
	//***************************//Foret Cube\\*****************************
	//**********************************************************************
	//de 80 à 30
	void SpawnCubeForest(){
		for (int i = 0; i < epaisseur; i++) {
			GameObject cuby = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cuby.AddComponent<DestroyDecor> ();
			cuby.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			Destroy (cuby.GetComponent<BoxCollider> ());
			float randomL = Random.Range (0, 10);
			float randomR = Random.Range (0, 10);
			if (randomL < 6f) {
				cuby.transform.position = new Vector3 (-15 + (-i), 24/*45*/, placement);
			}
			if (randomR < 6f) {
				cuby.transform.position = new Vector3 (15 + (i), 24/*45*/, placement);
			}
			cuby.transform.rotation = Quaternion.Euler (Random.Range (-3, 3), Random.Range (-3, 3), Random.Range (-3, 3));
			cuby.transform.localScale = new Vector3 (.2f,100,.2f);
			AssignColor (cuby);
		}
	}
	void AssignColor(GameObject decoration){
		int colo = Random.Range (0, 10);
		if (colo > 0) {
			decoration.GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/White") as Material;
		}
		if (colo > 3) {
			decoration.GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/DarkGrey") as Material;
		}
		if (colo > 6) {
			decoration.GetComponent<MeshRenderer> ().material = Resources.Load ("Materials/Blue") as Material;
		}
	}
}
