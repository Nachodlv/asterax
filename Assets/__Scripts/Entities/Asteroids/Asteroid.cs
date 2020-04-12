using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __Scripts.Asteroids
{
    [RequireComponent(typeof(Stats), typeof(SpaceObject) )]
    public class Asteroid: MonoBehaviour
    {
        [SerializeField] private float points;
        
        /// <summary>
        /// <para>Invoked on die</para>
        /// </summary>
        public event Action OnDestroy;
        public List<Asteroid> Children { get; set; }
        public float Points
        {
            get { return points; }
        }
        public bool DestroyedByBullet { get; set; }

        private Rigidbody _rigidbody;
        private Stats _stats;
        private Transform _transform;
        private bool _activated;
        private SpaceObject _spaceObject;
        private Vector3 _velocity;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _stats.OnDie += Destroy;
            _transform = transform;
            _spaceObject = GetComponent<SpaceObject>();
            _spaceObject.enabled = false;
        }
        
        /// <summary>
        /// <para>Rotates the transform in the z axis</para>
        /// </summary>
        private void FixedUpdate()
        {
            if (!_activated) return;
            
            _transform.Rotate(Vector3.back, _stats.RotationSpeed);
        }

        /// <summary>
        /// <para>Activates the RigidBody by adding the component into the GameObject. It also sets its properties</para>
        /// </summary>
        public void ActivateRigidbody()
        {
            if (_rigidbody != null) return;
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            _spaceObject.enabled = true;
            _activated = true;
            _rigidbody.velocity = _velocity;
        }

        /// <summary>
        /// <para>Sets the member variable _velocity equals to initialDirection multiplied by a random number between
        /// the movement speed and the maximum movement speed</para>
        /// </summary>
        /// <param name="initialDirection"></param>
        public void SetInitialDirection(Vector3 initialDirection)
        {
            var velocity = Random.Range(_stats.MovementSpeed, _stats.MaxSpeed);
            _velocity = initialDirection * velocity;
        }

        /// <summary>
        /// <para>Destroys the GameObject and invokes the OnDestroy event</para>
        /// </summary>
        private void Destroy()
        {
            if (OnDestroy != null) OnDestroy();
            Destroy(gameObject);
        }
        
    }
}