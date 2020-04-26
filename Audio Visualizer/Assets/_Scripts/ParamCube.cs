using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
	public int band;
	public float startScale, scaleMultiplier;
	public bool useBuffer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(useBuffer == true)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.bandBuffer[band] * scaleMultiplier) + startScale, transform.localScale.z);
		}

		if (useBuffer == false)
		{
			transform.localScale = new Vector3(transform.localScale.x, (AudioVisualize.freqBands[band] * scaleMultiplier) + startScale, transform.localScale.z);
		}
		
    }
}
