using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField]
    private GameObject leftPrefab;

    [SerializeField]
    private GameObject rightPrefab;

    [SerializeField]
    private GameObject soundBulletPrefab;

    [SerializeField]
    private int footstepPoolInitialSize = 10;

    [SerializeField]
    private int footstepPoolGrowthSize = 5;

    [SerializeField]
    private float footstepDistance = 0.6f;

    [SerializeField]
    private float footSpacing = 0.1f;

    [SerializeField]
    private float fadeSpeed = 1f;

    [SerializeField]
    private float soundBulletSpeed = 3;

    [SerializeField]
    private AudioClip[] footstepClips;

    [HideInInspector]
    public bool playerWalking;

    private Vector3 lastFootprint;
    private FootType lastFoot;

    private AudioSource footstepsAudioSource;
    private Queue<GameObject> availableRightFootstepDecals;
    private Queue<GameObject> availableLeftFootstepDecals;
    private LinkedList<Footstep> activeFootstepDecals;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        footstepsAudioSource = GetComponent<AudioSource>();
        availableRightFootstepDecals = new Queue<GameObject>();
        availableLeftFootstepDecals = new Queue<GameObject>();
        activeFootstepDecals = new LinkedList<Footstep>();
        playerMovement = GetComponent<PlayerMovement>();

        growPools(footstepPoolInitialSize);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastFootprint = leftPrefab.transform.position;
    }

    private void Update()
    {
        FadeOutActiveFootSteps();
    }

    private int footstepIdx = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerMovement.PlayerIsWalking)
        {
            float distanceSinceLastFootprint = Vector3.Distance(lastFootprint, transform.position);
            if (distanceSinceLastFootprint >= footstepDistance)
            {
                // Expand pool if necessary.
                if (availableLeftFootstepDecals.Count == 0 || availableRightFootstepDecals.Count == 0)
                {
                    growPools(footstepPoolGrowthSize);
                }

                // Spawn decals.
                Transform decalTransform = null;
                if (lastFoot == FootType.LEFT)
                {
                    decalTransform = SpawnDecal(FootType.RIGHT, -footSpacing);
                    lastFoot = FootType.RIGHT;
                }
                else if (lastFoot == FootType.RIGHT)
                {
                    decalTransform = SpawnDecal(FootType.LEFT, footSpacing);
                    lastFoot = FootType.LEFT;
                }
                lastFootprint = transform.position;
                SoundManager.Instance.SpawnSound(decalTransform.position, 50, soundBulletSpeed, 2f, spawnedBy: gameObject);

                // Play sound.
                float vol = Random.Range(0.3f, 1f);
                footstepsAudioSource.PlayOneShot(footstepClips[footstepIdx], vol);
                footstepIdx = (footstepIdx + 1) % footstepClips.Length;
            }
        }
    }

    private Transform SpawnDecal(FootType footType, float offset)
    {
        GameObject decal = footType == FootType.LEFT
            ? availableLeftFootstepDecals.Dequeue()
            : availableRightFootstepDecals.Dequeue();
        decal.transform.SetPositionAndRotation(transform.position, transform.rotation);
        decal.transform.Rotate(0, 0, -90f);
        decal.transform.Translate(0, offset, 0, transform);
        decal.SetActive(true);
        activeFootstepDecals.AddLast(new Footstep()
        {
            decal = decal,
            footType = footType
        });
        return decal.transform;
    }

    private void FadeOutActiveFootSteps()
    {
        LinkedListNode<Footstep> node = activeFootstepDecals.First;

        while (node != null)
        {
            var activeFootstepDecal = node.Value.decal;
            var activeFootType = node.Value.footType;

            // Fade out footstep decal.
            Renderer renderer = activeFootstepDecal.GetComponent<Renderer>();
            Color color = Color.Lerp(renderer.material.color, Color.clear, fadeSpeed * Time.deltaTime);
            renderer.material.color = color;

            if (renderer.material.color.a < 0.1f)
            {
                // Reset and re-add to pool.
                activeFootstepDecal.SetActive(false);
                renderer.material.color = Color.white;
                if (activeFootType == FootType.LEFT)
                {
                    availableLeftFootstepDecals.Enqueue(activeFootstepDecal);
                } 
                else if (activeFootType == FootType.RIGHT)
                {
                    availableRightFootstepDecals.Enqueue(activeFootstepDecal);
                }
                activeFootstepDecals.Remove(node);
            }

            node = node.Next;
        }
    }

    private void growPools(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject leftDecal = Instantiate(leftPrefab);
            GameObject rightDecal = Instantiate(rightPrefab);
            leftDecal.SetActive(false);
            rightDecal.SetActive(false);
            availableLeftFootstepDecals.Enqueue(leftDecal);
            availableRightFootstepDecals.Enqueue(rightDecal);
        }
    }
}

public enum FootType
{
    LEFT,
    RIGHT
}

public class Footstep
{
    public FootType footType;
    public GameObject decal;
}