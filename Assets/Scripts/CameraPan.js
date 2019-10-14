#pragma strict

public var parallaxItems:Transform[];
public var parallaxValues:float[];

private var mouseSensitivity : float = 0.1;
private var lastPosition : Vector3;
private var g : globals;
private var horzExtent:float;
private var vertExtent:float;

function Start(){
    horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
	vertExtent = Camera.main.camera.orthographicSize;  



}


function Update(){
	var v3 = transform.position;
	

	if(!g.gravitronActive){
	    if (Input.GetMouseButtonDown(0)){
	        lastPosition = Input.mousePosition;
	    }
	 
	    if (Input.GetMouseButton(0)){
	        var delta : Vector3 = Input.mousePosition - lastPosition;
	        if(v3.x >= g.leftBorder.transform.position.x + horzExtent + 30 && delta.x > 0){
	        	transform.Translate(-delta.x * mouseSensitivity, 0, 0);
	        	parallax(3);
        	}
	        if(v3.x <= g.rightBorder.transform.position.x - horzExtent - 10 && delta.x < 0){
	        	transform.Translate(-delta.x * mouseSensitivity, 0, 0);
	        	parallax(4);
        	}
	        if(v3.y <= g.topBorder.transform.position.y - vertExtent-10 && delta.y < 0){
	        	transform.Translate(0, -delta.y * mouseSensitivity, 0);
	        	parallax(2);
        	}
	        if(v3.y >= g.bottomBorder.transform.position.y + vertExtent +10 && delta.y > 0){
	        	transform.Translate(0, -delta.y * mouseSensitivity, 0);
	        	parallax(1);
        	}
        	
	        lastPosition = Input.mousePosition;
	    }
    }
    
}

private function parallax(direction:int){
	var i:int=0;
	if(direction == 1){//if direction is up, move down
		for( i = 0; i < parallaxItems.length; i++){
			parallaxItems[i].position.y += parallaxValues[i];
		}
	}
	if(direction == 2){//if direction is up, move down
		for( i = 0; i < parallaxItems.length; i++){
			parallaxItems[i].position.y -= parallaxValues[i];
		}
	}
	if(direction == 3){//if direction is up, move down
		for( i = 0; i < parallaxItems.length; i++){
			parallaxItems[i].position.x += parallaxValues[i];
		}
	}
	if(direction == 4){//if direction is up, move down
		for( i = 0; i < parallaxItems.length; i++){
			parallaxItems[i].position.x -= parallaxValues[i];
		}
	}
	
	
}





















