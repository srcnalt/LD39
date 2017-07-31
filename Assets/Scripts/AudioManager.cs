using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource effectSource;

    public AudioItem[] audioItems;

	void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string name)
    {
        AudioItem ai = getAudio(name);

        effectSource.clip = ai.audio;
        effectSource.loop = ai.loop;
        effectSource.Play();
    }

    public void PlayMusic(string name)
    {
        AudioItem ai = getAudio(name);

        musicSource.clip = ai.audio;
        musicSource.loop = ai.loop;
        musicSource.Play();
    }

    private AudioItem getAudio(string name)
    {
        foreach (AudioItem ai in audioItems)
        {
            if (ai.name == name)
            {
                return ai;
            }
        }

        Debug.LogError("Audio named '" + name + "' cannot be found!");
        return null;
    }
}
