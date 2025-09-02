using UnityEngine;


[System.Serializable]
public struct Particle {
    public float pressure;
    public float density;
    public Vector3 currentForce;
    public Vector3 velocity;
    public Vector3 position;
}

public class SPH : MonoBehaviour
{
    [Header("General")]
    public bool showSpheres = true;

    public Vector3Int numToSpawn = new Vector3Int(10, 10, 10);
    private int totalParticles
    {
        get
        {
            return numToSpawn.x * numToSpawn.y * numToSpawn.z;
        }
    }

    public Vector3Int boxSize = new Vector3Int(4, 10, 3);
    public Vector3Int spawnCenter;
    public float particleRadius = 0.1f;

    [Header("Particle Rendering")]
    public Mesh particleMesh;
    public float particleRenderSize = 8.0f;
    public Material material;

    [Header("Compute")]
    public ComputeShader shader;
    public Particle[] particles;


    private ComputeBuffer _argsBuffer;
    private ComputeBuffer _particleBuffer;

    private void Awake()
    {
        SpawnParticlesInBox();
        // setup args for instanced particle rendering
        uint[] args = {
            particleMesh.GetIndexCount(0),
            (uint)totalParticles,
            particleMesh.GetIndexStart(0),
            particleMesh.GetBaseVertex(0),
            0
        };

        _argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        _argsBuffer.SetData(args);

        // Set up particle buffer
        _particleBuffer = new ComputeBuffer(totalParticles, 44);
        _particleBuffer.SetData(particles);

    }

    private void SpawnParticlesInBox()
    {
        Vector3 spawnPoint = spawnCenter;
        List<Particle> _particles = new List<Particle>();

        for (int x = 0; x < numToSpawn.x; x++)
        {
            for (int y = 0; y < numToSpawn.y; y++)
            {
                for (int z = 0; z < numToSpawn.z; z++)
                {
                    Vector3 spawnPos = spawnPoint + new Vector3(x * particleRadius * 2, y * particleRadius * 2, z * particleRadius * 2);
                    Particle p = new Particle
                    {
                        position = spawnPos
                    };
                    _particles.Add(p);
                }
            }
        }

        particles = _particles.ToArray();
    }

    // private void OnDrawGizmoz()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireCube
    // }

    private static readonly int SizeProperty = Shader.PropertyToID("_size");
    private static readonly int ParticlesBufferProperty = Shader.PropertyToID("_particlesBuffer");


    private void Update()
    {
        material.setFloat(SizeProperty, particleRenderSize);
        material.setBuffer(ParticlesBufferProperty, _particleBuffer);

        if (showSpheres)
        {
            Graphics.DrawMeshInstancedIndirect(
                particleMesh,
                0,
                material,
                new Bounds(Vector3.zero, boxSize),
                _argsBuffer,
                castShadows: UnityEngine.Rendering.ShadowCastingMode.Off
            );
        }
    }





}
