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


    // Start is called before the first frame update
    void Start()
    {
        _fieldRenderer = _field.GetComponent<Renderer>();
        _fieldDefaultMaterial = _fieldRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (WinCondition.isWon)
        {
            //change material of field
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
