using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutObject : MonoBehaviour
{
	public float fadeTime = 1.5f;

	public List<Material> allMats = new List<Material>();
	public float timer;
	public bool hasStarted = false;
	void Start()
	{
		// Collect every renderer in the object and children
		var renderers = GetComponentsInChildren<Renderer>();

		foreach (var r in renderers)
		{
			// Duplicate the materials so fading doesn't affect shared assets
			foreach (var m in r.materials)
				allMats.Add(m);
		}
	}

	void Update()
	{
		if (!hasStarted)
		{
			return;
		}
		timer += Time.deltaTime;
		float a = Mathf.Lerp(1f, 0f, timer / fadeTime);

		foreach (var m in allMats)
		{
			if (m.HasProperty("_Color"))
			{
				Color c = m.color;
				c.a = a;
				m.color = c;
			}
		}

		if (a <= 0f)
		{
			gameObject.SetActive(false);
		}

	}

	public void StartFade()
	{
		hasStarted = true;
	}
}
