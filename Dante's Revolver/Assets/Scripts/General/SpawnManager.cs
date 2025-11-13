using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gerenciador principal para instanciar fim e setas indicadoras
/// em GameObjects vazios pré-existentes no mapa.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    [Tooltip("Lista de GameObjects vazios onde os objetos serão instanciados")]
    public List<GameObject> spawnPoints = new List<GameObject>();

    [Header("Prefabs")]
    [Tooltip("Prefab do fim (deve ter a tag 'Target')")]
    public GameObject endPrefab;

    [Tooltip("Prefab da seta indicadora")]
    public GameObject arrowPrefab;

    [Header("Configurações")]
    [Tooltip("Se verdadeiro, executa o spawn automaticamente ao iniciar")]
    public bool spawnOnStart = true;

    private GameObject currentEndPoint;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnObjects();
        }
    }

    /// <summary>
    /// Executa o spawn do fim e das setas nos pontos definidos
    /// </summary>
    public void SpawnObjects()
    {
        // Validações para evitar erros
        if (!ValidateSpawnConditions())
            return;

        // Limpa objetos existentes antes de spawnar novos
        ClearExistingObjects();

        // Seleciona aleatoriamente um ponto para o fim
        int endPointIndex = Random.Range(0, spawnPoints.Count);
        GameObject endSpawnPoint = spawnPoints[endPointIndex];

        // Instancia o fim no ponto selecionado
        currentEndPoint = Instantiate(endPrefab, endSpawnPoint.transform.position, endSpawnPoint.transform.rotation);

        // Garante que o fim tenha a tag correta
        if (!currentEndPoint.CompareTag("Target"))
        {
            currentEndPoint.tag = "Target";
            Debug.LogWarning("O prefab do fim não tinha a tag 'Target'. Tag atribuída automaticamente.");
        }

        // Instancia setas nos pontos restantes
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (i != endPointIndex) // Evita spawnar seta no mesmo local do fim
            {
                GameObject arrow = Instantiate(arrowPrefab, spawnPoints[i].transform.position, Quaternion.identity);

                // Configura a seta para apontar para o fim
                ArrowPointer arrowScript = arrow.GetComponent<ArrowPointer>();
                if (arrowScript != null)
                {
                    arrowScript.SetTarget(currentEndPoint.transform);
                }
                else
                {
                    Debug.LogWarning("Prefab da seta não contém o script ArrowPointer");
                }
            }
        }

        Debug.Log($"Spawn concluído: Fim instanciado em {endSpawnPoint.name}, {spawnPoints.Count - 1} setas criadas.");
    }

    /// <summary>
    /// Valida se todas as condições para o spawn estão atendidas
    /// </summary>
    private bool ValidateSpawnConditions()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("Lista de spawn points vazia! Adicione GameObjects vazios no Inspector.");
            return false;
        }

        if (endPrefab == null)
        {
            Debug.LogError("Prefab do fim não atribuído! Atribua no Inspector.");
            return false;
        }

        if (arrowPrefab == null)
        {
            Debug.LogError("Prefab da seta não atribuído! Atribua no Inspector.");
            return false;
        }

        if (arrowPrefab.GetComponent<ArrowPointer>() == null)
        {
            Debug.LogError("Prefab da seta não contém o script ArrowPointer!");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Remove todos os objetos spawnados anteriormente
    /// </summary>
    public void ClearExistingObjects()
    {
        ArrowPointer[] existingArrows = FindObjectsByType<ArrowPointer>(FindObjectsSortMode.None);
        foreach (ArrowPointer arrow in existingArrows)
        {
            if (arrow.gameObject != this.gameObject)
                DestroyImmediate(arrow.gameObject);
        }

        // Destrói o fim atual se existir
        if (currentEndPoint != null)
            DestroyImmediate(currentEndPoint);
    }

    /// <summary>
    /// Método para forçar um respawn (útil para debug ou reinício do jogo)
    /// </summary>
    public void RespawnObjects()
    {
        ClearExistingObjects();
        SpawnObjects();
    }
}
