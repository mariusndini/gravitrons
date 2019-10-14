#pragma strict

var platform:Transform;
var startTransform:Vector3;
var endTransform:Vector3;
var speed:float = 2;

private var direction:Vector3;
private var destination:Vector3;

function OnDrawGizmos(){
	Gizmos.DrawWireCube(startTransform, platform.localScale);
	Gizmos.DrawWireCube(endTransform, platform.localScale);
}

function Start () {
	setDestination(startTransform);
	platform = this.transform;
}

function Update () {
	platform.position = platform.position + direction * speed * Time.deltaTime;

	if(Vector3.Distance(platform.position, destination) < speed * Time.deltaTime){
		setDestination(destination == startTransform ? endTransform : startTransform);
	}

}

private function setDestination(dest){
	destination = dest;
	direction = (destination - platform.position).normalized;
	
}


















