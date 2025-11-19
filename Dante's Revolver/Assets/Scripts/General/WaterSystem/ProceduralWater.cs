using System;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralWater : MonoBehaviourPunCallbacks
{
    [Header("Mesh")]
    [SerializeField] private int gridResolution = 128;      // número de vértices por lado (>= 2)
    [SerializeField] private float gridSize = 100f;         // tamanho em metros
    [SerializeField] private bool recenterMesh = true;

    [Header("Waves (Gerstner)")]
    [SerializeField] private int waveCount = 4;             // quantidade de ondas somadas
    [SerializeField] private float amplitudeMin = 0.2f;     // metros
    [SerializeField] private float amplitudeMax = 1.2f;
    [SerializeField] private float wavelengthMin = 8f;      // metros
    [SerializeField] private float wavelengthMax = 30f;
    [SerializeField] private float speedMin = 1.0f;         // m/s
    [SerializeField] private float speedMax = 3.0f;
    [SerializeField][Range(0f, 1f)] private float steepness = 0.6f; // 0..1 (0.7 é limite seguro)
    [SerializeField] private int randomSeed = 12345;        // inicial; será sobrescrito pelo Photon

    [Header("Time")]
    [SerializeField] private float timeScale = 1f;          // aceleração do tempo da água
    [SerializeField] private bool useUnscaledTime = false;

    // ---- Internos ----
    private MeshFilter meshFilter;
    private Mesh mesh;
    private Vector3[] baseVertices;     // grid plano
    private Vector3[] displacedVertices; // com ondas
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uv;

    // Dados das ondas
    [Serializable]
    private struct Wave
    {
        public Vector2 dir;     // direção normalizada (x,z)
        public float k;         // número de onda = 2π / λ
        public float amp;       // amplitude
        public float speed;     // velocidade de fase
        public float phase;     // fase inicial
    }
    private Wave[] waves;

    // Chaves para Room Properties
    private const string ROOM_KEY = "WATER_PARAMS";

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        BuildFlatGrid();
        GenerateWaves(randomSeed);
    }

    void Start()
    {
        // Se estamos em uma sala Photon, coordena os parâmetros:
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // Master gera seed aleatória e publica
                int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                PublishRoomWaterParams(seed);
                ApplySeed(seed);
            }
        }
        else
        {
            // Offline/Singleplayer: usa seed local serializeField
            ApplySeed(randomSeed);
        }
    }

    public override void OnJoinedRoom()
    {
        // Quando entrar na sala, tenta ler parâmetros publicados
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(ROOM_KEY, out object boxed))
        {
            if (boxed is int seed)
            {
                ApplySeed(seed);
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.TryGetValue(ROOM_KEY, out object boxed))
        {
            if (boxed is int seed)
            {
                ApplySeed(seed);
            }
        }
    }

    private void PublishRoomWaterParams(int seed)
    {
        var ht = new Hashtable { { ROOM_KEY, seed } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
    }

    private void ApplySeed(int seed)
    {
        randomSeed = seed;
        GenerateWaves(randomSeed);
    }

    private void BuildFlatGrid()
    {
        gridResolution = Mathf.Max(2, gridResolution);

        int vertsPerSide = gridResolution;
        int vertCount = vertsPerSide * vertsPerSide;
        int quadCount = (vertsPerSide - 1) * (vertsPerSide - 1);

        baseVertices = new Vector3[vertCount];
        displacedVertices = new Vector3[vertCount];
        normals = new Vector3[vertCount];
        uv = new Vector2[vertCount];
        triangles = new int[quadCount * 6];

        float step = gridSize / (vertsPerSide - 1);
        float offset = recenterMesh ? gridSize * 0.5f : 0f;

        int v = 0;
        for (int z = 0; z < vertsPerSide; z++)
        {
            for (int x = 0; x < vertsPerSide; x++)
            {
                float px = x * step - offset;
                float pz = z * step - offset;
                baseVertices[v] = new Vector3(px, 0f, pz);
                uv[v] = new Vector2((float)x / (vertsPerSide - 1), (float)z / (vertsPerSide - 1));
                normals[v] = Vector3.up;
                v++;
            }
        }

        int t = 0;
        for (int z = 0; z < vertsPerSide - 1; z++)
        {
            for (int x = 0; x < vertsPerSide - 1; x++)
            {
                int i0 = z * vertsPerSide + x;
                int i1 = i0 + 1;
                int i2 = i0 + vertsPerSide;
                int i3 = i2 + 1;
                triangles[t++] = i0; triangles[t++] = i2; triangles[t++] = i1;
                triangles[t++] = i1; triangles[t++] = i2; triangles[t++] = i3;
            }
        }

        mesh = new Mesh();
        mesh.indexFormat = (vertCount > 65000) ? UnityEngine.Rendering.IndexFormat.UInt32 : mesh.indexFormat;
        mesh.vertices = baseVertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    private void GenerateWaves(int seed)
    {
        var rng = new System.Random(seed);
        int n = Mathf.Clamp(waveCount, 1, 16);
        waves = new Wave[n];

        for (int i = 0; i < n; i++)
        {
            // Direção randômica no plano XZ
            float angle = (float)(rng.NextDouble() * Math.PI * 2.0);
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            float amp = Mathf.Lerp(amplitudeMin, amplitudeMax, (float)rng.NextDouble());
            float wavelength = Mathf.Lerp(wavelengthMin, wavelengthMax, (float)rng.NextDouble());
            float k = 2f * Mathf.PI / Mathf.Max(0.0001f, wavelength);
            float c = Mathf.Lerp(speedMin, speedMax, (float)rng.NextDouble());
            float phase = (float)(rng.NextDouble() * Math.PI * 2.0);

            waves[i] = new Wave
            {
                dir = dir.normalized,
                amp = amp,
                k = k,
                speed = c,
                phase = phase
            };
        }
    }

    void Update()
    {
        if (mesh == null || baseVertices == null) return;

        float t = (useUnscaledTime ? Time.unscaledTime : Time.time) * Mathf.Max(0f, timeScale);
        SimulateVertices(t);
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals(); // simples (CPU). Para performance, calcule normais analiticamente ou via shader.
        mesh.UploadMeshData(false);
    }

    private void SimulateVertices(float t)
    {
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 p = baseVertices[i];
            Vector3 disp = p;

            // Soma de ondas de Gerstner
            for (int w = 0; w < waves.Length; w++)
            {
                var wave = waves[w];
                // posição em 2D (x,z) projetada na direção da onda
                float d = wave.dir.x * (p.x + transform.position.x) + wave.dir.y * (p.z + transform.position.z);
                float wt = wave.k * d - wave.speed * wave.k * t + wave.phase;

                float a = wave.amp;
                float q = steepness / (wave.k * waves.Length); // divide por N para evitar autointerseção

                float cos = Mathf.Cos(wt);
                float sin = Mathf.Sin(wt);

                disp.x += q * a * wave.dir.x * cos;
                disp.y += a * sin;
                disp.z += q * a * wave.dir.y * cos;
            }

            displacedVertices[i] = disp;
        }
    }

    /// <summary>
    /// Retorna a altura da água (e normal aproximada) para uma posição do mundo.
    /// Útil para boia/embarcação. Cara O(N) nas ondas, O(1) no espaço.
    /// </summary>
    public float GetWaterHeight(Vector3 worldPos, out Vector3 approxNormal)
    {
        float t = (useUnscaledTime ? Time.unscaledTime : Time.time) * Mathf.Max(0f, timeScale);

        float y = transform.position.y;
        Vector3 n = new Vector3(0f, 1f, 0f);

        // Aproximação das normais analíticas
        float nx = 0f, ny = 1f, nz = 0f;

        float px = worldPos.x;
        float pz = worldPos.z;

        for (int w = 0; w < waves.Length; w++)
        {
            var wave = waves[w];
            float d = wave.dir.x * (px) + wave.dir.y * (pz);
            float wt = wave.k * d - wave.speed * wave.k * t + wave.phase;

            float a = wave.amp;
            float q = steepness / (wave.k * waves.Length);

            float cos = Mathf.Cos(wt);
            float sin = Mathf.Sin(wt);

            // deslocamento vertical
            y += a * sin;

            // derivadas parciais (aprox)
            float dx = -q * a * wave.dir.x * wave.k * sin;
            float dz = -q * a * wave.dir.y * wave.k * sin;
            float dy = 1f - q * a * wave.k * cos; // componente Y da normal antes de normalizar

            nx += dx;
            ny += (dy - 1f); // acumulando apenas a variação
            nz += dz;
        }

        n = new Vector3(-nx, 1f, -nz).normalized; // construção típica de normal aproximada
        approxNormal = n;
        return y;
    }
    public IEnumerator PauseWater()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<ProceduralWater>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        StopCoroutine(PauseWater());
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Gabarito de visualização da altura no centro
        Gizmos.color = Color.cyan;
        Vector3 pos = transform.position;
        GetWaterHeight(pos, out var n);
        Gizmos.DrawRay(new Ray(new Vector3(pos.x, GetWaterHeight(pos, out _), pos.z), n * 2f));
    }
#endif
}