#pragma strict

public var flashEffectPrefab:GameObject;

private var _g:globals;
function Start () {

}

function OnTriggerEnter (other : Collider) {
	if(other.tag == gameTags.Player+""){
		var partSystem = Instantiate(flashEffectPrefab, this.transform.position, Quaternion.identity);
		partSystem.particleSystem.enableEmission = true;
		Destroy(other.gameObject);
		_g.onGameOver(_g.GOOD);
		
	}
}




























