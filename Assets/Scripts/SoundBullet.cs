using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBullet : MonoBehaviour
{
    public Vector2 SpawnedFrom { get; set; }
    public GameObject SpawnedBy { get; set; }
    public float SpawnTime { get; set; }
}
