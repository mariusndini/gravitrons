#pragma strict

var line1:GameObject;
var line2:GameObject;
var line3:GameObject;
var line4:GameObject;

private var showing = false;
private var start = 4.5;
private var end = -4.5;

function OnMouseDown(){	
	if(!showing){
		iTween.MoveTo(line1, {"x": start, "time": 0.3, "islocal":true});
		iTween.MoveTo(line2, {"x": start, "time": 0.6, "islocal":true});
		iTween.MoveTo(line3, {"x": start, "time": 0.9, "islocal":true});
		iTween.MoveTo(line4, {"x": start, "time": 1.1, "islocal":true});
		showing = true;
	}else{
		iTween.MoveTo(line1, {"x": end, "time": 0.3, "islocal":true});
		iTween.MoveTo(line2, {"x": end, "time": 0.6, "islocal":true});
		iTween.MoveTo(line3, {"x": end, "time": 0.9, "islocal":true});
		iTween.MoveTo(line4, {"x": end, "time": 1.1, "islocal":true});
		showing = false;
	}
	
}



