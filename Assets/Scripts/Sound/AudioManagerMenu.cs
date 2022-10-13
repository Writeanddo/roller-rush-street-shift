using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource[] source;
    private AudioData data;
    private bool beginMusicPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponents<AudioSource>();
        data = GetComponent<AudioData>();
        ConfigSource();
    }

    private void ConfigSource()
    {
        source[0].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        source[1].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambiance")[0];
    }

    // Update is called once per frame
    void Update()
    {
        AudioMusic();
    }

    private void AudioMusic()
    {
        if (source[0].clip == data.firstMusicLevel && !source[0].isPlaying)
            beginMusicPlayed = true;

        if (beginMusicPlayed)
            PlayMusic(0, data.musicLevel);
        else
            PlayMusic(0, data.firstMusicLevel);

        PlayMusic(1, data.ambianceLoop);
    }

    private void PlayMusic(int indexAudioSource, AudioClip audioClip)
    {
        if (!audioClip || (source[indexAudioSource].clip == audioClip && source[indexAudioSource].isPlaying))
            return;

        source[indexAudioSource].clip = audioClip;
        source[indexAudioSource].Play();

        if (!source[indexAudioSource].isPlaying)
            source[indexAudioSource].clip = null;
    }
}
