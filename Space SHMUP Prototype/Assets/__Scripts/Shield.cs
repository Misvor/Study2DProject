using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;


    [Header("Set Dynamically")]
    public int levelShown = 0;

    // hidden variables
    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        int currLvl = Mathf.FloorToInt(Hero.Self.ShieldLevel);

        if(levelShown != currLvl)
		{
            levelShown = currLvl;
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
		}

        float rotateZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);
    }
}
