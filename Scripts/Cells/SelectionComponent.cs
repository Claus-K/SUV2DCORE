using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cells
{
    public class SelectionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject selectionCircle;
        [SerializeField] private GameObject MinimapIcon;
        // private Color originalColorMMIcon;
        
        public Vector3[,] destinationsList { get; set; }
        public Transform assignedTarget { get; set; }
        public Vector3 destination { get; set; }

        private bool assignedMove { get; set; }
        private bool assignedMoveAttack { get; set; }
        private bool assignedAttack { get; set; }


        public bool AssignedMove
        {
            get => assignedMove;
            set
            {
                assignedMove = value;
                if (!value) return;
                assignedMoveAttack = false;
                assignedAttack = false;
            }
        }

        public bool AssignedMoveAttack
        {
            get => assignedMoveAttack;
            set
            {
                assignedMoveAttack = value;
                if (!value) return;
                assignedMove = false;
                assignedAttack = false;
            }
        }

        public bool AssignedAttack
        {
            get => assignedAttack;
            set
            {
                assignedAttack = value;
                if (!value) return;
                assignedMove = false;
                assignedMoveAttack = false;
            }
        }

        public void EnableSelection()
        {
            selectionCircle.SetActive(true);
            MinimapIcon.GetComponent<SpriteRenderer>().color = Color.white;
        }

        public void DisableSelection()
        {
            selectionCircle.SetActive(false);
            MinimapIcon.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }
}