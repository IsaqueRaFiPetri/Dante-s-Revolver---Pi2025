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

    void FixedUpdate()
    {
        Debug.Log(body.linearVelocity);

        moveDirection = Vector3.zero;

        if (Vector3.Distance(player.position, this.transform.position) < 25)
        {
            body.linearVelocity = transform.forward * moveSpeed;

            print("following");
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
        else if (Vector3.Distance(player.position, this.transform.position) >= 25 && Vector3.Distance(player.position, this.transform.position) <= 100)
        {
            body.linearVelocity = transform.forward * (moveSpeed * 5);

            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            print("trying to escape");
        }
        else
        {
            body.linearVelocity = transform.forward * moveSpeed * 0;
            print("bye");
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<IPlayable>() != null)
        {
            print("entered");
            player = collision.transform;
        }
    }
}