using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [SerializeField]
    private bool isRinging = false;
    private AudioSource ringingSound;

    void Start()
    {
        ringingSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isRinging)
        {
            if (!ringingSound.isPlaying)
            {
                ringingSound.Play();
                StartCoroutine(SpawnSound(15));
            }
        }
    }

    public void Answer()
    {
        isRinging = false;
        ringingSound.Stop();
    }

    public void MakeRing()
    {
        isRinging = true;
    }

    public void StopRing()
    {
        isRinging = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    private IEnumerator SpawnSound(int count) 
    {
        for (int i = 0; i < count; i++)
        {
            int soundCount = Random.Range(10, 20);
            Vector2 spawnLocation = new(transform.position.x, transform.position.y);
            SoundManager.Instance.SpawnSound(spawnLocation, soundCount, 3f, 2f, linearDrag: 1f, angleRandomizationFactor: 90f, angleStepRandomizationFactor: 90f);
            yield return new WaitForSeconds(0.12f);
        }
    }
}
