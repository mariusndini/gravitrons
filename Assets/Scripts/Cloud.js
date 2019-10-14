#pragma strict

private var speed = 0.0f;
private var g:globals;

function Start(){
	randomSpeed();
}

function Update () {
	if(transform.position.x >= g.rightBorder.position.x){
		transform.position.x = globals.leftBorder.position.x;
		randomSpeed();
	}else{
		transform.position.x += speed;
	}
}

private function randomSpeed(){
	speed = Random.Range(0.01f, 0.1f);
}

