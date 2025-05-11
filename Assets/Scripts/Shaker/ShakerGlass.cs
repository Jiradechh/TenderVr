using UnityEngine;
using System.Collections;

public class ShakerGlass : MonoBehaviour
{
    public GameObject liquidVisual;
    public Color currentColor = Color.clear;
    public int ingredientCode;
    private bool isFilled = false;
    private bool hasIngredientAdded = false;

    private Ingredient.Type currentIngredientType;


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



    public void FillWithWater(Color waterColor)
    {
        if (!isFilled)
        {
            isFilled = true;
            hasIngredientAdded = false;
            liquidVisual.SetActive(true);
            currentColor = waterColor;
            UpdateVisual();
        }
    }

       public void AddIngredient(Ingredient.Type type)
{
    if (isFilled)
    {
        currentIngredientType = type;
        Debug.Log($" Shaker added ingredient: {type}");
        UpdateVisual();
    }
}



    public bool HasWater() => isFilled;

private void UpdateVisual()
{
    var renderer = liquidVisual.GetComponent<Renderer>();
    if (renderer != null)
    {
        Material mat = renderer.material;

        if (mat.HasProperty("_DeepColor"))
        {
            mat.SetColor("_DeepColor", currentColor);
            Debug.Log("✅ Deep Color updated to: " + currentColor);
        }
        else
        {
            Debug.LogWarning("❌ Material missing DeepColor property.");
        }
    }
}



   private void OnParticleCollision(GameObject other)
{
    Debug.Log("OnParticleCollision: " + other.name);

    if (other.CompareTag("WaterParticle"))
    {
        FillWithWater(Color.cyan);
    }
    else if (other.CompareTag("OrangeParticle"))
    {
        FillWithWater(new Color(1f, 0.5f, 0f)); 
    }
    else if (other.CompareTag("GrapeParticle"))
    {
        FillWithWater(new Color(0.5f, 0f, 0.5f)); 
    }
    else if (other.CompareTag("AppleParticle"))
    {
        FillWithWater(Color.red);
    }
    else if (other.CompareTag("PineappleParticle"))
    {
        FillWithWater(Color.yellow);
    }
    else if (other.CompareTag("VegetableParticle"))
    {
        FillWithWater(Color.green);
    }
}


    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("SignalCheck"))
    {
        var signal = other.GetComponent<DrinkSignalManager>();
        if (signal != null && HasWater())
        {
            signal.OnDrinkDelivered(currentColor, currentIngredientType);
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
        AddIngredient(ingredient.ingredientType);
        hasIngredientAdded = true;
        Destroy(collision.collider.gameObject);
    }
}

}


}
