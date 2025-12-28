using UnityEngine;

namespace ChunkSystem
{
    [System.Serializable]
    public class ChunkInfo
    {
        // -------- Identity --------
        public string chunkId;

        // -------- Source --------
        public string relativePath;

        // -------- Policy --------
        public int priority;

        // Load rule (meters)
        // -1 = always loaded
        public float loadDistance;

        // -------- Runtime State (Unity-owned) --------
        [System.NonSerialized]
        public bool isLoaded = false;

        [System.NonSerialized]
        public GameObject instance;

        // -------- Constructor --------
        public ChunkInfo(string chunkId, string relativePath, int priority, float loadDistance)
        {
            this.chunkId = chunkId;
            this.relativePath = relativePath;
            this.priority = priority;
            this.loadDistance = loadDistance;
        }
    }
}
