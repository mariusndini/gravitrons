#pragma strict

public var GravityActiveParticle:GameObject;
private var circGravity : CircularGravity;
private var animator: Animator;
private var g : globals;
private var activeGravParticle:ParticleSystem;

function Start(){
	circGravity = GetComponent (CircularGravity);
	animator = GetComponent (Animator);
	activeGravParticle = 	GravityActiveParticle.GetComponent(ParticleSystem);
}

function Update(){
	if (g.isGameOver){
		Destroy(this.gameObject.collider);
	}
}


function OnMouseDown () {
    circGravity.enable = true;
    animator.SetInteger("state",1);
    g.gravitronActive = true;
	activeGravParticle.particleSystem.Play();
      
}

function OnMouseUp () {
    circGravity.enable = false;
    animator.SetInteger("state",0);
    g.gravitronActive = false;
    activeGravParticle.particleSystem.Stop();
}

function OnTriggerEnter (other : Collider) {
	g.gravitron = this.gameObject;
}

function OnTriggerExit (other : Collider) {
	g.gravitron = null;
}




