using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Surbrillance : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private Color highlightColor = Color.cyan;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private LayerMask bookLayerMask;
    [SerializeField] private LayerMask inventaireLayerMask;
    [SerializeField] private LayerMask inventaireMagicLayerMask;
    [SerializeField] private GameObject contextMenuPrefab;
    [SerializeField] private GameObject book;
    [SerializeField] private GameObject inventaire;
    [SerializeField] private GameObject contenaireInventaire;
    [SerializeField] private GameObject contenaireInventaireMag;
    [SerializeField] private InputActionReference clickAction;
    
    private Renderer _lastHighlightedObject;         
    private Color _originalColor;             
    private GameObject _currentContextMenu;
    private Transform _child;
    private GameObject _childGo;
    private Popup _recettePopup;
    private Renderer _renderer;
    
    private Camera _mainCamera;
    private bool _isClicking;
    private Ray _ray;
    private bool _isChosen;
    private void Start()
    {
       _mainCamera = Camera.main;
    }
    
    private void OnEnable()
    {
        clickAction.action.performed += ClickAction;
        clickAction.action.canceled += ClickActionCanceled;
        clickAction.action.Enable();
    }
    
    private void OnDisable()
    {
        clickAction.action.performed -= ClickAction;
        clickAction.action.canceled -= ClickActionCanceled;
        clickAction.action.Disable();
    }

    private void ClickActionCanceled(InputAction.CallbackContext obj)
    {
        _isClicking = false;
    }

    private void ClickAction(InputAction.CallbackContext obj)
    {
        _isClicking = obj.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainCamera == null)
        {
            Debug.LogWarning("Main Camera not found.");
            return;
        }
        
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (_isChosen == false)
        {
            if (Physics.Raycast(_ray, out RaycastHit hit, maxDistance, interactableLayerMask))
            {
                _renderer = hit.collider.GetComponent<Renderer>();
                if (_renderer != null && _renderer.material.HasProperty(Color1))
                {
                    // Mettre en surbrillance l'objet sous la souris
                    if (_renderer != _lastHighlightedObject)
                    {
                        ResetHighlight(); // Réinitialise la dernière surbrillance
                        _lastHighlightedObject = _renderer;
                        _originalColor = _renderer.material.color;
                        _renderer.material.color = highlightColor;
                    }

                    if (_isClicking && !hit.collider.gameObject.CompareTag("Feu"))
                    {
                        ShowContextMenu(hit.collider.gameObject, hit.point);
                        _isChosen = true;
                    }
                    else if (hit.collider.gameObject.CompareTag("Feu"))
                    {
                        _isChosen = true;
                        _child = hit.collider.transform.Find("Feu");
                        if (_child != null)
                        {
                            _childGo = _child.gameObject;
                            _childGo.SetActive(true);
                        }
                    }
                }
            }
            else if (Physics.Raycast(_ray, out hit, maxDistance, bookLayerMask))
            {
                _renderer = hit.collider.GetComponent<Renderer>();
                if (_renderer != null && _renderer.material.HasProperty(Color1))
                {
                    if (_renderer != _lastHighlightedObject)
                    {
                        ResetHighlight(); // Réinitialise la dernière surbrillance
                        _lastHighlightedObject = _renderer;
                        _originalColor = _renderer.material.color;
                        _renderer.material.color = highlightColor;
                    }

                    if (_isClicking)
                    {
                        if (hit.collider.gameObject.name == "LivreP1")
                        {
                            foreach (Transform child in book.transform)
                            {
                                if (child.gameObject.name == "Recette")
                                {
                                    foreach (Transform recette  in child.gameObject.transform)
                                    {
                                        if (recette.gameObject.name == "Recette1")
                                        {
                                            RectTransform recetteRectT = recette.GetComponent<RectTransform>();
                                            ShowRecipe(recetteRectT);
                                        }
                                    }
                                }
                            }
                        }
                        else if (hit.collider.gameObject.name == "LivreP2")
                        {
                            foreach (Transform child in book.transform)
                            {
                                if (child.gameObject.name == "Recette")
                                {
                                    foreach (Transform recette  in child.gameObject.transform)
                                    {
                                        if (recette.gameObject.name == "Recette2")
                                        {
                                            RectTransform recetteRectT = recette.GetComponent<RectTransform>();
                                            ShowRecipe(recetteRectT);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (Physics.Raycast(_ray, out hit, maxDistance, inventaireLayerMask))
            {
                _renderer = hit.collider.GetComponent<Renderer>();
                if (_renderer != null && _renderer.material.HasProperty(Color1))
                {
                    // Mettre en surbrillance l'objet sous la souris
                    if (_renderer != _lastHighlightedObject)
                    {
                        ResetHighlight(); // Réinitialise la dernière surbrillance
                        _lastHighlightedObject = _renderer;
                        _originalColor = _renderer.material.color;
                        _renderer.material.color = highlightColor;
                    }

                    if (_isClicking && hit.collider.gameObject.CompareTag("Rangement"))
                    {
                        ShowInventory();
                        _isChosen = true;
                    }
                    
                }
            }
            else if (Physics.Raycast(_ray, out hit, maxDistance, inventaireMagicLayerMask))
            {
                _renderer = hit.collider.GetComponent<Renderer>();
                if (_renderer != null && _renderer.material.HasProperty(Color1))
                {
                    // Mettre en surbrillance l'objet sous la souris
                    if (_renderer != _lastHighlightedObject)
                    {
                        ResetHighlight(); // Réinitialise la dernière surbrillance
                        _lastHighlightedObject = _renderer;
                        _originalColor = _renderer.material.color;
                        _renderer.material.color = highlightColor;
                    }

                    if (_isClicking && hit.collider.gameObject.CompareTag("Rangement"))
                    {
                        ShowInventoryMag();
                        _isChosen = true;
                    }
                    
                }
            }
            else
            {
                ResetHighlight();
            }
        }

        if (_currentContextMenu == null || _childGo == null)
        {
            _isChosen = false;
        }
    }

    private void ShowInventory()
    {
        inventaire.SetActive(true);
        contenaireInventaire.GetComponent<StorageScript>().InitializeTabs();
    }
    
    private void ShowInventoryMag()
    {
        inventaire.SetActive(true);
        contenaireInventaireMag.GetComponent<StorageMagic>().InitializeTabs();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowContextMenu(GameObject target, Vector3 point)
    {
        HideContextMenu();
        
        _currentContextMenu = Instantiate(contextMenuPrefab, target.transform.position, Quaternion.identity);
        _currentContextMenu.transform.position = target.transform.position + new Vector3(0, 1f, 0);
        
        ContextMenuHandler contextMenuHandler = _currentContextMenu.GetComponent<ContextMenuHandler>();
        if (contextMenuHandler != null)
        {
            contextMenuHandler.SetTarget(target, point);
        }
    }
    
    private void HideContextMenu()
    {
        if (_currentContextMenu != null)
        {
            Destroy(_currentContextMenu);
            _currentContextMenu = null;
        }
    }

    private void ResetHighlight()
    {
        if (_lastHighlightedObject != null)
        {
            _lastHighlightedObject.material.color = _originalColor;
            _lastHighlightedObject = null;
            if (_childGo != null)
            {
                _childGo.SetActive(false);
            }
        }
    }

    private void ShowRecipe(RectTransform target)
    {
        _recettePopup = book.GetComponent<Popup>();
        if (_recettePopup != null)
        {
            _recettePopup.SetTarget(target);
        }
    }
}
