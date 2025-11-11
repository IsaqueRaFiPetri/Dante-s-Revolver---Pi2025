using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterBuoyancy : MonoBehaviour
{
    [Header("Buoyancy Settings")]
    [SerializeField] private float buoyancyForce = 10f;
    [SerializeField] private float waterDrag = 1f;
    [SerializeField] private float waterAngularDrag = 1f;
    [SerializeField] private float buoyancyOffset = 0f;
    [SerializeField] private bool enableTorque = true;
    [SerializeField] private float stabilizationTorque = 2f;

    [Header("Buoyancy Points")]
    [SerializeField] private Transform[] buoyancyPoints = new Transform[1];
    [SerializeField] private float pointForceMultiplier = 1f;

    [Header("References")]
    [SerializeField] private ProceduralWater waterSurface;
    [SerializeField] private bool autoFindWater = true;

    // Internals
    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;
    private bool isInWater = false;

    // Eventos
    public System.Action OnEnterWater;
    public System.Action OnExitWater;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;

        // Se n�o h� pontos de boia definidos, usa o pr�prio transform
        if (buoyancyPoints == null || buoyancyPoints.Length == 0)
        {
            buoyancyPoints = new Transform[1] { transform };
        }

        // Tenta encontrar a �gua automaticamente
        if (autoFindWater && waterSurface == null)
        {
            waterSurface = FindFirstObjectByType<ProceduralWater>();
            if (waterSurface == null)
            {
                Debug.LogWarning("Water surface not found! Buoyancy will not work.");
            }
        }
    }

    void FixedUpdate()
    {
        if (waterSurface == null) return;

        ApplyBuoyancy();
        if (enableTorque)
        {
            ApplyStabilization();
        }
    }

    private void ApplyBuoyancy()
    {
        bool anyPointInWater = false;
        float totalForce = 0f;

        foreach (Transform point in buoyancyPoints)
        {
            if (point == null) continue;

            Vector3 worldPos = point.position;
            float waterHeight = waterSurface.GetWaterHeight(worldPos, out Vector3 waterNormal);

            // Calcula qu�o submerso est� o ponto (0 = na superf�cie, 1 = totalmente submerso)
            float submersion = CalculateSubmersion(worldPos.y, waterHeight);

            if (submersion > 0f)
            {
                anyPointInWater = true;
                ApplyPointBuoyancy(point.position, submersion, waterNormal);
                totalForce += submersion;
            }
        }

        // Aplica arrasto quando na �gua
        if (anyPointInWater)
        {
            if (!isInWater)
            {
                EnterWater();
            }
            ApplyWaterDrag(totalForce / buoyancyPoints.Length);
        }
        else
        {
            if (isInWater)
            {
                ExitWater();
            }
        }
    }

    private float CalculateSubmersion(float objectY, float waterY)
    {
        float depth = waterY - (objectY + buoyancyOffset);
        return Mathf.Clamp01(depth);
    }

    private void ApplyPointBuoyancy(Vector3 pointPosition, float submersion, Vector3 waterNormal)
    {
        // For�a de boia (Arquimedes)
        float forceMagnitude = buoyancyForce * submersion * pointForceMultiplier * Time.fixedDeltaTime;
        Vector3 buoyancyForceVector = Vector3.up * forceMagnitude;

        // Aplica no ponto espec�fico
        rb.AddForceAtPosition(buoyancyForceVector, pointPosition, ForceMode.Force);

        // For�a adicional baseada na normal da �gua
        Vector3 surfaceAlignmentForce = waterNormal * forceMagnitude * 0.1f;
        rb.AddForceAtPosition(surfaceAlignmentForce, pointPosition, ForceMode.Force);
    }

    private void ApplyWaterDrag(float averageSubmersion)
    {
        // Drag linear
        rb.linearDamping = Mathf.Lerp(originalDrag, waterDrag, averageSubmersion);
        rb.angularDamping = Mathf.Lerp(originalAngularDrag, waterAngularDrag, averageSubmersion);

        // Drag adicional baseado na velocidade
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        Vector3 dragForce = -localVelocity.normalized * localVelocity.sqrMagnitude * waterDrag * 0.1f * Time.fixedDeltaTime;
        rb.AddRelativeForce(dragForce, ForceMode.Force);
    }

    private void ApplyStabilization()
    {
        if (!isInWater) return;

        // Obt�m a normal da �gua na posi��o atual
        waterSurface.GetWaterHeight(transform.position, out Vector3 waterNormal);

        // Calcula o torque para alinhar com a superf�cie da �gua
        Vector3 currentUp = transform.up;
        Vector3 targetUp = waterNormal;

        // Usa produto vetorial para encontrar o eixo de rota��o necess�rio
        Vector3 torqueAxis = Vector3.Cross(currentUp, targetUp);
        float torqueMagnitude = Vector3.Angle(currentUp, targetUp) * 0.01f * stabilizationTorque * Time.fixedDeltaTime;

        Vector3 stabilizationTorqueVector = torqueAxis * torqueMagnitude;
        rb.AddTorque(stabilizationTorqueVector, ForceMode.Force);
    }

    private void EnterWater()
    {
        isInWater = true;
        OnEnterWater?.Invoke();
    }

    private void ExitWater()
    {
        isInWater = false;
        rb.linearDamping = originalDrag;
        rb.angularDamping = originalAngularDrag;
        OnExitWater?.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        // Desenha os pontos de boia
        if (buoyancyPoints != null)
        {
            Gizmos.color = Application.isPlaying && isInWater ? Color.cyan : Color.blue;
            foreach (Transform point in buoyancyPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.2f);
                }
            }
        }

        // Desenha linha para a superf�cie da �gua
        if (waterSurface != null && Application.isPlaying)
        {
            Vector3 worldPos = transform.position;
            float waterHeight = waterSurface.GetWaterHeight(worldPos, out Vector3 waterNormal);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(worldPos, new Vector3(worldPos.x, waterHeight, worldPos.z));

            // Desenha a normal da �gua
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(new Vector3(worldPos.x, waterHeight, worldPos.z), waterNormal * 2f);
        }
    }

    // M�todos p�blicos para controle
    public void SetWaterSurface(ProceduralWater water)
    {
        waterSurface = water;
    }

    public bool IsInWater()
    {
        return isInWater;
    }

    public float GetSubmersionLevel()
    {
        if (waterSurface == null) return 0f;

        float totalSubmersion = 0f;
        int pointsInWater = 0;

        foreach (Transform point in buoyancyPoints)
        {
            if (point == null) continue;

            float waterHeight = waterSurface.GetWaterHeight(point.position, out _);
            float submersion = CalculateSubmersion(point.position.y, waterHeight);

            if (submersion > 0f)
            {
                totalSubmersion += submersion;
                pointsInWater++;
            }
        }

        return pointsInWater > 0 ? totalSubmersion / pointsInWater : 0f;
    }
}