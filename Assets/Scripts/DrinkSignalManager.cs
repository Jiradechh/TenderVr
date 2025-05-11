using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;


public class DrinkSignalManager : MonoBehaviour
{

    private Dictionary<Color, Ingredient.Type> colorToIngredientMap = new Dictionary<Color, Ingredient.Type>();

    public Renderer signalRenderer;
    public Color[] recipeColors;
    [Header("UI")]
    public TextMeshProUGUI gameTimerText;
    private Color currentOrder;
    [SerializeField]
   private Ingredient.Type currentIngredient;


    private float orderTimer = 0f;
    private float orderTimeLimit = 30f;
    private bool orderActive = false;

    private float gameTimer = 0f;
    private float gameDuration = 180f;

    private bool gameRunning = false;
    
    private bool currentOrderSet = false;

   void Start()
{
    gameRunning = true;
    gameTimer = 0f;

    InitializeColorMap();

    GenerateNewOrder();
}

    private void InitializeColorMap()
    {
        colorToIngredientMap.Clear();

        colorToIngredientMap.Add(new Color(1f, 0.5f, 0f), Ingredient.Type.Orange);      
        colorToIngredientMap.Add(new Color(0.5f, 0f, 0.5f), Ingredient.Type.Grape);    
        colorToIngredientMap.Add(Color.yellow, Ingredient.Type.Pineapple);            
        colorToIngredientMap.Add(Color.red, Ingredient.Type.Apple);                   
        colorToIngredientMap.Add(Color.green, Ingredient.Type.Vegetable);            
    }

    public void BeginOrder()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        if (!gameRunning) return;

        gameTimer += Time.deltaTime;

        float timeRemaining = Mathf.Max(0f, gameDuration - gameTimer);
        UpdateTimerUI(timeRemaining); 

        if (timeRemaining <= 0f)
        {
            EndGame();
            return;
        }

        if (orderActive)
        {
            orderTimer += Time.deltaTime;
            if (orderTimer >= orderTimeLimit)
            {
                FailOrder();
            }
        }
    }
    private void UpdateTimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        gameTimerText.text = $"{minutes:00}:{seconds:00}";
    }


        public void GenerateNewOrder()
    {
        if (!currentOrderSet)
        {
            List<Color> keys = new List<Color>(colorToIngredientMap.Keys);
            currentOrder = keys[Random.Range(0, keys.Count)];
            currentIngredient = colorToIngredientMap[currentOrder];

            signalRenderer.material.color = currentOrder;
            currentOrderSet = true;
        }

        orderTimeLimit = CalculateTimeLimit(ScoreManager.Instance?.GetScore() ?? 0);
        orderTimer = 0f;
        orderActive = true;
    }



        public void OnDrinkDelivered(Color deliveredColor, Ingredient.Type deliveredIngredient)
    {
        orderActive = false;


        if (CheckDrink(deliveredColor, deliveredIngredient))
    {
        ScoreManager.Instance?.AddScore(5);

        currentOrderSet = false;

        var customer = FindObjectOfType<CustomerBehavior>();
        if (customer != null)
        {
            customer.SetHappy();
        }

        GenerateNewOrder();
    }

        else
        {
            ScoreManager.Instance?.RemoveScore(3);
            GenerateNewOrder();
        }
    }


            public bool CheckDrink(Color drinkColor, Ingredient.Type ingredient, float tolerance = 0.15f)
    {
        bool colorMatch =
            Mathf.Abs(drinkColor.r - currentOrder.r) < tolerance &&
            Mathf.Abs(drinkColor.g - currentOrder.g) < tolerance &&
            Mathf.Abs(drinkColor.b - currentOrder.b) < tolerance;

        bool ingredientMatch = ingredient.ToString() == currentIngredient.ToString();

        Debug.Log($"🔍 ColorMatch: {colorMatch}, IngredientMatch: {ingredientMatch}");
        Debug.Log($"➡️ Expected Ingredient: {currentIngredient} | Delivered: {ingredient}");

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
    }


        private void EndGame()
    {
        gameRunning = false;

        ScoreManager.Instance?.CheckAndSetHighScore();


        Invoke(nameof(ResetScene), 2f);
    }


    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Color GetCurrentOrderColor() => currentOrder;
}
