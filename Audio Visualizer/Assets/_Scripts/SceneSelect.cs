﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
	public void selectDemo()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void selectXMas()
	{
		SceneManager.LoadScene("XMas");
	}

	public void selectInTheClouds()
	{
		SceneManager.LoadScene("InTheClouds");
	}
}
