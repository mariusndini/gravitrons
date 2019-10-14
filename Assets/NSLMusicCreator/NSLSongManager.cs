using UnityEngine;
using System.Collections;

public class NSLSongManager : MonoBehaviour {

	public double nextBeat = 0.0;
	public float bpMinute = 120.0f;
	public int bpMeasure = 16;
	float beatTime = 0.0f;
	public bool loopPlaylist = false;
	public bool playOnStart = false;
	bool songPlaying = false;
	public NSLSong[] playlist;
	public int currentSong = 0;
	int currentStep = 0;
	int maxTracks = 0;
	float[] allVolume;
	float[] allPitch;
	bool[] allFireClip;
	bool[] allStopClip;
	public bool[] trackUsed;
	public int[] playBackTracks;
	public AudioClip[] songClips;

	public delegate void trackHandlerAll(float[] theVolume, float[] thePitch, bool[] fireClip, bool[] stopClip, double nextBeatTime);
	public static event trackHandlerAll allTrackEvents;

	public delegate void stepHandlerAll(float[] theVolume, float[] thePitch, bool[] fireClip, bool[] stopClip);
	public static event stepHandlerAll allEvents;

	public delegate void songChangeHandler(AudioClip[] newClips, bool[] usedTracks, int[] playBackTrack);
	public static event songChangeHandler songChange;

	public void LoadNewSong(int newSongNumber)
	{
		if (newSongNumber >= playlist.Length) { Debug.Log("Song number " + newSongNumber + " is greater than the number of available songs in the playlist"); return; }

		currentSong = newSongNumber;

		for (int i = 0; i < maxTracks; i++)
		{
			songClips[i] = null;
			trackUsed[i] = false;
		}

		bpMinute = playlist [currentSong].tempo;
		beatTime = 15 / bpMinute;
		nextBeat = AudioSettings.dspTime + (beatTime * 2);
		currentStep = 0;

		allVolume = new float[playlist[currentSong].tracks.Length];
		allPitch = new float[playlist[currentSong].tracks.Length];
		allFireClip = new bool[playlist[currentSong].tracks.Length];
		allStopClip = new bool[playlist[currentSong].tracks.Length];

		for (int i = 0; i < playlist[currentSong].tracks.Length; i++)
		{
			songClips[i] = playlist[currentSong].tracks[i].audioClip;
			allVolume[i] = 1.0f;
			allPitch[i] = 1.0f;
			allFireClip[i] = false;
			allStopClip[i] = false;
			trackUsed[playlist[currentSong].tracks[i].trackNumber] = true;
			playBackTracks[playlist[currentSong].tracks[i].trackNumber] = i;
		}

		songChange(songClips, trackUsed, playBackTracks);

		return;
	}

	public void PlaySong(int songNumber)
	{
		if (songNumber != currentSong)
		{
			currentSong = songNumber;
			LoadNewSong (songNumber);
		}
		
		nextBeat = AudioSettings.dspTime + beatTime;
		songPlaying = true;
	}

	public void StopSong()
	{
		songPlaying = false;
	}

	void Start () {

		beatTime = 15 / bpMinute;
		nextBeat = AudioSettings.dspTime + (beatTime * 2);

		//determine greatest number of tracks present in a playlist
		for (int i = 0; i < playlist.Length; i++)
		{
			if (playlist[i].sequenceDetails[0].sequenceTracks.Length > maxTracks) { maxTracks = playlist[i].sequenceDetails[0].sequenceTracks.Length; }
		}

		songClips = new AudioClip[maxTracks];
		trackUsed = new bool[maxTracks];
		playBackTracks = new int[maxTracks];

		for (int i = 0; i < maxTracks; i++)
		{
			songClips[i] = null;
			trackUsed[i] = false;
			playBackTracks[i] = i;
		}

		if (songChange != null) { LoadNewSong (0); }
		if (playOnStart) { PlaySong(currentSong); }

	}

	void Update () {

		if(songPlaying && AudioSettings.dspTime >= nextBeat + (beatTime/2))
		{
			if (allEvents != null) { allEvents(allVolume, allPitch, allFireClip, allStopClip); }

			if(loopPlaylist && currentStep >= playlist[currentSong].tracks[0].steps.Length)
			{
				currentSong++;
				if (currentSong >= playlist.Length) { currentSong = 0; }
				LoadNewSong(currentSong);
				currentStep = 0;
				return;
			}

			else if(!loopPlaylist && currentStep >= playlist[currentSong].tracks[0].steps.Length)
			{
				currentStep = 0;
			}

			for (int i = 0; i < playlist[currentSong].tracks.Length; i++)
			{
				if(playlist[currentSong].tracks[i].steps[currentStep].fireBeat)
				{
					allVolume[i] = playlist[currentSong].tracks[i].steps[currentStep].volume;
					allPitch[i] = playlist[currentSong].tracks[i].steps[currentStep].pitch;
					allFireClip[i] = true;
					allStopClip[i] = false;
				}
				
				else if(playlist[currentSong].tracks[i].steps[currentStep].stopBeat)
				{
					allFireClip[i] = false;
					allStopClip[i] = true;
				}

				else
				{
					allFireClip[i] = false;
					allStopClip[i] = false;
				}

			}
			nextBeat += beatTime;
			currentStep++;
			if (allTrackEvents != null) { allTrackEvents(allVolume, allPitch, allFireClip, allStopClip, nextBeat); }
		}	
	}
}
