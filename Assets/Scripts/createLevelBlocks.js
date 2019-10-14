#pragma strict

var blockSprite:Sprite;
var cameraLocation:Transform;
var cols = 7;
var rows = 4;
private var _g:globals;

private var horzExtent:float;
private var vertExtent:float;

function Start(){
	/*
    horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
	vertExtent = Camera.main.camera.orthographicSize;  

	var camX = cameraLocation.position.x;
	var camY = cameraLocation.position.y;

	var blockSize = 5;
	var spaceBetween = blockSize * 3;
	var count = 0;

	for(var r = 0; r < rows; r++){
		for (var c = 0; c < cols; c++){
			count++;
			var block = new GameObject();
			block.name = "Level_"+count;
			block.tag = "LevelTag";
			block.AddComponent(BoxCollider);
			block.GetComponent(BoxCollider).size = Vector3(3,3,3);

			block.AddComponent(SpriteRenderer);
			block.GetComponent(SpriteRenderer).sprite = blockSprite;
			block.transform.localScale = Vector3(blockSize,blockSize,blockSize);
			
			block.transform.position = Vector3 (camX - (horzExtent - blockSize*2) + ((horzExtent/rows) * c + blockSize), 
												camY + (vertExtent - spaceBetween) - (spaceBetween*r), 0);
												
			
			var lvlNum = new GameObject();
			lvlNum.AddComponent(GUIText);
			lvlNum.name = "lvlTxt_"+count;	
			var txt:GUIText = lvlNum.guiText;
			txt.text = count+"";
			txt.fontSize = 50;
			lvlNum.transform.position = Vector3(0.5,0.5, 0);;										
												
		}
		c = 0;
	}
	*/
}























