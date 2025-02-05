// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(CharacterController), typeof(Input))]
// public class Movement : MonoBehaviour
// {
//     public Inputs input;
//     public PlayerController player;
//     public Vector3 movement;

//     public CharacterController characterController;

//     private float playerRotation;
//     private string playerDirection = "right";
//     [SerializeField] public float playerVelConstant = 3f;
//     [SerializeField] public float playerVel = 3f;
//     //Jump
//     [SerializeField] public float _jumpForce = 4f;
//     [SerializeField] public float _playerVelJump = 3.2f;
//     [SerializeField] public float _doubleJumpForce = 4.2f;
//     [SerializeField] private float _playerVelDoubleJump = 3.5f;
//     private float _bufferTime = 0.25f;
//     private float _bufferTimer;
//     [SerializeField] private bool _doubleJump =false;
//     [SerializeField] public bool _inAir =false;

//     //States
//     [SerializeField] public float _FrogVel=2f;
//     [SerializeField] public float _FrogJumpForce=15f;
//     [SerializeField] public bool _frogJumpComplete =false;

//     //GroundSensor
//     [SerializeField] Transform _sensorPosition;
//     [SerializeField]  LayerMask _groundLayer;
//     [SerializeField]  float _groundSensorX = 0.65f;
//     [SerializeField]  float _groundSensorY = 0.5f;
//     [SerializeField]  float _groundSensorZ = 0.61f;
//     [SerializeField] private float _slideSpeed = 1f;
//     [SerializeField] private float _raySize = 1f;

//     //Gravity
//     [SerializeField] private float  _gravity = -37f;
//     [SerializeField] private Vector3 _playerGravity;


//     void Awake(){
//         input = GetComponent<Inputs>();
//         characterController = GetComponent<CharacterController>();
//     }

//     void Update()
//     {
//         PlayerMovement();

//         if (input.jump && IsGrounded()) {
//             if (_isFrog && !_frogJumpComplete){
//                 Jump(_FrogJumpForce); 
//                 _frogJumpComplete = true;
//             }
//             else if (!_isFrog) Jump(_jumpForce); 
//         }
//         InAir(_inAir);
//         if(input.firstTransformation && IsGrounded() && !_frogJumpComplete){
//             if (!_isFrog) FrogTransformation();
//             else if(_isFrog) SetNormalState();
//         }
//         Gravity();
//         if(!IsGrounded()) Checkcorner();
//     }

//     public void PlayerMovement() {
    
//         movement.z = input.inputHorizontal * playerVel;
//         if (input.inputHorizontal > 0){
//             if (playerDirection == "left") {
//                 playerRotation = -180f;
//                 this.transform.Rotate(Vector3.up, playerRotation);
//                 playerDirection = "right";
//             }
//         } else if (input.inputHorizontal < 0) {
//             if (playerDirection == "right") {
//                 playerRotation = 180f;
//                 this.transform.Rotate(Vector3.up, playerRotation);
//                 playerDirection = "left";
//             }
//         }
//         Vector3 totalMovement = movement * Time.deltaTime; 
//         characterController.Move(totalMovement*playerVel);
//     }

//     public void InAir(bool inAir){
//         if (inAir) { 
//             if(_doubleJump==true && input.jump && !player._isFrog){
//                 _doubleJump=false;
//                 Jump(_doubleJumpForce);
//             }
            
//             if(!_doubleJump && input.jump) _bufferTimer=_bufferTime;
//             _bufferTimer -= Time.deltaTime;

//         } else if (_playerGravity.y < 0) {
//             _inAir=false;
        
//             if (_frogJumpComplete){
//                 _frogJumpComplete = false; 
//                 player.SetNormalState(); 
//             }
//             _doubleJump=true;
//             if (!player._isFrog && _doubleJump) playerVel=playerVelConstant;
//             if(_bufferTimer>0) Jump(_jumpForce);
//         }
//     }

//    public void Gravity(){
//     if(!IsGrounded()){  
//         _playerGravity.y += _gravity *Time.deltaTime;
//         _inAir = true;
//     }   
//     else if(IsGrounded() && _playerGravity.y <0 ){
//         _playerGravity.y = -1;
//         _inAir = false;
//     }

//         characterController.Move(_playerGravity * Time.deltaTime);
//    }

//     public void Jump(float jumpForce){
//         if(!player._isFrog && _doubleJump) playerVel=_playerVelJump;
//         else if(!player._isFrog && !_doubleJump) playerVel=_playerVelDoubleJump;
//         _playerGravity.y = Mathf.Sqrt(jumpForce * -2 * _gravity);
//         _bufferTimer=0;
//     }

//     public bool IsGrounded(){
//         return Physics.CheckBox(
//         _sensorPosition.position, 
//         new Vector3(_groundSensorX, _groundSensorY, _groundSensorZ),     
//         Quaternion.identity,      
//         _groundLayer.value        
//         );
//     }

//     public void Checkcorner(){
//         RaycastHit hit;
//         if(Physics.Raycast(_sensorPosition.position, transform.forward, out hit, _raySize, _groundLayer) || Physics.Raycast(_sensorPosition.position, -transform.forward, out hit, _raySize, _groundLayer)){
//             SlideCorner(hit.normal);
//         }
//     }

//     void SlideCorner(Vector3 slideDirection){
//         characterController.Move((slideDirection*_slideSpeed+Vector3.down)*Time.deltaTime);
//     }

//     void OnDrawGizmos() {
//         Vector3 halfExtents = new Vector3(_groundSensorX, _groundSensorY, _groundSensorZ);

//         Gizmos.color = Color.green;
//         Gizmos.DrawWireCube(_sensorPosition.position, halfExtents * 2); 
    
//         Gizmos.color = Color.red;
//         Gizmos.DrawRay(_sensorPosition.position, transform.forward*_raySize);
//         Gizmos.DrawRay(_sensorPosition.position, -transform.forward*_raySize);
//     }
// }
