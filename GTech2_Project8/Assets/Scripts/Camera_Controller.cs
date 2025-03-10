using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("D�placement")]
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
        // Verrouille et cache le curseur pour une exp�rience FPS plus immersive
        if (verrouillagesCurseur)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
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
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        }

        // D�PLACEMENT - ZQSD/WASD et fl�ches
        float mouvementAvant = 0;
        float mouvementLateral = 0;

        // Mouvement avant/arri�re
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            mouvementAvant += 1.0f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            mouvementAvant -= 1.0f;

        // Mouvement lat�ral
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            mouvementLateral += 1.0f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            mouvementLateral -= 1.0f;

        // Calculer le vecteur de d�placement
        Vector3 deplacement = transform.forward * mouvementAvant + transform.right * mouvementLateral;

        // Normaliser pour �viter une vitesse plus rapide dans les diagonales
        if (deplacement.magnitude > 1.0f)
            deplacement.Normalize();

        // Appliquer le d�placement (multiplier par vitesse et deltaTime pour mouvement fluide)
        transform.position += deplacement * vitesse * Time.deltaTime;
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
