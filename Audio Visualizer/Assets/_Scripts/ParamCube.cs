using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
	public AudioVisualize audioVisualize;
	public int band;
	public float startScale, scaleMultiplier;
	public bool useBuffer;

	public bool useColorChange;
	Material material;

    void Start()
    {
		if(useColorChange == true)
		{
			material = GetComponent<MeshRenderer>().materials[0];
		}
    }


    void Update()
    {
		if (useBuffer == true && audioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (audioVisualize.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
			if (useColorChange == true)
			{
				Color color = new Color(audioVisualize.audioBandBuffer[band], audioVisualize.audioBandBuffer[band], audioVisualize.audioBandBuffer[band]);
				material.SetColor("_EmissionColor", color);
			}
		}

		if (useBuffer == false && audioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (audioVisualize.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
			if (useColorChange == true)
			{
				Color color = new Color(audioVisualize.audioBand[band], audioVisualize.audioBand[band], audioVisualize.audioBand[band]);
				material.SetColor("_EmissionColor", color);
			}
		}
    }
}
