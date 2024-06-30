using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackOrigin : MonoBehaviour
{
    [SerializeField] private WinCondition WinCondition;

    [SerializeField] private GameObject _field;

    private Material _fieldDefaultMaterial;
    [SerializeField] private Material _fieldWonMaterial;

    private Renderer _fieldRenderer;

    void Start()
    {
        _fieldRenderer = _field.GetComponent<Renderer>();
        _fieldDefaultMaterial = _fieldRenderer.material;
    }

    void Update()
    {
        if (WinCondition.isWon)
        {

            _fieldRenderer.material = _fieldWonMaterial;
        }
        else
        {
            SetMaterial(_fieldDefaultMaterial, _field);
        }
    }

    private void SetMaterial(Material material, GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on object: " + obj.name);
        }
    }
}