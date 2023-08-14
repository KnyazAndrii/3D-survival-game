using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Material Red;
    public Material Green;
    
    public List<GameObject> InTrigger;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (InTrigger.Count > 0)
        {
            _meshRenderer.material = Red;
        }
        else
        {
            _meshRenderer.material = Green;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.gameObject.CompareTag("Ground"))
        {
            bool isInTrigger = false;

            for (int i = 0; i < InTrigger.Count; i++)
            {
                if (InTrigger[i].gameObject == other.gameObject)
                {
                    isInTrigger = true;
                    break;
                }
            }

            if (!isInTrigger)
            {
                InTrigger.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < InTrigger.Count; i++)
        {
            if (InTrigger[i].gameObject == other.gameObject)
            {
                InTrigger.Remove(other.gameObject);
            }
        }
    }
}
