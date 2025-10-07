using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ColorBlindnessSetting : MonoBehaviourPunCallbacks
{
    public enum ColorBlindMode
    {
        Normal,
        Deuteranopia,
        Protanopia,
        Tritanopia
    }

    public static ColorBlindnessSetting Instance;

    [SerializeField] private LayerMask layerMask;
    [Header("Colorblind Settings")]
    public ColorBlindMode currentMode = ColorBlindMode.Normal;
    public int paletteSize = 8;

    private Dictionary<ColorBlindMode, Color[]> palettes = new Dictionary<ColorBlindMode, Color[]>();
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GeneratePalettes();
        CacheOriginalColors();

        // Sync mode if already set in room
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("ColorBlindMode", out object mode))
            {
                currentMode = (ColorBlindMode)(int)mode;
            }
        }

        ApplyPalette(currentMode);
    }

    // -------------------- UI Methods --------------------
    public void SetColorBlindMode_Normal() => ChangeMode(ColorBlindMode.Normal);
    public void SetColorBlindMode_Deuteranopia() => ChangeMode(ColorBlindMode.Deuteranopia);
    public void SetColorBlindMode_Protanopia() => ChangeMode(ColorBlindMode.Protanopia);
    public void SetColorBlindMode_Tritanopia() => ChangeMode(ColorBlindMode.Tritanopia);

    private void ChangeMode(ColorBlindMode mode)
    {
        currentMode = mode;
        ApplyPalette(mode);

        // Sync to all players
        if (PhotonNetwork.InRoom)
        {
            Hashtable props = new Hashtable { { "ColorBlindMode", (int)mode } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    // -------------------- Core Functions --------------------
    private void CacheOriginalColors()
    {
        originalColors.Clear();
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & layerMask) != 0)
            {
                Renderer r = obj.GetComponent<Renderer>();
                if (r != null && r.material != null && !originalColors.ContainsKey(obj))
                {
                    originalColors.Add(obj, r.material.color);
                }
            }
        }
    }

    private void GeneratePalettes()
    {
        Color[] basePalette = GetColorBlindSafePalette(paletteSize);

        palettes[ColorBlindMode.Normal] = null; // Normal mode restores original
        palettes[ColorBlindMode.Deuteranopia] = SimulateDeuteranopia(basePalette);
        palettes[ColorBlindMode.Protanopia] = SimulateProtanopia(basePalette);
        palettes[ColorBlindMode.Tritanopia] = SimulateTritanopia(basePalette);
    }

    public void ApplyPalette(ColorBlindMode mode)
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<GameObject> objectsInLayer = new List<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & layerMask) != 0)
                objectsInLayer.Add(obj);
        }

        if (mode == ColorBlindMode.Normal)
        {
            foreach (GameObject obj in objectsInLayer)
            {
                if (originalColors.TryGetValue(obj, out Color orig))
                {
                    Renderer r = obj.GetComponent<Renderer>();
                    if (r != null && r.material != null) r.material.color = orig;
                }
            }
        }
        else if (palettes.TryGetValue(mode, out Color[] palette))
        {
            int i = 0;
            foreach (GameObject obj in objectsInLayer)
            {
                if (originalColors.TryGetValue(obj, out Color orig) && orig != Color.black)
                {
                    Renderer r = obj.GetComponent<Renderer>();
                    if (r != null && r.material != null)
                    {
                        r.material.color = palette[i % palette.Length];
                        i++;
                    }
                }
            }
        }
    }

    // -------------------- Photon Event --------------------
    private void OnPhotonEvent(EventData photonEvent)
    {
        // Custom property sync
        if (photonEvent.Code == 0) return; // ignore other events

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("ColorBlindMode", out object mode))
        {
            currentMode = (ColorBlindMode)(int)mode;
            ApplyPalette(currentMode);
        }
    }

    // Call this whenever a new Photon object spawns
    public void ApplyPaletteToObject(GameObject obj)
    {
        if (((1 << obj.layer) & layerMask) == 0) return;

        Renderer r = obj.GetComponent<Renderer>();
        if (r == null || r.material == null) return;

        if (!originalColors.ContainsKey(obj)) originalColors.Add(obj, r.material.color);

        if (currentMode == ColorBlindMode.Normal) r.material.color = originalColors[obj];
        else if (originalColors[obj] != Color.black)
        {
            Color[] palette = palettes[currentMode];
            r.material.color = palette[0]; // assign first color (or pick logic as needed)
        }
    }

    // -------------------- Color Simulation --------------------
    private Color[] GetColorBlindSafePalette(int count)
    {
        Color[] safeColors = new Color[]
        {
            new Color(0.0f, 0.45f, 0.70f),
            new Color(0.87f, 0.37f, 0.00f),
            new Color(0.00f, 0.60f, 0.50f),
            new Color(0.95f, 0.90f, 0.25f),
            new Color(0.80f, 0.47f, 0.65f),
            new Color(0.35f, 0.70f, 0.90f),
            new Color(0.90f, 0.60f, 0.00f),
            new Color(0.50f, 0.50f, 0.50f)
        };

        if (count <= safeColors.Length)
        {
            Color[] subset = new Color[count];
            System.Array.Copy(safeColors, subset, count);
            return subset;
        }
        else
        {
            List<Color> extended = new List<Color>(safeColors);
            while (extended.Count < count)
                extended.Add(safeColors[extended.Count % safeColors.Length]);
            return extended.ToArray();
        }
    }

    private Color[] SimulateDeuteranopia(Color[] original)
    {
        Color[] result = new Color[original.Length];
        for (int i = 0; i < original.Length; i++)
        {
            Color c = original[i];
            result[i] = new Color(0.625f * c.r + 0.375f * c.g, 0.70f * c.g, 0.30f * c.b);
        }
        return result;
    }

    private Color[] SimulateProtanopia(Color[] original)
    {
        Color[] result = new Color[original.Length];
        for (int i = 0; i < original.Length; i++)
        {
            Color c = original[i];
            result[i] = new Color(0.566f * c.r + 0.433f * c.g, 0.558f * c.g, 0.442f * c.b);
        }
        return result;
    }

    private Color[] SimulateTritanopia(Color[] original)
    {
        Color[] result = new Color[original.Length];
        for (int i = 0; i < original.Length; i++)
        {
            Color c = original[i];
            result[i] = new Color(0.95f * c.r + 0.05f * c.g, 0.433f * c.g + 0.567f * c.b, 0.475f * c.g + 0.525f * c.b);
        }
        return result;
    }
}
