	#pragma strict


var explosionPrefab:GameObject;
var ThisCam:GameObject;
private var explosionRadius = 4.0f;
 

 

function OnTriggerEnter (other:Collider) {
	if(other.tag == "asteroid" || other.tag == "Player"|| other.tag == "Bomb"){
		if(other.tag == "Bomb"){
			yield WaitForSeconds(0.2);
		}	
	
	    transform.position += Vector3(0.001,0.001,0);
	 	var col = this.GetComponent(SphereCollider);
	 	col.radius = explosionRadius;
	 	if(ThisCam){
	 		iTween.ShakePosition(ThisCam, new Vector2(4f,4f), 0.25f);
		}
		playExplosion(explosionPrefab, transform.position);
		
		yield WaitForSeconds(0.2);
		Destroy(this.gameObject);

	}

}



private function playExplosion(prefab:GameObject, loc:Vector3){
	prefab.renderer.sortingOrder = 1000;
	var partSystem = Instantiate(prefab, loc, Quaternion.identity);
	partSystem.particleSystem.enableEmission = true;
	
}






