using UnityEngine;

/// <summary>
/// Controla o comportamento das setas indicadoras
/// Faz a seta apontar constantemente na direção do alvo
/// </summary>
public class ArrowPointer : MonoBehaviour
{
    [Header("Configurações da Seta")]
    [Tooltip("Alvo que a seta deve apontar. Se vazio, busca automaticamente por tag 'Target'")]
    public Transform target;

    [Tooltip("Velocidade de rotação da seta (graus por segundo)")]
    public float rotationSpeed = 180f;

    [Tooltip("Se verdadeiro, a seta atualiza a rotação no FixedUpdate (para física)")]
    public bool useFixedUpdate = false;

    [Header("Otimização")]
    [Tooltip("Intervalo entre atualizações de busca do alvo (segundos)")]
    public float targetSearchInterval = 2f;

    private float lastTargetSearchTime;
    private bool hasValidTarget;

    void Start()
    {
        InitializeTarget();
    }

    /// <summary>
    /// Inicializa o alvo da seta
    /// </summary>
    private void InitializeTarget()
    {
        // Se o alvo não foi atribuído manualmente, busca automaticamente
        if (target == null)
        {
            FindTargetByTag();
        }

        hasValidTarget = target != null;

        if (!hasValidTarget)
        {
            Debug.LogWarning("Seta não encontrou um alvo válido! Buscará novamente periodicamente.");
        }
    }

    /// <summary>
    /// Busca o alvo pela tag "Target"
    /// </summary>
    private void FindTargetByTag()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("Target");
        if (targetObject != null)
        {
            target = targetObject.transform;
            hasValidTarget = true;
            Debug.Log("Seta encontrou alvo com tag 'Target': " + targetObject.name);
        }
    }

    void Update()
    {
        if (!useFixedUpdate)
        {
            UpdateArrowRotation();
        }

        // Busca periódica do alvo se não tiver um válido
        if (!hasValidTarget && Time.time - lastTargetSearchTime >= targetSearchInterval)
        {
            FindTargetByTag();
            lastTargetSearchTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            UpdateArrowRotation();
        }
    }

    /// <summary>
    /// Atualiza a rotação da seta para apontar para o alvo
    /// </summary>
    private void UpdateArrowRotation()
    {
        if (!hasValidTarget || target == null)
            return;

        // Calcula a direção para o alvo
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; // Mantém a seta nivelada no plano horizontal

        // Se a direção for válida, calcula e aplica a rotação
        if (directionToTarget != Vector3.zero)
        {
            // Calcula a rotação desejada
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Aplica a rotação com suavização
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Define manualmente o alvo da seta
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasValidTarget = target != null;
    }

    /// <summary>
    /// Habilita/desabilita a seta
    /// </summary>
    public void SetArrowEnabled(bool enabled)
    {
        this.enabled = enabled;
    }

    // Debug visual no Editor
    void OnDrawGizmosSelected()
    {
        if (hasValidTarget && target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}