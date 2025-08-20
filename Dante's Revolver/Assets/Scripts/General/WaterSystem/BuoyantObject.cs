using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyantObject : MonoBehaviour
{
    public ProceduralWater water;
    public float buoyancy = 15f;    // for�a de empuxo
    public float dragInWater = 2f;  // arrasto dentro d'�gua
    public float angularDragInWater = 1f;

    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;
    }

    void FixedUpdate()
    {
        if (water == null) return;

        Vector3 n;
        float waterY = water.GetWaterHeight(transform.position, out n);
        float depth = waterY - transform.position.y;

        bool submerged = depth > 0f;

        if (submerged)
        {
            // empuxo proporcional � profundidade (simples)
            rb.AddForce(Vector3.up * buoyancy * depth, ForceMode.Acceleration);

            // alinhamento suave com a normal da �gua
            Quaternion targetRot = Quaternion.FromToRotation(transform.up, n) * rb.rotation;
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 0.1f));
        }

        rb.linearDamping = submerged ? dragInWater : originalDrag;
        rb.angularDamping = submerged ? angularDragInWater : originalAngularDrag;
    }
}
