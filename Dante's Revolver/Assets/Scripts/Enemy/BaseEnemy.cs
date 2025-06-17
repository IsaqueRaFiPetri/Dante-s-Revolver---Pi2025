using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseEnemy : MonoBehaviour
{
    Transform player;
    RaycastHit hit;
    Rigidbody body;
    Vector3 moveDirection;

    [SerializeField] float moveSpeed;
    [SerializeField] Transform vision;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        moveDirection = Vector3.zero;

        float direction = Vector3.Distance(player.position, this.transform.position);

        Vector3 dir = player.position - this.transform.position;
        dir.y = 0;

        if (direction < 15)
        {
            body.linearVelocity = transform.forward * moveSpeed;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("following");
        }
        else if (direction >= 15 && direction < 40)
        {
            body.linearVelocity = transform.forward * (moveSpeed * 5);

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 0.1f);

            print("trying to escape");
        }
        else
        {
            body.linearVelocity = transform.forward * moveSpeed * 0;
            print("bye");
        }

        if (Physics.Linecast(vision.position, player.position, out hit))
        {
            if (hit.distance >= 35) 
                return;
            
            if (hit.collider.GetComponent<IPlayable>() != null)
            {
                player = hit.collider.transform;
            }
        }
    }
}