using System.Collections;
using System.Collections.Generic;
using PathFinder;
using UnityEngine;

namespace Enemy
{
    public class Enemies
    {
        private float detectionRange;
        private float moveSpeed;
        private float sightRange;
        private float life;

        // Affected Body Component

        public float DetectionRange => detectionRange;
        public float MoveSpeed => moveSpeed;
        public float SightRange => sightRange;
        public float Life => life;
        
        public Enemies(float detectionRange, float sightRange, float moveSpeed, float life)
        {
            this.detectionRange = detectionRange;
            this.sightRange = sightRange;
            this.moveSpeed = moveSpeed;
            this.life = life;
        }
    }
}