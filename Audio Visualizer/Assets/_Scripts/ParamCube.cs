using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
	public int band;
	public float startScale, scaleMultiplier;
	public bool useBuffer;
	Material material;

    void Start()
    {

    }


    void Update()
    {
		if (useBuffer == true && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
		}

		if (useBuffer == false && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
		}
		
    }
}
