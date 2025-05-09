using UnityEngine;
using System.Collections;

public class ShakerGlass : MonoBehaviour
{
    public GameObject liquidVisual;
    public Color currentColor = Color.clear;
    private bool isFilled = false;
    private bool hasIngredientAdded = false;

    public ParticleSystem waterPourEffect;
    public float pourAngleThreshold = 120f;
    private void Update()
    {
        if (!isFilled || isPouringNow)
            return;

        float upDot = Vector3.Dot(transform.up, Vector3.up);
        float angle = Mathf.Acos(upDot) * Mathf.Rad2Deg;
        bool isPouring = angle > pourAngleThreshold;

        if (isPouring)
        {
            StartCoroutine(PourAndDrain());
        }
        else
        {
            if (waterPourEffect.isPlaying)
            {
                waterPourEffect.Stop();
            }
        }
    }

    private bool isPouringNow = false;

    private IEnumerator PourAndDrain()
    {
        isPouringNow = true;

        if (!waterPourEffect.isPlaying)
            waterPourEffect.Play();

        yield return new WaitForSeconds(0.5f);

        isFilled = false;
        currentColor = Color.clear;
        liquidVisual.SetActive(false);
        waterPourEffect.Stop();

        isPouringNow = false;
    }



    public void FillWithWater()
    {
        if (!isFilled)
        {
            isFilled = true;
            hasIngredientAdded = false;
            liquidVisual.SetActive(true);
            currentColor = Color.cyan;
            UpdateVisual();
        }
    }

    public void AddIngredient(Color ingredientColor)
    {
        if (isFilled)
        {
            currentColor = ingredientColor; 
            UpdateVisual();
        }
    }

    public bool HasWater() => isFilled;

    private void UpdateVisual()
    {
        var renderer = liquidVisual.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = currentColor;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("OnParticleCollision: " + other.name);

        if (other.CompareTag("WaterParticle"))
        {
            FillWithWater();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SignalCheck"))
        {
            var signal = other.GetComponent<DrinkSignalManager>();
            if (signal != null && HasWater())
            {
                signal.OnDrinkDelivered(currentColor);
                Destroy(gameObject); 
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
{
    if (collision.collider.CompareTag("Floor"))
    {

        ScoreManager.Instance?.RemoveScore(1);
        Destroy(gameObject);
        return;
    }


    if (!isFilled || hasIngredientAdded) return;

    if (collision.collider.CompareTag("Ingredient"))
    {
        Ingredient ingredient = collision.collider.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            AddIngredient(ingredient.GetColor());
            hasIngredientAdded = true;
            Destroy(collision.collider.gameObject);
        }
    }
}


}
