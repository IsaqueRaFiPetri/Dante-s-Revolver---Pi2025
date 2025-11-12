using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    public float buoyancyForce = 10f;
    public float waterDrag = 1f;
    public float waterAngularDrag = 0.5f;

    [Header("Buoyancy Points")]
    public Transform[] buoyancyPoints;
    public float pointForceMultiplier = 1f;

    [Header("Layer Detection Settings")]
    public LayerMask waterLayerMask;

    private Rigidbody rb;
    private bool isInWater = false;
    private float originalDrag;
    private float originalAngularDrag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Buoyancy script requires a Rigidbody component!");
            return;
        }

        // If no buoyancy points specified, use the object's position
        if (buoyancyPoints == null || buoyancyPoints.Length == 0)
        {
            buoyancyPoints = new Transform[1];
            buoyancyPoints[0] = transform;
        }

        // Store original values
        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;

        // Log layer mask info for debugging
        if (waterLayerMask.value == 0)
        {
            Debug.LogWarning("Water Layer Mask is not set! Please assign the water layer in the inspector.");
        }
    }

    void FixedUpdate()
    {
        bool wasInWater = isInWater;
        isInWater = false;

        // Check each buoyancy point for water
        foreach (Transform point in buoyancyPoints)
        {
            if (IsPointInWater(point.position))
            {
                isInWater = true;
                ApplyBuoyancyAtPoint(point.position);
            }
        }

        // Update physics properties when entering/exiting water
        if (isInWater && !wasInWater)
        {
            EnterWater();
        }
        else if (!isInWater && wasInWater)
        {
            ExitWater();
        }
    }

    bool IsPointInWater(Vector3 point)
    {
        // Raycast downward to find water surface
        RaycastHit hit;
        float rayLength = 10f;

        if (Physics.Raycast(point + Vector3.up * rayLength / 2, Vector3.down, out hit, rayLength, waterLayerMask))
        {
            return point.y <= hit.point.y;
        }

        return false;
    }

    void ApplyBuoyancyAtPoint(Vector3 point)
    {
        // Calculate how submerged the point is
        float submergeDepth = CalculateSubmergeDepth(point);

        // Apply force at the buoyancy point
        Vector3 buoyantForce = Vector3.up * buoyancyForce * submergeDepth * pointForceMultiplier;
        rb.AddForceAtPosition(buoyantForce, point, ForceMode.Force);
    }

    float CalculateSubmergeDepth(Vector3 point)
    {
        // Raycast to find water surface and calculate submerge depth
        RaycastHit hit;
        float rayLength = 10f;

        if (Physics.Raycast(point + Vector3.up * rayLength / 2, Vector3.down, out hit, rayLength, waterLayerMask))
        {
            float waterSurfaceY = hit.point.y;
            float depth = Mathf.Max(0, waterSurfaceY - point.y);
            return Mathf.Clamp01(depth);
        }

        return 0f;
    }

    void EnterWater()
    {
        // Increase drag when entering water
        rb.linearDamping = waterDrag;
        rb.angularDamping = waterAngularDrag;
    }

    void ExitWater()
    {
        // Restore original drag values
        rb.linearDamping = originalDrag;
        rb.angularDamping = originalAngularDrag;
    }

    // Visualize buoyancy points in the editor
    void OnDrawGizmosSelected()
    {
        if (buoyancyPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in buoyancyPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.1f);
                }
            }
        }
    }
}