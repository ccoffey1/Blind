using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayTrigger : MonoBehaviour
{
    [SerializeField]
    private string textToSay;
    [SerializeField]
    private bool sayOnce = true;

    private bool said = false;

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sayOnce && said)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.Say(textToSay);
            said = true;
        }
    }
}
