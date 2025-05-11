using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkSignalManager : MonoBehaviour
{
    public Renderer signalRenderer;
    public Color[] recipeColors;
    private Color currentOrder;
    [SerializeField]
    private int currentIngredient;

    private float timer = 0f;
    private bool isTiming = false;
    private float currentTimeLimit = 600f;

    void Start()
    {
        //GenerateNewOrder();
    }

    void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;

            if (timer >= currentTimeLimit)
            {
                Debug.Log("⏰ Time's up! Order failed.");
                FailOrder();
            }
        }
    }
    public void BeginOrder()
    {
        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        currentOrder = recipeColors[Random.Range(0, recipeColors.Length)];
        currentIngredient = Random.Range(0, 4);
        signalRenderer.material.color = currentOrder;

        currentTimeLimit = CalculateTimeLimit(ScoreManager.Instance?.GetScore() ?? 0);
        timer = 0f;
        isTiming = true;

        Debug.Log($"📦 New Order: Color = {currentOrder}, IngredientCode = {currentIngredient}, TimeLimit = {currentTimeLimit}s");
    }

    private float CalculateTimeLimit(int score)
    {
        if (score >= 7) return 15f;
        if (score >= 5) return 20f;
        if (score >= 3) return 25f;
        return 30f;
    }

    public void OnDrinkDelivered(Color deliveredColor, int ingredientCode)
    {
        isTiming = false;

        Debug.Log($"📤 Delivered drink: Color = {deliveredColor}, IngredientCode = {ingredientCode}");

        if (CheckDrink(deliveredColor, ingredientCode))
        {
            Debug.Log("✅ Correct drink delivered! +1 score");
            ScoreManager.Instance?.AddScore(1);
            GenerateNewOrder();
        }
        else
        {
            Debug.Log("❌ Wrong drink! -1 score");
            FailOrder();
        }
    }

    public bool CheckDrink(Color drinkColor, int ingredient, float tolerance = 0.15f)
    {
        bool colorMatch =
            Mathf.Abs(drinkColor.r - currentOrder.r) < tolerance &&
            Mathf.Abs(drinkColor.g - currentOrder.g) < tolerance &&
            Mathf.Abs(drinkColor.b - currentOrder.b) < tolerance;

        bool ingredientMatch = ingredient == currentIngredient;

        Debug.Log($"🔍 ColorMatch: {colorMatch}, IngredientMatch: {ingredientMatch}");

        return colorMatch && ingredientMatch;
    }

    private void FailOrder()
    {
        isTiming = false;
        ScoreManager.Instance?.RemoveScore(1);

        Debug.Log($"🟥 Order failed! Score = {ScoreManager.Instance?.GetScore()}");

        if (ScoreManager.Instance != null && ScoreManager.Instance.GetScore() <= 0)
        {
            Debug.Log("💀 Game Over: Reloading Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        GenerateNewOrder();
    }

    public Color GetCurrentOrderColor() => currentOrder;
}
