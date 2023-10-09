using Unity.VisualScripting;

namespace Cells
{
    public class BaseCell
    {
        public float detectionRange;
        public float moveSpeed;
        public float sightRange;
        public float life;
        public InfectionComponent _infection;

        public BaseCell(float detectionRange, float sightRange, float moveSpeed, float life=50)
        {
            this.detectionRange = detectionRange;
            this.sightRange = sightRange;
            this.moveSpeed = moveSpeed;
            this.life = life;
        }
    }
}