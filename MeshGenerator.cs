using UnityEngine;
using Random = UnityEngine.Random;
using FishNet.Object;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : NetworkBehaviour
{
    private static MeshGenerator instance;

    [SerializeField] private int size = 20;

    [Header("Perlin Noise Settings")] 
    [SerializeField] private float scale = 10f;
    [SerializeField] private float distance = 0.2f;
    [SerializeField] private float offset = 0f;

    [Header("Falloff settings")]
    [SerializeField] private float falloffDistance = 18f;
    [SerializeField] private AnimationCurve falloffCurve;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private float randomOffsetX, randomOffsetZ;

    private void Awake()
    {
        instance = this;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void Start()
    {
        // Only the server generates and shares the world seed
        if (IsServer)
        {
            int seed = Random.Range(0, 999999);
            InitializeWorld(seed); // Calls the RPC which is buffered for new clients
        }
    }

    [ObserversRpc(BufferLast = true)]
    private void InitializeWorld(int seed)
    {
        Random.InitState(seed);

        randomOffsetX = Random.Range(-50000f, 50000f);
        randomOffsetZ = Random.Range(-50000f, 50000f);

        CreateShape();
        UpdateMesh();

        // If you have a system to spawn additional objects like trees, rocks, etc.
        NatureGenerator.SpawnNature();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(size + 1) * (size + 1)];
        Vector2 center = new Vector2(size / 2f, size / 2f);

        for (int i = 0, z = 0; z <= size; z++)
        {
            for (int x = 0; x <= size; x++)
            {
                float y = Mathf.PerlinNoise((x + offset + randomOffsetX) * distance, (z + offset + randomOffsetZ) * distance) * scale;

                float distanceFromCenter = Vector2.Distance(center, new Vector2(x, z));
                float relativeDistance = Mathf.InverseLerp(0, falloffDistance, distanceFromCenter);
                y *= -falloffCurve.Evaluate(relativeDistance) + 1;

                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[size * size * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= size; z++)
        {
            for (int x = 0; x <= size; x++)
            {
                uvs[i] = new Vector2((float)x / size, (float)z / size);
                i++;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uvs;

        if (TryGetComponent(out Renderer renderer))
            renderer.material.mainTextureScale = new Vector2(size, size);

        if (TryGetComponent(out MeshCollider collider))
            collider.sharedMesh = mesh;
    }

    public static float GetSize()
    {
        return instance.size;
    }
}