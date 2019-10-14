#pragma strict

public var _leftBorder:Transform;
public var _rightBorder:Transform;
public var _topBorder:Transform;
public var _bottomBorder:Transform;
public var _gameOverScreenPass:GameObject;
public var _gameOverScreenFail:GameObject;
public var _ReplayButton:GameObject;
public var _HomeButton:GameObject;

public var _star_1:GameObject;
public var _star_2:GameObject;
public var _star_3:GameObject;

public static var gravitronActive:boolean = false;
public static var gravitron:GameObject;

public static var leftBorder:Transform;
public static var rightBorder:Transform;
public static var topBorder:Transform;
public static var bottomBorder:Transform;
public static var gameOverScreenPass:GameObject;
public static var gameOverScreenFail:GameObject;
public static var ReplayButton:GameObject;
public static var HomeButton:GameObject;
public static var isGameOver = false;
public static var GOOD = true; //for game over type
public static var BAD = false; //for game over type
public static var sceneToLoad:String ="LevelSelect";
public static var wasHit:boolean = false;
public static var starCollected:boolean = false;
public static var star_1:GameObject;
public static var star_2:GameObject;
public static var star_3:GameObject;
public static var soundEffectsVolumne:float = 1000.0f;
public static var score:int = 0;
public static var maxLevelCompleted:int = 0;
public static var totalStarCount = 0;
public static var worldsUnlocked = 1;
public static var levelsToUnlock:int = 1;
public static var WorldToLoad:int;

private static var slow:float = 	5.0f;
private static var large:float = 	15.0f;
public enum collisionType{slow, medium,large};
public enum gameTags{Player};



function Start(){
	Screen.sleepTimeout = SleepTimeout.NeverSleep;//keep screen from dimming
	leftBorder = _leftBorder;
	rightBorder = _rightBorder;
	topBorder = _topBorder;
	bottomBorder = _bottomBorder;
	gravitronActive = false;
	isGameOver = false;
	wasHit = false;
	starCollected=false;
	gameOverScreenPass = _gameOverScreenPass;
	gameOverScreenFail = _gameOverScreenFail;
	ReplayButton = _ReplayButton;
	HomeButton = _HomeButton;
	
	star_1 = _star_1;
	star_2 = _star_2;
	star_3 = _star_3;
	score = 0;	
	totalStarCount = 0;
	
}

public static function getForce(speed:Vector3){
	var maxSpeed =  Mathf.Max(Mathf.Abs(speed.x), Mathf.Abs(speed.y), Mathf.Abs(speed.z));
	var result = collisionType.slow;
	
	if (maxSpeed <= slow){
		result =  collisionType.slow;
	}
	if (maxSpeed > slow && maxSpeed < large){
		result =  collisionType.medium;
	}
	if (maxSpeed >= large){
		result =  collisionType.large;
	}
	return result;
}


public static function onGameOver(type:boolean){
	isGameOver = true;
	ShowAds();
	
	if(type == BAD){
		iTween.MoveTo(gameOverScreenFail, {"y": 6,"x": 0, "time": 0.3, "delay":1, "islocal":true});
		ReplayButton.GetComponent(SpriteRenderer).color = Color(1,1,1,1);
		HomeButton.GetComponent(SpriteRenderer).color = Color(1,1,1,1);
		//iTween.MoveTo(ReplayButton, {"y": -3,"x": -7, "time": 0.3, "delay":1, "islocal":true});
		//iTween.MoveTo(HomeButton, {"y": -3,"x": 8, "time": 0.3, "delay":1, "islocal":true});


	}
	if(type == GOOD){
		iTween.MoveTo(gameOverScreenPass, {"y": 6, "x": 0,"time": 0.3, "delay":1, "islocal":true});
		ReplayButton.GetComponent(SpriteRenderer).color = Color(1,1,1,1);
		HomeButton.GetComponent(SpriteRenderer).color = Color(1,1,1,1);
		
		iTween.ScaleTo(star_1, {"x":0.7f, "y":0.7f, "delay":1.5f,  "easetype":"EaseOutBounce"});
		iTween.ColorTo(star_1, {"color":Color.yellow, "delay":1.5f});
		score++;
		
		if(!wasHit){
			iTween.ScaleTo(star_2, {"x":0.7f, "y":0.7f, "delay":1.85f,  "easetype":"EaseOutBounce"});
			iTween.ColorTo(star_2, {"color":Color.yellow, "delay":1.85f});
			score++;
		}
		if(starCollected){
			iTween.ScaleTo(star_3, {"x":0.7f, "y":0.7f, "delay":2.20f,  "easetype":"EaseOutBounce"});
			iTween.ColorTo(star_3, {"color":Color.yellow, "delay":2.20f});
			score++;
		}
		
		if(!PlayerPrefs.GetInt(sceneToLoad) || PlayerPrefs.GetInt(sceneToLoad) < score){
			PlayerPrefs.SetInt(sceneToLoad, score);
		}
	}

}

public static function ShowAds () {
	AdMob.init("a15308b6feb763f", "a153091e05beaee" );
	AdMob.createBanner("ca-app-pub-6502691088342550/2302799579", "ca-app-pub-6502691088342550/6872599973", AdMobBanner.SmartBanner, AdMobLocation.BottomCenter );


}
											   
public static function killAds(){
	AdMob.destroyBanner();
}

public static function setWorldNumber(worldNum:int){
	WorldToLoad = worldNum;
}

































