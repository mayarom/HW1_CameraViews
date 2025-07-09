using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowPlayer : MonoBehaviour
{
    public Transform player;  // גרור לכאן את השחקן
    public Vector3 offset = new Vector3(0, 0, -10);
    public float zoomSize = 3f;  // גודל זום של המצלמה במיני־מפה

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = zoomSize;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
