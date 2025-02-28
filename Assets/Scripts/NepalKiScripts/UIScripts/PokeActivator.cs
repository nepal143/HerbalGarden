using UnityEngine;

public class EnablePokedexObjects : MonoBehaviour
{
    void Update()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // Check if the object is tagged "Pokedex" and is inactive
            if (obj.CompareTag("Pokedex") && !obj.activeInHierarchy)
            {
                obj.SetActive(true);
                Debug.Log("Activated: " + obj.name);
            }
        }
    }
}
// mile sur mera thumhara to sur bane hamara bankai owowowowowowoweowowowowowoow
// hello silly code written by nepal143 on 2/8/2025