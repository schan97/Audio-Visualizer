using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (canvasGroup.alpha == 1)
			{
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
				canvasGroup.alpha = 0; 
			}

			else if (canvasGroup.alpha == 0)
			{
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
				canvasGroup.alpha = 1;
			}
		}
	}
}
