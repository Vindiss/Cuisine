using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Informations de la recette")]
    public string title;
    [TextArea(3, 5)]
    public string description;

    [Header("Ingrédients")]
    public List<string> ingredients = new List<string>();

    /// <summary>
    /// Affiche la recette dans la console (ou utilisez cette méthode pour d'autres affichages).
    /// </summary>
    public void DisplayRecipe()
    {
        Debug.Log($"Titre : {title}\nDescription : {description}\nIngrédients :");
        foreach (string ingredient in ingredients)
        {
            Debug.Log($"- {ingredient}");
        }
    }
}
