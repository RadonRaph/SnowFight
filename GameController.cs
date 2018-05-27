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
    //Fonction Start dans la scene "Menu"
	void Start(){
        Screen.SetResolution(1024, 768, false);
		DontDestroyOnLoad (this);//Ligne permetant le passage inter-scene
		couleur_joueur1 = new Color(1,0,0);
		couleur_joueur2 = new Color(0,0,1);
        sound = true;


	}

    //Fonction start dans la scene "Game"
	public void StartGame (UIController ui) {
		bloc_list = new List<GameObject>(9999);

        //Link des joueurs
		player1 = GameObject.Find("Player1").GetComponent<Player>();
		player2 = GameObject.Find("Player2").GetComponent<Player>();

		tempete = GameObject.Find ("Tempete");
		tempete.SetActive (false);

		player1.couleur_Joueur = couleur_joueur1;
		player2.couleur_Joueur = couleur_joueur2;

        //Demarrage des joueurs
		player1.Start();
		player2.Start();

		uicontroller = ui;


        //Bloque le déplacement des joueurs
		player1.p_rb.isKinematic = true;
		player2.p_rb.isKinematic = true;
	}

	public void debut_partie(){
		
        //Liberation des déplacement
		player1.p_rb.isKinematic = false;
		player2.p_rb.isKinematic = false;
	}
	
	// Update is called once per frame
	void Update () {

        //Fonction qui active ou coupe le son
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


        //Demarage de la tempête toute les 5 min
        if (uicontroller)
        {
            //if (uicontroller.minute % 5 == 0) 
            if (uicontroller.minute % 2 == 0) //Baisser à 2 min pour la démonstration
            {
                if (uicontroller.minute != 0)
                {
                    StartCoroutine(lancertempete());
                }
            }
        }

	}


    //Ajouter un bloc à la liste des bloc
	public void AddBloc(GameObject bloc){
		bloc_list.Add (bloc);
	}


    //Retirer un bloc de la liste
	public void removeBloc(GameObject bloc){
		int id = bloc_list.IndexOf (bloc);
		bloc_list.RemoveAt (id);
	}


    //Fin de la partie
    public void endGame()
    {
        player1.p_rb.isKinematic = true;
        player2.p_rb.isKinematic = true;
		uicontroller.FinPartie ();
    }


    //(bouton) pour le son
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


    //Fonction de la tempête
	IEnumerator lancertempete(){
		tempete.SetActive (true);
		yield return new WaitForSeconds (10f);//Attente pour que les particles charge
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
