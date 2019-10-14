#pragma strict

var amplitudeX:float = 10.0f;
var amplitudeY:float = 10.0f;
var omegaX:float = 1.0f;
var omegaY:float = 5.0f;
var speed:float = 0.25;
private var index:float;



function Update () {
    index += Time.deltaTime;
    var x:float = amplitudeX * Mathf.Cos (omegaX*index*speed);
    var y:float = amplitudeY * Mathf.Sin (omegaY*index*speed);
    var vectNorm = Vector3(x,y,0).normalized;
    transform.position +=  vectNorm;


}




