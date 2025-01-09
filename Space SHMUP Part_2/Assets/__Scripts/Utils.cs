using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
	/// <summary>
	/// returns all materials in provided object and its descendants.
	/// </summary>
	/// <param name="go"> Game Object.</param>
	/// <returns > array of materials.</returns>
  public static Material[] GetAllMaterials(GameObject go)
	{
		var rends = go.GetComponentsInChildren<Renderer>();

		var mats = new List<Material>();

		foreach (var rend in rends)
		{
			mats.Add(rend.material);
		}
		return mats.ToArray();
	}
}
