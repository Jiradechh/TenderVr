using UnityEngine;
using FMODUnity; 

public class SpawnIngredientButton : MonoBehaviour
{
    public GameObject[] ingredientPrefabs;
    public Transform spawnPoint;
    public float pressThreshold = 0.015f;

    [Header("FMOD")]
    [SerializeField]
    private EventReference buttonPressSound; 

    private Vector3 startLocalPos;
    private bool canPress = true;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float pressedZ = startLocalPos.z - transform.localPosition.z;

        if (pressedZ > pressThreshold && canPress)
        {
            SpawnRandomIngredient();
        }
    }

    void SpawnRandomIngredient()
    {
        canPress = false;

        if (!buttonPressSound.IsNull)
            RuntimeManager.PlayOneShot(buttonPressSound, transform.position);
        else
            Debug.LogWarning("Button press sound not assigned!");

        int index = Random.Range(0, ingredientPrefabs.Length);
        Instantiate(ingredientPrefabs[index], spawnPoint.position, Quaternion.identity);

        Invoke(nameof(ResetPress), 0.3f); 
    }

    void ResetPress()
    {
        canPress = true;
    }
}
