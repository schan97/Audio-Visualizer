using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightsOnAudio : MonoBehaviour
{
	public AudioVisualize audioVisualize;
	public int band;
	public float minIntensity, maxIntensity;
	Light lights;

    void Start()
    {
		lights = GetComponent<Light>();
    }

    void Update()
    {
		if(audioVisualize.audioBand[band] > 0)
		{
			lights.intensity = (audioVisualize.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
		}
    }
}
