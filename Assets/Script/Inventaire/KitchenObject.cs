using UnityEngine;


[CreateAssetMenu(fileName = "KitchenObject", menuName = "Scriptable Objects/KitchenObject")]
public class KitchenObject : ScriptableObject
{
    [Header("Informations de l'objet")]
    [Tooltip("Description de l'objet, utilisée dans le menu")]
    [TextArea] public string description;

    [Header("Prefab associé")]
    [Tooltip("Prefab de l'objet, utilisé pour l'instanciation dans le jeu")]
    public GameObject prefab;

    [Header("Image d'affichage")]
    [Tooltip("Image de l'objet, affichée dans la grille de sélection")]
    public Texture2D image;
}
