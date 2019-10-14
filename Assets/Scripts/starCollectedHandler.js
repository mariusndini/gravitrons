#pragma strict


var partSystem:GameObject;
private var changeColor = false;
private var PS:ParticleSystem;

function Start(){
	PS = partSystem.GetComponent(ParticleSystem).particleSystem;
}

function OnTriggerEnter (other : Collider){
	if(other.tag == gameTags.Player+""){
		PS.startSize = 10;
		PS.startColor = Color.black;
		
		globals.starCollected = true;
		Destroy(this.gameObject, 1.5f);
	
	}
}





