using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube64 : MonoBehaviour
{
	public AudioVisualize audioVisualize;
	public int band;
	public float startScale, scaleMultiplier;
	public bool useBuffer64;

	public bool useColorChange;
	Material material;

	void Start()
	{
		if (useColorChange == true)
		{
			material = GetComponent<MeshRenderer>().materials[0];
		}
	}


	void Update()
	{
		if (useBuffer64 == true && audioVisualize.audioBand64[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (audioVisualize.audioBandBuffer64[band] * scaleMultiplier) + startScale, transform.localScale.z);
			if (useColorChange == true)
			{
				Color color = new Color(audioVisualize.audioBandBuffer64[band], audioVisualize.audioBandBuffer64[band], audioVisualize.audioBandBuffer64[band]);
				material.SetColor("_EmissionColor", color);
			}
		}

		if (useBuffer64 == false && audioVisualize.audioBand64[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (audioVisualize.audioBand64[band] * scaleMultiplier) + startScale, transform.localScale.z);
			if (useColorChange == true)
			{
				Color color = new Color(audioVisualize.audioBand64[band], audioVisualize.audioBand64[band], audioVisualize.audioBand64[band]);
				material.SetColor("_EmissionColor", color);
			}
		}
	}
}
