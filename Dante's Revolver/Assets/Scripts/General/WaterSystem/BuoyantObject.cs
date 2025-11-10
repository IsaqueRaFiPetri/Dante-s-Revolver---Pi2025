using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyantObject : MonoBehaviour
{
    [Header("Water Reference")]
    public ProceduralWater water;

    [Header("Buoyancy Settings")]
    public float buoyancyPower = 15f;    // Força BASE do empuxo
    public float maxBuoyancyForce = 25f; // Força máxima
    public float submergedThreshold = 0.1f; // Quão submerso precisa estar para aplicar força

    [Header("Stabilization")]
    public float stabilizationTorque = 5f;
    public float rightingSpeed = 2f;

    [Header("Drag Settings")]
    public float dragInWater = 2f;
    public float angularDragInWater = 1f;

    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;
    private bool isSubmerged;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;
    }

    void FixedUpdate()
    {


        if (water == null) return;

        if (water == null) return;

        Vector3 normal;
        float waterHeight = water.GetWaterHeight(transform.position, out normal);

        // CÁLCULO CORRIGIDO: quanto MAIOR o valor, mais SUBMERSO está
        float submersionDepth = waterHeight - transform.position.y;

        // DEBUG - remove depois
        Debug.Log($"Water Height: {waterHeight:F2}, Object Y: {transform.position.y:F2}, " +
                  $"Submersion: {submersionDepth:F2}, Buoyancy Force: {buoyancyPower * submersionDepth:F2}");


        isSubmerged = submersionDepth > submergedThreshold;

        if (isSubmerged)
        {
            // Calcula a força de empuxo (limitada)
            float buoyancyForce = Mathf.Min(
                buoyancyPower * submersionDepth,
                maxBuoyancyForce
            );

            // Aplica empuxo (quanto mais submerso, MAIS força)
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

            // Estabilização - apenas se significativamente submerso
            if (submersionDepth > submergedThreshold * 2f)
            {
                StabilizeInWater(normal);
            }
        }
        // Aplica arrasto apenas quando submerso
        ApplyWaterDrag();
    }

    private void StabilizeInWater(Vector3 waterNormal)
    {
        // Suavemente alinha com a normal da água
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, waterNormal) * rb.rotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rightingSpeed * Time.fixedDeltaTime));

        // Adiciona torque extra para estabilização
        Vector3 torque = Vector3.Cross(transform.up, Vector3.up).normalized * stabilizationTorque;
        rb.AddTorque(torque, ForceMode.Acceleration);
    }

    private void ApplyWaterDrag()
    {
        rb.linearDamping = isSubmerged ? dragInWater : originalDrag;
        rb.angularDamping = isSubmerged ? angularDragInWater : originalAngularDrag;
    }

    // Debug visual
    void OnDrawGizmos()
    {
        if (water == null || !Application.isPlaying) return;

        Vector3 normal;
        float waterHeight = water.GetWaterHeight(transform.position, out normal);
        float submersionDepth = waterHeight - transform.position.y;

        // Linha mostrando submersão
        Gizmos.color = isSubmerged ? Color.blue : Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, waterHeight, transform.position.z));

        // Texto debug (precisa de GUI ou Debug.Log)
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 2f,
            $"Submersion: {submersionDepth:F2}\nSubmerged: {isSubmerged}");
#endif
    }
}