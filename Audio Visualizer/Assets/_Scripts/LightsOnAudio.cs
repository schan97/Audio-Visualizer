using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightsOnAudio : MonoBehaviour
{
	public int band;
	public float minIntensity, maxIntensity;
	Light lights;
    // Start is called before the first frame update
    void Start()
    {
		lights = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
		if(AudioVisualize.audioBand[band] > 0)
		{
			lights.intensity = (AudioVisualize.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
		}
    }
}
