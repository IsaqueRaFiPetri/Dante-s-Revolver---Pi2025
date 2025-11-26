using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Gerenciador principal para reposicionar o exit point e setas indicadoras
/// em GameObjects vazios pré-existentes no mapa.
/// </summary>
public class SpawnManager : MonoBehaviourPunCallbacks
{
    [Header("Configurações de Spawn")]
    [Tooltip("Lista de GameObjects vazios onde os objetos serão posicionados")]
    public List<GameObject> spawnPoints = new List<GameObject>();

    [Header("Referências")]
    [Tooltip("Referência para o exit point já existente na cena")]
    public GameObject exitPoint;

    [Tooltip("Prefab da seta indicadora")]
    public GameObject arrowPrefab;

    [Header("Configurações")]
    [Tooltip("Se verdadeiro, executa o reposicionamento automaticamente ao iniciar")]
    public bool repositionOnStart = true;

    [Header("Configurações Multiplayer")]
    [Tooltip("Nome do objeto do exit point na rede (para sincronização)")]
    public string exitPointNetworkName = "NetworkExitPoint";

    private List<GameObject> currentArrows = new List<GameObject>();
    private int selectedExitPointIndex = -1;

    void Start()
    {
        if (repositionOnStart && PhotonNetwork.IsConnected)
        {
            // Apenas o mestre da sala executa o reposicionamento inicial
            if (PhotonNetwork.IsMasterClient)
            {
                RepositionObjects();
            }
        }
        else if (repositionOnStart && !PhotonNetwork.IsConnected)
        {
            // Modo singleplayer
            RepositionObjects();
        }
    }

    /// <summary>
    /// Reposiciona o exit point e as setas nos pontos definidos
    /// </summary>
    public void RepositionObjects()
    {
        // Validações para evitar erros
        if (!ValidateRepositionConditions())
            return;

        // Limpa setas existentes antes de criar novas
        ClearExistingArrows();

        // Seleciona aleatoriamente um ponto para o exit point
        selectedExitPointIndex = Random.Range(0, spawnPoints.Count);
        GameObject exitSpawnPoint = spawnPoints[selectedExitPointIndex];

        if (PhotonNetwork.IsConnected)
        {
            // Multiplayer: Sincroniza a escolha do ponto
            photonView.RPC("SyncExitPointSelection", RpcTarget.AllBuffered, selectedExitPointIndex);

            // Reposiciona o exit point na rede
            if (exitPoint.GetComponent<PhotonView>() != null)
            {
                photonView.RPC("RepositionExitPoint", RpcTarget.AllBuffered, exitPoint.GetComponent<PhotonView>().ViewID, exitSpawnPoint.transform.position);
            }
            else
            {
                // Se não tiver PhotonView, apenas o mestre reposiciona
                if (PhotonNetwork.IsMasterClient)
                {
                    exitPoint.transform.position = exitSpawnPoint.transform.position;
                    exitPoint.transform.rotation = exitSpawnPoint.transform.rotation;
                }
            }
        }
        else
        {
            // Singleplayer: Reposiciona localmente
            exitPoint.transform.position = exitSpawnPoint.transform.position;
            exitPoint.transform.rotation = exitSpawnPoint.transform.rotation;
        }

        // Cria as setas indicadoras
        CreateArrows(selectedExitPointIndex);

        Debug.Log($"Reposicionamento concluído: Exit point movido para {exitSpawnPoint.name}, {currentArrows.Count} setas criadas.");
    }

    [PunRPC]
    private void SyncExitPointSelection(int exitPointIndex)
    {
        selectedExitPointIndex = exitPointIndex;

        // Recria as setas para todos os jogadores
        if (!PhotonNetwork.IsMasterClient)
        {
            ClearExistingArrows();
            CreateArrows(selectedExitPointIndex);
        }
    }

    [PunRPC]
    private void RepositionExitPoint(int exitPointViewID, Vector3 newPosition)
    {
        PhotonView targetView = PhotonView.Find(exitPointViewID);
        if (targetView != null && targetView.gameObject != null)
        {
            targetView.transform.position = newPosition;
        }
    }

    /// <summary>
    /// Cria setas nos pontos que não são o exit point
    /// </summary>
    private void CreateArrows(int exitPointIndex)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (i != exitPointIndex) // Evita criar seta no mesmo local do exit point
            {
                GameObject arrow;

                if (PhotonNetwork.IsConnected)
                {
                    // Multiplayer: Instancia a seta na rede
                    arrow = PhotonNetwork.Instantiate("NetworkArrow", spawnPoints[i].transform.position, Quaternion.identity);
                }
                else
                {
                    // Singleplayer: Instancia localmente
                    arrow = Instantiate(arrowPrefab, spawnPoints[i].transform.position, Quaternion.identity);
                }

                // Configura a seta para apontar para o exit point
                ArrowPointer arrowScript = arrow.GetComponent<ArrowPointer>();
                if (arrowScript != null)
                {
                    arrowScript.SetTarget(exitPoint.transform);
                    currentArrows.Add(arrow);
                }
                else
                {
                    Debug.LogWarning("Prefab da seta não contém o script ArrowPointer");
                }
            }
        }
    }

    /// <summary>
    /// Valida se todas as condições para o reposicionamento estão atendidas
    /// </summary>
    private bool ValidateRepositionConditions()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("Lista de spawn points vazia! Adicione GameObjects vazios no Inspector.");
            return false;
        }

        if (exitPoint == null)
        {
            Debug.LogError("Exit point não atribuído! Atribua o GameObject do exit point no Inspector.");
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

        // Verifica se estamos no multiplayer e se o PhotonView está configurado
        if (PhotonNetwork.IsConnected && photonView == null)
        {
            Debug.LogError("PhotonView não encontrado no objeto! Adicione um componente PhotonView.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Remove todas as setas existentes
    /// </summary>
    public void ClearExistingArrows()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
        {
            // Apenas o mestre pode destruir objetos de rede
            return;
        }

        foreach (GameObject arrow in currentArrows)
        {
            if (arrow != null)
            {
                if (PhotonNetwork.IsConnected)
                {
                    if (arrow.GetComponent<PhotonView>() != null && PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Destroy(arrow.gameObject);
                    }
                }
                else
                {
                    DestroyImmediate(arrow.gameObject);
                }
            }
        }
        currentArrows.Clear();

        // Limpa também qualquer seta órfã
        ArrowPointer[] existingArrows = FindObjectsByType<ArrowPointer>(FindObjectsSortMode.None);
        foreach (ArrowPointer arrow in existingArrows)
        {
            if (arrow.gameObject != this.gameObject && arrow.gameObject != exitPoint)
            {
                if (PhotonNetwork.IsConnected)
                {
                    if (arrow.GetComponent<PhotonView>() != null && PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Destroy(arrow.gameObject);
                    }
                }
                else
                {
                    DestroyImmediate(arrow.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Método para forçar um reposition (útil para debug ou reinício do jogo)
    /// </summary>
    public void RepositionObjectsAgain()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("Apenas o mestre da sala pode executar reposition.");
            return;
        }

        RepositionObjects();
    }

    /// <summary>
    /// Chamado quando o mestre da sala muda
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Mestre da sala alterado. Novo mestre: " + newMasterClient.NickName);

        // Se necessário, o novo mestre pode reassumir o controle
        if (selectedExitPointIndex == -1 && repositionOnStart)
        {
            RepositionObjects();
        }
    }

    /// <summary>
    /// Método para obter a posição atual do exit point
    /// </summary>
    public Vector3 GetCurrentExitPointPosition()
    {
        if (selectedExitPointIndex >= 0 && selectedExitPointIndex < spawnPoints.Count)
        {
            return spawnPoints[selectedExitPointIndex].transform.position;
        }
        return exitPoint.transform.position;
    }

    /// <summary>
    /// Método para obter o índice atual do exit point
    /// </summary>
    public int GetCurrentExitPointIndex()
    {
        return selectedExitPointIndex;
    }
}