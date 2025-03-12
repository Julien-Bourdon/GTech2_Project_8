using UnityEngine;

public class Script_Ball : MonoBehaviour
{
    public float forceRebond = 5f;
    public float distanceDevantCamera = 2.0f;
    public bool estSurJoueur = true;
    public GameObject player;
    private Rigidbody rb;
    private Renderer rend;
    private Camera cam;
   

    void Start()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }

        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        cam = Camera.main;
    }

    void Update()
    {
        // Emp�cher la balle de tomber infiniment
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 3, 0);
            rb.velocity = Vector3.zero;
        }

        if (estSurJoueur)
        {

            Vector3 positionDevantCamera = cam.transform.position + cam.transform.forward * distanceDevantCamera;
            transform.position = positionDevantCamera;
        }
    }

    public void LancerBalle(float force)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // Calculer la direction de la cam�ra vers le point d'impact
                Vector3 direction = (hit.point - cam.transform.position).normalized;

                // Ajouter une l�g�re �l�vation pour �viter que la balle ne roule simplement au sol
                direction += new Vector3(0, 0.2f, 0);
                direction.Normalize();

                // Appliquer la force dans cette direction
                rb.velocity = Vector3.zero; // R�initialiser la vitesse actuelle
                rb.AddForce(direction * force);

                // Changer la couleur al�atoirement
                rend.material.color = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                );
            }
        }

        estSurJoueur = false;
    }

    public void RetournerBalle() 
    {
        estSurJoueur = true;
        rb.velocity = Vector3.zero;
    }
}
