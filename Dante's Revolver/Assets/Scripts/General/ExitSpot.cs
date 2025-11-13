using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ExitSpot : MonoBehaviourPunCallbacks
{
    [Header("Spot Components")]
    public GameObject hiddenObject;
    public Image hintImage;
    public Button spotButton;

    [Header("Spot Settings")]
    public bool isExit = false;
    private int row = -1;
    private int col = -1;
    private int spotIndex = -1;
    private HintType hintType;
    private bool hasHint = false;

    private bool isRevealed = false;
    private ExitSpotManager manager;

    void Start()
    {
        // Find the manager
        manager = FindFirstObjectByType<ExitSpotManager>();

        // Set up button click
        if (spotButton != null)
        {
            spotButton.onClick.AddListener(OnSpotClicked);
        }

        // Initially hide the hint image
        if (hintImage != null)
        {
            hintImage.gameObject.SetActive(false);
        }
    }

    public void SetGridPosition(int rowPos, int colPos)
    {
        row = rowPos;
        col = colPos;
    }

    public void SetSpotIndex(int index)
    {
        spotIndex = index;
    }

    public int GetRow() { return row; }
    public int GetCol() { return col; }
    public int GetSpotIndex() { return spotIndex; }

    public void SetHintSprite(Sprite sprite, HintType type, bool hasValidHint)
    {
        if (hintImage != null)
        {
            hintImage.sprite = sprite;
            hintType = type;
            hasHint = hasValidHint;
        }
    }

    public void SetAsExit(bool exit)
    {
        isExit = exit;
    }

    void OnSpotClicked()
    {
        if (isRevealed) return;

        photonView.RPC("RevealSpot", RpcTarget.All);
    }

    [PunRPC]
    void RevealSpot()
    {
        isRevealed = true;

        if (isExit)
        {
            // This is the exit spot - level complete!
            Debug.Log($"Exit found at position: ({row}, {col})!");
            if (hintImage != null)
            {
                hintImage.gameObject.SetActive(true);
                hintImage.sprite = null; // Or use a special exit sprite
                hintImage.color = Color.green;
            }

            // Notify manager
            if (manager != null)
            {
                manager.OnExitFound(spotIndex);
            }
        }
        else
        {
            // Show hint
            if (hintImage != null)
            {
                hintImage.gameObject.SetActive(true);

                if (hasHint)
                {
                    // Color code based on hint type
                    switch (hintType)
                    {
                        case HintType.SameRow:
                            hintImage.color = Color.red;
                            break;
                        case HintType.SameColumn:
                            hintImage.color = Color.blue;
                            break;
                        case HintType.Neither:
                            hintImage.color = Color.yellow;
                            break;
                    }
                }
                else
                {
                    // No hint available for this spot
                    hintImage.color = Color.gray;
                }
            }
        }

        // Hide the clickable object
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(false);
        }

        // Disable the button
        if (spotButton != null)
        {
            spotButton.interactable = false;
        }
    }
}