using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate316Cubes : MonoBehaviour
{
	public AudioVisualize audioVisualize;
	public GameObject cubePrefab;
	GameObject[] cubeArray = new GameObject[316];
	public float maxScale;
	void Start()
	{
		for (int i = 0; i < cubeArray.Length; i++)
		{
			GameObject instanceCubeArray = (GameObject)Instantiate(cubePrefab);
			instanceCubeArray.transform.position = this.transform.position;
			instanceCubeArray.transform.parent = this.transform;
			instanceCubeArray.name = "Cube" + i;
			this.transform.eulerAngles = new Vector3(0, -1.139240506329114f * i, 0);
			instanceCubeArray.transform.position = Vector3.forward * 100;
			cubeArray[i] = instanceCubeArray;
		}
	}


	void Update()
	{
		for (int i = 0; i < cubeArray.Length; i++)
		{
			if (cubeArray != null)
			{
				cubeArray[i].transform.localScale = new Vector3(10, ((audioVisualize.samplesLeft[i] + audioVisualize.samplesRight[i]) * maxScale) + 2, 10);
			}
		}
	}
}
