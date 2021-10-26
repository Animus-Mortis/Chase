using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 playerVector;
    int speed = 10;

    private void Update()
    {
        if (player != null)
        {
            playerVector = new Vector3(0, 7.5f, -1.5f);
            transform.position = player.position + playerVector;
        }
    }
}
