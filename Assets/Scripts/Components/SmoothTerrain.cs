using UnityEngine;

public class SmoothTerrain : MonoBehaviour
{
    public int borderWidth = 600;
    public float smoothFactor = 0.01f;

    private void OnEnable()
    {
        SmoothTerrainEdges();
    }

    public void SmoothTerrainEdges()
    {
        Terrain terrain = GetComponent<Terrain>();

        TerrainData terrainData = terrain.terrainData;
        int heightmapWidth = terrainData.heightmapResolution;
        int heightmapHeight = terrainData.heightmapResolution;

        float[,] heights = terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        for (int x = 0; x < heightmapWidth; x++)
        {
            for (int y = 0; y < heightmapHeight; y++)
            {
                float distanceToEdge = Mathf.Min(
                    x / (float)borderWidth,
                    (heightmapWidth - 1 - x) / (float)borderWidth,
                    y / (float)borderWidth,
                    (heightmapHeight - 1 - y) / (float)borderWidth
                );

                if (distanceToEdge < 1.0f)
                {
                    // Используем синусоидальное сглаживание
                    float smoothMultiplier = Mathf.Sin(distanceToEdge * Mathf.PI / 2);
                    heights[y, x] *= Mathf.Lerp(smoothFactor, 1.0f, smoothMultiplier);
                }
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
