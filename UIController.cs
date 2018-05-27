using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    /* fonction public de GameObject et GameController*/
    public Slider barre_de_vie_joueur1;
	public Slider barre_de_vie_joueur2;

	public Text munitions_joueur1;
	public Text munitions_joueur2;
	public Text timer;

	public Player player1;
	public Player player2;

	public int seconde;
	public int minute;
	public int heure;

    /* définit le délais de départ à 10 secondes*/
    public int delais = 10;
	public Text menu_chrono;
	public GameObject menu_debut;
	public GameObject menu_fin;

	public GameController GameController;


	// Use this for initialization
	void Start () {

        /* récupérer le GameController */
        GameController = GameObject.Find ("GameController").GetComponent<GameController> ();
		GameController.StartGame (this);

        /* récupérer le texte timer */
        timer = GameObject.Find ("timer").GetComponent<Text> ();

        /* récupérer les GameObject players */
        player1 = GameObject.Find ("Player1").GetComponent<Player> ();
		player2 = GameObject.Find ("Player2").GetComponent<Player> ();

        /* récupérer plusieurs GameObjects  */
        menu_chrono = GameObject.Find ("menu_chrono").GetComponent<Text> ();
		menu_debut = GameObject.Find ("menu_debut");
		menu_fin = GameObject.Find ("menu_fin");

		menu_fin.SetActive (false);

		StartCoroutine (timer_debut ());
	}
	
	// Update is called once per frame
	void Update () {
        /* assigner la valeur des munitions */
        munitions_joueur1.text = player1.Munitions.ToString();
		munitions_joueur2.text = player2.Munitions.ToString();
        /* assigner la valeur de la barre de vie */
        barre_de_vie_joueur1.value = player1.Vie;
		barre_de_vie_joueur2.value = player2.Vie;
		
	}
    /* Chronomètre */
    IEnumerator timerFonction(){
		string text = "";
		while (seconde >= 60) {
			minute++;
			seconde = seconde - 60;
		}

		while (minute >= 60) {
			heure++;
			minute = minute - 60;
		}


        /*  */
        if (minute < 10) {
			text += "0" + minute.ToString() + ":";
		} else {
			text += minute.ToString() + ":";
		}

		if (seconde < 10) {
			text += "0" + seconde.ToString();
		} else {
			text += seconde.ToString();
		}


        /* assigner la valeur au texte et afficher le temps */
        timer.text = text;

		yield return new WaitForSeconds (1f);

		seconde++;

		StartCoroutine (timerFonction ());

	}

	IEnumerator timer_debut(){

		while (delais > 0){
			delais = delais-1;
			menu_chrono.text = delais.ToString();
			yield return new WaitForSeconds (1f);

		}

		menu_debut.SetActive (false);
		GameController.debut_partie ();
		StartCoroutine (timerFonction ());
		
	}
    /* fonction permettant de recommencer une partie (bouton)*/
    public void Recommencer(){
		SceneManager.LoadScene ("Game");
	}

    /* fonction permettant de revenir au menu principal (bouton)*/
    public void MenuPincipale(){
		SceneManager.LoadScene ("menu");

	}

	public void FinPartie (){ 
		menu_fin.SetActive (true);
	}
}
