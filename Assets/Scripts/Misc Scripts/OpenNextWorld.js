#pragma strict

var StarsNeededToUnlock:int = 32;
var LevelsNeededToUnlock:int = 20;

function Start () {

	if (globals.totalStarCount >= StarsNeededToUnlock || globals.maxLevelCompleted >= LevelsNeededToUnlock){
		this.collider.enabled = true;

	}else{
		iTween.FadeTo(this.gameObject, {"alpha":0.50, "includechildren":true, "time":0.2});
		this.gameObject.collider.enabled = false;
		Destroy(GetComponent(Animator));
	}
}



