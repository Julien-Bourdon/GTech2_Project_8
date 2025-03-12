using UnityEngine;

public class Camera_Script : MonoBehaviour
{

    public Transform player;  // R�f�rence au joueur
    public Vector3 offset = new Vector3(0, 5, -10); // D�calage de la cam�ra

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

}
