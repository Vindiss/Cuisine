using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GestionBook : MonoBehaviour
{
    [Header("Gestion Panel1")]
    [SerializeField] private TextMeshProUGUI titleText1;
    [SerializeField] private TextMeshProUGUI descriptionText1;
    [SerializeField] private TextMeshProUGUI ingrediantText1;
    [Header("Gestion Panel2")]
    [SerializeField] private TextMeshProUGUI titleText2;
    [SerializeField] private TextMeshProUGUI descriptionText2;
    [SerializeField] private TextMeshProUGUI ingrediantText2;
    [Header("Book")]
    [SerializeField] private Book recipeBook;
    [SerializeField] private GameObject player;
    [Header("New Recipe")]
    [SerializeField] private TextMeshProUGUI titleNewR;
    [SerializeField] private TextMeshProUGUI descriptionNewR;
    [SerializeField] private TextMeshProUGUI ingrediantNewR;
    [FormerlySerializedAs("Panelnewrecipe")] [SerializeField] private GameObject panelnewrecipe;
    
    private readonly string _defaultText = "Aucune recette disponible";
    private int _currentPage;

    private void Start()
    {      
        LoadAllRecipes();
        UpdateUI();
    }

    private void LoadAllRecipes()
    {
        Recipe[] recipes = Resources.LoadAll<Recipe>("Recipe");

        foreach (Recipe recipe in recipes)
        {
            recipeBook.recipes.Add(recipe);
        }
    }

    public void UpdateUI()
    {
        if (recipeBook == null || recipeBook.recipes.Count == 0)
        {
            // Aucun livre ou aucune recette
            titleText1.text = _defaultText;
            descriptionText1.text = "";
            ingrediantText1.text = "";
            titleText2.text = _defaultText;
            descriptionText2.text = "";
            ingrediantText2.text = "";
            return;
        }

        // Affichage de la recette de gauche (panneau 1)
        if (_currentPage < recipeBook.recipes.Count)
        {
            Recipe recipe1 = recipeBook.recipes[_currentPage];
            titleText1.text ="Titre: " + recipe1.title;
            descriptionText1.text = "Description: "+recipe1.description;
            ingrediantText1.text = FormatIngredients(recipe1.ingredients);
        }
        else
        {
            titleText1.text = _defaultText;
            descriptionText1.text = "";
            ingrediantText1.text = "";
        }

        // Affichage de la recette de droite (panneau 2)
        if (_currentPage + 1 < recipeBook.recipes.Count)
        {
            Recipe recipe2 = recipeBook.recipes[_currentPage + 1];
            titleText2.text ="Titre: " + recipe2.title;
            descriptionText2.text = "Description: "+recipe2.description;
            ingrediantText2.text = FormatIngredients(recipe2.ingredients);
        }
        else
        {
            titleText2.text = _defaultText;
            descriptionText2.text = "";
            ingrediantText2.text = "";
        }
    }
    
    private string FormatIngredients(List<string> ingredients)
    {
        if (ingredients == null || ingredients.Count == 0)
        {
            return "Aucun ingrédient spécifié.";
        }

        string formatted = "Ingrédients : ";
        foreach (string ingredient in ingredients)
        {
            formatted += $" {ingredient},";
        }
        return formatted.TrimEnd(); // Retire le dernier retour à la ligne
    }

    public void ShowNextPage()
    {
        Debug.Log("ShowNextPage");
        if (recipeBook != null && _currentPage + 2 < recipeBook.recipes.Count)
        {
            _currentPage += 2;
            UpdateUI();
        }
    }

    public void ShowPagePrevious()
    {
        Debug.Log("ShowPagePrevious");
        if (_currentPage - 2 >= 0)
        {
            _currentPage -= 2;
            UpdateUI();
        }
    }

    public void CreateNewRecipe()
    {
        panelnewrecipe.SetActive(true);
        player.GetComponent<PlayerControl>().setIsMoving(false);
        Debug.Log(player.GetComponent<PlayerControl>().getIsMoving());
    }

    public void SaveNewRecipe()
    {
        // Créer une nouvelle instance de Recipe
        Recipe newRecipe = ScriptableObject.CreateInstance<Recipe>();
        
        // Remplir ses champs
        newRecipe.title = titleNewR.text;
        newRecipe.description = descriptionNewR.text;
        string ingredientsInput = ingrediantNewR.text; // Texte brut saisi par l'utilisateur
        string[] ingredientsArray = ingredientsInput.Split(','); // Découper par virgules

        foreach (string ingredient in ingredientsArray)
        {
            string trimmedIngredient = ingredient.Trim(); // Supprimer les espaces inutiles
            if (!string.IsNullOrEmpty(trimmedIngredient))
            {
                newRecipe.ingredients.Add(trimmedIngredient); // Ajouter à la liste
            }
        }
        
        recipeBook.recipes.Add(newRecipe);
        UpdateUI();
#if UNITY_EDITOR
        // Sauvegarder comme un asset dans le projet
        string path = "Assets/Script/Book/Recipe/" + newRecipe.title + ".asset"; // Exemple de chemin
        AssetDatabase.CreateAsset(newRecipe, path);
        AssetDatabase.SaveAssets();
#else
        // Sauvegarder en JSON (runtime uniquement)
        string savePath = Application.persistentDataPath + "/" + newRecipe.title + ".json";
        SaveRecipeAsJson(newRecipe, savePath);
#endif
        panelnewrecipe.SetActive(false);
        player.GetComponent<PlayerControl>().setIsMoving(true);
    }
    
    
    private void SaveRecipeAsJson(Recipe recipe, string filePath)
    {
        // Convertir la recette en un objet sérialisable
        RecipeData data = new RecipeData
        {
            title = recipe.title,
            description = recipe.description,
            ingredients = recipe.ingredients
        };

        // Convertir en JSON
        string json = JsonUtility.ToJson(data, true);

        // Écrire dans un fichier
        File.WriteAllText(filePath, json);
        Debug.Log($"Recette sauvegardée en JSON à {filePath}");
    }
    
    [System.Serializable]
    private class RecipeData
    {
        public string title;
        public string description;
        public List<string> ingredients;
    }
}
