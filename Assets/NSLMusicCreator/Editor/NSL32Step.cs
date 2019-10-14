using UnityEngine;
using UnityEditor;

public class NSL32Step : EditorWindow {

	[MenuItem ("Window/NSL Music Creator/32 Step 4|8")]

	static void Init ()
	{
		int zBPMe = 32;
		int zTrackTotal = 12;
		int zSequenceTotal = 20;
		int zTimingBeat = 4;
		float zStepWidth = 33.75f;
		float zStepHeight = 40.0f;
		string zSequencerType = "32 Step 4|8";

		NSLMusicCreator window = (NSLMusicCreator)EditorWindow.GetWindow (typeof (NSLMusicCreator));
		window.position = new Rect (20, 20, 200 + (int) (zStepWidth * zBPMe), 200 + (int) (zStepHeight * zTrackTotal));
		window.initRun = true;
		window.SetSequenceParameters(zBPMe, zTrackTotal, zSequenceTotal, zTimingBeat, zStepWidth, zStepHeight, zSequencerType);
	}
}
