using System.Collections;
using UnityEngine;
using FMODUnity;

public class WaterLever : MonoBehaviour
{
    public ParticleSystem waterEffect;
    public float pressThreshold = 0.015f;
    public float pourDuration = 3f;

    [Header("FMOD")]
    [SerializeField]
    private EventReference leverSound;

    private Vector3 startLocalPos;
    private bool isPouring = false;
    private bool canTrigger = true;

    private bool hasPlayedSound = false;

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

    IEnumerator StartPouring()
    {
        canTrigger = false;
        isPouring = true;

        if (!hasPlayedSound && !leverSound.IsNull)
        {
            RuntimeManager.PlayOneShot(leverSound, transform.position);
            hasPlayedSound = true;
        }

        waterEffect.Play();

        yield return new WaitForSeconds(pourDuration);

        waterEffect.Stop();
        isPouring = false;
        canTrigger = true;
        hasPlayedSound = false;
    }
}
