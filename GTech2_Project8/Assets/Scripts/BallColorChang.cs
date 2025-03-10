using UnityEngine;

public class BallColorChanger : MonoBehaviour
{
    private Renderer ballRenderer;

    void Start()
    {
        // Récupérer le composant Renderer de la balle
        ballRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Générer une couleur aléatoire
        Color randomColor = new Color(
            Random.value, // Rouge (0-1)
            Random.value, // Vert (0-1)
            Random.value, // Bleu (0-1)
            1.0f         // Alpha (opacité)
        );

        // Appliquer la couleur au matériau de la balle
        ballRenderer.material.color = randomColor;
    }
}
