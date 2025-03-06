using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasteApplication : MonoBehaviour
{
    public Material appliedMaterial;
    private Renderer rend;
    private bool isApplied = false;
    public int pasteAreaIndex;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Squeegee") && !isApplied && StencilControl.moveCount == pasteAreaIndex) 
        {
            Debug.Log("Applying paste to " + gameObject.name);
            rend.enabled = true;
            isApplied = true;
            rend.material = appliedMaterial;
        }
    }
}
