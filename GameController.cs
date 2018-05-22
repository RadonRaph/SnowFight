using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class GameController : MonoBehaviour {
	
	public List<GameObject> bloc_list;
	public Player player1;
	public Player player2;
	public Color couleur_joueur1;
	public Color couleur_joueur2;
	public UIController uicontroller;

    public bool sound;
    public AudioMixer soundMixer;

	public GameObject tempete;

	// Use this for initialization
	void Start(){
        Screen.SetResolution(1024, 768, false);
		DontDestroyOnLoad (this);
		couleur_joueur1 = Color.red;
		couleur_joueur2 = Color.blue;
        sound = true;


	}






	public void StartGame (UIController ui) {
		bloc_list = new List<GameObject>(9999);
		player1 = GameObject.Find("Player1").GetComponent<Player>();
		player2 = GameObject.Find("Player2").GetComponent<Player>();

		tempete = GameObject.Find ("Tempete");
		tempete.SetActive (false);

		player1.couleur_Joueur = couleur_joueur1;
		player2.couleur_Joueur = couleur_joueur2;


		player1.Start();
		player2.Start();

		uicontroller = ui;

		player1.p_rb.isKinematic = true;
		player2.p_rb.isKinematic = true;
	}

	public void debut_partie(){
		

		player1.p_rb.isKinematic = false;
		player2.p_rb.isKinematic = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (UnityEngine.Object.FindObjectsOfType<AudioSource>().Length > 0)
        {
            if (sound == false)
            {
                for (int i = 0; i < UnityEngine.Object.FindObjectsOfType<AudioSource>().Length; i++)
                {
                    UnityEngine.Object.FindObjectsOfType<AudioSource>()[i].volume = 0;
                }
            }
            else
            {
                for (int i = 0; i < UnityEngine.Object.FindObjectsOfType<AudioSource>().Length; i++)
                {
                    UnityEngine.Object.FindObjectsOfType<AudioSource>()[i].volume = 1;
                }
            }
        }

        if (uicontroller)
        {
            if (uicontroller.minute % 5 == 0)
            {
                if (uicontroller.minute != 0)
                {
                    StartCoroutine(lancertempete());
                }
            }
        }

	}

	public void AddBloc(GameObject bloc){
		bloc_list.Add (bloc);
	}

	public void removeBloc(GameObject bloc){
		int id = bloc_list.IndexOf (bloc);
		bloc_list.RemoveAt (id);
	}

    public void endGame()
    {
        player1.p_rb.isKinematic = true;
        player2.p_rb.isKinematic = true;
		uicontroller.FinPartie ();
    }

    public void SoundSwitch()
    {
        if (sound == true)
        {
            sound = false;
        }
        else
        {
            sound = true;
        }
    }

	IEnumerator lancertempete(){
		tempete.SetActive (true);
		yield return new WaitForSeconds (10f);
		for (int i = 0; i < bloc_list.Count; i++) {
			if (bloc_list [i].GetComponent<Bloc> ().bloc == "Neige") {
				Destroy (bloc_list [i].gameObject);
				bloc_list.RemoveAt (i);
				yield return new WaitForEndOfFrame ();
			}

		}
		yield return new WaitForSeconds (10f);
		tempete.SetActive (false);
	}
}
