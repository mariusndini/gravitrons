using UnityEngine;
using UnityEditor;

public class NSL16Step : EditorWindow {

	[MenuItem ("Window/NSL Music Creator/16 Step 4|4")]
	static void Init ()
	{
		int zBPMe = 16;
		int zTrackTotal = 8;
		int zSequenceTotal = 12;
		int zTimingBeat = 4;
		float zStepWidth = 37.5f;
		float zStepHeight = 50.0f;
		string zSequencerType = "16 Step 4|4";

		NSLMusicCreator window = (NSLMusicCreator)EditorWindow.GetWindow (typeof (NSLMusicCreator));
		window.position = new Rect (20, 20, 200 + (int) (zStepWidth * zBPMe), 200 + (int) (zStepHeight * zTrackTotal));
		window.initRun = true;
		window.SetSequenceParameters(zBPMe, zTrackTotal, zSequenceTotal, zTimingBeat, zStepWidth, zStepHeight, zSequencerType);
	}
}
