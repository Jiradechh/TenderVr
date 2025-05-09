using UnityEngine;

public class Ingredient : MonoBehaviour
{
      public enum Type { Orange, Grape, Apple, Pineapple, Vegetable }
    public Type ingredientType;
     private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = GetColor();
        }
    }
    public Color GetColor()
    {
        return ingredientType switch
        {
            Type.Orange => new Color(1f, 0.5f, 0f),    
            Type.Grape => new Color(0.5f, 0f, 0.5f),    
            Type.Apple => Color.red,                    
            Type.Pineapple => Color.yellow,             
            Type.Vegetable => Color.green,              
            _ => Color.white
        };
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shaker"))
        {
            var shaker = other.GetComponent<ShakerGlass>();
            if (shaker != null && shaker.HasWater())
            {
                shaker.AddIngredient(GetColor());
                Destroy(gameObject);
            }
        }
    }
}
