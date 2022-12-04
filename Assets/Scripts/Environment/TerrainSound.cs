using Assets.Scripts.Enum;
using UnityEngine;

public class TerrainSound : MonoBehaviour
{
    [SerializeField]
    private TerrainType terrainType;

    [SerializeField]
    private AudioClip[] terrainFootstepClips;

    private Footsteps playerFootsteps;

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(0, 1f, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

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