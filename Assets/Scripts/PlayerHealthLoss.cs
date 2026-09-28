using System.Collections;
using UnityEngine;
public class PlayerHealthLoss : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer renduJoueur;

    [Header("Interface")]
    [SerializeField]
    private CanvasGroup flashEcran;

    [Header("Animation")]
    [SerializeField]
    private Color couleurDegat = Color.red;

    [SerializeField, Min(0.1f)]
    private float dureeEffet = 0.45f;

    [SerializeField, Min(1)]
    private int nombreClignotements = 3;

    [SerializeField, Min(1f)]
    private float agrandissement = 1.12f;

    private Color couleurInitiale;
    private Vector3 echelleInitiale;
    private Coroutine effetEnCours;

    private void Awake()
    {
        if (renduJoueur == null)
        {
            renduJoueur = GetComponent<SpriteRenderer>();
        }

        if (renduJoueur != null)
        {
            couleurInitiale = renduJoueur.color;
        }

        echelleInitiale = transform.localScale;

        if (flashEcran != null)
        {
            flashEcran.alpha = 0f;
        }
    }

    public void DeclencherEffet()
    {
        if (effetEnCours != null)
        {
            StopCoroutine(effetEnCours);
        }

        effetEnCours = StartCoroutine(JouerEffetDegats());
    }

    private IEnumerator JouerEffetDegats()
    {
        float dureeEtape =
            dureeEffet / (nombreClignotements * 2f);

        transform.localScale =
            echelleInitiale * agrandissement;

        for (int i = 0; i < nombreClignotements; i++)
        {
            if (renduJoueur != null)
            {
                renduJoueur.color = couleurDegat;
            }

            if (flashEcran != null)
            {
                flashEcran.alpha = 0.55f;
            }

            yield return new WaitForSeconds(dureeEtape);

            if (renduJoueur != null)
            {
                renduJoueur.color = couleurInitiale;
            }

            if (flashEcran != null)
            {
                flashEcran.alpha = 0f;
            }

            yield return new WaitForSeconds(dureeEtape);
        }

        if (renduJoueur != null)
        {
            renduJoueur.color = couleurInitiale;
        }

        if (flashEcran != null)
        {
            flashEcran.alpha = 0f;
        }

        transform.localScale = echelleInitiale;
        effetEnCours = null;
    }

    private void OnDisable()
    {
        if (renduJoueur != null)
        {
            renduJoueur.color = couleurInitiale;
        }

        if (flashEcran != null)
        {
            flashEcran.alpha = 0f;
        }

        transform.localScale = echelleInitiale;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
