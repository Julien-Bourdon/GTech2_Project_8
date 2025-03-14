using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("D�placement")]
    public float vitesseDeplacement = 5.0f;
    public float vitesseCourse = 10.0f; // Vitesse quand on appuie sur Shift
    public KeyCode toucheCourse = KeyCode.LeftShift;

    // Variables pour le saut
    [Header("Saut")]
    public float forceSaut = 5.0f;
    public KeyCode toucheSaut = KeyCode.Space;
    private bool estAuSol;

    [Header("Escalade")]
    public KeyCode toucheEscalade = KeyCode.E; // Touche pour escalader
    public float vitesseEscalade = 3.0f;
    public float distanceMurDetect = 1.2f; // Distance pour d�tecter un mur
    private bool estEnEscalade = false;


    [Header("Rotation")]
    public float sensibiliteSouris = 2.0f;
    public float limiteRotationVerticale = 80.0f; // Angle max de rotation vers le haut/bas

    [Header("Options")]
    public bool verrouillagesCurseur = true;

    //variables pour le point central
    [Header("Point central")]
    public bool afficherPointCentral = true;
    public Color couleurPoint = Color.white;
    public float taillePoint = 4f;

    // Variables pour la balle
    [Header("Balle")]
    public GameObject balle;
    public KeyCode toucheLancer = KeyCode.Mouse0;
    public KeyCode toucheRecuperer = KeyCode.R;
    public float forceLancer = 600f;
    public float forceLancerMax = 1200f; // Force maximale de lancer
    public float tempsChargeMax = 2.0f; // Temps maximum de charge
    private float tempsCharge = 0.0f;
    private bool estEnCharge = false;
    public Script_Ball script_Ball;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool estEnCourse = false;
    public Rigidbody rigidBody;

    void Start()
    {
        // Verrouille et cache le curseur pour une exp�rience FPS plus immersive
        if (verrouillagesCurseur)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // R�cup�rer le composant Rigidbody
        if(rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        if (balle != null)
        {
            script_Ball = balle.GetComponentInChildren<Script_Ball>();
            Debug.Log("Balle trouv�e : " + balle.name);
            if(script_Ball == null)
            {
                Debug.LogError("Aucun script 'Script_Ball' trouv� sur la balle !");
            }
        }
        else
        {
            Debug.LogError("Aucune balle trouv�e avec le tag 'Ball' !");
        }

    }

    void Update()
    {
        float mouvementAvant = 0.0f;
        float mouvementLateral = 0.0f;
        float mouvementVertical = 0.0f;
        Vector3 deplacement;

        // Gestion du curseur (�chap pour d�verrouiller/reverrouiller)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            verrouillagesCurseur = !verrouillagesCurseur;
            Cursor.lockState = verrouillagesCurseur ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !verrouillagesCurseur;
        }

        // V�rification si le joueur est en train de courir
        estEnCourse = Input.GetKey(toucheCourse);
        float vitesse = estEnCourse ? vitesseCourse : vitesseDeplacement;

        // ROTATION - Contr�le avec la souris
        if (verrouillagesCurseur) // Ne pas tourner si le curseur est libre
        {
            float sourisX = Input.GetAxis("Mouse X") * sensibiliteSouris;
            float sourisY = Input.GetAxis("Mouse Y") * sensibiliteSouris;

            // Rotation horizontale (gauche/droite)
            rotationY += sourisX;

            // Rotation verticale (haut/bas) avec limite pour �viter de faire des tours complets
            rotationX -= sourisY; // Inverser pour que ce soit intuitif
            rotationX = Mathf.Clamp(rotationX, -limiteRotationVerticale, limiteRotationVerticale);

            // Appliquer les rotations
            transform.GetChild(0).rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        RaycastHit hit;
        bool estContreMur = Physics.Raycast(transform.position, transform.forward, out hit, distanceMurDetect);

        // Mode escalade
        if (estContreMur && Input.GetKey(toucheEscalade))
        {

            estEnEscalade = true;
            GetComponent<Rigidbody>().useGravity = false; // D�sactiver la gravit�
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Stopper les mouvements involontaires

            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                mouvementVertical += 1.0f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                mouvementVertical -= 1.0f;

        }
        else
        {
            estEnEscalade = false;
            rigidBody.useGravity = true; // R�activer la gravit� quand on l�che la touche
        }


        if (!estEnEscalade)
        {

            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                mouvementAvant += 1.0f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                mouvementAvant -= 1.0f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                mouvementLateral += 1.0f;
            if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                mouvementLateral -= 1.0f;

            //transform.position += deplacement * vitesse * Time.deltaTime;
        }


        // Gestion de la charge pour lancer la balle
        if (Input.GetKey(toucheLancer) && script_Ball != null && script_Ball.estSurJoueur)
        {
            estEnCharge = true;
            tempsCharge += Time.deltaTime;
            tempsCharge = Mathf.Clamp(tempsCharge, 0, tempsChargeMax);
        }

        if (Input.GetKeyUp(toucheLancer) && estEnCharge)
        {
            float force = Mathf.Lerp(forceLancer, forceLancerMax, tempsCharge / tempsChargeMax);
            script_Ball.LancerBalle(force);
            estEnCharge = false;
            tempsCharge = 0.0f;
        }

        // R�cup�rer la balle 
        if (Input.GetKeyDown(toucheRecuperer) && script_Ball != null && !script_Ball.estSurJoueur)
        {
            script_Ball.RetournerBalle();
        }

        // SAUT - Appliquer une force vers le haut
        if (Input.GetKeyDown(toucheSaut) && (estAuSol || estEnEscalade))
        {
            rigidBody.AddForce(Vector3.up * forceSaut, ForceMode.Impulse);
        }

        // Calculer le vecteur de d�placement
        Vector3 forward = transform.GetChild(0).forward;
        forward.y = 0; // Annuler la composante verticale
        forward.Normalize(); // Normaliser le vecteur

        Vector3 right = transform.GetChild(0).right;

        deplacement = forward * mouvementAvant * vitesse * Time.deltaTime + right * mouvementLateral * vitesse * Time.deltaTime + mouvementVertical * Vector3.up * vitesseEscalade * Time.deltaTime;

        // Normaliser pour �viter une vitesse plus rapide dans les diagonales
        if (deplacement.magnitude > 1.0f)
            deplacement.Normalize();

        // Appliquer le d�placement (multiplier par vitesse et deltaTime pour mouvement fluide)
        transform.position += deplacement;
    }

    void OnCollisionStay(Collision collision)
    {
        // V�rifier si le joueur est au sol
        if (collision.gameObject.CompareTag("Floor"))
        {
            estAuSol = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // V�rifier si le joueur quitte le sol
        if (collision.gameObject.CompareTag("Floor"))
        {
            estAuSol = false;
        }
    }

    void OnGUI()
    {
        // Cette fonction dessine un petit carr� au centre de l'�cran
        if (afficherPointCentral)
        {
            // Cr�e un style temporaire pour notre point
            GUIStyle stylePoint = new GUIStyle();

            // Cr�e une texture de la couleur souhait�e
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, couleurPoint);
            texture.Apply();

            // Applique la texture au background du style
            stylePoint.normal.background = texture;

            // Dessine un petit carr� au milieu de l'�cran avec notre style personnalis�
            float centreX = Screen.width / 2;
            float centreY = Screen.height / 2;
            GUI.Box(new Rect(centreX - taillePoint / 2, centreY - taillePoint / 2, taillePoint, taillePoint), "", stylePoint);
        }
    }



}