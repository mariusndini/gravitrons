using UnityEngine;
using System.Collections;

public class NSLTrackReceiver : MonoBehaviour {

	public int songTrack = 0;
	public AudioSource audioSource;
	int playTrack = 0;

	// Use this for initialization
	void Awake () {
	
		NSLSongManager.songChange += HandleSongChange;

	}

	void HandleallTrackEvents (float[] theVolume, float[] thePitch, bool[] fireClip, bool[] stopClip, double nextBeatTime)
	{
		if(fireClip[playTrack])
		{
			audioSource.volume = theVolume [playTrack];
			audioSource.pitch = thePitch [playTrack];
			audioSource.PlayScheduled(nextBeatTime);
		}
		else if (stopClip[playTrack])
		{
			audioSource.Stop();
		}
		return;
	}

	void HandleSongChange (AudioClip[] newClips, bool[] usedTracks, int[] playBackTrack)
	{
		if (usedTracks[songTrack])
		{
			playTrack = playBackTrack[songTrack];
			audioSource.clip = newClips[playTrack];
			NSLSongManager.allTrackEvents += HandleallTrackEvents;
		}

		else
		{
			NSLSongManager.allTrackEvents -= HandleallTrackEvents;
		}
	}
}
