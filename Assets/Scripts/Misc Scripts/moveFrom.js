#pragma strict

var from:Vector3;
var time:float;


function Start () {
	iTween.MoveFrom(this.gameObject, from, time);
	//"easetype":"EaseOutBounce",
}
