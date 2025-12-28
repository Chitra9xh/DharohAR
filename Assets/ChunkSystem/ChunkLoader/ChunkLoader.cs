using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GLTFast;
using ChunkSystem;

public class ChunkLoader : MonoBehaviour
{
    private List<ChunkInfo> chunks = new List<ChunkInfo>();

    // ---------------- UNITY LIFECYCLE ----------------

    private void Awake()
    {
        InitializeChunks();
    }

    // NOTE:
    // We DO NOT auto-load in Start() anymore.
    // RuleEvaluator will decide what loads.

    // ---------------- INITIALIZATION ----------------

    private void InitializeChunks()
    {
        chunks = new List<ChunkInfo>
        {
            // Always loaded
            new ChunkInfo("Structure_Base",   "Chunks/Structure_Base.glb",   0, -1),
            new ChunkInfo("Structure_main",   "Chunks/Structure_main.glb",   1, -1),

            // Load within 3 meters
            new ChunkInfo("Structure_Pillar1","Chunks/Structure_Pillar1.glb",2, 3.0f),
            new ChunkInfo("Structure_Pillar2","Chunks/Structure_Pillar2.glb",2, 3.0f),
            new ChunkInfo("Structure_Pillar3","Chunks/Structure_Pillar3.glb",2, 3.0f),
            new ChunkInfo("Structure_Pillar4","Chunks/Structure_Pillar4.glb",2, 3.0f),

            // Load within 2 meters
            new ChunkInfo("Structure_Roof",   "Chunks/Structure_Roof.glb",   3, 2.0f),
        };
    }

    // ---------------- PUBLIC API (FOR RULE EVALUATOR) ----------------

    public List<ChunkInfo> GetChunks()
    {
        return chunks;
    }

    public async void EnsureLoaded(ChunkInfo chunk)
    {
        if (chunk.isLoaded)
            return;

        await LoadChunk(chunk);
    }

    public void EnsureUnloaded(ChunkInfo chunk)
    {
        if (!chunk.isLoaded)
            return;

        Destroy(chunk.instance);
        chunk.instance = null;
        chunk.isLoaded = false;
    }

    // ---------------- INTERNAL LOADING ----------------

    private async System.Threading.Tasks.Task LoadChunk(ChunkInfo chunk)
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, chunk.relativePath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError($"[ChunkLoader] File not found: {fullPath}");
            return;
        }

        var gltf = new GltfImport();
        bool success = await gltf.Load(fullPath);

        if (!success)
        {
            Debug.LogError($"[ChunkLoader] Failed to load GLB: {chunk.chunkId}");
            return;
        }

        GameObject chunkGO = new GameObject(chunk.chunkId);
        chunkGO.transform.SetParent(transform, false);

        await gltf.InstantiateMainSceneAsync(chunkGO.transform);
        chunk.instance = chunkGO;
        chunk.isLoaded = true;
    }
}
