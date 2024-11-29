using UnityEngine;
public class TerrainMirror : MonoBehaviour
{
    public Terrain sourceTerrain;  // �������� �������
    public Terrain targetTerrain;  // �������, ������� ����� �����������
    public bool mirrorX = true;    // �������� �� ��� X
    public bool mirrorZ = false;   // �������� �� ��� Z
    public int edgeFadeWidth = 10; // ������ �� ����� ��� �������� ������� �����

    private void Start()
    {
        MirrorTerrain();
    }

    public void MirrorTerrain()
    {
        if (sourceTerrain == null || targetTerrain == null)
        {
            Debug.LogError("Assign both sourceTerrain and targetTerrain!");
            return;
        }

        TerrainData sourceData = sourceTerrain.terrainData;
        TerrainData targetData = targetTerrain.terrainData;
        targetData.size = sourceData.size;

        int resolution = sourceData.heightmapResolution;
        float[,] sourceHeights = sourceData.GetHeights(0, 0, resolution, resolution);
        float[,] targetHeights = new float[resolution, resolution];

        // ������� ����������� ������ � �������� ��������
        float minHeight = float.MaxValue;
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                if (sourceHeights[z, x] < minHeight)
                {
                    minHeight = sourceHeights[z, x];
                }
            }
        }

        float heightOnEdge = 0;
        // �������������� �����
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                int mirroredX = mirrorX ? resolution - 1 - x : x;
                int mirroredZ = mirrorZ ? resolution - 1 - z : z;
                float mirroredHeight = sourceHeights[mirroredZ, mirroredX];

                // ����������, �������� �� ������� ����� �����
                bool isEdgeX = mirrorX && (x == 0 || x == resolution - 1);
                bool isEdgeZ = mirrorZ && (z == 0 || z == resolution - 1);

                if (isEdgeX || isEdgeZ)
                {
                    // ��������� ������������ ������ �� ����
                    targetHeights[z, x] = mirroredHeight;
                    heightOnEdge = mirroredHeight;
                }
                else
                {
                    // ��������� ���������� �� ���������� ����
                    int distanceToEdge = CalculateDistanceToEdge(x, z, resolution);

                    if (distanceToEdge <= edgeFadeWidth)
                    {
                        // �������������� ����������� �� ���� � ����������� ������
                        float t = 1.0f - ((float)distanceToEdge / edgeFadeWidth); // ����������� t
                        float smoothT = t * t; // ������������ ������������

                        // ������������� �� ������� ������ � �����������
                        targetHeights[z, x] = Mathf.Lerp(minHeight, heightOnEdge, smoothT);
                    }
                    else
                    {
                        // �� ��������� ���� �������� ���������� ����������� ������
                        targetHeights[z, x] = minHeight;
                    }
                }
            }
        }

        targetData.SetHeights(0, 0, targetHeights);
        CopyTerrainTextures(sourceData, targetData);
    }

    private int CalculateDistanceToEdge(int x, int z, int resolution)
    {
        int distanceX = mirrorX ? Mathf.Min(x, resolution - 1 - x) : int.MaxValue;
        int distanceZ = mirrorZ ? Mathf.Min(z, resolution - 1 - z) : int.MaxValue;
        return Mathf.Min(distanceX, distanceZ);
    }

    private void CopyTerrainTextures(TerrainData source, TerrainData target)
    {
        // ����������� TerrainLayers
        target.terrainLayers = source.terrainLayers;

        // ����������� �����-����� ��� ������� ����
        int alphaWidth = source.alphamapWidth;
        int alphaHeight = source.alphamapHeight;
        float[,,] sourceAlphaMaps = source.GetAlphamaps(0, 0, alphaWidth, alphaHeight);
        float[,,] targetAlphaMaps = new float[alphaWidth, alphaHeight, sourceAlphaMaps.GetLength(2)];

        int blendPixels = 50; // ���������� �������� ��� ������������
        for (int x = 0; x < alphaWidth; x++)
        {
            for (int z = 0; z < alphaHeight; z++)
            {
                int mirroredX = mirrorX ? alphaWidth - 1 - x : x;
                int mirroredZ = mirrorZ ? alphaHeight - 1 - z : z;

                // ���������� ���������� �� ����
                int distanceToEdge = CalculateDistanceToEdge(x, z, alphaWidth, alphaHeight);

                for (int layer = 0; layer < sourceAlphaMaps.GetLength(2); layer++)
                {
                    if (distanceToEdge <= blendPixels)
                    {
                        // ������� ���������� ��� ���� ��������
                        float t = distanceToEdge / (float)blendPixels;
                        t = Mathf.SmoothStep(0f, 1f, t); // ���������� ������� ���
                        float sourceValue = sourceAlphaMaps[mirroredZ, mirroredX, layer];
                        float targetValue = (layer == 0) ? 1f : 0f; // �������� ���� (R) �������� ��� 1, ��������� - 0
                        targetAlphaMaps[z, x, layer] = Mathf.Lerp(sourceValue, targetValue, t);
                    }
                    else
                    {
                        // ����� ���� ���������� � ������������� ��������
                        targetAlphaMaps[z, x, layer] = (layer == 0) ? 1f : 0f;
                    }
                }

                // ������������ ����� ��� ������� �������
                NormalizeWeights(targetAlphaMaps, z, x);
            }
        }

        target.SetAlphamaps(0, 0, targetAlphaMaps);
    }

    private int CalculateDistanceToEdge(int x, int z, int alphaWidth, int alphaHeight)
    {
        int distanceX = mirrorX ? Mathf.Min(x, alphaWidth - 1 - x) : int.MaxValue;
        int distanceZ = mirrorZ ? Mathf.Min(z, alphaHeight - 1 - z) : int.MaxValue;
        return Mathf.Min(distanceX, distanceZ);
    }

    private void NormalizeWeights(float[,,] alphaMaps, int z, int x)
    {
        float sum = 0f;

        // ��������� ��� ���� �����
        for (int layer = 0; layer < alphaMaps.GetLength(2); layer++)
        {
            sum += alphaMaps[z, x, layer];
        }

        // ����������� ����
        if (sum > 0f)
        {
            for (int layer = 0; layer < alphaMaps.GetLength(2); layer++)
            {
                alphaMaps[z, x, layer] /= sum;
            }
        }
    }
}