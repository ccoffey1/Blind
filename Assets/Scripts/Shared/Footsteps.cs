using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.Pool;

public class Footsteps : MonoBehaviour
{
    public FootstepType FootstepType = FootstepType.Walking;

    public AudioClip[] TerrainFootstepClips { get; set; }

    [SerializeField]
    private Footstep footstepPrefab;

    [Header("Footstep Distances")]
    [SerializeField]
    private float tipToeFootstepDistance = 0.3f;
    [SerializeField]
    private float walkFootstepDistance = 0.6f;
    [SerializeField]
    private float sprintFootstepDistance = 1.2f;
    [Space(5)]

    [Header("Bullet Counts")]
    [SerializeField]
    private int tipToeBulletCount = 25;
    [SerializeField]
    private int walkBulletCount = 50;
    [SerializeField]
    private int sprintBulletCount = 70;
    [Space(20)]

    [Header("Bullet Speeds")]
    [SerializeField]
    private float tipToeBulletSpeed = 2f;
    [SerializeField]
    private float walkBulletSpeed = 3f;
    [SerializeField]
    private float sprintBulletSpeed = 6f;
    [Space(20)]

    [Header("Bullet Fade Factor")]
    [SerializeField]
    private float tipToeBulletFadeFactor = 3f;
    [SerializeField]
    private float walkBulletFadeFactor = 2f;
    [SerializeField]
    private float sprintBulletFadeFactor = 1f;
    [Space(5)]

    [SerializeField]
    private float footSpacing = 0.1f;

    [SerializeField]
    private float fadeSpeed = 1f;

    [Header("Footstep Volumes")]
    [SerializeField, Range(0, 1f)]
    private float tipToeFootstepVolume = 0.2f;
    [SerializeField, Range(0, 1f)]
    private float walkFootstepVolume = 1f;
    [SerializeField, Range(0, 1f)]
    private float sprintFootstepVolume = 1f;
    [Space(5)]

    [SerializeField]
    private AudioClip[] defaultFootstepClips;

    public bool Active { get; set; }

    private Vector3 positionDuringLastFootprint;
    private FootType lastFoot;
    private ObjectPool<Footstep> pool;
    private AudioSource audioSource;

    private void Awake()
    {
        pool = new ObjectPool<Footstep>(CreateFootstep, OnGetFootstepFromPool, OnReturnFootstepToPool);
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        positionDuringLastFootprint = transform.position;
        lastFoot = FootType.LEFT;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Active)
        {
            float distanceSinceLastFootprint = Vector3.Distance(positionDuringLastFootprint, transform.position);
            if (distanceSinceLastFootprint >= GetFootstepDistance())
            {
                // Spawn decals.
                Vector2 footstepPosition;
                Footstep footstep = pool.Get();

                if (lastFoot == FootType.LEFT)
                {
                    footstepPosition = footstep.Spawn(transform, FootType.RIGHT, fadeSpeed, -footSpacing);
                    lastFoot = FootType.RIGHT;
                }
                else
                {
                    footstepPosition = footstep.Spawn(transform, FootType.LEFT, fadeSpeed, footSpacing);
                    lastFoot = FootType.LEFT;
                }

                positionDuringLastFootprint = transform.position;
                GenerateFootstepSound(footstepPosition);
            }
        }
    }

    private Footstep CreateFootstep()
    {
        Footstep footstep = Instantiate(footstepPrefab);
        footstep.Pool = pool;
        return footstep;
    }

    private void OnGetFootstepFromPool(Footstep footstep)
    {
        footstep.gameObject.SetActive(true);
    }

    private void OnReturnFootstepToPool(Footstep footstep)
    {
        footstep.gameObject.SetActive(false);
    }

    private int footstepIdx = 0;

    private void GenerateFootstepSound(Vector2 spawnAt)
    {
        AudioClip[] footstepClips = TerrainFootstepClips ?? defaultFootstepClips;
        float vol = Random.Range(0.3f, 1f);
        footstepIdx = (footstepIdx + 1) % footstepClips.Length;
        audioSource.volume = GetFootstepVolume();
        audioSource.PlayOneShot(footstepClips[footstepIdx], vol);
        SoundManager.Instance.SpawnSound(
            spawnAt, 
            GetBulletCount(), 
            GetBulletSpeed(), 
            GetBulletFadeTime(),
            GetBulletLinearDrag(),
            spawnedBy: gameObject);
    }

    private float GetFootstepDistance()
    {
        return FootstepType == FootstepType.TipToe
            ? tipToeFootstepDistance
                : FootstepType == FootstepType.Walking
                    ? walkFootstepDistance
                        : FootstepType == FootstepType.Sprinting
                            ? sprintFootstepDistance
                                : 0f;
    }

    private float GetFootstepVolume()
    {
        return FootstepType == FootstepType.TipToe
            ? tipToeFootstepVolume
                : FootstepType == FootstepType.Walking
                    ? walkFootstepVolume
                        : FootstepType == FootstepType.Sprinting
                            ? sprintFootstepVolume
                                : 0f;
    }

    private float GetBulletSpeed()
    {
        return FootstepType == FootstepType.TipToe
            ? tipToeBulletSpeed
                : FootstepType == FootstepType.Walking
                    ? walkBulletSpeed
                        : FootstepType == FootstepType.Sprinting
                            ? sprintBulletSpeed
                                : 0f;
    }

    private float GetBulletFadeTime()
    {
        return FootstepType == FootstepType.TipToe
            ? tipToeBulletFadeFactor
                : FootstepType == FootstepType.Walking
                    ? walkBulletFadeFactor
                        : FootstepType == FootstepType.Sprinting
                            ? sprintBulletFadeFactor
                                : 0f;
    }

    private int GetBulletCount()
    {
        return FootstepType == FootstepType.TipToe
            ? tipToeBulletCount
                : FootstepType == FootstepType.Walking
                    ? walkBulletCount
                        : FootstepType == FootstepType.Sprinting
                            ? sprintBulletCount
                                : 0;
    }

    private float GetBulletLinearDrag()
    {
        return FootstepType == FootstepType.TipToe
            ? 4f
                : FootstepType == FootstepType.Walking
                    ? 2f
                        : FootstepType == FootstepType.Sprinting
                            ? 1f
                                : 0;
    }
}