using System;
using System.Collections;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private bool collected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect(Transform playerTransform)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(playerTransform));
    }

    IEnumerator MoveTowardsPlayer(Transform playerTransform)
    {
        float timer = 0;
        Vector2 initialPosition = transform.position;

        while (timer < 1)
        {
            transform.position = Vector2.Lerp(initialPosition, playerTransform.position, timer);
            timer += Time.deltaTime;
           yield return null;
        }

        Collected();
    }

    private void Collected()
    {
        gameObject.SetActive(false);
    }
}
