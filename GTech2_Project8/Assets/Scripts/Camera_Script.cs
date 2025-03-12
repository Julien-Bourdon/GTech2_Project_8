using UnityEngine;

public class Camera_Script : MonoBehaviour
{

    public Transform player;  // Référence au joueur
    public Vector3 offset = new Vector3(0, 5, -10); // Décalage de la caméra

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

}
