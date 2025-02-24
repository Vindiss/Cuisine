using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StorageScript : MonoBehaviour
{
    [SerializeField] private GameObject storagePanel;
    [SerializeField] private GameObject ingredientButtonPrefab;
    [SerializeField] private GameObject tabButtonPrefab;
    [SerializeField] private Transform tabButtonContainer;
    [SerializeField] private Transform ingredientButtonContainer;
    [SerializeField] private LayerMask interactableLayer; // Les objets 
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    private int _currentTab; // Variable pour stocker l'index de l'onglet actif
    private KitchenObject _objectSelected;
    
    [System.Serializable]
    public struct ItemSlot
    {
        public string tabName;
        public List<KitchenObject> itemList;
    }

    public List<ItemSlot> storageTabs = new List<ItemSlot>();

    private void Start()
    {
        storagePanel.SetActive(false);
    }

    public void CloseUI()
    {
        storagePanel.SetActive(false);
    }

    public void InitializeTabs()
    {
        // Supprimer les anciens boutons d'onglets et ingr�dients
        foreach (Transform child in tabButtonContainer)
        {
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Transform child in ingredientButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Cr�er un bouton pour chaque onglet
        for (int i = 0; i < storageTabs.Count; i++)
        {
            GameObject tabButton = Instantiate(tabButtonPrefab, tabButtonContainer);
            TMP_Text buttonText = tabButton.GetComponentInChildren<TMP_Text>();
            
            if (buttonText != null)
            {
                buttonText.text = storageTabs[i].tabName;
            }

            // Ajouter un �v�nement au bouton pour afficher le contenu de cet onglet
            int tabIndex = i; // Capture de l'index dans la closure
            tabButton.GetComponent<Button>().onClick.AddListener(() => DisplayTab(tabIndex));
        }

        // Afficher le premier onglet par d�faut
        if (storageTabs.Count > 0)
        {
            DisplayTab(0);
        }
    }

    public void DisplayTab(int tabIndex)
    {
        // Mettre � jour l'onglet actif
        _currentTab = tabIndex;

        // Supprimer les boutons d'ingr�dients pr�c�dents
        foreach (Transform child in ingredientButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Ajouter les boutons d'ingr�dients pour cet onglet
        foreach (var kitchenObject in storageTabs[tabIndex].itemList)
        {
            GameObject ingredientButton = Instantiate(ingredientButtonPrefab, ingredientButtonContainer);

            // R�cup�rer l'image du bouton d'ingr�dient
            RawImage buttonImage = ingredientButton.GetComponentInChildren<RawImage>();

            if (buttonImage != null)
            {
                buttonImage.texture = kitchenObject.image;
            }

            // Ajouter l'�v�nement au bouton pour traiter l'ingr�dient
            ingredientButton.GetComponent<Button>().onClick.AddListener(() => OnIngredientClicked(kitchenObject));
        }
    }

    private void OnIngredientClicked(KitchenObject kitchenObject)
    {
        descriptionText.text = kitchenObject.description;
        storagePanel.GetComponent<ContextMenuHandler>().SetInventaryTarget(kitchenObject.prefab);
        _objectSelected = kitchenObject;
    }

    public void DropObeject()
    {
        if (_currentTab >= 0 && _currentTab < storageTabs.Count)
        {
            storageTabs[_currentTab].itemList.Remove(_objectSelected);
        }
        CloseUI();
    }
}
