using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.Pool;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] TerrainFootstepClips { get; set; }

    [SerializeField]
    private Footstep footstepPrefab;

    [SerializeField]
    private float footstepDistance = 0.6f;

    [SerializeField]
    private float footSpacing = 0.1f;

    [SerializeField]
    private float fadeSpeed = 1f;

    [SerializeField]
    private float soundBulletSpeed = 3;

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
            if (distanceSinceLastFootprint >= footstepDistance)
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
        audioSource.PlayOneShot(footstepClips[footstepIdx], vol);
        SoundManager.Instance.SpawnSound(spawnAt, 50, soundBulletSpeed, 2f, spawnedBy: gameObject);
    }
}