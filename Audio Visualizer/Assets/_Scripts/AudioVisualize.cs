using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent (typeof (AudioSource))]

public class AudioVisualize : MonoBehaviour
{
	AudioSource _audioSource;
	public float[] _samples = new float[512];

	string path;

    // Start is called before the first frame update
    void Start()
    {
		_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		GetSpectrumAudioSource();
    }

	void GetSpectrumAudioSource()
	{
		_audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
	}

	public void OpenSingleFile()
	{

		path = EditorUtility.OpenFilePanel("Open Mp3 File", "", "mp3");
		LoadSong();

	}

	public void LoadSong()
	{
		if (path != null)
		{
			UpdateSong();
		}
	}

	public void UpdateSong()
	{
		WWW www = new WWW("file:///" + path);
		_audioSource.clip = NAudioPlayer.FromMp3Data(www.bytes);
	}
}
