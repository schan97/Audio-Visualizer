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
	public static float[] freqBands = new float[8];
	public static float[] bandBuffer = new float[8];
	float[] bufferDecrease = new float[8];

	float[] freqBandHighest = new float[8];
	public static float[] audioBand = new float[8];
	public static float[] audioBandBuffer = new float[8];


	string path;

	public Text musicTitle;
	public Text clipTime;

	string musicFileName;

	private int fullLength;
	private int playTime;
	private int seconds;
	private int minutes;

	private bool canJump = false;

    void Start()
    {
		audioSource = GetComponent<AudioSource>();
		musicFileName = "Song Name";
    }

    void Update()
    {
		GetSpectrumAudioSource();
		MakeFrequencyBands();
		BandBuffer();
		CreateAudioBand();

		ShowPlayTime();
		ShowCurrentTitle();
	}

	void CreateAudioBand()
	{
		for(int i = 0; i < 8; i++)
		{
			if(freqBands[i] > freqBandHighest[i])
			{
				freqBandHighest[i] = freqBands[i];
			}

			audioBand[i] = (freqBands[i] / freqBandHighest[i]);
			audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
		}
	}


	void GetSpectrumAudioSource()
	{
		audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	}

	void BandBuffer()
	{
		for(int i = 0; i < 8; ++i)
		{
			if (freqBands[i] > bandBuffer[i])
			{
				bandBuffer[i] = freqBands[i];
				bufferDecrease[i] = 0.005f;
			}

			if (freqBands[i] < bandBuffer[i])
			{
				bufferDecrease[i] = (bandBuffer[i] -freqBands[i])/8;
				bandBuffer[i] -= bufferDecrease[i];
				
			}
		}
	}

	void MakeFrequencyBands()
	{
		/*
		 *	22050 / 512 = 43 hertz per sample
		 * 	
		 * 	20-60 hertz
		 * 	60-250 hertz
		 * 	250-500 hertz
		 * 	500-2000 hertz
		 * 	2000-4000 hertz
		 * 	4000-6000 hertz
		 * 	6000-20000 hertz
		 *  
		 *  0 : 2 =	86 hertz (43*2)
		 *  1 : 4 = 172 hertz		range 87-258
		 *  2 : 8 = 344 hertz		259 - 602
		 *  3 : 16 = 688 hertz		603 - 1290
		 *  4 : 32 = 1376 hertz		1291 - 2666
		 *  5 : 64 = 2752 hertz		2667 - 5418
		 *  6 : 128 = 5504 hertz	5419 - 10922
		 *  7 : 256 = 11008 hertz	10923 - 21930
		 * 	510
		 */

		int count = 0;

		for (int i = 0; i < 8; i++)
		{
			float avg = 0;
			int sampleCount = (int)Mathf.Pow(2, i) * 2;

			if (i == 7)
			{
				sampleCount += 2;
			}

			for (int j = 0; j < sampleCount; j++)
			{
				avg += samples[count] * (count + 1);
				count++;
			}

			avg /= count;

			freqBands[i] = avg * 10;
		}
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
