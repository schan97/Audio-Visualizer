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
	public static float[] samplesLeft = new float[512];
	public static float[] samplesRight = new float[512];

	public static float[] freqBands = new float[8];
	public static float[] bandBuffer = new float[8];
	float[] bufferDecrease = new float[8];
	float[] freqBandHighest = new float[8];

	public static float[] freqBands64 = new float[64];
	public static float[] bandBuffer64 = new float[64];
	float[] bufferDecrease64 = new float[64];
	float[] freqBandHighest64 = new float[64];

	public static float[] audioBand = new float[8];
	public static float[] audioBandBuffer = new float[8];

	public static float[] audioBand64 = new float[64];
	public static float[] audioBandBuffer64 = new float[64];

	public static float amplitude, amplitudeBuffer;
	float amplitudeHighest;

	public static float amplitude64, amplitudeBuffer64;
	float amplitudeHighest64;

	public float audioProfileVal;

	public enum _channel { Stereo, Left, Right};
	public _channel channel = new _channel();


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
		MakeFrequencyBands64();

		BandBuffer();
		BandBuffer64();

		CreateAudioBand();
		CreateAudioBand64();

		GetAmplitude();
		GetAmplitude64();

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

	void GetAmplitude64()
	{
		float currentAmp64 = 0;
		float currentAmpBuffer64 = 0;
		for (int i = 0; i < audioBand64.Length; i++)
		{
			currentAmp64 += audioBand64[i];
			currentAmpBuffer64 += audioBandBuffer64[i];
		}

		if (currentAmp64 > amplitudeHighest64)
		{
			amplitudeHighest64 = currentAmp64;
		}

		amplitude64 = currentAmp64 / amplitudeHighest64;
		amplitudeBuffer64 = currentAmpBuffer64 / amplitudeHighest64;
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

	void CreateAudioBand64()
	{
		for (int i = 0; i < audioBand64.Length; i++)
		{
			if (freqBands64[i] > freqBandHighest64[i])
			{
				freqBandHighest64[i] = freqBands64[i];
			}

			audioBand64[i] = (freqBands64[i] / freqBandHighest64[i]);
			audioBandBuffer64[i] = (bandBuffer64[i] / freqBandHighest64[i]);
		}
	}

	void GetSpectrumAudioSource()
	{
		audioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
		audioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
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

	void BandBuffer64()
	{
		for (int i = 0; i < bandBuffer64.Length; ++i)
		{
			if (freqBands64[i] > bandBuffer64[i])
			{
				bandBuffer64[i] = freqBands64[i];
				bufferDecrease64[i] = 0.005f;
			}

			if (freqBands64[i] < bandBuffer64[i])
			{
				bufferDecrease64[i] = (bandBuffer64[i] - freqBands64[i]) / 8;
				bandBuffer64[i] -= bufferDecrease64[i];

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
				if (channel == _channel.Stereo)
				{
					avg += (samplesLeft[count] + samplesRight[count]) * (count + 1);
				}

				if (channel == _channel.Left)
				{
					avg += (samplesLeft[count]) * (count + 1);
				}

				if (channel == _channel.Right)
				{
					avg += (samplesRight[count]) * (count + 1);
				}
				
				count++;
			}

			avg /= count;

			freqBands[i] = avg * 10;
		}
	}

	/*
	 *	0-15 = 1 sample : 16
	 *	16-32 = 2 samples : 32
	 *	32-39 = 4 samples : 32
	 *	40-47 = 8 samples : 48
	 *	48-55 = 16 samples : 128
	 *	56-63 = 32 samples : 256
	 *				 total : 512
	 */

	void MakeFrequencyBands64()
	{
		int count = 0;
		int sampleCount = 1;
		int power = 0;

		for (int i = 0; i < 64; i++)
		{
			float avg = 0;

			if (i == 16 || i == 32 || i == 48 || i == 56)
			{
				power++;
				sampleCount = (int)Mathf.Pow(2, power);
				if(power == 3)
				{
					sampleCount -= 2;
				}
			}

			for (int j = 0; j < sampleCount; j++)
			{
				if (channel == _channel.Stereo)
				{
					avg += (samplesLeft[count] + samplesRight[count]) * (count + 1);
				}

				if (channel == _channel.Left)
				{
					avg += (samplesLeft[count]) * (count + 1);
				}

				if (channel == _channel.Right)
				{
					avg += (samplesRight[count]) * (count + 1);
				}

				count++;
			}

			avg /= count;

			freqBands64[i] = avg * 80;
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
