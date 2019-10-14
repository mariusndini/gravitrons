using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BeatInfo
{
	public int seqTrack = 0;
	public bool fireBeat = false;
	public bool stopBeat = false;
	public float pitch = 0.0f;
	public bool playShort = false;
	public float volume = 1.0f;
	public int rollLocation = 0;
}

public class SongSequence
{
	public BeatInfo[] beatInfo;
	public Color sequenceColor;
}

public class NSLMusicCreator : EditorWindow {

	Texture2D buttonTex1 = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/button1.png", typeof(Texture2D)) as Texture2D;
	Texture2D buttonTex2 = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/button2.png", typeof(Texture2D)) as Texture2D;
	Texture2D buttonBack = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/ButtonBack1.png", typeof(Texture2D)) as Texture2D;
	Texture2D pianoBack = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/PianoBack1.png", typeof(Texture2D)) as Texture2D;
	Texture2D pianoButtonTex1 = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/PianoButton1.png", typeof(Texture2D)) as Texture2D;
	Texture2D pianoButtonTex2 = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/PianoButton2.png", typeof(Texture2D)) as Texture2D;
	Texture2D pianoButtonTex3 = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/PianoButton3.png", typeof(Texture2D)) as Texture2D;
	Texture2D pianoKeys = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/PianoKeys1.png", typeof(Texture2D)) as Texture2D;
	Texture2D sampleEmptyTex = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/EmptySample.png", typeof(Texture2D)) as Texture2D;
	Texture2D sampleLoadedTex = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/LoadedSample.png", typeof(Texture2D)) as Texture2D;
	Texture2D blackBackground = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/1PixBlack.png", typeof(Texture2D)) as Texture2D;
	Texture2D blueBackground = AssetDatabase.LoadAssetAtPath("Assets/NSLMusicCreator/Editor/Images/1PixBlue.png", typeof(Texture2D)) as Texture2D;
	Texture2D songTex;
	Texture2D seqTex;
	Texture2D[] sampleTex;
	int selectGrid = 0;
	int sequenceGrid = 0;
	int sequenceTotal = 8;
	int currentSequence = 0;
	int stepLength = 128;
	int trackTotal = 8;
	public AudioClip[] samples;
	GameObject[] audioObjs;
	AudioSource[] audioSources;
	GUIStyle stepStyle = new GUIStyle();
	SongSequence[] sequences;
	Texture2D[] selectTex;
	Texture2D[] pianoRollTex;
	string[] sequenceStrings;
	bool[] stopOnNext;
	int previousHotControl = 100;
	Rect gridRect;
	Rect pianoKeysRect;
	Rect sequenceRect;
	Rect sequenceBackRect;
	Rect songRect;
	Rect songTitleRect = new Rect (180, 15, 250, 20);
	Rect[] sampleRects;
	Rect[] pianoRollSwitchRects;
	Rect[] volumeSliderRects;
	Rect[] stepSliderRects;
	Rect[] blueBackRects;
	bool partPlaying = false;
	bool songPlaying = false;
	List<int> songArrangement = new List<int> ();
	int songPixelWidth;
	public double nextBeat = 0.0;
	public int beatCount = 0;
	public float bpMinute = 120.0f;
	public int bpMeasure = 16;
	float beatTime = 0.0f;
	int playSequence = 0;
	bool pianoRollOn = false;
	int pianoRollSelection = 0;
	int currentTrack = 0;
	int pianoKeyCount = 25;
	float[] stepSliderVolumes;
	string songTitle = "New Song";
	string tempoInput = "120";
	GameObject songManagerObject;
	GameObject trackListObject;
	GameObject songListObject;
	GameObject saveSongObject;
	float[] trackVolumes;
	bool windowInitialized = false;
	public bool initRun = false;
	public string sequencerType = "16 Step";
	public int timingBeat = 4;
	public float stepWidth = 37.5f;
	public float stepHeight = 50.0f;
	
	//[MenuItem ("Window/NSL Music Creator/16 Step")]

	void OnEnable()
	{
		//If Window was left open on last Unity Editor exit, attempts to close the window.  Prevents errors generated if window intitalization does not run on Unity startup.
		if(!windowInitialized && Time.realtimeSinceStartup < 1.4f) { this.Close(); }
	}

	public void SetSequenceParameters(int zBPMe, int zTrackTotal, int zSequenceTotal, int zTimingBeat, float zStepWidth, float zStepHeight, string zSequencerType)
	{
		bpMeasure = zBPMe;
		trackTotal = zTrackTotal;
		sequenceTotal = zSequenceTotal;
		timingBeat = zTimingBeat;
		sequencerType = zSequencerType;
		stepWidth = zStepWidth;
		stepHeight = zStepHeight;

		windowInitialized = true;
		DontDestroyOnLoad (songManagerObject);

		songTex = new Texture2D (200, 1);
		songTex.hideFlags = HideFlags.HideAndDontSave;
		seqTex = new Texture2D (200, 1);
		seqTex.hideFlags = HideFlags.HideAndDontSave;

		gridRect = new Rect (180, 120, (int) (stepWidth * bpMeasure), (stepHeight * trackTotal));
		pianoKeysRect = new Rect (140, 120, 40, gridRect.height);
		sequenceRect = new Rect (180, 60, gridRect.width, 40);
		sequenceBackRect = new Rect (175, 55, 10 + gridRect.width, 50);
		songRect = new Rect (20, 140 + gridRect.height, 160 + gridRect.width, 40);

		int sampleRectHeight = (int)(gridRect.height / trackTotal);
		stepLength = bpMeasure * trackTotal;
		selectTex = new Texture2D[stepLength];
		pianoRollTex = new Texture2D[pianoKeyCount * bpMeasure];
		stepSliderVolumes = new float[bpMeasure];
		samples = new AudioClip[trackTotal];
		sampleRects = new Rect[trackTotal];
		audioObjs = new GameObject[trackTotal];
		audioSources = new AudioSource[trackTotal];
		pianoRollSwitchRects = new Rect[trackTotal];
		volumeSliderRects = new Rect[trackTotal];
		stepSliderRects = new Rect[bpMeasure];
		blueBackRects = new Rect[(int) bpMeasure / timingBeat];
		sampleTex = new Texture2D[trackTotal];
		trackVolumes = new float[trackTotal];
		stopOnNext = new bool[trackTotal];
		sequenceStrings = new string[sequenceTotal];
		beatTime = 15 / bpMinute;
		nextBeat = AudioSettings.dspTime + (beatTime * 2);
		sequences = new SongSequence[sequenceTotal];
		Color sequenceNewColor = Color.blue;
		sequenceNewColor.b = 0.5f;
		float colorChange = 1.0f / sequenceTotal;
		int testLocation = 0;
		int testSeq = 0;
		float blueBackWidth = stepWidth + 1;
		float blueBackSpacing = gridRect.width / blueBackRects.Length;

		NSLTrackReceiver newReceiver;

		//Set up the empty Arranger texture
		for (int i = 0; i < songTex.width; i++)
		{
			songTex.SetPixel(i, 0, Color.gray);
			songTex.Apply();
		}

		//Set default PianoRoll icons for SelectionGrid
		for (int i = 0; i < pianoRollTex.Length; i++)
		{
			pianoRollTex[i] = null;
		}

		//Set default values for all sequences and steps
		for (int i = 0; i < sequenceTotal; i++)
		{
			sequenceStrings[i] = (i+1).ToString();
			sequences[i] = new SongSequence();
			sequences[i].beatInfo = new BeatInfo[stepLength];
			sequences[i].sequenceColor = sequenceNewColor;
			for (int x = 0; x < stepLength; x++) 
			{
				selectTex[i] = buttonTex1;
				sequences[i].beatInfo[x] = new BeatInfo();
				sequences[i].beatInfo[x].fireBeat = false;
				sequences[i].beatInfo[x].stopBeat = false;
				sequences[i].beatInfo[x].pitch = 0.0f;
				sequences[i].beatInfo[x].playShort = false;
				sequences[i].beatInfo[x].volume = 1.0f;
			}
			sequenceNewColor.r += colorChange;
			if (sequenceNewColor.r < 0.51f) {sequenceNewColor.g += (colorChange * 2); sequenceNewColor.b += colorChange;}
			else {sequenceNewColor.g -= (colorChange); sequenceNewColor.b -= colorChange * 2;}
		}

		//Create texture that displays behind sequence selector
		for (int i = 0; i < seqTex.width; i++)
		{
			if(i > testLocation + (int)(seqTex.width / sequenceTotal) && testSeq < sequences.Length - 1)
			{
				testLocation += (int)(seqTex.width / sequenceTotal);
				testSeq += 1;
			}
			seqTex.SetPixel(i, 0, sequences[testSeq].sequenceColor);
			seqTex.Apply();
		}

		for (int i = 0; i < blueBackRects.Length; i++)
		{
			blueBackRects[i] = new Rect(gridRect.x + (i * blueBackSpacing), gridRect.y, blueBackWidth, gridRect.height);
		}

		//Create Song Manager, Track List, and Song List GameObjects if they do not exist
		if (GameObject.Find("Song Manager") == null)
		{
			songManagerObject = new GameObject("Song Manager");
			songManagerObject.AddComponent<NSLSongManager>();
		}
		else
		{
			songManagerObject = GameObject.Find("Song Manager");
			if(songManagerObject.GetComponent<NSLSongManager>() == null){ songManagerObject.AddComponent<NSLSongManager>(); }
		}

		if (GameObject.Find("Track List") == null)
		{
			trackListObject = new GameObject("Track List");
		}
		else {trackListObject = GameObject.Find("Track List");}

		if (GameObject.Find("NSL Songs") == null)
		{
			songListObject = new GameObject("NSL Songs");
		}
		else {songListObject = GameObject.Find("NSL Songs");}

		trackListObject.transform.parent = songManagerObject.transform;
		songListObject.transform.parent = songManagerObject.transform;
		
		//Create sample import, volume slider, and Piano Roll selector rects. Add AudioSources.
		for (int i = 0; i < trackTotal; i++) 
		{
			stopOnNext[i] = false;
			sampleRects[i] = new Rect(100, gridRect.y + (i * sampleRectHeight), 70, sampleRectHeight);
			sampleTex[i] = sampleEmptyTex;
			pianoRollSwitchRects[i] = new Rect(10, gridRect.y + (i * sampleRectHeight), 80, sampleRectHeight / 2);
			volumeSliderRects[i] = new Rect(10, gridRect.y + (i * sampleRectHeight + (sampleRectHeight / 2)), 80, sampleRectHeight / 2);
			trackVolumes[i] = 1.0f;

			if (GameObject.Find("Audio Track " + (i+1).ToString()) != null)
			{
				audioObjs[i] = GameObject.Find("Audio Track " + (i+1).ToString());
				if (audioObjs[i].GetComponent<AudioSource>() == null) { audioSources[i] = audioObjs[i].AddComponent<AudioSource>(); }
				else { audioSources[i] = audioObjs[i].GetComponent<AudioSource> (); }

				if (audioObjs[i].GetComponent<NSLTrackReceiver>() == null) { audioObjs[i].AddComponent<NSLTrackReceiver>(); }

				newReceiver = audioObjs[i].GetComponent<NSLTrackReceiver>();
				newReceiver.audioSource = audioObjs[i].GetComponent<AudioSource>();
				newReceiver.songTrack = i;

				audioSources[i].playOnAwake = false;
				audioSources[i].dopplerLevel = 0.0f;
			}

			else
			{
				audioObjs[i] = new GameObject("Audio Track " + (i+1).ToString());
				audioObjs[i].transform.parent = trackListObject.transform;
				audioSources[i] = audioObjs[i].AddComponent<AudioSource> ();
				audioObjs[i].AddComponent<NSLTrackReceiver>();
				audioSources[i].playOnAwake = false;
				audioSources[i].dopplerLevel = 0.0f;

				newReceiver = audioObjs[i].GetComponent<NSLTrackReceiver>();
				newReceiver.audioSource = audioObjs[i].GetComponent<AudioSource>();
				newReceiver.songTrack = i;
			}
		}

		testLocation = 0;

		//Create individual step volume sliders for the piano roll
		for (int i = 0; i < bpMeasure; i++)
		{
			stepSliderRects[i] = new Rect (190 + testLocation, 140 + gridRect.height, (int) (gridRect.width / bpMeasure), 50);
			testLocation += (int)(gridRect.width / bpMeasure) + 1;
			stepSliderVolumes[i] = 1.0f;
		}
		//Load first sequence
		LoadSequence (0);

		//Specific invalid value to identify first Arrangement placement
		songArrangement.Add (500);
	}	

	//Runs when an item is selected on the Sequence Selector, sets current sequence and updates step icons
	private void LoadSequence(int seq)
	{
		currentSequence = seq;

		//load step sequencer icons
		if(!pianoRollOn)
		{
			for (int i = 0; i < stepLength; i++) 
			{
				if(sequences[currentSequence].beatInfo[i].fireBeat)
				{
					selectTex[i] = buttonTex2;
				}

				if(!sequences[currentSequence].beatInfo[i].fireBeat)
				{
					selectTex[i] = buttonTex1;
				}
			}
		}

		//load piano roll icons
		if(pianoRollOn)
		{
			LoadPianoRoll(currentTrack, seq);
		}
	}

	//Identifies inputs inside selection grids (alternative to checking for EventTpye.MouseUp/Down to avoid registering multiple clicks)
	private string GetGridMouse(int currentHotControl, int selected, string type, int mButton)
	{
		string mouseState = "None";

		if(currentHotControl > 0 && previousHotControl == 0)
		{
			mouseState = "Down";
		}
		if(currentHotControl > 0 && previousHotControl > 0)
		{
			mouseState = "Held";
		}
		if(currentHotControl == 0 && previousHotControl > 0)
		{
			mouseState = "Up";

			if(type == "seqSet" && mButton == 0) {SetBeat(selected);}
			if(type == "seqSet" && mButton == 1) {RemoveBeat(selected);}
			if(type == "seqSelect" && mButton == 0) {LoadSequence(selected);}
			if(type == "noteSet" && mButton == 0) {SetNote(selected, "Set");}
			if(type == "noteSet" && mButton == 1) {SetNote(selected, "Remove");}
		}

		previousHotControl = currentHotControl;

		return mouseState;
	}

	//Right click removes a pattern in the Arranger
	private void RemoveSequenceFromSong(float mouseX)
	{
		partPlaying = false;
		songPlaying = false;

		float rectX = mouseX - (float)songRect.x;
		int newPosition = 0;
		int testLocation = 0;
		int setPixelWidth = (int)(songPixelWidth * songRect.width)/songTex.width;

		//Determine where the user has clicked
		for (int i = 0; i < songArrangement.Count; i++)
		{
			if (rectX > testLocation && rectX <= (testLocation + setPixelWidth))
			{
				newPosition = i;
			}
						
			testLocation += setPixelWidth;
		}

		//Remove sequence from songArrangement List<T> and regenerate texture
		//if (songArrangement[newPosition] != null && songArrangement.Count > 1)
		if (songArrangement.Count > 1)
		{
			songArrangement.RemoveAt(newPosition);
			songPixelWidth = (int)(songTex.width / songArrangement.Count);
			testLocation = 0;
			
			for (int i = 0; i < songArrangement.Count; i++)
			{
				for (int x = 0; x < songPixelWidth; x++)
				{
					if ((testLocation + x) < songTex.width)
					{
						if (x <= 3) {songTex.SetPixel (testLocation, 0, Color.black);}
						songTex.SetPixel (testLocation + x, 0, sequences [songArrangement [i]].sequenceColor);
					}
				}
				testLocation += songPixelWidth;
			}
			songTex.Apply();
		}
	}

	//Left Click adds a pattern to the Arranger
	private void AddSequenceToSong(int seq, float mouseX)
	{
		partPlaying = false;
		songPlaying = false;

		float rectX = mouseX - (float)songRect.x;
		int newPosition = 0;
		int testLocation = 0;
		int setPixelWidth = (int)(songPixelWidth * songRect.width)/songTex.width;
		Color tempColor = Color.black;
		tempColor.a = 0.0f;

		//Determine if this is the first pattern being added to the Arranger, add and regenerate texture
		if(songArrangement[0] == 500)
		{
			songArrangement[0] = seq;
			songPixelWidth = songTex.width;			
			for (int i = 0; i < songTex.width; i++)
			{
				songTex.SetPixel(i, 0, sequences[seq].sequenceColor);
			}
			songTex.Apply();
			return;
		}

		//If this is not the first addition, determine where the user is clicking
		if (songArrangement [0] != 500)
		{
			for (int i = 0; i < songArrangement.Count; i++)
			{
				if (rectX > testLocation && rectX <= (testLocation + (setPixelWidth / 2)))
				{
					newPosition = i;
				}

				if (rectX > (testLocation + (setPixelWidth / 2)) && rectX <= (testLocation + setPixelWidth))
				{
					newPosition = i + 1;
				}

				testLocation += setPixelWidth;
			}

			//Add sequence to songArrangement and regenerate texture
			songArrangement.Insert (newPosition, seq);
			songPixelWidth = (int)(songTex.width / songArrangement.Count);
			testLocation = 0;

			for (int i = 0; i < songArrangement.Count; i++)
			{
				for (int x = 0; x < songPixelWidth; x++)
				{
					if ((testLocation + x) < songTex.width)
					{
						if (x <= 1) {songTex.SetPixel (testLocation, 0, tempColor);}
						songTex.SetPixel (testLocation + x, 0, sequences [songArrangement [i]].sequenceColor);
					}
				}
				testLocation += songPixelWidth;
			}
			songTex.Apply();
		}
	}

	//remove BeatInfo for selected step
	private void RemoveBeat(int selected)
	{
		int seqTrack = 0;
		int testLocation = 0;
		for (int i = 0; i < trackTotal; i++)
		{
			if(selected >= testLocation && selected < testLocation + bpMeasure){seqTrack = i;}
			testLocation += bpMeasure;
		}

		if (selectTex[selected] == buttonTex2)
		{
			selectTex[selected] = buttonTex1;
			sequences[currentSequence].beatInfo[selected].fireBeat = false;
			sequences[currentSequence].beatInfo[selected].seqTrack = seqTrack;
			sequences[currentSequence].beatInfo[selected].rollLocation = (selected - (seqTrack * bpMeasure));
		}
	}
	//Set BeatInfo for selected step
	private void SetBeat (int selected)
	{
		int seqTrack = 0;
		int testLocation = 0;
		for (int i = 0; i < trackTotal; i++)
		{
			if(selected >= testLocation && selected < testLocation + bpMeasure){seqTrack = i;}
			testLocation += bpMeasure;
		}

		if (selectTex[selected] == buttonTex1)
		{
			selectTex[selected] = buttonTex2;
			sequences[currentSequence].beatInfo[selected].fireBeat = true;
			sequences[currentSequence].beatInfo[selected].seqTrack = seqTrack;
			sequences[currentSequence].beatInfo[selected].pitch = 1;
			sequences[currentSequence].beatInfo[selected].rollLocation = (selected - (seqTrack * bpMeasure) + (12 * bpMeasure));
		}
	}

	//Sets a note in the piano roll, similar to adding a step, but also includes pitch setting
	private void SetNote (int selected, string noteAction)
	{
		//find note
		int testLocation = 0;
		int foundNote = 0;

		for (int i = 0; i < pianoKeyCount; i++)
		{
			if(selected >=testLocation && selected < testLocation + bpMeasure)
			{
				foundNote = i;			
			}
			testLocation += bpMeasure;
		}

		testLocation = (foundNote * bpMeasure);
		foundNote -= 12;
		foundNote = foundNote * -1;

		//find step location
		int stepLocation = (selected - testLocation) + (currentTrack * bpMeasure);
		float newPitch = Mathf.Pow(1.05946f, foundNote);

		if (!sequences[currentSequence].beatInfo[stepLocation].fireBeat && !sequences[currentSequence].beatInfo[stepLocation].stopBeat && noteAction == "Remove") { noteAction = "Stop"; }

		if (noteAction == "Set")
		{
			pianoRollTex[sequences[currentSequence].beatInfo[stepLocation].rollLocation] = null;
			sequences[currentSequence].beatInfo[stepLocation].fireBeat = true;
			sequences[currentSequence].beatInfo[stepLocation].stopBeat = false;
			sequences[currentSequence].beatInfo[stepLocation].pitch = newPitch;
			sequences[currentSequence].beatInfo[stepLocation].volume = stepSliderVolumes[stepLocation - (currentTrack * bpMeasure)];
			sequences[currentSequence].beatInfo[stepLocation].seqTrack = currentTrack;
			sequences[currentSequence].beatInfo[stepLocation].rollLocation = selected;
			pianoRollTex[selected] = pianoButtonTex2;
			audioSources[currentTrack].pitch = newPitch;
			audioSources[currentTrack].volume = 1.0f;
			audioSources[currentTrack].Play();
		}

		if (noteAction == "Remove")
		{
			pianoRollTex[selected] = null;
			sequences[currentSequence].beatInfo[stepLocation].fireBeat = false;
			sequences[currentSequence].beatInfo[stepLocation].stopBeat = false;
			sequences[currentSequence].beatInfo[stepLocation].pitch = 1.0f;
		}

		if (noteAction == "Stop")
		{
			pianoRollTex[sequences[currentSequence].beatInfo[stepLocation].rollLocation] = null;
			sequences[currentSequence].beatInfo[stepLocation].fireBeat = false;
			sequences[currentSequence].beatInfo[stepLocation].stopBeat = true;
			sequences[currentSequence].beatInfo[stepLocation].pitch = newPitch;
			sequences[currentSequence].beatInfo[stepLocation].volume = stepSliderVolumes[stepLocation - (currentTrack * bpMeasure)];
			sequences[currentSequence].beatInfo[stepLocation].seqTrack = currentTrack;
			sequences[currentSequence].beatInfo[stepLocation].rollLocation = selected;
			pianoRollTex[selected] = pianoButtonTex3;
			audioSources[currentTrack].pitch = newPitch;
			audioSources[currentTrack].volume = 1.0f;
		}
	}

	//Adds drag-dropped sample to sample array
	private void LoadSample(int samp, Object obj)
	{
		samples[samp] = (AudioClip)obj;
		audioSources[samp].clip = samples [samp];
	}

	//Preview sample
	private void PlayBeat(int samp)
	{
		if(samples[samp] != null)
		{
			audioSources[0].PlayOneShot(samples[samp]);
		}
	}

	private void LoadPianoRoll(int track, int seq)
	{
		for (int i = 0; i < pianoRollTex.Length; i++)
		{
			pianoRollTex[i] = null;
		}

		int firstStep = track * bpMeasure;

		for (int i = 0; i < bpMeasure; i++)
		{
			if(sequences[seq].beatInfo[firstStep + i].fireBeat)
			{
				pianoRollTex[sequences[seq].beatInfo[firstStep + i].rollLocation] = pianoButtonTex2;
				stepSliderVolumes[i] = sequences[seq].beatInfo[firstStep + i].volume;
			}

			if(sequences[seq].beatInfo[firstStep + i].stopBeat)
			{
				pianoRollTex[sequences[seq].beatInfo[firstStep + i].rollLocation] = pianoButtonTex3;
				stepSliderVolumes[i] = sequences[seq].beatInfo[firstStep + i].volume;
			}
		}
	}

	//Allow song title to be dragged and dropped
	private void DragSongTitle(Event gEvent)
	{

		if(songTitleRect.Contains(gEvent.mousePosition))
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			
			if(gEvent.type == EventType.DragPerform)
			{
				DragAndDrop.AcceptDrag();
				gEvent.Use();
				//GameObject tempSongObject = DragAndDrop.objectReferences[0].GetType();
				songTitle = DragAndDrop.objectReferences[0].ToString();
				songTitle = songTitle.Replace(" (UnityEngine.GameObject)", "");
				LoadSong(songTitle);
			}
		}
	}

	//Allow samples to be dropped into sequencer
	private void SampleWindows(int samp, Event gEvent)
	{
		if(sampleRects[samp].Contains(gEvent.mousePosition))
		{
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			
			if(gEvent.type == EventType.MouseUp)
			{
				PlayBeat(samp);
			}
			
			if(gEvent.type == EventType.DragPerform)
			{
				sampleTex[samp] = sampleLoadedTex;
				DragAndDrop.AcceptDrag();
				gEvent.Use();
				LoadSample(samp, DragAndDrop.objectReferences[0]);
			}
		}
	}

	private void LoadSong(string loadTitle)
	{
		//Look for a GameObject matching the Song Title
		if (GameObject.Find(songTitle) == null)
		{
			if (EditorUtility.DisplayDialog("Hmmmm...", "Unable to locate " + songTitle + ". Veriify that a GameObject with a matching name exists", "OK"))
			{
				return;
			}
		}			 
			
		GameObject loadSongObject = GameObject.Find (songTitle);
		NSLSong loadSong = loadSongObject.GetComponent<NSLSong>();

		//If selected object does not contain NSL Song component
		if (loadSong == null)
		{
			if (EditorUtility.DisplayDialog("Hmmmm...", songTitle + " does not contain an NSL Song component.", "OK"))
			{
				return;
			}
		}

		//if selected song was created with a different sequencer
		if (loadSong.sequencerType != sequencerType)
		{
			if (EditorUtility.DisplayDialog("Hmmmm...", songTitle + " was created in Sequencer Type " + loadSong.sequencerType + ".  Please open a new " + loadSong.sequencerType + " window to load this song.", "OK"))
			{
				return;
			}
		}

		//Verify Overwite
		if (!EditorUtility.DisplayDialog("Are you sure?", "Unsaved changes to the current song will be lost.  Do you want to continue loading the new song", "Load Song", "Cancel"))
		{
			return;
		}

		bpMinute = loadSong.tempo;
		beatTime = 15 / bpMinute;
		tempoInput = bpMinute.ToString();
		bpMeasure = loadSong.beatsPerMeasure;
		trackTotal = loadSong.sequenceDetails[0].sequenceTracks.Length;
		sequences = new SongSequence[loadSong.sequenceDetails.Length];
		trackVolumes = loadSong.trackVolumes;
		samples = new AudioClip[trackTotal];
		songArrangement.Clear();
		currentTrack = 0;
		currentSequence = 0;
		sequenceGrid = 0;

		float colorChange = 1.0f / sequenceTotal;
		Color sequenceNewColor = Color.blue;
		sequenceNewColor.b = 0.5f;
		
		//Load sequences
		for (int i = 0; i < sequences.Length; i++)
		{
			sequences[i] = new SongSequence();
			sequences[i].beatInfo = new BeatInfo[bpMeasure * trackTotal];
			sequences[i].sequenceColor = sequenceNewColor;
			sequenceNewColor.r += colorChange;
			if (sequenceNewColor.r < 0.51f) {sequenceNewColor.g += (colorChange * 2); sequenceNewColor.b += colorChange;}
			else {sequenceNewColor.g -= (colorChange); sequenceNewColor.b -= colorChange * 2;}
			
			for (int x = 0; x < trackTotal; x++)
			{
				samples[x] = loadSong.sequenceDetails[i].sequenceTracks[x].audioClip;
				
				for (int y = 0; y < bpMeasure; y++)
				{
					sequences[i].beatInfo[(x * bpMeasure) + y] = new BeatInfo();
					sequences[i].beatInfo[(x * bpMeasure) + y].fireBeat = loadSong.sequenceDetails[i].sequenceTracks[x].steps[y].fireBeat;
					sequences[i].beatInfo[(x * bpMeasure) + y].stopBeat = loadSong.sequenceDetails[i].sequenceTracks[x].steps[y].stopBeat;
					sequences[i].beatInfo[(x * bpMeasure) + y].volume = loadSong.sequenceDetails[i].sequenceTracks[x].steps[y].volume;
					sequences[i].beatInfo[(x * bpMeasure) + y].pitch = loadSong.sequenceDetails[i].sequenceTracks[x].steps[y].pitch;
					sequences[i].beatInfo[(x * bpMeasure) + y].rollLocation = loadSong.sequenceDetails[i].sequenceTracks[x].steps[y].rollLocation;
					sequences[i].beatInfo[(x * bpMeasure) + y].seqTrack = x;
				}
			}
		}

		//Load song arrangement
		for (int i = 0; i < loadSong.songArrangement.Length; i++)
		{
			songArrangement.Add(loadSong.songArrangement[i]);
		}

		//Change textures for loaded samples
		for (int i = 0; i < trackTotal; i++)
		{
			if (samples[i] != null) {sampleTex[i] = sampleLoadedTex; }
			else  { sampleTex[i] = sampleEmptyTex; }
		}

		pianoRollOn = false;
		LoadSequence(0);

		int testLocation = 0;
		Color tempColor = Color.black;
		tempColor.a = 0.0f;

		if (songArrangement[0] != 500)
		{
			songPixelWidth = (int)(songTex.width / songArrangement.Count);
			
			for (int i = 0; i < songArrangement.Count; i++)
			{
				for (int x = 0; x < songPixelWidth; x++)
				{
					if ((testLocation + x) < songTex.width)
					{
						if (x <= 1) {songTex.SetPixel (testLocation, 0, tempColor);}
						songTex.SetPixel (testLocation + x, 0, sequences [songArrangement [i]].sequenceColor);
					}
				}
				testLocation += songPixelWidth;
			}
			songTex.Apply();
		}
	}
	
	//Save current song to a GameObject
	private void SaveSong(string newTitle)
	{
		//Warn about missing song arrangement
		if (songArrangement[0] == 500 && newTitle != "BackupSave " + sequencerType)
		{
			if (!EditorUtility.DisplayDialog("FYI", "No sequences have been added to the arrangement for " + songTitle + ". This song will not play during runtime.  To add to the arragement, select a sequence and click the gray box at the bottom of the window.", "OK"))
			{
			}
		}

		NSLSong newSong;
		int trackCount = 0;

		if (GameObject.Find(newTitle) == null)
		{
			saveSongObject = new GameObject(newTitle);
			saveSongObject.transform.parent = songListObject.transform;
			newSong = saveSongObject.AddComponent<NSLSong>();
		}

		else if (newTitle == "BackupSave " + sequencerType)
		{
			saveSongObject = GameObject.Find(newTitle);
			newSong = saveSongObject.GetComponent<NSLSong>();
		}

		else 
		{
			if (EditorUtility.DisplayDialog("Wait a sec!", "A song titled " + newTitle + " already exists.  Do you want to overwrite this song?", "Overwrite", "Cancel"))
			{
				saveSongObject = GameObject.Find(newTitle);
				newSong = saveSongObject.GetComponent<NSLSong>();
			}
			else { return; }
		}

		int stepCounter = 0;

		for (int i = 0; i < samples.Length; i++)
		{
			if (samples[i] != null) { trackCount++; }
		}

		newSong.tempo = bpMinute;
		newSong.beatsPerMeasure = bpMeasure;
		newSong.tracks = new TrackInfo[trackCount];
		newSong.sequenceDetails = new SequenceDetail[sequenceTotal];
		newSong.songArrangement = new int[songArrangement.Count];
		newSong.trackVolumes = trackVolumes;
		newSong.sequencerType = sequencerType;

		//Save each individual sequence
		for (int i = 0; i < sequenceTotal; i++)
		{
			newSong.sequenceDetails[i] = new SequenceDetail();
			newSong.sequenceDetails[i].sequenceTracks = new TrackInfo[trackTotal];

			for (int x = 0; x < trackTotal; x++)
			{
				newSong.sequenceDetails[i].sequenceTracks[x] = new TrackInfo();
				newSong.sequenceDetails[i].sequenceTracks[x].audioClip = samples[x];
				newSong.sequenceDetails[i].sequenceTracks[x].steps = new StepInfo[bpMeasure];

				for (int y = 0; y < bpMeasure; y++)
				{
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y] = new StepInfo();
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y].fireBeat = sequences[i].beatInfo[(bpMeasure * x) + y].fireBeat;
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y].stopBeat = sequences[i].beatInfo[(bpMeasure * x) + y].stopBeat;
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y].volume = sequences[i].beatInfo[(bpMeasure * x) + y].volume;
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y].pitch = sequences[i].beatInfo[(bpMeasure * x) + y].pitch;
					newSong.sequenceDetails[i].sequenceTracks[x].steps[y].rollLocation = sequences[i].beatInfo[(bpMeasure * x) + y].rollLocation;
				}
			}
		}

		trackCount = 0;

		//Save entire song
		if(songArrangement[0] != 500)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				stepCounter = 0;

				if (samples[i] != null)
				{
					newSong.tracks[trackCount] = new TrackInfo();
					newSong.tracks[trackCount].trackNumber = i;
					newSong.tracks[trackCount].audioClip = samples[i];
					newSong.tracks[trackCount].steps = new StepInfo[songArrangement.Count * bpMeasure];
					for (int x = 0; x < songArrangement.Count; x++)
					{
						for (int y = 0; y < bpMeasure; y++)
						{
							newSong.tracks[trackCount].steps[stepCounter] = new StepInfo();
							newSong.tracks[trackCount].steps[stepCounter].fireBeat = sequences[songArrangement[x]].beatInfo[(bpMeasure * i) + y].fireBeat;
							newSong.tracks[trackCount].steps[stepCounter].stopBeat = sequences[songArrangement[x]].beatInfo[(bpMeasure * i) + y].stopBeat;
							newSong.tracks[trackCount].steps[stepCounter].volume = sequences[songArrangement[x]].beatInfo[(bpMeasure * i) + y].volume * trackVolumes[i];
							newSong.tracks[trackCount].steps[stepCounter].pitch = sequences[songArrangement[x]].beatInfo[(bpMeasure * i) + y].pitch;
							newSong.tracks[trackCount].steps[stepCounter].rollLocation = sequences[songArrangement[x]].beatInfo[(bpMeasure * i) + y].rollLocation;
							stepCounter++;
						}
					}
					trackCount++;
				}
			}
		}

		//Save arrangement
		for (int i = 0; i < songArrangement.Count; i++)
		{
			newSong.songArrangement[i] = songArrangement[i];
		}
	}

	private void SaveAndClose()
	{
		this.Close();
	}
	
	void OnGUI () {

		if (!windowInitialized) { return; }

		if (windowInitialized)
		{
			Event gEvent = Event.current;
			int testTempoInput = 0;
			stepStyle = GUI.skin.button;
			stepStyle.normal.background.alphaIsTransparency = true;
			stepStyle.normal.background = buttonBack;
			stepStyle.fontSize = 14;

			GUI.DrawTexture (new Rect(0,0,200 + gridRect.width, 200 + gridRect.height), blackBackground);

			if(GUI.Button(new Rect(10,9,160,30), "Play Sequence"))
			{
				partPlaying = true;
				songPlaying = false;
				beatCount = 0;
				nextBeat = AudioSettings.dspTime + (beatTime * 2);
			}

			if(GUI.Button(new Rect(10,42,160,30), "Play Song"))
			{
				if (songArrangement[0] == 500)
				{
					if (EditorUtility.DisplayDialog("FYI", "No sequences have been added to the arrangement for " + songTitle + ". To add to the arragement, select a sequence and click the gray box at the bottom of the window.", "OK"))
					{
						return;
					}
				}
				partPlaying = false;
				songPlaying = true;
				playSequence = 0;
				beatCount = 0;
				playSequence = 0;
				nextBeat = AudioSettings.dspTime + (beatTime * 2);
			}
			if(GUI.Button(new Rect(10,75,160,30), "Stop Playback"))
			{
				partPlaying = false;
				songPlaying = false;
				beatCount = 0;
			}

			songTitle = GUI.TextField(songTitleRect, songTitle, 35);
			DragSongTitle(gEvent);

			if(GUI.Button(new Rect(440,10,100,30), "Save"))
			{
				partPlaying = false;
				songPlaying = false;
				SaveSong(songTitle);
			}

			if(GUI.Button(new Rect(550,10,100,30), "Load"))
			{
				partPlaying = false;
				songPlaying = false;
				LoadSong(songTitle);
			}

			GUI.Label (new Rect (660, 20, 55, 20), "BPM");

			GUI.SetNextControlName ("TempoText");
			tempoInput = GUI.TextField(new Rect(695, 15, 50, 20), tempoInput, 35);

			if (gEvent.isKey && gEvent.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "TempoText")
			{
				if(int.TryParse(tempoInput, out testTempoInput))
				{
					tempoInput = testTempoInput.ToString();
					bpMinute = (float)testTempoInput;
					beatTime = 15 / bpMinute;
				}
				else { tempoInput = bpMinute.ToString(); }
			}

			if(GUI.Button(new Rect(735,10,50,30), "Set"))
			{
				if(int.TryParse(tempoInput, out testTempoInput))
				{
					tempoInput = testTempoInput.ToString();
					bpMinute = (float)testTempoInput;
					beatTime = 15 / bpMinute;
				}
				else { tempoInput = bpMinute.ToString(); }
			}

			GUI.DrawTexture (sequenceBackRect, seqTex);

			//Piano Roll Mode
			if(pianoRollOn)
			{
				GUI.DrawTexture (gridRect, pianoBack);
				GUI.DrawTexture (pianoKeysRect, pianoKeys);

				for (int i = 0; i < blueBackRects.Length; i++)
				{
					GUI.DrawTexture (blueBackRects[i], blueBackground);
				}

				stepStyle.normal.background = pianoButtonTex1;
				pianoRollSelection = GUI.SelectionGrid (gridRect, pianoRollSelection, pianoRollTex, bpMeasure, stepStyle);
				stepStyle.normal.background = buttonBack;

				if(GUI.Button(new Rect(10,gridRect.height + 80,100,30), "Sequencer"))
				{
					pianoRollOn = false;
					LoadSequence(currentSequence);
				}

				//When a note is clicked
				if (gridRect.Contains(Event.current.mousePosition) && gEvent.type == EventType.Used)
				{
					GetGridMouse(GUIUtility.hotControl, pianoRollSelection, "noteSet", gEvent.button);
				}

				//StepVolume Sliders
				for (int i = 0; i < bpMeasure; i++)
				{
					stepSliderVolumes[i] = GUI.VerticalSlider(stepSliderRects[i], stepSliderVolumes[i], 1.0f, 0.1f);
					sequences[currentSequence].beatInfo[(currentTrack * bpMeasure) + i].volume = stepSliderVolumes[i];
				}
			}

			//Step Sequencer Mode
			if(!pianoRollOn)
			{
				for (int i = 0; i < blueBackRects.Length; i++)
				{
					GUI.DrawTexture (blueBackRects[i], blueBackground);
				}

				GUI.DrawTexture (songRect, songTex);
				
				//Overlay sequence numbers on the Song Arranger
				if (songArrangement[0] != 500)
					{
					for (int i = 0; i < songArrangement.Count; i++)
					{
						int seqNum = songArrangement[i] + 1;
						int setPixelWidth = (int)(songPixelWidth * songRect.width)/songTex.width;
						if (i == 0) {GUI.Label(new Rect((int) 20 + (setPixelWidth / 2), 150 + gridRect.height, 20, 20), seqNum.ToString(), stepStyle);}
						else {GUI.Label(new Rect((int)(20 + (i * setPixelWidth)) + (setPixelWidth / 2), 150 + gridRect.height, 20, 20), seqNum.ToString(), stepStyle);}
					}
				}
				
				//Detect left click on the Song Arranger
				if (songRect.Contains(gEvent.mousePosition) && gEvent.type == EventType.MouseDown && gEvent.button == 0)
				{
					AddSequenceToSong(currentSequence, gEvent.mousePosition.x);
					Repaint();
				}
				
				//Detect right click  on the Song Arranger
				if (songRect.Contains(gEvent.mousePosition) && gEvent.type == EventType.MouseDown && gEvent.button == 1)
				{
					RemoveSequenceFromSong(gEvent.mousePosition.x);
					Repaint();
				}

				//Piano Roll buttons and track volume sliders
				for (int i = 0; i < trackTotal; i++)
				{
					if(GUI.Button(pianoRollSwitchRects[i], "Roll", stepStyle))
					{
						pianoRollOn = true;
						currentTrack = i;
						LoadPianoRoll(currentTrack, currentSequence);
					}

					trackVolumes[i] = GUI.HorizontalSlider(volumeSliderRects[i], trackVolumes[i], 0.2f, 1.0f);
				}

				//Step sequencer grid selection, represents all steps for all tracks in a single sequence/pattern
				selectGrid = GUI.SelectionGrid(gridRect, selectGrid, selectTex, bpMeasure, stepStyle);

				//When a step is clicked
				if (gridRect.Contains(gEvent.mousePosition) && gEvent.type == EventType.Used)
				{
					GetGridMouse(GUIUtility.hotControl, selectGrid, "seqSet", gEvent.button);
				}		

				//Displays BeatInfo overlayed on sequencer selection grid.  Change "pitch" in "sequences[currentSequence].beatInfo[i].pitch.ToString()" to see a different parameter.
				/*
				for (int i = 0; i < stepLength; i++)
				{
					if(i==stepsPer){labelTop += ((int) gridRect.height / trackTotal); labelLeft = 180; stepsPer += bpMeasure;}
					GUI.Label(new Rect(labelLeft, labelTop, (int)gridRect.width / bpMeasure, (int) gridRect.height / trackTotal), sequences[currentSequence].beatInfo[i].pitch.ToString());
					labelLeft += ((int)gridRect.width / bpMeasure) +1;
				}
				*/

				//Draws sample drag-drop areas, labels the track with sample name if it exists
				for (int i = 0; i < trackTotal; i++)
				{
					GUI.DrawTexture (sampleRects[i], sampleTex[i]);
					if(samples[i] != null){GUI.Label (sampleRects[i], samples[i].ToString());}
					SampleWindows(i, gEvent);
				}
			}

			//The Sequence Selector selection grid
			sequenceGrid = GUI.SelectionGrid (sequenceRect, sequenceGrid, sequenceStrings, sequenceTotal);

			//When Sequence Selector is clicked
			if (sequenceRect.Contains(gEvent.mousePosition))
			{
				GetGridMouse(GUIUtility.hotControl, sequenceGrid, "seqSelect", gEvent.button);
			}
		}
	}

	void Update()
	{
		if (EditorApplication.isPlayingOrWillChangePlaymode)
		{
			SaveAndClose();
		}
		//Play a single pattern/sequence
		if(partPlaying && AudioSettings.dspTime >= nextBeat + (beatTime/2))
		{
			if(beatCount == bpMeasure) {beatCount = 0;}
			nextBeat += beatTime;
			int realBeat = 0;

			for (int i = 0; i < trackTotal - 1; i++)
			{
				if(stopOnNext[i]) { audioSources[i].Stop(); stopOnNext[i] = false;}

				if(sequences[currentSequence].beatInfo[beatCount + realBeat].fireBeat && samples[i] != null)
				{
					audioSources[i].pitch = sequences[currentSequence].beatInfo[beatCount + realBeat].pitch;
					audioSources[i].volume = sequences[currentSequence].beatInfo[beatCount + realBeat].volume * trackVolumes[i];
					audioSources[i].PlayScheduled(nextBeat);
				}

				else if(sequences[currentSequence].beatInfo[beatCount + realBeat].stopBeat && samples[i] != null)
				{
					stopOnNext[i] = true;
				}

				realBeat += bpMeasure;
			}
			beatCount++;
		}

		//Play entire Song Arrangement
		if(songPlaying && AudioSettings.dspTime >= nextBeat + (beatTime/2))
		{
			if(beatCount == bpMeasure)
			{
				beatCount = 0;
				playSequence++;
				if (playSequence == songArrangement.Count){playSequence = 0;}
			}
			nextBeat += beatTime;

			int realBeat = 0;
			
			for (int i = 0; i < trackTotal - 1; i++)
			{
				if(sequences[songArrangement[playSequence]].beatInfo[beatCount + realBeat].fireBeat && samples[i] != null)
				{
					audioSources[i].pitch = sequences[songArrangement[playSequence]].beatInfo[beatCount + realBeat].pitch;
					audioSources[i].volume = sequences[songArrangement[playSequence]].beatInfo[beatCount + realBeat].volume * trackVolumes[i];
					audioSources[i].PlayScheduled(nextBeat);
				}

				else if(sequences[currentSequence].beatInfo[beatCount + realBeat].stopBeat && samples[i] != null)
				{
					audioSources[i].Stop();
				}

				realBeat += bpMeasure;
			}			
			beatCount++;
		}
	}

	//Current song info saved to a backup whenever the window is closed
	void OnDisable()
	{
		DestroyImmediate (songTex);
		DestroyImmediate (seqTex);
		if (windowInitialized) { this.SaveSong("BackupSave " + sequencerType); }
	}
}