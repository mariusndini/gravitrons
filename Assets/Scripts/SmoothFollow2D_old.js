
#pragma strict

var target : Transform;
var smoothTime = 0.1;
public var _X:float = 10.0;
public var _Y:float = 10.0;

private var thisTransform : Transform;
private var velocity : Vector2;
private var oldLocation:Vector3 = Vector3.zero;
private var curLocation:Vector3 = Vector3.zero;
private var g : globals;



function Start(){
	thisTransform = transform;
	oldLocation = target.position;
	curLocation = target.position;
}

function Update() {
	oldLocation = target.position;
	Debug.Log(g.gravitronActive);
	if(g.gravitronActive && g.gravitron != null){
		thisTransform.position.x = Mathf.SmoothDamp(thisTransform.position.x, g.gravitron.transform.position.x, velocity.x, smoothTime);
		thisTransform.position.y = Mathf.SmoothDamp(thisTransform.position.y, g.gravitron.transform.position.y, velocity.y, smoothTime);
	}else {
		if(curLocation.x < oldLocation.x){//moving forward
			thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, target.position.x + _X,  velocity.x, smoothTime);	
		}
		if(curLocation.x > oldLocation.x){
			thisTransform.position.x = Mathf.SmoothDamp( thisTransform.position.x, target.position.x - _X, velocity.x, smoothTime);
		}
		if(curLocation.y < oldLocation.y){
			thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, target.position.y + _Y, velocity.y, smoothTime);
		}
		if(curLocation.y > oldLocation.y){
			thisTransform.position.y = Mathf.SmoothDamp( thisTransform.position.y, target.position.y - _Y, velocity.y, smoothTime);
		}
	}


}


function LateUpdate(){
	//get location AFTER the ship has been moved
	curLocation = target.position;
	
}






