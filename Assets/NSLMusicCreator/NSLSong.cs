using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StepInfo
{
	public bool fireBeat = false;
	public bool stopBeat = false;
	public float pitch = 0.0f;
	public float volume = 1.0f;
	public int rollLocation;
}

[System.Serializable]
public class TrackInfo
{
	public AudioClip audioClip;
	public StepInfo[] steps;
	public int trackNumber;
}

[System.Serializable]
public class SequenceDetail
{
	public TrackInfo[] sequenceTracks;
}

public class NSLSong : MonoBehaviour {

	public string sequencerType = "None";
	public int beatsPerMeasure;
	public float tempo;
	public int[] songArrangement;
	public float[] trackVolumes;
	public TrackInfo[] tracks;
	public SequenceDetail[] sequenceDetails;
}
