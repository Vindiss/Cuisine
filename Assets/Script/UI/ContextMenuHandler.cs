using System;
using UnityEngine;

public class ContextMenuHandler : MonoBehaviour
{
    private HandUI _hand;
    private GameObject _targetObject;
    private GameObject _targetObjectInv;
    private Camera _mainCamera;
    private Vector3 _position;

    private bool _handLeftPlug;
    private bool _handRightPlug;
    private void Awake()
    {
        GameObject hand = GameObject.Find("HandCanvas");
        _hand = hand.GetComponent<HandUI>();
        _mainCamera = Camera.main;
    }

    public void SetTarget(GameObject target, Vector3 position)
    {
        _targetObject = target;
        
    }

    public void SetInventaryTarget(GameObject target)
    {
        _targetObjectInv = target;
    }

    public void LeftHand()
    {
        if (_targetObjectInv != null)
        {
            if (_handLeftPlug == false && !_targetObjectInv.CompareTag("Table") && !_targetObjectInv.CompareTag("Feu"))
            {
                _hand.GetComponent<HandUI>().SetLeftHandObjectInv(_targetObjectInv);
            }
            else if (_handLeftPlug && _targetObjectInv.CompareTag("Table"))
            {
                _hand.GetComponent<HandUI>().ClearLeftHand(_targetObjectInv.transform,_position);
            }
        }
        else
        {
            if (_handLeftPlug == false && !_targetObject.CompareTag("Table") && !_targetObject.CompareTag("Feu"))
            {
                _hand.GetComponent<HandUI>().SetLeftHandObject(_targetObject);
            }
            else if (_handLeftPlug && _targetObject.CompareTag("Table"))
            {
                _hand.GetComponent<HandUI>().ClearLeftHand(_targetObject.transform, _position);
            }
            CloseMenu();
        }
    }

    public void RightHand()
    {
        if (_targetObjectInv != null)
        {
            if (_handLeftPlug == false && !_targetObjectInv.CompareTag("Table") && !_targetObjectInv.CompareTag("Feu"))
            {
                _hand.GetComponent<HandUI>().SetRightHandObjectInv(_targetObjectInv);
            }
            else if (_handLeftPlug && _targetObjectInv.CompareTag("Table") || _targetObject.CompareTag("Feu"))
            {
                _hand.GetComponent<HandUI>().ClearRightHand(_targetObjectInv.transform, _position);
            }
        }
        else
        {
            if (_handRightPlug == false && !_targetObject.CompareTag("Table") && !_targetObject.CompareTag("Feu"))
            {
                _hand.GetComponent<HandUI>().SetRightHandObject(_targetObject);
            }
            else if (_handRightPlug && _targetObject.CompareTag("Table"))
            {
                _hand.GetComponent<HandUI>().ClearRightHand(_targetObject.transform, _position);
            }
            CloseMenu();
        }
    }

    public void CloseMenu()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _handLeftPlug = _hand.GetComponent<HandUI>().GetLeftHand();
        _handRightPlug = _hand.GetComponent<HandUI>().GetRightHand();
    }
}
