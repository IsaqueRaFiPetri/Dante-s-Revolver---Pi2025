using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public enum HintType
{
    SameRow,
    SameColumn,
    Neither
}
public class ExitSpotManager : MonoBehaviourPunCallbacks
{
    [Header("Spot Positions")]
    public List<Transform> spotPositions = new List<Transform>();

    [Header("Exit Spot Prefab")]
    public GameObject exitSpotPrefab;

    [Header("Hint Sprites")]
    public Sprite sameRowSprite;
    public Sprite sameColumnSprite;
    public Sprite neitherSprite;
    public Sprite neutralSprite;

    [Header("Randomization")]
    [Range(0f, 1f)]
    public float hintChance = 0.7f; // 70% chance to show a hint

    private List<ExitSpot> allSpots = new List<ExitSpot>();
    private int exitSpotIndex = -1;
    private Vector2Int exitGridPos = Vector2Int.zero;

    // Define your grid layout (adjust based on your empty objects arrangement)
    private Vector2Int[] gridPositions = new Vector2Int[]
    {
        new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
        new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
        new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2)
    };

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CreateExitSpots();
        }
    }

    void CreateExitSpots()
    {
        allSpots.Clear();

        // Validate that we have enough positions
        if (spotPositions.Count != gridPositions.Length)
        {
            Debug.LogError($"Number of spot positions ({spotPositions.Count}) doesn't match grid positions ({gridPositions.Length})");
            return;
        }

        // Create spots at each pre-set position
        for (int i = 0; i < spotPositions.Count; i++)
        {
            if (spotPositions[i] == null) continue;

            // Instantiate via Photon Network at the pre-set position
            GameObject spotObj = PhotonNetwork.Instantiate(
                exitSpotPrefab.name,
                spotPositions[i].position,
                spotPositions[i].rotation
            );

            ExitSpot spot = spotObj.GetComponent<ExitSpot>();
            allSpots.Add(spot);

            // Set grid position based on predefined layout
            Vector2Int gridPos = gridPositions[i];
            spot.SetGridPosition(gridPos.x, gridPos.y);
            spot.SetSpotIndex(i);
        }

        // Master client chooses the exit spot randomly
        ChooseExitSpot();
    }

    void ChooseExitSpot()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        exitSpotIndex = Random.Range(0, spotPositions.Count);
        exitGridPos = gridPositions[exitSpotIndex];

        // RPC to sync exit spot with all clients
        photonView.RPC("SyncExitSpot", RpcTarget.All, exitSpotIndex, exitGridPos.x, exitGridPos.y);

        // Set up position-based hints
        SetupPositionHints();
    }

    [PunRPC]
    void SyncExitSpot(int exitIndex, int row, int col)
    {
        exitSpotIndex = exitIndex;
        exitGridPos = new Vector2Int(row, col);
        Debug.Log($"Exit spot is at position: ({row}, {col}) - Index: {exitIndex}");
    }

    void SetupPositionHints()
    {
        for (int i = 0; i < allSpots.Count; i++)
        {
            if (i == exitSpotIndex) continue;

            ExitSpot spot = allSpots[i];
            Vector2Int spotGridPos = gridPositions[i];

            // Determine hint type based on position relative to exit
            HintType hintType = GetHintType(spotGridPos.x, spotGridPos.y);

            // Random chance to show hint
            bool showHint = Random.Range(0f, 1f) <= hintChance;

            // RPC to set the hint for this spot
            photonView.RPC("SetSpotHint", RpcTarget.All, i, (int)hintType, showHint);
        }
    }

    HintType GetHintType(int spotRow, int spotCol)
    {
        bool sameRow = (spotRow == exitGridPos.x);
        bool sameColumn = (spotCol == exitGridPos.y);

        if (sameRow && sameColumn)
        {
            // This should be the exit spot, but just in case
            return HintType.Neither;
        }
        else if (sameRow)
        {
            return HintType.SameRow;
        }
        else if (sameColumn)
        {
            return HintType.SameColumn;
        }
        else
        {
            return HintType.Neither;
        }
    }

    Sprite GetSpriteForHintType(HintType hintType)
    {
        switch (hintType)
        {
            case HintType.SameRow: return sameRowSprite;
            case HintType.SameColumn: return sameColumnSprite;
            case HintType.Neither: return neitherSprite;
            default: return neutralSprite;
        }
    }

    [PunRPC]
    void SetSpotHint(int spotIndex, int hintType, bool showHint)
    {
        if (spotIndex < allSpots.Count)
        {
            HintType type = (HintType)hintType;
            Sprite hintSprite = showHint ? GetSpriteForHintType(type) : neutralSprite;

            allSpots[spotIndex].SetHintSprite(hintSprite, type, showHint);
        }
    }

    // Called when a player finds the exit
    public void OnExitFound(int spotIndex)
    {
        Vector2Int gridPos = gridPositions[spotIndex];
        photonView.RPC("LevelCompleted", RpcTarget.All, spotIndex, gridPos.x, gridPos.y);
    }

    [PunRPC]
    void LevelCompleted(int spotIndex, int row, int col)
    {
        Debug.Log($"Level Completed! Exit was at position: ({row}, {col}) - Index: {spotIndex}");
        // Add your level completion logic here
    }
}