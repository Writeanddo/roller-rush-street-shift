using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Mobility on the ground")]
    [Tooltip("How much velocity is added each frame to the player when he's moving on the ground")]
    public float acceleration=25;
    [Tooltip("Maximum horizontal velocity that the player can reach period")]
    public float maxHorizontalSpeed = Mathf.Infinity;
    [Tooltip("Maximum horizontal velocity that the player can reach with the left and right control")]
    public float maxControlHorizontalSpeed = Mathf.Infinity;
    [Tooltip("How hight can the player jump")]
    public float jumpHeight = 40;
    [Tooltip("How hight the player detecte the ground")]
    public float groundDetection = 1.5f;

    [Header("Mobility in the air")]
    [Tooltip("How much velocity is added each frame to the player when he's moving in the air")]
    public float speedAirControl = 10;
    [Tooltip("Gravity applied to the player when he's in the air")]
    public float gravity = 15;
    [Tooltip("Gravity added to the player when he's falling")]
    public float fallingGravity = 10;
    [Tooltip("Gravity added to the player when he's pushing the down button while in the air")]
    public float fastFallingGravity = 10;


    [Header("Crouching")]
    [Tooltip("Attraction created by the ground")]
    public float attractionForce=30;
    [Tooltip("How much velocity is added each frame to the player when he's crouching")]
    public float crouchingSpeed=30;
    [Tooltip("Maximum horizontal velocity that the player can reach while crouched")]
    public float maxCrouchingSpeed = Mathf.Infinity;

    [Header("Figure")]
    [Tooltip("How high the player must be to start a figure")]
    public float startFigureMinGround = 5f;
    [Tooltip("How long does it take to charge a figure")]
    public float figureTime=.5f;
    [Tooltip("Velocity boost added to the player after he performed a figure (cumulative up to 3 times)")]
    public float figureBoostSpeed=25;
    [Tooltip("How long is the player stuck when he fail a figure")]
    public float timeStopByFailFigure = 0.25f;
    [Tooltip("How long is the player animation when he perform a figure")]
    public float timeAnimationSuccessFigure = 2;
    [Tooltip("Color of player when he success a figure")] [ColorUsage(true, false)]
    public Color colorSuccessFigure;
    [Tooltip("Color of player when he failed a figure")] [ColorUsage(true, false)]
    public Color colorFailFigure;

    [Header("Shift")]
    [Tooltip("Duration of the shift")]
    public float transitionTime = 0.15f;
    [Tooltip("Cooldown between two shift (in seconds)")]
    public float shiftCooldown = 0.5f;

    [Header("Camera")]
    [Tooltip("Minimum offset to the player camera (when the player he's standing still)")]
    public Vector3 minCameraOffset = new Vector3(0, 5, -30);
    [Tooltip("Maximum offset to the player camera (when the player he's at max speed)")]
    public Vector3 maxCameraOffset = new Vector3(0, 5, -50);
    [Tooltip("Speed of the rotations")]
    [Range(0.01f, 1f)] public float rotationSpeed= 0.03f;
    
    [Range(0.01f, 1f)] public float smoothSpeed = 0.125f;
    public float swapDirectionTime = 5;


    public Camera camera;

    [Header("Tackle")]
    [Tooltip("When the player drop to this speed or bellow the tackle is cancelled")]
    public float tackleMinimumSpeed = 5f;
    [Tooltip("After this duration the player will start slowing down")]
    public float tackleDurationBeforeSlow = 1f;
    [Tooltip("After this duration the tackle is cancelled")]
    public float tackleMaxDuration = 3f;
    [Tooltip("Cooldown between 2 tackle from the same player")]
    public float tackleCooldown = 3f;
    [Tooltip("Slow added to player when tackleDurationBeforeSlow is exceeded")]
    public float tackleSlow = 80f;
    [Tooltip("How many seconds is a tackled player immune to tackles")]
    public float tackleInvicibilityDuration = 3f;
    [Tooltip("How many percentage the tackled slow the player touched")]
    public float percentageSlowedByTackle = 50f;

    [Header("Rules")]
    public float respawnHeight = -50f;

    [Header("Vibration")]
    [Tooltip("Duration in seconde")]
    public float vibrationDuration = 2f;
    [Tooltip("Intencity of the vibration in the start")]
    public float vibrationLowFrequence = 0.123f;
    [Tooltip("Intencity of the vibration in the end")]
    public float vibrationHighFrequence = 0.234f;

    #region privateData

    // player camera
    [HideInInspector] public Vector3 currentCameraOffset;
    [HideInInspector] public float currentCameraRotation;

    // other players
    [HideInInspector] public int totalPlayerCount;

    // end
    [HideInInspector] public bool raceFinished;

    // player movememt
    [HideInInspector] public bool onGround = false;
    [HideInInspector] public float angle;
    [HideInInspector] public bool goOnRight;
    [HideInInspector] public bool noSpeedLimit;

    // player figure
    [HideInInspector] public bool failFigure = false;
    [HideInInspector] public bool doFigure;

    // player tackle
    [HideInInspector] public bool doingTackle = false;
    [HideInInspector] public bool isTackleImmune = false;
    [HideInInspector] public bool isTackleVibration = false;

    // switch plan
    [HideInInspector] public bool hitWallSwitchPlan = false;

    // menu
    [HideInInspector] public GameObject pauseMenu;

    // rank
    [HideInInspector] public float progressionInTheLevel;
    [HideInInspector] public int rank;

    // timer 
    [HideInInspector] public float timer;
    // Audio
    [HideInInspector] public float speed;
    [HideInInspector] public bool lastOnGround;
    [HideInInspector] public bool jump;
    [HideInInspector] public bool crunching;
    [HideInInspector] public bool isShifting;
    [HideInInspector] public bool isBumped;
    [HideInInspector] public bool isBoosted;
    [HideInInspector] public bool successFigure;
    [HideInInspector] public bool leftSkateWasPlay;

    #endregion
}
