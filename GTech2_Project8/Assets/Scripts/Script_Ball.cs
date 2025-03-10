using UnityEngine;

public class Script_Ball : MonoBehaviour
{
    public float forceLancer = 300f;
    public float forceRebond = 5f;
    private Rigidbody rb;
    private Renderer rend;
    private Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        cam = Camera.main;
    }

    void Update()
    {
        // Lancer la balle avec clic gauche
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Calculer la direction de la caméra vers le point d'impact
                    Vector3 direction = (hit.point - cam.transform.position).normalized;

                    // Ajouter une légère élévation pour éviter que la balle ne roule simplement au sol
                    direction += new Vector3(0, 0.2f, 0);
                    direction.Normalize();

                    // Appliquer la force dans cette direction
                    rb.velocity = Vector3.zero; // Réinitialiser la vitesse actuelle
                    rb.AddForce(direction * forceLancer);

                    // Changer la couleur aléatoirement
                    rend.material.color = new Color(
                        Random.Range(0f, 1f),
                        Random.Range(0f, 1f),
                        Random.Range(0f, 1f)
                    );
                }
            }
        }

        // Super-rebond avec barre d'espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * forceRebond, ForceMode.Impulse);
        }

        // Empêcher la balle de tomber infiniment
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 3, 0);
            rb.velocity = Vector3.zero;
        }
    }
}
