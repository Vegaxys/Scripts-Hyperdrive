using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacle : MonoBehaviour {

	public GameObject obstaclePrefab, arch, boss;
	PlayerController playerController;
	MapGeneration mapGen;
	public Transform player;
	float timeBeginning;
	/// <summary>
	/// Espacement entre chaque obstacle
	/// </summary>
	public int placement; 
	/// <summary>
	/// A 4, l'arche apparait
	/// </summary>
	public int futurPlacement; 
	/// <summary>
	/// Espacement entre chaque wave
	/// </summary>
	public int zone;
	/// <summary>
	/// A 5, le boss arrive
	/// </summary>
	public int timeBoss;
	/// <summary>
	/// Différentes waves
	/// </summary>
	public int game;
	 public bool alternateur, waveBoss;

	void Start(){
		playerController = player.GetComponent<PlayerController> ();
		mapGen = GetComponent<MapGeneration> ();
		waveBoss = false;
	}

	void Update(){
		if (playerController.placement > placement) {
			switch (game) {
			case 0:
				placement = playerController.placement;
				break;
			case 1:
				print ("Aléatoires");
				SpawnPattern000 ();
				placement = playerController.placement + 20;		//10
				zone++;
				TestNextWave (30);
				break;
			case 2:
				print ("Qinconces");
				SpawnPattern001 ();
				placement = playerController.placement + 100;		//50
				zone++;
				TestNextWave (10);
				break;
			case 3:
				print ("Damier");
				SpawnPattern002 ();
				placement = playerController.placement + 40;		//20
				zone++;
				TestNextWave (10);
				break;
			case 4:
				print ("Escalier");
				placement = playerController.placement;
				SpawnPattern003 ();
				placement += 300;		//200
				zone++;
				TestNextWave (0);
				break;
			case 5: 
				print ("EscalierReverse");
				placement = playerController.placement;
				SpawnPattern003Reverse ();
				placement += 300;		//200
				zone++;
				TestNextWave (0);
				break;
			case 6:
				print ("Boss");
				SpawnBoss ();
				game = 0;
				break;
			}
		}
	}
	void TestNextWave(int timing){
		if (zone > timing && futurPlacement < 4) {
			futurPlacement++;
			zone = 0;
			int next = Random.Range (1, 6);
			if (next != game) {
				game = next;
			} else {
				TestNextWave (-1);
			}
		}
		if (zone > timing && futurPlacement >= 4) {
			futurPlacement = 0;
			zone = 0;
			//mapGen.typeDecor = 0;
			SpawnStart ();
			placement += 350;
			int next = Random.Range (1, 6);
			if (next != game) {
				game = next;
			} else {
				TestNextWave (-1);
			}
		}
	}

	//***********Start
	void SpawnStart(){
		Vector3 pos = new Vector3 (0, 5, placement + 170);
		GameObject obstacle = Instantiate (arch, pos, Quaternion.Euler(-90, 0, 0));
		Destroy (obstacle, 15);
	}
	void SpawnBoss(){
		Instantiate (boss, new Vector3 (0, 0, placement), Quaternion.identity);
	}
	//**********************************************************************
	//****************************//Escalier\\******************************
	//**********************************************************************
	//***********Pattern Escalier
	void SpawnPattern003(){
		int k = -2;
		for (int j = 0; j < 5; j++) {
			for (int i = -2; i < 3; i++) {
				Vector3 pos = new Vector3 (i * 4.1f, 1.5f, placement + j * 40);
				if (k != i) {
					SpawnPrefab (pos);
				}
			}
			k++;
		}
	}
	//***********Pattern Escalier Reverse
	void SpawnPattern003Reverse(){
		int k = 2;
		for (int j = 0; j < 5; j++) {
			for (int i = -2; i < 3; i++) {
				Vector3 pos = new Vector3 (i * 4.1f, 1.5f, placement + j * 40);
				if (k != i) {
					SpawnPrefab (pos);
				}
			}
			k--;
		}
	}
	//**********************************************************************
	//*****************************//Damier\\*******************************
	//**********************************************************************
	void SpawnPattern002(){
		if (alternateur) {
			SpawnDamierFunction (1);
		} else {
			SpawnDamierFunction (-1);
		}
		alternateur = !alternateur;
	}
	void SpawnDamierFunction(int type){
		Vector3 pos = Vector3.zero;
		if (type == 1) {
			for (int i = -2; i < 3; i += 2) {
				pos = new Vector3 (i * 4.1f, 1.5f, placement);
				SpawnPrefab (pos);
			}
		}else{
			for (int i = -1; i < 3; i += 2) {
				pos = new Vector3 (i * 4.1f, 1.5f, placement);
				SpawnPrefab (pos);
			}
		}
	}
	//**********************************************************************
	//****************************//Escalier\\******************************
	//**********************************************************************
	//***********Crée une double ligne avec deux entrées
	void SpawnPattern001(){
		if (alternateur) {
			Pattern001 ();
		} else {
			Pattern001_Reverse ();
		}
		alternateur = !alternateur;
	}
	//***********Créer une diagonale avec une ouverture sur la droite
	void Pattern001(){
		for (int i = -2; i < 2; i++) {
			Vector3 pos = new Vector3(i * 4.1f,1.5f,placement + (i * 4.1f));
			SpawnPrefab (pos);
		}
	}
	//***********Créer une diagonale avec une ouverture sur la gauche
	void Pattern001_Reverse(){
		for (int i = 2; i > -2; i--) {
			Vector3 pos = new Vector3(i * 4.1f,1.5f,placement - (i * 4.1f));
			SpawnPrefab (pos);
		}
	}
	//**********************************************************************
	//*****************************//Random\\*******************************
	//**********************************************************************
	int alea;
	void SpawnPattern000(){
		Vector3 pos = new Vector3 (RandomPosition() * 4.1f, 1.5f, placement);
		SpawnPrefab (pos);
	}
	int RandomPosition(){
		int ale = 0;
		while (alea == ale) {
			ale = (int)Random.Range (-2, 3);
		}
		alea = ale;
		return ale;
	}
	//*********Fonction
	void SpawnPrefab(Vector3 position){
		GameObject obstacle = Instantiate (obstaclePrefab, position, Quaternion.identity);
		Destroy (obstacle, 8);
	}
}