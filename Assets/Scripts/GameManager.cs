using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int viesInitiales = 20;
    [SerializeField] private TMP_Text texteEliminations;
    [SerializeField] private TMP_Text texteVies;
    [SerializeField] private int objectifEnemies = 15;
    [SerializeField] private GameObject porteSortie;
    [SerializeField] private PlayerMovement joueur;
    [SerializeField] private GameObject panneauVictoire;
    [SerializeField] private GameObject panneauDefaite;
    [SerializeField] private GameAudio audioJeu;
    [SerializeField] private PlayerHealthLoss effetDegatsJoueur;

    private int scoreEnemies;

    private int vies;
    private bool partieTerminee;
    public bool PartieTerminee => partieTerminee;
    public bool ObjectifAtteint => scoreEnemies >= objectifEnemies;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vies = viesInitiales;
        scoreEnemies = 0;
        partieTerminee = false;
        panneauVictoire.SetActive(false);
        panneauDefaite.SetActive(false);
        porteSortie.SetActive(false);
        ActualiserInterface();
        audioJeu?.JouerLancement();
    }

    public void AjouterBatterie(int valeur = 1)
    {
        // Ignore la collecte si la partie est terminée.
        if (partieTerminee) return;

        // Augmente le compteur.
        scoreEnemies += valeur;

        // Joue le son de collecte.
        audioJeu?.JouerElimination();

        // Met à jour le compteur et la barre.
        ActualiserInterface();

        // Vérifie si le joueur a suffisamment de batteries.
        if (ObjectifAtteint)
        {
            // Rend la sortie visible et active.
            porteSortie.SetActive(true);

            // Joue le son indiquant que l'objectif est atteint.
            audioJeu?.JouerObjectif();
        }
    }

    public void PerdreVie()
    {
        // Ne retire plus de vies après la fin de la partie.
        if (partieTerminee)
            return;

        // Retire une vie sans descendre sous zéro.
        // Mathf.Max retourne la plus grande des deux valeurs.
        vies = Mathf.Max(vies - 1, 0);

        // Joue le son d'impact.
        audioJeu?.JouerImpact();

        // Déclenche l'effet visuel si le composant est disponible.
        if (effetDegatsJoueur != null)
            effetDegatsJoueur.DeclencherEffet();

        // Actualise notamment le texte des vies.
        ActualiserInterface();

        // Si aucune vie ne reste, termine la partie par une défaite.
        if (vies == 0)
            DeclencherDefaite();
    }

    public void DeclencherVictoire()
    {
        // Refuse la victoire si :
        // - la partie est déjà terminée;
        // - OU l'objectif d'éliminations n'est pas atteint.
        // || signifie OU et ! signifie NON.
        if (partieTerminee || !ObjectifAtteint) return;

        // Marque la partie comme terminée.
        partieTerminee = true;

        // Affiche le panneau de victoire.
        panneauVictoire.SetActive(true);

        // Empêche le joueur de continuer à utiliser ses commandes.
        joueur.DesactiverCommandes();

        // Joue le son de victoire.
        audioJeu?.JouerVictoire();
    }

    public void DeclencherDefaite()
    {
        // Évite de déclencher plusieurs fois la fin de partie.
        if (partieTerminee) return;

        partieTerminee = true;

        // Affiche le panneau de défaite.
        panneauDefaite.SetActive(true);

        // Désactive les commandes du joueur.
        joueur.DesactiverCommandes();

        // Joue le son de défaite.
        audioJeu?.JouerDefaite();
    }

    public void TempsEcoule() => DeclencherDefaite();

    // Synchronise les éléments de l'interface avec l'état du jeu.
    private void ActualiserInterface()
    {
        // Exemple : "Batteries : 2/3".
        texteEliminations.text =
            $"Batteries : {scoreEnemies}/{objectifEnemies}";

        // Exemple : "Vies : 2".
        texteVies.text = $"Vies : {vies}";
    }

    public void RecommencerPartie()
    {
        // Recharge la scène active à partir de son index.
        // Les objets de la scène sont recréés et la partie réinitialisée.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
