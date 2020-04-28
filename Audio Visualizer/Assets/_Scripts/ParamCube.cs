using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
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
		if (useBuffer == true && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
			if (useColorChange == true)
			{
				Color color = new Color(AudioVisualize.audioBandBuffer[band], AudioVisualize.audioBandBuffer[band], AudioVisualize.audioBandBuffer[band]);
				material.SetColor("_EmissionColor", color);
			}
		}

		if (useBuffer == false && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
		}
		
    }
}
