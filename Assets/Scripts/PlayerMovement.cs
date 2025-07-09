using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 move = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            0
        );

        if (move != Vector3.zero)
        {
            transform.position += move * moveSpeed * Time.deltaTime;
            Debug.Log("Player moved to: " + transform.position);
        }
    }
}
