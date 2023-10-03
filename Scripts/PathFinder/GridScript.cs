using UnityEngine;


namespace PathFinder
{
    public class GridScript : MonoBehaviour
    {
        private int width;

        private int height;
        private float cellSize;

        private int[,] gridArray;
        public TextMesh textMeshPrefab;
        private TextMesh[,] _textMeshes;
        public bool drawGrid; 

        public void Initialize(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            gridArray = new int[this.width, this.height];
            // textMeshPrefab.characterSize = Mathf.Max(this.cellSize / 2, 0.1f);
            if (drawGrid)
            {
                DrawGrid();
            }
            
            SetValue(2, 1, 56);
        }

        private void DrawGrid()
        {
            _textMeshes = new TextMesh[width, height];
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    // _textMeshes[x, y] = Instantiate(textMeshPrefab,
                    //     GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2), Quaternion.identity,
                    //     this.transform);
                    // _textMeshes[x, y].text = gridArray[x, y].ToString();
                    // _textMeshes[x, y].characterSize = Mathf.Max(cellSize / 2, 0.1f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.green, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.green, 100f);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.y / cellSize);
        }

        private void SetValue(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && width > x && height > y)
            {
                gridArray[x, y] = value;
                // _textMeshes[x, y].text = value.ToString();
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            GetXY(worldPosition, out var x, out var y);
            SetValue(x, y, value);
        }

        public int GetValue(int x, int y)
        {
            if (x >= 0 && y >= 0 && width > x && height > y)
            {
                return gridArray[x, y];
            }
            return -1;
        }

        public int GetValue(Vector3 worldPosition)
        {
            GetXY(worldPosition, out var x, out var y);
            return GetValue(x, y);
        }
    }
}