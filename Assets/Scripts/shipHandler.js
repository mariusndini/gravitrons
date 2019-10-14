#pragma strict
var explosionPrefabSlow:GameObject;
var explosionPrefabMed:GameObject;
var explosionPrefabLarge:GameObject;
var shipExplosion:GameObject;
var shipDamaged:Sprite;
var thisCam:GameObject;
var shipCrashSmallSound:AudioClip;
var shipCrashMediumSound:AudioClip;
var shipExplosionSound:AudioClip;

private var _g:globals;


function OnTriggerEnter(other:Collider){
	if(other.tag == "Bomb"){
		killShip(transform.position);
				
	}

}


function OnCollisionEnter(collision: Collision){
	for (var item :ContactPoint in collision.contacts){
		if(item.thisCollider == collider){
			if(_g.getForce(collision.relativeVelocity) == collisionType.slow){
				playExplosion(explosionPrefabSlow, item.point, false);
				iTween.ShakePosition(thisCam, new Vector2(0.3,0.3), 0.5f);
				iTween.Stab(this.gameObject, {"audioClip":shipCrashSmallSound, "volume":_g.soundEffectsVolumne});
			}	
			if(_g.getForce(collision.relativeVelocity) == collisionType.medium){
				iTween.Stab(this.gameObject, {"audioClip":shipCrashMediumSound, "volume":_g.soundEffectsVolumne});
				playExplosion(explosionPrefabMed, item.point, true);
				iTween.ShakePosition(thisCam, new Vector2(0.6,0.6), 0.5f);
				_g.wasHit = true;
			}
			if(_g.getForce(collision.relativeVelocity) == collisionType.large){
				killShip(item.point);
				
			}
			
			
			
		}
	}
}

private function killShip(loc : Vector3){
	playExplosion(explosionPrefabLarge, loc, true);
	playExplosionSHIP(shipExplosion, this.transform.position);
	iTween.ShakePosition(thisCam, new Vector2(2,2), 0.5f);
	_g.onGameOver(_g.BAD);
	transform.collider.enabled=false;
	transform.renderer.enabled=false;

	iTween.Stab(this.gameObject, {"audioClip":shipExplosionSound, "volume":_g.soundEffectsVolumne, 
									"oncomplete":"destroyThis"});

				
}

private function destroyThis(){
	Destroy(this.gameObject);
}

private function playExplosion(prefab:GameObject, loc:Vector3, isShippedDamaged:boolean){
	prefab.renderer.sortingOrder = 1000;
	if(isShippedDamaged){
		this.transform.GetComponent(SpriteRenderer).sprite = shipDamaged;
	}
	var partSystem = Instantiate(prefab, loc, Quaternion.identity);
	partSystem.particleSystem.enableEmission = true;
}

private function playExplosionSHIP(prefab:GameObject, loc:Vector3){
	prefab.renderer.sortingOrder = 1000;
	var partSystem = Instantiate(prefab, loc, Quaternion.identity);
	partSystem.particleSystem.enableEmission = true;
	
}















