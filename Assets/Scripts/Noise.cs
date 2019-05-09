using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    int seed;
    float frequency;
    float amplitude;

    float lacunarity;
    float persistence;

    int octaves;
    public Noise(int seed,float frequency, float amplitude, float lacunarity, float persistence, int octaves)
    {
        this.seed = seed;
        this.frequency = frequency;
        this.amplitude = amplitude;
        this.lacunarity = lacunarity;
        this.persistence = persistence;
        this.octaves = octaves;
    }
    public float[,] GetNoiseValues(int width, int height,int offsetX=0, int offsetY=0)
    {
        float[,] noiseValues = new float[width, height];
        float max = 0f;
        float min = float.MaxValue;
        for(int i = 0; i < width;i++)
        {
            for(int j = 0;j<height;j++)
            {
                float tempA = amplitude;
                float tempF = frequency;
                noiseValues[i, j] = 0f;
                for(int k = 0; k < octaves; k++)
                {
                    noiseValues[i, j] += Mathf.PerlinNoise(((i + seed + offsetX) / (float)width * frequency), (j+seed+offsetY) / (float)height * frequency) * amplitude;
                    frequency *= lacunarity;
                    amplitude *= persistence;
                }
                amplitude = tempA;
                frequency = tempF;
                if (noiseValues[i, j] > max) max = noiseValues[i, j];
                if (noiseValues[i, j] < min) min = noiseValues[i, j];
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseValues[i, j] = Mathf.InverseLerp(max, min, noiseValues[i, j]);
            }
        }


        return noiseValues;
    }

}
