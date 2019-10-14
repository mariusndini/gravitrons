#pragma strict

var To:Vector3;
var time:float;


function Start () {
	iTween.MoveTo(this.gameObject, To, time);
	
}
