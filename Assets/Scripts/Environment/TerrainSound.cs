using Assets.Scripts.Enum;
using UnityEngine;

public class TerrainSound : MonoBehaviour
{
    [SerializeField]
    private TerrainType terrainType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Player entered terrain of type " + terrainType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Player has left terrain of type " + terrainType);
        }
    }
}