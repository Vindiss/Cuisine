using UnityEngine;
using UnityEngine.UI;

public class Cuisson : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    private float _feuCuisson;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _feuCuisson = slider.value;
    }
}
