using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
	public int band;
	public float startScale, scaleMultiplier;
	public bool useBuffer;
	Material material;
    // Start is called before the first frame update
    void Start()
    {
		//material = GetComponent<MeshRenderer>().materials[0];
		//material.EnableKeyword("EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
		if(useBuffer == true && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
			//Color color = new Color(148, (AudioVisualize.bandBuffer[band] -.5f), AudioVisualize.bandBuffer[band] -.5f);
			//material.SetColor("_EmissionColor", color);
		}

		if (useBuffer == false && AudioVisualize.audioBand[band] > 0)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.audioBand[band] * scaleMultiplier) + startScale, transform.localScale.z);
			//Color color = new Color(255, AudioVisualize.freqBands[band], AudioVisualize.freqBands[band]);
			//material.SetColor("_EmissionColor", color);
		}
		
    }
}
