#pragma strict

var orbitObject:GameObject;
var speed:float = 50;

function Start () {

}

function Update () {
	if(orbitObject)
		this.transform.RotateAround(orbitObject.transform.position, Vector3.back, speed * Time.deltaTime);
	
	
}