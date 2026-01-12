using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunks")]
    public GameObject[] chunks;          // Your procedural chunk prefabs
    public GameObject startChunk;        // Optional starting chunk
    public GameObject finishChunk;       // Optional finishing chunk
    public int numberOfChunks = 10;      // Number of procedural chunks
    public float chunkHeight = 16f;      // Height of each chunk
    public bool randomizeOrder = true;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        Vector3 spawnPos = Vector3.zero;

        // Spawn starting chunk
        if (startChunk != null)
        {
            Instantiate(startChunk, spawnPos, Quaternion.identity, transform);
            spawnPos.y += chunkHeight;
        }

        // Spawn procedural chunks
        for (int i = 0; i < numberOfChunks; i++)
        {
            GameObject chunkToSpawn;

            if (randomizeOrder)
            {
                int randIndex = Random.Range(0, chunks.Length);
                chunkToSpawn = chunks[randIndex];
            }
            else
            {
                chunkToSpawn = chunks[i % chunks.Length];
            }

            Instantiate(chunkToSpawn, spawnPos, Quaternion.identity, transform);
            spawnPos.y += chunkHeight;
        }

        // Spawn finishing chunk
        if (finishChunk != null)
        {
            Instantiate(finishChunk, spawnPos, Quaternion.identity, transform);
        }
    }
}
