using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUI_Hellephant : MonoBehaviour 
{
	public Image image;
	Color lowHealthColour = new Color(1f, 0f, 0f, 1f);
	Slider healthSlider;

	Quaternion originalRotation;

	void Awake()
	{
		originalRotation = transform.rotation;
		healthSlider = GetComponentInChildren<Slider> ();
	}

	void Update () 
	{
		transform.rotation = originalRotation;
	}

	public void SetHealth(int value)
	{
		healthSlider.value = value;
	}

	public void TakeDamage(int value)
	{
		healthSlider.value -= value;
	}

	public void SetLowHealthStatus()
	{
		image.color = lowHealthColour;
	}
}
