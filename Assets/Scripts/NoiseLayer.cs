using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseLayer
{
    public string layerName;
    public float noiseOffsetX, noiseOffsetY;
    public float noiseScale;
    public bool subtractive = false;
}
