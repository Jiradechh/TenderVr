using UnityEngine;
using UnityEngine.SceneManagement;

public class DrinkSignalManager : MonoBehaviour
{
    public Renderer signalRenderer;
    public Color[] recipeColors;
    private Color currentOrder;
    [SerializeField]
    private int currentIngredient;

    private float orderTimer = 0f;
    private float orderTimeLimit = 30f;
    private bool orderActive = false;

    private float gameTimer = 0f;
    private float gameDuration = 180f;

    private bool gameRunning = false;

    void Start()
    {
        gameRunning = true;
        gameTimer = 0f;

        GenerateNewOrder();
    }

    public void BeginOrder()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        if (!gameRunning) return;

        gameTimer += Time.deltaTime;
        if (gameTimer >= gameDuration)
        {
            EndGame();
            return;
        }

        if (orderActive)
        {
            orderTimer += Time.deltaTime;
            if (orderTimer >= orderTimeLimit)
            {
                Debug.Log("⏰ Order timeout.");
                FailOrder();
            }
        }
    }

    public void GenerateNewOrder()
    {
        currentOrder = recipeColors[Random.Range(0, recipeColors.Length)];
        currentIngredient = Random.Range(0, 4);
        signalRenderer.material.color = currentOrder;

        orderTimeLimit = CalculateTimeLimit(ScoreManager.Instance?.GetScore() ?? 0);
        orderTimer = 0f;
        orderActive = true;

        Debug.Log($"📦 New Order: Color = {currentOrder}, IngredientCode = {currentIngredient}, TimeLimit = {orderTimeLimit}s");
    }

    public void OnDrinkDelivered(Color deliveredColor, int ingredientCode)
    {
        orderActive = false;

        Debug.Log($"📤 Delivered drink: Color = {deliveredColor}, IngredientCode = {ingredientCode}");

        if (CheckDrink(deliveredColor, ingredientCode))
        {
            Debug.Log("✅ Correct drink delivered! +5 score");
            ScoreManager.Instance?.AddScore(5);
        }
        else
        {
            Debug.Log("❌ Wrong drink! -3 score");
            ScoreManager.Instance?.RemoveScore(3); 
        }

        GenerateNewOrder();
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

    private float CalculateTimeLimit(int score)
    {
        if (score >= 7) return 15f;
        if (score >= 5) return 20f;
        if (score >= 3) return 25f;
        return 30f;
    }

    private void FailOrder()
    {
        orderActive = false;
        ScoreManager.Instance?.RemoveScore(3);

        Debug.Log($"🟥 Order failed. Score = {ScoreManager.Instance?.GetScore()}");

        GenerateNewOrder();
    }

    private void EndGame()
    {
        gameRunning = false;
        Debug.Log("🏁 Time's up! Final score: " + ScoreManager.Instance?.GetScore());

        Invoke(nameof(ResetScene), 2f);
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Color GetCurrentOrderColor() => currentOrder;
}
