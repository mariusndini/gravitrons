#pragma strict


function Update () {
	if (Input.GetMouseButton(0)) {
		var ray: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit: RaycastHit;
		
		if (Physics.Raycast(ray, hit)) {
			var item = hit.transform;

			//insert the ift statements to call proper fnction when hitting and object
			if(item.name == "replayButton"){
				//Application.LoadLevel(Application.loadedLevel);
				loadingLogic(globals.sceneToLoad+"");
			}//end if
			if(item.name == "HomeButton"){
				if(globals.WorldToLoad == 1){
					loadingLogic("LevelSelect");
				}if(globals.WorldToLoad == 2){
					loadingLogic("LevelSelectW2");
				}else{
					loadingLogic("LevelSelect");
				}
			}//end if
			if(item.tag == "LevelTag"){
				//Application.LoadLevel(item.name+"");
				loadingLogic(item.name+"");
			}
			if(item.name == "nextLevel"){
				//Application.LoadLevel((parseInt(Application.loadedLevel+"")+1)+"");
				loadingLogic((parseInt(Application.loadedLevelName+"")+1)+"");
			}
			if(item.name == "StartButton"){
				iTween.MoveTo(item.gameObject, {"x":35, 
											    "time": 0.25, 
											    "OnComplete":"LoadMenuScreen", 
											    "onCompleteTarget":this.gameObject});
				
			}
			if(item.name == "startScreen"){
				iTween.MoveTo(item.gameObject, {"x":35, 
											    "time": 0.25, 
											    "OnComplete":"loadingLogic", 
											    "oncompleteparams":"startScreen",
											    "onCompleteTarget":this.gameObject});
			}
			
			if(item.name == "World2LevelSelect"){
				loadingLogic("LevelSelectW2");
				globals.levelsToUnlock = 0;//resets the number of levels to open

			}
			if(item.name == "World1LevelSelect"){
				loadingLogic("LevelSelect");
				globals.levelsToUnlock = 0;//due to unintentional unlockin of levels 
			}
			
		}//end if
	}//end if
}//end update

private function LoadMenuScreen(){
	//Application.LoadLevel("LevelSelect");
	loadingLogic("LevelSelect");
}
private function loadingLogic(screen:String){
	globals.sceneToLoad = screen;
	Application.LoadLevel("LoadingScreen");
}








