using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{

    [Header("Set in Inspector")]

    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

	private void Awake()
	{
        cloudInstances = new GameObject[numClouds];

        var anchor = GameObject.Find("CloudAnchor");

        GameObject cloud;
        for (var i = 0; i < numClouds; i++)
		{
            cloud = Instantiate<GameObject>(cloudPrefab);

            var cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            var scaleU = Random.value;
            var scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            cPos.z = 100 - 90 * scaleU;

            cloud.transform.position = cPos;

            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);

            cloudInstances[i] = cloud;
		}

	}
    

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var cloud in cloudInstances)
		{
            var scaleVal = cloud.transform.localScale.x;
            var cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            if(cPos.x <= cloudPosMin.x)
			{
                cPos.x = cloudPosMax.x;
			}
            cloud.transform.position = cPos;
		}
    }
}
