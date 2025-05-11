using UnityEngine;
using UnityEngine.SceneManagement;


public class DrinkSignalManager : MonoBehaviour
{
    public Renderer signalRenderer;
    public Color[] recipeColors;
    private Color currentOrder;
    [SerializeField]
    private int currentIngredient;
    private int currenSnack;

    private float timer = 0f;
    private bool isTiming = false;
    private float currentTimeLimit = 600f;

    void Start()
    {
        GenerateNewOrder();
    }

    void Update()
    {
        
        if (isTiming)
        {
            timer += Time.deltaTime;

            if (timer >= currentTimeLimit)
            {
                FailOrder();
            }
        }
    }

    public void GenerateNewOrder()
    {
        currentOrder = recipeColors[Random.Range(0, recipeColors.Length)];
        currentIngredient = Random.Range(0,4);
        signalRenderer.material.color = currentOrder;

        currentTimeLimit = CalculateTimeLimit(ScoreManager.Instance?.GetScore() ?? 0);
        timer = 0f;
        isTiming = true;

    }

    private float CalculateTimeLimit(int score)
    {
        if (score >= 7) return 15f;
        if (score >= 5) return 20f;
        if (score >= 3) return 25f;
        return 30f;
    }

    public void OnDrinkDelivered(Color deliveredColor,int ingredientCode)
    {
        isTiming = false;

        if (CheckDrink(deliveredColor,ingredientCode))
        {
            ScoreManager.Instance?.AddScore(1);
            GenerateNewOrder();
        }
        else
        {
            FailOrder();
        }
    }

    public bool CheckDrink(Color drinkColor, int ingredient, float tolerance = 0.15f)
    {
        return Mathf.Abs(drinkColor.r - currentOrder.r) < tolerance &&
               Mathf.Abs(drinkColor.g - currentOrder.g) < tolerance &&
               Mathf.Abs(drinkColor.b - currentOrder.b) < tolerance &&
         ingredient == currentIngredient;
    }

    private void FailOrder()
    {
        isTiming = false;

        ScoreManager.Instance?.RemoveScore(1);

        if (ScoreManager.Instance != null && ScoreManager.Instance.GetScore() <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        GenerateNewOrder();
    }


    public Color GetCurrentOrderColor() => currentOrder;
}
