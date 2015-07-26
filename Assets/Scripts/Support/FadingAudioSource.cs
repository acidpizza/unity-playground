using UnityEngine;

/// <summary>
///   Audio source that fades between clips instead of playing them immediately.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class FadingAudioSource : MonoBehaviour
{
	#region Fields

	/// <summary>
	///   Volume change per second when fading.
	/// </summary>
	public float FadeSpeed = 0.05f;
	
	/// <summary>
	///   Actual audio source.
	/// </summary>
	public AudioSource audioSource;

	public float clipVolume = 1f;

	/// <summary>
	///   Whether the audio source is currently fading, in or out.
	/// </summary>
	private FadeState fadeState = FadeState.None;

	#endregion
	
	#region Enums
	
	public enum FadeState
	{
		None,
		
		FadingOut,
		
		FadingIn
	}
	
	#endregion
	
	#region Public Properties
	
	/// <summary>
	///   Current clip of the audio source.
	/// </summary>
	public AudioClip clip
	{
		get
		{
			return this.audioSource.clip;
		}
		set
		{
			this.audioSource.clip = value;
		}
	}

	/// <summary>
	///   Whether the audio source is looping the current clip.
	/// </summary>
	public bool loop
	{
		get
		{
			return this.audioSource.loop;
		}
		set
		{
			this.audioSource.loop = value;
		}
	}

	#endregion
	
	#region Public Methods and Operators

	public void FadeOut()
	{
		this.fadeState = FadeState.FadingOut;
	}

	public void FadeIn()
	{
		this.audioSource.Play ();
		this.audioSource.volume = 0;
		this.fadeState = FadeState.FadingIn;
	}
	
	/// <summary>
	///   Continues fading in the current audio clip.
	/// </summary>
	public void Play()
	{
		this.audioSource.Play();
		this.fadeState = FadeState.None;
		this.audioSource.volume = this.clipVolume;
	}
	
	/// <summary>
	///   Stop playing the current audio clip immediately.
	/// </summary>
	public void Stop()
	{
		this.audioSource.Stop();
		this.fadeState = FadeState.None;
	}
	
	#endregion
	
	#region Methods
	
	private void Awake()
	{
		//this.audioSource = this.GetComponent<AudioSource>();
		this.audioSource.volume = 0f;
	}

	private void Update()
	{
		if (this.fadeState == FadeState.FadingOut)
		{
			if(this.audioSource.volume > 0)
			{
				// Fade out current clip.
				this.audioSource.volume -= this.FadeSpeed * Time.deltaTime;
			}
			else
			{
				// Stop fading out.
				this.fadeState = FadeState.None;
			}
		}
		else if (this.fadeState == FadeState.FadingIn)
		{
			if (this.audioSource.volume < this.clipVolume)
			{
				// Fade in current clip.
				this.audioSource.volume += this.FadeSpeed * Time.deltaTime;
			}
			else
			{
				// Stop fading in.
				this.fadeState = FadeState.None;
			}
		}
	}
	
	#endregion
}