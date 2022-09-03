using Assets.Scripts.Enum;
using UnityEngine;

public class TerrainSound : MonoBehaviour
{
    [SerializeField]
    private TerrainType terrainType;

    [SerializeField]
    private AudioClip[] terrainFootstepClips;

    private Footsteps playerFootsteps;

    private void Awake()
    {
        playerFootsteps = GameObject.FindGameObjectWithTag("Player").GetComponent<Footsteps>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerFootsteps.TerrainFootstepClips = terrainFootstepClips;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerFootsteps.TerrainFootstepClips = null;
        }
    }
}