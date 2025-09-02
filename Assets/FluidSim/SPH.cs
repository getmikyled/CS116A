using UnityEngine;

public class SPH : MonoBehaviour {
    [Header("General")]
    public bool showSpheres = true;

    public Vector3Int numToSpawn = new Vector3Int(10, 10, 10);
    private int totalParticles {
        get {
            return numToSpawn.x * numToSpawn.y * numToSpawn.z;
        }
    }
    
    public Vector3Int boxSize = new Vector3Int(4, 10,3 );
    public Vector3Int spawnCenter;
    public float particleRadius = 0.1f;

    [Header("Particle Rendering")]
    public Mesh particleMesh;
    public float 


}
