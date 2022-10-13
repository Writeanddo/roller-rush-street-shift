using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioData))]

public class AudioManager : MonoBehaviour
{
    // 0 source : musique
    // 1 source : ambiance
    // 2 source : player 1 - loop player
    // 3 source : player 1 - effets
    // 4 source : player 1 - effets
    // 5 source : player 2 - loop player
    // 6 source : player 2 - effets
    // 7 source : player 2 - effets
    // 8 source : player 3 - loop player
    // 9 source : player 3 - effets
    // 10source : player 3 - effets
    // 11source : player 4 - loop player
    // 12source : player 4 - effets
    // 13source : player 4 - effets

    [SerializeField] private AudioMixer audioMixer;
    private AudioSource[] source;
    private AudioData data;
    private List<PlayerData> playersData = new List<PlayerData>();

    private Dictionary<int, List<AudioClip>> saveAudioOneTime = new Dictionary<int, List<AudioClip>>();

    private bool beginMusicPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponents<AudioSource>();
        data = GetComponent<AudioData>();

        // Config playerData
        foreach (GameObject player in GameObject.Find("GameManager").GetComponent<GameManager>().players)
            playersData.Add(player.GetComponentInChildren<PlayerData>());

        // Config saveAudio
        for(int i = 0; i < playersData.Count; i++)
            saveAudioOneTime.Add(i, new List<AudioClip>());


        ConfigSource();        
    }

    private void ConfigSource()
    {
        // music
        source[0].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        source[1].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Ambiance")[0];

        // player 1
        source[2].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Player_Loop")[0];
        source[3].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];
        source[4].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];

        // player 2
        source[5].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Player_Loop")[0];
        source[6].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];
        source[7].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];

        // player 3
        source[8].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Player_Loop")[0];
        source[9].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];
        source[10].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];

        // player 4
        source[11].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Player_Loop")[0];
        source[12].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];
        source[13].outputAudioMixerGroup = audioMixer.FindMatchingGroups("Effets")[0];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        AudioMusic();
        AudioPlayer();
    }

    private void AudioMusic()
    {
        if (source[0].clip == data.firstMusicLevel && !source[0].isPlaying)
            beginMusicPlayed = true;

        if(beginMusicPlayed)
            PlayMusic(0, data.musicLevel);
        else
            PlayMusic(0, data.firstMusicLevel);

        PlayMusic(1, data.ambianceLoop);
    }

    private void AudioPlayer()
    {
        PlayerLoop();
        PlayerEffets();
    }

    private void PlayerLoop()
    {
        int indexLoop   = 2;

        foreach (PlayerData playerData in playersData)
        {
            if (playerData.speed > 1.5f && playerData.onGround)
                PlayAudio(indexLoop, data.rollLoop);                                                        // roll
            else if (playerData.doFigure)
                PlayAudio(indexLoop, data.playerTricks);                                                    // playerTricks
            else if (!playerData.onGround)
                PlayAudio(indexLoop, data.jumpWindLoop);                                                    // jumpWindLoop
            else
                StopPlay(indexLoop);

            indexLoop   += 3;
        }
    }

    private void PlayerEffets()
    {
        int indexEffets = 3;
        int indexEffets2 = 4;
        int playerId = 0;

        foreach (PlayerData playerData in playersData)
        {
            bool land = false;
            if (playerData.onGround && !playerData.lastOnGround)
                land = true;

            if (playerData.crunching)
                PlayAudio(indexEffets, data.playerSlide);                                                   // playerSlide
            else if (playerData.jump)
                PlayAudio(indexEffets, data.jumpTrigger);                                                   // jumpTrigger
            else if (playerData.doingTackle)
                PlayAudio(indexEffets, data.playerTackle);                                                  // playerTackle
            else if (playerData.speed > 1f && playerData.onGround)
            {
                PlayAudio(indexEffets, data.roller);                                                        // roller
            }
            else if (playerData.isShifting)
                PlayAudio(indexEffets, data.playerSwitch);                                                  // playerSwitch
            else if (playerData.speed > 1f && !playerData.onGround)
                PlayAudio(indexEffets, data.jumpWheelsLoop);                                                // jumpWheelsLoop
            else
                StopPlay(indexEffets);

            // Effect 2
            if (playerData.isBoosted)
                PlayAudio(indexEffets2, data.speedBoost, playerId, true);                                   // speedBoost
            else if (playerData.doFigure)
            {
                PlayAudio(indexEffets2, data.trickGauge);                                                    // trickGauge
                //Debug.Break();
            }
            else if (playerData.isBumped)
                PlayAudio(indexEffets2, data.bumper, playerId, true);                                       // bumper
            else if (playerData.successFigure)
                PlayAudio(indexEffets2, data.playerSuccess, playerId, true);                                // playerSuccess
            else if (playerData.failFigure)
                PlayAudio(indexEffets2, data.playerFail, playerId, true);                                   // playerFail
            else if (land)
                PlayAudioLand(indexEffets2, data.land, out land);                                          // land
            else
                StopPlay(indexEffets2);

            indexEffets += 3;
            indexEffets2 += 3;
            playerId++;
        }
    }

    private void PlayAudio(int indexAudioSource, AudioClip audioClip, int playerId = 0, bool playOneTime = false)
    {
        if (!audioClip || source[indexAudioSource].clip == audioClip || AlreadyPlayed(playerId, audioClip))
            return;

        source[indexAudioSource].clip = audioClip;
        source[indexAudioSource].Play();

        if (playOneTime && !AlreadyPlayed(playerId, audioClip))
            StartCoroutine(SaveSoundOneTime(playerId, audioClip));

        if(!source[indexAudioSource].isPlaying)
            source[indexAudioSource].clip = null;
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

    private void PlayAudio(int indexAudioSource, AudioClip[] audioClips, int playerId = 0, bool playOneTime = false)
    {
        bool sameAudio = false;

        if (!audioClips[0])
            return;

        foreach(AudioClip audioClip in audioClips)
        {
            if (source[indexAudioSource].clip == audioClip)
            {
                sameAudio = true;
                return;
            }
        }

        AudioClip audioPlay = audioClips[(int)Random.Range(0f, audioClips.Length)];

        if (playOneTime)
            Debug.Log(AlreadyPlayed(playerId, audioPlay));

        if (sameAudio || source[indexAudioSource].isPlaying || AlreadyPlayed(playerId, audioPlay))
            return;

        source[indexAudioSource].clip = audioPlay;
        source[indexAudioSource].Play();

        if (playOneTime && !AlreadyPlayed(playerId, audioPlay))
            StartCoroutine(SaveSoundOneTime(playerId, audioPlay));

        if (!source[indexAudioSource].isPlaying)
            source[indexAudioSource].clip = null;
    }

    private void PlayAudioLand(int indexAudioSource, AudioClip[] audioClips, out bool land)
    {
        bool sameAudio = false;
        land = true;

        if (!audioClips[0])
            return;

        foreach (AudioClip audioClip in audioClips)
        {
            if (source[indexAudioSource].clip == audioClip)
            {
                sameAudio = true;
                return;
            }
        }

        AudioClip audioPlay = audioClips[(int)Random.Range(0f, audioClips.Length)];

        if (sameAudio || source[indexAudioSource].isPlaying)
            return;

        source[indexAudioSource].clip = audioPlay;
        source[indexAudioSource].Play();

        if (!source[indexAudioSource].isPlaying)
        {
            land = false;
            source[indexAudioSource].clip = null;
        }
    }

    private void PlayAudioRoller(int indexAudioSource, AudioClip[] audioClips, PlayerData playerData)
    {
        bool sameAudio = false;

        if (!audioClips[0])
            return;

        AudioClip audioPlay = audioClips[(int)Random.Range(0f, audioClips.Length)];

        if (sameAudio || source[indexAudioSource].isPlaying)
            return;

        source[indexAudioSource].clip = audioPlay;
        source[indexAudioSource].Play();

        playerData.leftSkateWasPlay = !playerData.leftSkateWasPlay;

        if (!source[indexAudioSource].isPlaying)
            source[indexAudioSource].clip = null;
    }

    private bool AlreadyPlayed(int idPlayer, AudioClip audioClip)
    {
        if (saveAudioOneTime[idPlayer].Contains(audioClip))
            return true;
        return false;
    }

    private IEnumerator SaveSoundOneTime(int playerId, AudioClip audioClip)
    {
        saveAudioOneTime[playerId].Add(audioClip);
        yield return new WaitForSeconds(.05f);
        saveAudioOneTime[playerId].Remove(audioClip);
    }

    private void StopPlay(int indexAudioSource)
    {
        source[indexAudioSource].Stop();
        source[indexAudioSource].clip = null;
    }
}
