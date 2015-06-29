using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour 
{
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	Slider healthSlider;

	bool damaged = false;

	void Awake()
	{
		healthSlider = GetComponentInChildren<Slider> ();
	}

	void Update () 
	{
		if(damaged)
		{
			damageImage.color = flashColour;
		}
		else
		{
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		damaged = false;
	}

	public void SetHealth(int value)
	{
		healthSlider.value = value;
	}

	public void TakeDamage(int value)
	{
		damaged = true;
		healthSlider.value -= value;
	}
}
