using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    /* variables public de GameObject et GameController*/
    public Color[] couleurs;
    public Image Image_player_1;
    public Image Image_player_2;
    public GameController GameController;
    public int index_joueur_1;
    public int index_joueur_2;

    // Use this for initialization
    void Start () {

        /* récupérer les images des joueurs */
        Image_player_1 = GameObject.Find("Image_player_1").GetComponent<Image>();
        Image_player_2 = GameObject.Find("Image_player_2").GetComponent<Image>();

        /*Récuperer GameController
          Boucle for permettant de passer toutes les couleurs afin de les comparer*/
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        for (int i = 0; i < couleurs.Length; i++)
        {
            if (GameController.couleur_joueur1 == couleurs[i] )
            {
                index_joueur_1 = i;
            }

            if (GameController.couleur_joueur2 == couleurs[i])
            {
                index_joueur_2 = i;
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        /*  Afficher la couleur*/
        Image_player_1.color = couleurs[index_joueur_1];
        Image_player_2.color = couleurs[index_joueur_2];

        /* Assigner la couleur en passant par le GameController */
        GameController.couleur_joueur1 = couleurs[index_joueur_1];
        GameController.couleur_joueur2 = couleurs[index_joueur_2];

    }
    /* fonction permettant de commencer une partie (bouton)*/
    public void Debut(){
		SceneManager.LoadScene ("Game");
	}
    /* fonction permettant de quitter le jeu (bouton)*/
    public void Quitter(){
		Application.Quit();
		
	}
    /* fonction permattant passer à l'index supérieur du joueur 1 */
    public void Couleur_plus(int nb){
        if (nb == 1)
        {
            if (index_joueur_1 + 1 < couleurs.Length)
            {
                index_joueur_1++;
            }
            else
            {
                index_joueur_1 = 0;
            }
            
        }
        /* fonction permattant passer à l'index supérieur du joueur 2 */
        else if (nb == 2)
        {
            if (index_joueur_2 + 1 < couleurs.Length)
            {
                index_joueur_2++;
            }
            else
            {
                index_joueur_2 = 0;
            }

        }
  
    }
    /* fonction permattant passer à l'index inférieur du joueur 1 */
    public void Couleur_moins(int nb){
        if (nb == 1)
        {
            if (index_joueur_1 - 1 >= 0)
            {
                index_joueur_1--;
            }
            else
            {
                index_joueur_1 = couleurs.Length - 1;
            }

        }
        /* fonction permattant passer à l'index inférieur du joueur 2 */
        else if (nb == 2)
        {
            if (index_joueur_2 - 1 >= 0)
            {
                index_joueur_2--;
            }
            else
            {
                index_joueur_2 = couleurs.Length - 1;
            }

        }

    }

}
