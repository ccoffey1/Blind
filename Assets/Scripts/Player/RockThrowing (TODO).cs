using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrowing : MonoBehaviour
{
    [SerializeField]
    private GameObject rockPrefab;

    private float throwForce = 10f;
    private float maxThrowForce = 20f;
    private float chargeModifier = 20f;

    // Update is called once per frame
    void Update()
    {
        // Charge up...
        if (Input.GetMouseButton(0))
        {
            throwForce = Mathf.Min(throwForce + chargeModifier * Time.deltaTime, maxThrowForce);
        }

        print(throwForce);

        // Shoot!
        if (Input.GetMouseButtonUp(0))
        {
            GameObject spawnedRock = Instantiate(rockPrefab);
            spawnedRock.transform.position = transform.position;

            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (Vector2)((worldMousePos - transform.position));
            direction.Normalize();
            spawnedRock.GetComponent<Rigidbody2D>().velocity = direction * throwForce;

            throwForce = 1f;
        }
    }
}
