using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("Déplacement")]
    public float vitesseDeplacement = 5.0f;
    public float vitesseCourse = 10.0f; // Vitesse quand on appuie sur Shift
    public KeyCode toucheCourse = KeyCode.LeftShift;

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

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private bool estEnCourse = false;

    void Start()
    {
        // Verrouille et cache le curseur pour une expérience FPS plus immersive
        if (verrouillagesCurseur)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        // Gestion du curseur (échap pour déverrouiller/reverrouiller)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            verrouillagesCurseur = !verrouillagesCurseur;
            Cursor.lockState = verrouillagesCurseur ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !verrouillagesCurseur;
        }

        // Vérification si le joueur est en train de courir
        estEnCourse = Input.GetKey(toucheCourse);
        float vitesse = estEnCourse ? vitesseCourse : vitesseDeplacement;

        // ROTATION - Contrôle avec la souris
        if (verrouillagesCurseur) // Ne pas tourner si le curseur est libre
        {
            float sourisX = Input.GetAxis("Mouse X") * sensibiliteSouris;
            float sourisY = Input.GetAxis("Mouse Y") * sensibiliteSouris;

            // Rotation horizontale (gauche/droite)
            rotationY += sourisX;

            // Rotation verticale (haut/bas) avec limite pour éviter de faire des tours complets
            rotationX -= sourisY; // Inverser pour que ce soit intuitif
            rotationX = Mathf.Clamp(rotationX, -limiteRotationVerticale, limiteRotationVerticale);

            // Appliquer les rotations
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        // DÉPLACEMENT - ZQSD/WASD et flèches
        float mouvementAvant = 0;
        float mouvementLateral = 0;

        // Mouvement avant/arrière
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            mouvementAvant += 1.0f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            mouvementAvant -= 1.0f;

        // Mouvement latéral
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            mouvementLateral += 1.0f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            mouvementLateral -= 1.0f;

        // Calculer le vecteur de déplacement
        Vector3 deplacement = transform.forward * mouvementAvant + transform.right * mouvementLateral;

        // Normaliser pour éviter une vitesse plus rapide dans les diagonales
        if (deplacement.magnitude > 1.0f)
            deplacement.Normalize();

        // Appliquer le déplacement (multiplier par vitesse et deltaTime pour mouvement fluide)
        transform.position += deplacement * vitesse * Time.deltaTime;
    }

    void OnGUI()
    {
        // Cette fonction dessine un petit carré au centre de l'écran
        if (afficherPointCentral)
        {
            // Crée un style temporaire pour notre point
            GUIStyle stylePoint = new GUIStyle();

            // Crée une texture de la couleur souhaitée
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, couleurPoint);
            texture.Apply();

            // Applique la texture au background du style
            stylePoint.normal.background = texture;

            // Dessine un petit carré au milieu de l'écran avec notre style personnalisé
            float centreX = Screen.width / 2;
            float centreY = Screen.height / 2;
            GUI.Box(new Rect(centreX - taillePoint / 2, centreY - taillePoint / 2, taillePoint, taillePoint), "", stylePoint);
        }
    }

}
