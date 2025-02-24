using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField] private GameObject leftHandPanel;  // Panneau pour la main gauche
    [SerializeField] private GameObject rightHandPanel; // Panneau pour la main droite
    [SerializeField] private Camera leftHandCamera;
    [SerializeField] private Camera rightHandCamera;
    [SerializeField] private RenderTexture rightHandTexture;
    [SerializeField] private RenderTexture leftHandTexture;
    [SerializeField] private Transform player;
    
    private GameObject _leftHandObject;                 // Objet tenu par la main gauche
    private GameObject _rightHandObject;                // Objet tenu par la main droite
    private bool _leftHandPlug;
    private bool _rightHandPlug;

    public void SetLeftHandObject(GameObject obj)
    {
        if (_leftHandPlug == false)
        {
            _leftHandObject = obj;
            _leftHandObject.layer = LayerMask.NameToLayer("HandLeft");
            _leftHandObject.transform.SetParent(player, true);
            foreach (Transform child in _leftHandObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandRight");
            }
            UpdatePanel(leftHandPanel.transform, _leftHandObject, leftHandCamera, leftHandTexture);
            _leftHandPlug = true;
        }
    }
    public void SetLeftHandObjectInv(GameObject obj)
    {
        if (_leftHandPlug == false)
        {
            _leftHandObject = Instantiate(obj, player, true);
            _leftHandObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _leftHandObject.transform.position = player.transform.position + player.transform.forward;
            _leftHandObject.layer = LayerMask.NameToLayer("HandLeft");
            foreach (Transform child in _leftHandObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandRight");
            }
            UpdatePanel(leftHandPanel.transform, _leftHandObject, leftHandCamera, leftHandTexture);
            _leftHandPlug = true;
        }
    }
    
    public void SetRightHandObject(GameObject obj)
    {
        if (_rightHandPlug == false)
        {
            _rightHandObject = obj;
            _rightHandObject.layer = LayerMask.NameToLayer("HandRight");
            _rightHandObject.transform.SetParent(player, true);
            foreach (Transform child in _rightHandObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandRight");
            }
            UpdatePanel(rightHandPanel.transform, _rightHandObject, rightHandCamera, rightHandTexture);
            _rightHandPlug = true;
        }
    }
    public void SetRightHandObjectInv(GameObject obj)
    {
        if (_rightHandPlug == false)
        {
            _rightHandObject = Instantiate(obj, player, true);
            _rightHandObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _rightHandObject.transform.position = player.transform.position + player.transform.forward;
            _rightHandObject.layer = LayerMask.NameToLayer("HandRight");
            foreach (Transform child in _rightHandObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("HandRight");
            }
            UpdatePanel(rightHandPanel.transform, _rightHandObject, rightHandCamera, rightHandTexture);
            _rightHandPlug = true;
        }
    }
    
    public void ClearLeftHand(Transform target, Vector3 position)
    {
        _leftHandObject.transform.SetParent(target, true);
        _leftHandObject.transform.position = new Vector3(_leftHandObject.transform.position.x,target.transform.position.y + 0.85f,_leftHandObject.transform.position.z);
        _leftHandObject.layer = LayerMask.NameToLayer("interact");
        _leftHandObject = null;
        ClearPanel(leftHandPanel.transform);
        _leftHandPlug = false;
    }

    public void ClearRightHand(Transform target,Vector3 position )
    {
        _rightHandObject.transform.SetParent(target, true);
        _rightHandObject.transform.localPosition = Vector3.zero;
        _rightHandObject.transform.localPosition += new Vector3(0, -0.85f, 0);
        _rightHandObject.layer = LayerMask.NameToLayer("interact");
        _rightHandObject = null;
        ClearPanel(rightHandPanel.transform);
        _rightHandPlug = false;
    }
    
    private void ClearPanel(Transform panel)
    {
        RawImage rawImage = panel.GetComponentInChildren<RawImage>();
        if (rawImage != null)
        {
            rawImage.texture = null;
            rawImage.gameObject.SetActive(false);
        }
    }
    
    
    private void UpdatePanel(Transform panel, GameObject obj, Camera targetCamera, RenderTexture texture)
    {
        if (obj != null)
        {
            // Positionner l'objet devant la caméra dédiée
            targetCamera.transform.position = obj.transform.position - obj.transform.forward;
            targetCamera.transform.LookAt(obj.transform);

            // Utiliser une RawImage pour afficher la RenderTexture
            RawImage rawImage = panel.GetComponentInChildren<RawImage>();
            if (rawImage != null)
            {
                rawImage.texture = texture;
                rawImage.gameObject.SetActive(true);
            }
        }
    }

    public bool GetLeftHand()
    {
        return _leftHandPlug;
    }

    public bool GetRightHand()
    {
        return _rightHandPlug;
    }
    
}
