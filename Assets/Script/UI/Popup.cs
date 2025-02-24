using Unity.Cinemachine;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camPlayer;
    [SerializeField] private float distanceFromCam = 2f;
    
    private RectTransform _targetObject;
    private Vector2 _originalSize;
    private Vector3 _originalPosition;
    private Quaternion _orginalRotation;
    private bool _isPopup;

    public void SetTarget(RectTransform target)
    {
        _originalPosition = target.position;
        _originalSize = target.sizeDelta;
        _orginalRotation = target.rotation;
        _targetObject = target;
        Invoke(nameof(ShowPopup), 0.2f );
    }

    public void ShowPopup()
    {
        if (!_isPopup)
        {
            Vector3 cameraForward = camPlayer.transform.forward;
            Vector3 popupPosition = camPlayer.transform.position + cameraForward * distanceFromCam;

            // Appliquer la position
            _targetObject.position = popupPosition;

            // Faire face à la caméra
            _targetObject.rotation = Quaternion.LookRotation(cameraForward, Vector3.up) * Quaternion.Euler(0,0,180);
        
            _isPopup = true;
            
            Invoke(nameof(HidePopup), 10f );
        }
        
    }

    public void HidePopup()
    {
        _targetObject.sizeDelta = _originalSize;
        _targetObject.position = _originalPosition;
        _targetObject.rotation = _orginalRotation;
        _isPopup = false;
    }
}
