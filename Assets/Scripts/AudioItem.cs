using UnityEngine;

[System.Serializable]
public class AudioItem {
    public string name;
    public AudioClip audio;
    public bool loop;

    public AudioItem (string _name, AudioClip _audio, bool _loop)
    {
        name = _name;
        audio = _audio;
        loop = _loop;
    }
}
