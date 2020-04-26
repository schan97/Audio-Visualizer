using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using NAudio;
using NAudio.Wave;
using SimpleFileBrowser;

[RequireComponent (typeof (AudioSource))]

public class AudioVisualize : MonoBehaviour
{
	AudioSource audioSource;
	public static float[] samples = new float[512];

	string path;

	public Text musicTitle;
	public Text clipTime;

	string musicFileName;

	private int fullLength;
	private int playTime;
	private int seconds;
	private int minutes;

	private bool canJump = false;

    // Start is called before the first frame update
    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		musicFileName = "Song Name";
    }

    // Update is called once per frame
    void Update()
    {
		GetSpectrumAudioSource();
		ShowPlayTime();
		ShowCurrentTitle();
	}

	void GetSpectrumAudioSource()
	{
		audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	}

	void OpenMp3File()
	{
		FileBrowser.SetFilters(false, new FileBrowser.Filter("Open MP3 File", ".mp3"));
		FileBrowser.SetDefaultFilter(".mp3");
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load MP3", "Select");
		path = FileBrowser.Result;

		if(FileBrowser.Success)
		{
			byte[] audioFile = FileBrowserHelpers.ReadBytesFromFile(path);
			musicFileName = FileBrowserHelpers.GetFilename(path);
			audioSource.clip = NAudioPlayer.FromMp3Data(audioFile);
			fullLength = (int)audioSource.clip.length;

			canJump = true;
			audioSource.time = 0;
		}
	}

	void ShowCurrentTitle()
	{
		musicTitle.text = musicFileName;	
	}

	void ShowPlayTime()
	{
		playTime = (int)audioSource.time;
		seconds = playTime % 60;
		minutes = (playTime / 60) % 60;
		clipTime.text = minutes + ":" + seconds.ToString("D2") + "/" + ((fullLength / 60) % 60) + ":" + (fullLength % 60).ToString("D2");
	}

	public void JumpAhead()
	{
		if(canJump != false)
		{
			if (audioSource.time + 5 < audioSource.clip.length)
			{
				audioSource.time += 5;
			}

			else
			{
				audioSource.time = 0;
			}
		}
	}

	public void JumpBack()
	{
		if (canJump != false)
		{
			if (audioSource.time - 5 > 0)
			{
				audioSource.time -= 5;
			}

			else
			{
				audioSource.time = 0;
			}
		}
	}


}
