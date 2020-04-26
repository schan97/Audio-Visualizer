using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
	public GameObject cubePrefab;
	GameObject[] cubeArray = new GameObject[512];
	public float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cubeArray.Length; i++)
		{
			GameObject instanceCubeArray = (GameObject)Instantiate(cubePrefab);
			instanceCubeArray.transform.position = this.transform.position;
			instanceCubeArray.transform.parent = this.transform;
			instanceCubeArray.name = "Cube" + i;
			this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
			instanceCubeArray.transform.position = Vector3.forward * 100;
			cubeArray[i] = instanceCubeArray;
		}
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < cubeArray.Length; i++)
		{
			if(cubeArray != null)
			{
				cubeArray[i].transform.localScale = new Vector3(10, (AudioVisualize.samples[i] * maxScale) + 2, 10);
			}
		}
    }
}
