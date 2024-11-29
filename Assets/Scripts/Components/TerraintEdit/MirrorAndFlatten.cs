using UnityEngine;
public class TerrainMirror : MonoBehaviour
{
    public Terrain sourceTerrain;  // Исходный террейн
    public Terrain targetTerrain;  // Террейн, который нужно отзеркалить
    public bool mirrorX = true;    // Отразить по оси X
    public bool mirrorZ = false;   // Отразить по оси Z
    public int edgeFadeWidth = 10; // Отступ от краев для плавного сужения высот

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

        // Находим минимальную высоту в исходном террейне
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
        // Отзеркаливание высот
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                int mirroredX = mirrorX ? resolution - 1 - x : x;
                int mirroredZ = mirrorZ ? resolution - 1 - z : z;
                float mirroredHeight = sourceHeights[mirroredZ, mirroredX];

                // Определяем, является ли текущая точка краем
                bool isEdgeX = mirrorX && (x == 0 || x == resolution - 1);
                bool isEdgeZ = mirrorZ && (z == 0 || z == resolution - 1);

                if (isEdgeX || isEdgeZ)
                {
                    // Сохраняем оригинальную высоту на краю
                    targetHeights[z, x] = mirroredHeight;
                    heightOnEdge = mirroredHeight;
                }
                else
                {
                    // Вычисляем расстояние до ближайшего края
                    int distanceToEdge = CalculateDistanceToEdge(x, z, resolution);

                    if (distanceToEdge <= edgeFadeWidth)
                    {
                        // Параболическое сглаживание от края к минимальной высоте
                        float t = 1.0f - ((float)distanceToEdge / edgeFadeWidth); // Инвертируем t
                        float smoothT = t * t; // Квадратичная интерполяция

                        // Интерполируем от текущей высоты к минимальной
                        targetHeights[z, x] = Mathf.Lerp(minHeight, heightOnEdge, smoothT);
                    }
                    else
                    {
                        // За пределами зоны перехода используем минимальную высоту
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
        // Копирование TerrainLayers
        target.terrainLayers = source.terrainLayers;

        // Копирование альфа-мапов для каждого слоя
        int alphaWidth = source.alphamapWidth;
        int alphaHeight = source.alphamapHeight;
        float[,,] sourceAlphaMaps = source.GetAlphamaps(0, 0, alphaWidth, alphaHeight);
        float[,,] targetAlphaMaps = new float[alphaWidth, alphaHeight, sourceAlphaMaps.GetLength(2)];

        int blendPixels = 50; // Количество пикселей для интерполяции
        for (int x = 0; x < alphaWidth; x++)
        {
            for (int z = 0; z < alphaHeight; z++)
            {
                int mirroredX = mirrorX ? alphaWidth - 1 - x : x;
                int mirroredZ = mirrorZ ? alphaHeight - 1 - z : z;

                // Определяем расстояние до края
                int distanceToEdge = CalculateDistanceToEdge(x, z, alphaWidth, alphaHeight);

                for (int layer = 0; layer < sourceAlphaMaps.GetLength(2); layer++)
                {
                    if (distanceToEdge <= blendPixels)
                    {
                        // Плавное смешивание для зоны перехода
                        float t = distanceToEdge / (float)blendPixels;
                        t = Mathf.SmoothStep(0f, 1f, t); // Используем плавный шаг
                        float sourceValue = sourceAlphaMaps[mirroredZ, mirroredX, layer];
                        float targetValue = (layer == 0) ? 1f : 0f; // Основной слой (R) получает вес 1, остальные - 0
                        targetAlphaMaps[z, x, layer] = Mathf.Lerp(sourceValue, targetValue, t);
                    }
                    else
                    {
                        // После зоны смешивания — фиксированные значения
                        targetAlphaMaps[z, x, layer] = (layer == 0) ? 1f : 0f;
                    }
                }

                // Нормализация весов для каждого пикселя
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

        // Суммируем все веса слоев
        for (int layer = 0; layer < alphaMaps.GetLength(2); layer++)
        {
            sum += alphaMaps[z, x, layer];
        }

        // Нормализуем веса
        if (sum > 0f)
        {
            for (int layer = 0; layer < alphaMaps.GetLength(2); layer++)
            {
                alphaMaps[z, x, layer] /= sum;
            }
        }
    }
}