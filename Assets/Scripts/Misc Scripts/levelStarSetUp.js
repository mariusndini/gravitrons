#pragma strict

var star_1:GameObject;
var star_2:GameObject;
var star_3:GameObject;

private var level:int;
private var hasKey:boolean;
private var score:int;

		
function Awake(){
	level = int.Parse(this.transform.name);
	hasKey = PlayerPrefs.HasKey(level+"");
	score = PlayerPrefs.GetInt(level+"");
	
	if(hasKey){
		globals.levelsToUnlock = Mathf.Max(level + score, globals.levelsToUnlock);
		globals.totalStarCount += score;
		globals.maxLevelCompleted = Mathf.Max(globals.maxLevelCompleted, level);
	}
}

function Start () {
	iTween.MoveFrom(this.gameObject, {"y":transform.position.y, "x":150, "time":1.0f});	
	
	if(globals.levelsToUnlock >= level || checkLevel(level)){
		if(score==1){
			iTween.ColorTo(star_2, {"color":Color.yellow, "delay": 0.1f});
			//iTween.FadeTo(star_1,  {"alpha":0.50});
			//iTween.FadeTo(star_3,  {"alpha":0.50});
		}
		if(score==2){
			iTween.ColorTo(star_1, {"color":Color.yellow, "delay":0.1f});
			iTween.ColorTo(star_2, {"color":Color.yellow, "delay":0.1f});
			//iTween.FadeTo(star_3,  {"alpha":0.50});
		}
		if(score >= 3){
			iTween.ColorTo(star_1, {"color":Color.yellow, "delay":0.1f});
			iTween.ColorTo(star_2, {"color":Color.yellow, "delay":0.1f});
			iTween.ColorTo(star_3, {"color":Color.yellow, "delay":0.1f});
		}
	}else{
		iTween.FadeTo(this.gameObject, {"alpha":0.50, "includechildren":true, "time":0.2});
		this.gameObject.collider.enabled = false;
		Destroy(GetComponent(Animator));
	}
}

private function checkLevel(level){
	if (level == 1 ||level == 21 || level == 41 || level == 61 || level == 81 )
		return true;
	else
		return false;
}





