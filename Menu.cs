using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public Color[] couleurs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Debut(){
		SceneManager.LoadScene ("Game");
	}

	public void Quitter(){
		Application.Quit();
		
	}



}
