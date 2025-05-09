using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLever : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public float pressThreshold = 0.015f; 
    public float pourDuration = 3f;

    private Vector3 startLocalPos;
    private bool isPouring = false;
    private bool canTrigger = true;

    void Start()
    {
        startLocalPos = transform.localPosition;
        waterEffect.Stop();
    }

    void Update()
    {
        float pressedAmount = startLocalPos.z - transform.localPosition.z;

        if (pressedAmount > pressThreshold && canTrigger)
        {
            StartCoroutine(StartPouring());
        }
    }

    System.Collections.IEnumerator StartPouring()
    {
        canTrigger = false;
        isPouring = true;
        waterEffect.Play();

        yield return new WaitForSeconds(pourDuration);

        waterEffect.Stop();
        isPouring = false;
        canTrigger = true;
    }
}