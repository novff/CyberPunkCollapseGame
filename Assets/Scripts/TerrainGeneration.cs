using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Perlin Noise Texture Settings")]
    public int Width = 512;
    public int Height = 512;
    public int Depth = 40;
    public float Scale = 20F;
    public float OffsetX = 100F;
    public float OffsetY = 100F;

    void Start()
        {
            Terrain terrain = GetComponent<Terrain>();
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
            //OffsetX = Random.Range(0f, 1000000f);
            //OffsetY = Random.Range(0f, 1000000f);
        }

    TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = Width + 1;
            terrainData.size = new Vector3(Width, Depth, Height);
            terrainData.SetHeights(0, 0, GenerateHeights());
            return terrainData;
        }

    float[,] GenerateHeights()
        {
            float[,] heights = new float [Width, Height];
            for(int x = 0; x < Width; x++)
                {
                    for(int y = 0; y < Width; y++)
                        {
                            heights[x, y] = CalcHeight(x, y);
                        }
                }
            return heights;
        }
    float CalcHeight(int x, int y)
        {
            float PerlinX = (float)x / Width * Scale + OffsetX;
            float PerlinY = (float)y / Width * Scale + OffsetY;
            return Mathf.PerlinNoise(PerlinX, PerlinY);
        }
}
