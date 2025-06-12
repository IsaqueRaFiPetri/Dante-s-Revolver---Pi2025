using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BaseEnemy : MonoBehaviour
{
    Transform player;
    Rigidbody body;
    Vector3 moveDirection;
    [SerializeField] float moveSpeed;
    
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<IPlayable>() != null)
        {
            print("entered");
            player = collision.transform;
        }
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
    }
}