using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{

    [Header("Set in Inspector")]
    public GameObject basketPrefab;

    public int numBaskets = 3;

    public float basketBottomY = -14f;

    public float basketSpacingY = 2f;

    public List<GameObject> baskets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for( var i = 0; i < numBaskets; i++)
		{
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            baskets.Add(tBasketGO);
		}        
    }

    public void AppleDestroyed()
    {
        var tAppleAttay = GameObject.FindGameObjectsWithTag("Apple");
        foreach (var appleObject in tAppleAttay)
        {
            Destroy(appleObject);
        }

        var basketIndex = baskets.Count - 1;

        var basketToDelete = baskets[basketIndex];

        baskets.RemoveAt(basketIndex);
        Destroy(basketToDelete);
        if (baskets.Count == 0)
		{
            SceneManager.LoadScene("_Scene_0");
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
