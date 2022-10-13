using UnityEngine;

public class AudioData : MonoBehaviour
{
    // Move
    [Header("Movement player")]
    [Tooltip("Roller loop as player moves")] public AudioClip rollLoop;
    [Tooltip("Roller sounds left")]          public AudioClip roller;

    // Jump
    [Header("Jump player")]
    [Tooltip("Sound of player jumping")]    public AudioClip[] jumpTrigger;
    [Tooltip("Wheels in the Air Loop")]     public AudioClip jumpWheelsLoop;
    [Tooltip("Sound of wind when jumping")] public AudioClip jumpWindLoop;
    [Tooltip("player who lands")]           public AudioClip[] land;

    // Crouching
    [Header("Crunching player")]
    [Tooltip("player sliding on the floor")] public AudioClip playerSlide;

    // Switch
    [Header("Switch plan")]
    [Tooltip("player who switches to another plane")] public AudioClip playerSwitch;

    // Figure
    [Header("Figure player")]
    [Tooltip("player doing a trick")]         public AudioClip playerTricks;
    [Tooltip("Tricks yellow progress sound")] public AudioClip[] trickGauge;
    [Tooltip("successful figure")]            public AudioClip playerSuccess;
    [Tooltip("fail figure")]                  public AudioClip playerFail;

    // Tackle
    [Header("Tackle player")]
    [Tooltip("sound when player tackle")] public AudioClip playerTackle;

    // Bumper
    [Header("Bumper")]
    [Tooltip("sound when the player takes a bumper")] public AudioClip bumper;

    // Boost
    [Header("Boost")]
    [Tooltip("sound when the player takes the accelerator")] public AudioClip speedBoost;

    // UI
    [Header("UI")]
    [Tooltip("player navigates menus")] public AudioClip uiSelect;

    // Music
    [Header("Music")]
    [Tooltip("sound level")]   public AudioClip firstMusicLevel;
    [Tooltip("sound level")]   public AudioClip musicLevel;
    [Tooltip("Loop of ambient sounds in the background to dress the levels")] public AudioClip ambianceLoop;
}
