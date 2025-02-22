using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamically")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bounds;
    private Renderer cubeRend;

	private void Awake()
	{
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bounds = GetComponent<BoundsCheck>();
        cubeRend = GetComponent<Renderer>();

        var vel = Random.onUnitSphere;

        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);

        rigid.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));
        birthTime = Time.time;
	}

	private void FixedUpdate()
	{
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        var alpha = (Time.time - (lifeTime + birthTime)) / fadeTime;

        if(alpha >= 1)
		{
            Destroy(this.gameObject);
            return;
		}

        if(alpha > 0)
		{
            var color = cubeRend.material.color;
            color.a = 1 - alpha;
            cubeRend.material.color = color;

            color = letter.color;
            color.a = 1f - (alpha * 0.5f);
            letter.color = color;
		}

		if (!bounds.isOnScreen)
		{
            Destroy(gameObject);
		}
	}

    public void SetType( WeaponType wt)
	{
        var def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;

        // letter.color = def.color;
        letter.text = def.letter;
        type = wt;
	}

    public void AbsorbedBy(GameObject target)
	{
        Destroy(this.gameObject);
	}
}
