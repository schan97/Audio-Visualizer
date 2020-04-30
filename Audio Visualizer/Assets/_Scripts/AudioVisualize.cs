using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using NAudio;
using NAudio.Wave;
using SimpleFileBrowser;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (AudioSource))]

public class AudioVisualize : MonoBehaviour
{
	AudioSource audioSource;
	public static float[] samples = new float[512];
	public static float[] freqBands = new float[8];
	public static float[] bandBuffer = new float[8];
	float[] bufferDecrease = new float[8];

	public float[] freqBandHighest = new float[8];
	public static float[] audioBand = new float[8];
	public static float[] audioBandBuffer = new float[8];

	public static float amplitude, amplitudeBuffer;
	float amplitudeHighest;

	public float audioProfileVal;


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
		AudioProfile(audioProfileVal);

	}

    void Update()
    {
		GetSpectrumAudioSource();
		MakeFrequencyBands();
		BandBuffer();
		CreateAudioBand();
		GetAmplitude();

		ShowPlayTime();
		ShowCurrentTitle();
	}

	void AudioProfile(float audioProfileVal)
	{
		for(int i = 0; i < freqBandHighest.Length; i++)
		{
			freqBandHighest[i] = audioProfileVal;
		}
	}

	void GetAmplitude()
	{
		float currentAmp = 0;
		float currentAmpBuffer = 0;
		for(int i = 0; i < audioBand.Length; i++)
		{
			currentAmp += audioBand[i];
			currentAmpBuffer += audioBandBuffer[i];
		}

		if(currentAmp > amplitudeHighest)
		{
			amplitudeHighest = currentAmp;
		}

		amplitude = currentAmp / amplitudeHighest;
		amplitudeBuffer = currentAmpBuffer / amplitudeHighest;
	}

	void CreateAudioBand()
	{
		for (int i = 0; i < audioBand.Length; i++)
		{
			if (freqBands[i] > freqBandHighest[i])
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
		for (int i = 0; i < bandBuffer.Length; ++i)
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

	public void OpenMp3File()
	{
		FileBrowser.SetFilters(false, new FileBrowser.Filter("Open MP3 File", ".mp3"));
		FileBrowser.SetDefaultFilter(".mp3");
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load MP3", "Select");
		path = FileBrowser.Result;

		if (FileBrowser.Success)
		{
			byte[] audioFile = FileBrowserHelpers.ReadBytesFromFile(path);
			musicFileName = FileBrowserHelpers.GetFilename(path);
			audioSource.clip = NAudioPlayer.FromMp3Data(audioFile);
			fullLength = (int)audioSource.clip.length;

			canJump = true;
			audioSource.time = 0;
			AudioProfile(audioProfileVal);
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
		if (canJump != false)
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

	public void Quit()
	{
		Application.Quit();
	}

	public void SceneSelect()
	{
		SceneManager.LoadScene("SceneSelect");
	}


}
