using UnityEngine;
using System.Collections;
using ChunkSystem;

public class ChunkRuleEvaluator : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera;

    [SerializeField]
    private float evaluationInterval = 0.5f;

    private ChunkLoader chunkLoader;

    private void Awake()
    {
        chunkLoader = GetComponent<ChunkLoader>();

        if (chunkLoader == null)
        {
            Debug.LogError("ChunkLoader not found on MonumentRoot");
        }

        if (arCamera == null)
        {
            arCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(EvaluateLoop());
    }

    private IEnumerator EvaluateLoop()
    {
        while (true)
        {
            EvaluateChunks();
            yield return new WaitForSeconds(evaluationInterval);
        }
    }

    private void EvaluateChunks()
    {
        float distance = Vector3.Distance(
            arCamera.transform.position,
            transform.position
        );

        foreach (var chunk in chunkLoader.GetChunks())
        {
            if (chunk.loadDistance < 0)
            {
                chunkLoader.EnsureLoaded(chunk);
            }
            else
            {
                if (distance <= chunk.loadDistance)
                {
                    chunkLoader.EnsureLoaded(chunk);
                }
                else
                {
                    chunkLoader.EnsureUnloaded(chunk);
                }
            }
        }
    }
}
