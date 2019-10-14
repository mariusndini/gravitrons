#pragma strict
private var _g:globals;
var smokePrefabLarge:GameObject;
var smokePrefabMedium:GameObject;
var smokePrefabSmall:GameObject;
var asteroidPrefabSMALL:GameObject;
var asteroidPrefabMEDIUM:GameObject;
var asteroidPrefabLARGE:GameObject;
var rockCollideSmallSound:AudioClip;
var rockCollideMediumSound:AudioClip;
var rockCollideLargeSound:AudioClip;


enum asteroidSize{
	small = 2, 
	medium= 3, 
	large = 5};
	
var size:asteroidSize;


private var thisTrans:Transform;
private var sizeInt:int = 0;


function Start () {
	thisTrans = this.transform;
	sizeInt = parseInt(size);
	thisTrans.localScale = Vector3(sizeInt, sizeInt, sizeInt);
	this.rigidbody.mass = sizeInt * 2;
	
}

function OnTriggerEnter(other:Collider){
	if(other.tag == "Bomb"){
		playSmoke(smokePrefabLarge, thisTrans.position);
		breakUpAsteroid();
	}
}


function OnCollisionEnter(collision: Collision){
	for (var item :ContactPoint in collision.contacts){		
		if(_g.getForce(collision.relativeVelocity) == collisionType.slow){

		}
			
		if(_g.getForce(collision.relativeVelocity) == collisionType.medium){
			playSmoke(smokePrefabMedium, thisTrans.position);
			breakUpAsteroid();
		}
		if(_g.getForce(collision.relativeVelocity) == collisionType.large){
			playSmoke(smokePrefabLarge, thisTrans.position);
			breakUpAsteroid();			
		}
	}
}


private function playSmoke(prefab:GameObject, loc:Vector3){
	prefab.renderer.sortingOrder = 999;
	var partSystem = Instantiate(prefab, loc, Quaternion.identity);
	partSystem.particleSystem.enableEmission = true;
	
}

private function breakUpAsteroid(){
	var directionNormal:Vector3;
	var offset:Vector3;
	directionNormal = this.transform.position;
	directionNormal = directionNormal.normalized;
	
	transform.collider.enabled=false;
	transform.renderer.enabled=false;
	
	
	if(size == parseInt(asteroidSize.small)){
		iTween.Stab(this.gameObject, {"audioClip":rockCollideSmallSound, "volume":_g.soundEffectsVolumne, 
												"oncomplete":"destroyThis"});

	}
	if(size == parseInt(asteroidSize.medium)){
		iTween.Stab(this.gameObject, {"audioClip":rockCollideMediumSound, "volume":_g.soundEffectsVolumne, 
												"oncomplete":"destroyThis"});
		offset = Vector3(2, 2, 0);
		var newObjects = Instantiate(asteroidPrefabSMALL, thisTrans.position+offset, thisTrans.rotation);
		newObjects.rigidbody.AddForce(directionNormal * 2000);
			
		newObjects = Instantiate(asteroidPrefabSMALL, thisTrans.position-offset, thisTrans.rotation);
		newObjects.rigidbody.AddForce(directionNormal * 2000);
		
	}
	if(size == parseInt(asteroidSize.large)){
		iTween.Stab(this.gameObject, {"audioClip":rockCollideLargeSound, "volume":_g.soundEffectsVolumne, 
												"oncomplete":"destroyThis"});
		offset = Vector3(3, 3, 0);
		var newObjectsL = Instantiate(asteroidPrefabMEDIUM, thisTrans.position+offset, thisTrans.rotation);
		newObjectsL.rigidbody.AddForce(directionNormal * 4000);
		
		newObjectsL = Instantiate(asteroidPrefabMEDIUM, thisTrans.position-offset, thisTrans.rotation);
		newObjectsL.rigidbody.AddForce(directionNormal * 4000);
	}
	
}

private function destroyThis(){
	Destroy(this.gameObject);
}











