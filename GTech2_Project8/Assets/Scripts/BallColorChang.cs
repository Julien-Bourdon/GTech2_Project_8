using UnityEngine;

public class BallColorChanger : MonoBehaviour
{
    private Renderer ballRenderer;

    void Start()
    {
        // R�cup�rer le composant Renderer de la balle
        ballRenderer = GetComponent<Renderer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // G�n�rer une couleur al�atoire
        Color randomColor = new Color(
            Random.value, // Rouge (0-1)
            Random.value, // Vert (0-1)
            Random.value, // Bleu (0-1)
            1.0f         // Alpha (opacit�)
        );

        // Appliquer la couleur au mat�riau de la balle
        ballRenderer.material.color = randomColor;
    }
}
