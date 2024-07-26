using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinder;

namespace Enemy
{
    public class Viruses : Enemies
    {
        // Movement Varibles
        public Rigidbody2D _rb;
        public string[] searchTag = { "CellDef", "BloodCell" };
        public PathAgent pg = new();

        // Target Variables
        public Collider2D[] overlapResults = new Collider2D[50];
        public TargetService TargetServiceScript = new();
        public Transform target;


        public Viruses(float detectionRange, float sightRange, float moveSpeed, float life = 50) : base(detectionRange,
            sightRange, moveSpeed, life)
        {
        }
    }
}