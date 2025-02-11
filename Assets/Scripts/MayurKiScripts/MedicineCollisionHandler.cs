using UnityEngine;
using System.Collections;

public class MedicineCollisionHandler : MonoBehaviour
{
    [SerializeField] private Collider[] collidersToEnable;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private GameObject objectToDisable;
    
    private string[] medicineTags = {
        "Chyawanprash", "Triphala Churna", "Ashwagandha Rasayana", "Kadha",
        "Brahmi Tonic", "Neem Oil", "Bael Syrup", "Musli Pak", "Turmeric Milk"
    };
    
    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in medicineTags)
        {
            if (other.CompareTag(tag))
            {
                StartCoroutine(HandleCollisionEffects());
                break;
            }
        }
    }
    
    private IEnumerator HandleCollisionEffects()
    {
        yield return new WaitForSeconds(0.5f);
        
        // Enable triggers on specified colliders
        foreach (Collider col in collidersToEnable)
        {
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        // Enable and disable game objects as needed
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
    }
}
