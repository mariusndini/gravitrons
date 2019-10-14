#pragma strict


public var gravitronExp:GameObject;
public var starsExp:GameObject;
public var starCollect:GameObject;

function Start(){
	starCollect.renderer.sortingOrder = 1002;
}

function OnMouseUp(){
	iTween.MoveTo(gravitronExp, Vector3(-130, gravitronExp.transform.position.y, 0), 1);
	iTween.MoveTo(starsExp, Vector3(-130, starsExp.transform.position.y, 0), 1.5);
}