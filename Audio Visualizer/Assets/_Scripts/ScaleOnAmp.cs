using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmp : MonoBehaviour
{
	public AudioVisualize audioVisualize;

	public float startScale, scaleMultiplier;

	public bool useBuffer;
	public bool useColorChange;

	Material material;

	public float red, green, blue;
    void Start()
    {
		if (useColorChange == true)
		{
			material = GetComponent<MeshRenderer>().materials[0];
		}
	}

    void Update()
    {
		if (useBuffer == true && audioVisualize.amplitude > 0)
		{
			transform.localScale = new Vector3((audioVisualize.amplitudeBuffer * scaleMultiplier) + startScale, (audioVisualize.amplitudeBuffer * scaleMultiplier) + startScale, (audioVisualize.amplitudeBuffer * scaleMultiplier) + startScale);
			if (useColorChange == true)
			{
				Color color = new Color(red * audioVisualize.amplitudeBuffer, green * audioVisualize.amplitudeBuffer, blue * audioVisualize.amplitudeBuffer);
				material.SetColor("_EmissionColor", color);
			}
		}

		if (useBuffer == false && audioVisualize.amplitude > 0)
		{
			transform.localScale = new Vector3((audioVisualize.amplitude * scaleMultiplier) + startScale, (audioVisualize.amplitude * scaleMultiplier) + startScale, (audioVisualize.amplitude * scaleMultiplier) + startScale);
			if (useColorChange == true)
			{
				Color color = new Color(red * audioVisualize.amplitude, green * audioVisualize.amplitude, blue * audioVisualize.amplitude);
				material.SetColor("_EmissionColor", color);
			}
		}

	}
}
