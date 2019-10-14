using UnityEngine;
using System.Collections;

public class LightFlasher : MonoBehaviour {

	Light theLight;
	Color lightColor = Color.white;

	//Sets which audio track the light will respond to
	public int songTrack = 1;

	void Awake () {
	
		theLight = gameObject.GetComponent<Light> ();

		//Subscribe to allEvents
		NSLSongManager.allEvents += HandleLightFlash;
	}

	//When a step event is recieved, increase light brightness and adjust color based on pitch
	void HandleLightFlash(float[] theVolume, float[] thePitch, bool[] fireClip, bool[] stopClip)
	{
		if (fireClip[songTrack])
		{
			theLight.intensity = theVolume [songTrack] * 3;
			lightColor.b = 1.0f - (thePitch [songTrack] / 2);
			lightColor.g = 0.0f + (thePitch [songTrack] / 2);
			theLight.color = lightColor;
		}
	}

	void Update () {

		//Fade Light
		if (light.intensity > 0)
		{
			theLight.intensity -= 0.05f;
		}	
	}
}
