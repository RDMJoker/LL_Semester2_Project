using UnityEngine;
using UnityEngine.Tilemaps;

namespace LL_Unity_Utils.Generic
{
    public class ObjectGrid<TObject>
    {
        readonly float cellSize;
        Tile chooseableTile;
        readonly bool debug;
        readonly TObject[,] gridArray;
        readonly Vector3 startingPos;

        readonly Tilemap tilemap;
        // int gridMiddlePoint;
        // int gridArrayMaxY;

        public ObjectGrid(int _width, int _height, float _cellSize, Vector3 _startingPos, Tilemap _tilemap, bool _debug = false)
        {
            Width = _width;
            Height = _height;
            cellSize = _cellSize;
            startingPos = _startingPos;
            debug = _debug;
            tilemap = _tilemap;
            // gridMiddlePoint = (int)(_width * 0.5f);
            // -1 to generate a small buffer towards the top of the grid map
            // gridArrayMaxY = _height - 1;

            gridArray = new TObject[Width, Height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (debug) Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 500f);
                if (debug) Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 500f);
            }

            if (debug) Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, 500f);
            if (debug) Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, 500f);
        }

        public ObjectGrid(int _width, int _height)
        {
            Width = _width;
            Height = _height;
            gridArray = new TObject[Width, Height];
        }

        public int Height { get; }

        public int Width { get; }

        public Vector3 GetWorldPosition(int _x, int _y)
        {
            return new Vector3(_x, _y) * cellSize + startingPos;
        }

        public void GetXY(Vector3 _worldPos, out int _x, out int _y)
        {
            if (startingPos.x > 0 && startingPos.y > 0)
            {
                _x = Mathf.FloorToInt((_worldPos + startingPos).x / cellSize);
                _y = Mathf.FloorToInt((_worldPos + startingPos).y / cellSize);
            }
            else if (startingPos.x > 0 && startingPos.y < 0)
            {
                _x = Mathf.FloorToInt((_worldPos + startingPos).x / cellSize);
                _y = Mathf.FloorToInt((_worldPos - startingPos).y / cellSize);
            }
            else if (startingPos.x < 0 && startingPos.y > 0)
            {
                _x = Mathf.FloorToInt((_worldPos - startingPos).x / cellSize);
                _y = Mathf.FloorToInt((_worldPos + startingPos).y / cellSize);
            }
            else
            {
                _x = Mathf.FloorToInt((_worldPos - startingPos).x / cellSize);
                _y = Mathf.FloorToInt((_worldPos - startingPos).y / cellSize);
            }
        }

        public void GetX(Vector3 _worldPos, out int _x)
        {
            if (startingPos.x > 0)
                _x = Mathf.FloorToInt((_worldPos + startingPos).x / cellSize);
            else if (startingPos.x > 0)
                _x = Mathf.FloorToInt((_worldPos + startingPos).x / cellSize);
            else if (startingPos.x < 0)
                _x = Mathf.FloorToInt((_worldPos - startingPos).x / cellSize);
            else
                _x = Mathf.FloorToInt((_worldPos - startingPos).x / cellSize);
        }

        public void SetValue(int _x, int _y, TObject _value)
        {
            if (_x >= startingPos.x && _y >= startingPos.y && _x < Width && _y < Height) gridArray[_x, _y] = _value;
        }

        public void SetValue(Vector2Int _arrayPos, TObject _value)
        {
            if (IsOutsideBounds(_arrayPos)) return;
            SetValue(_arrayPos.x,_arrayPos.y,_value);
        }

        public void SetValue(Vector3 _worldPos, TObject _value)
        {
            int x, y;
            GetXY(_worldPos, out x, out y);
            SetValue(x, y, _value);
        }

        /// <summary>
        ///     Function to get XY of the gird without calculating with starting position. Useful if the given position is already
        ///     calculated or i.e. the position of the mouse.
        /// </summary>
        /// <param name="_worldPos"></param>
        /// <returns></returns>
        public Vector3 GetGridPositionFromMouse(Vector3 _worldPos)
        {
            int x, y;
            x = Mathf.FloorToInt(_worldPos.x / cellSize);
            y = Mathf.FloorToInt(_worldPos.y / cellSize);
            return new Vector3(x, y);
        }

        public TObject GetValue(int _x, int _y)
        {
            if (_x >= 0 && _y >= 0 && _x < Width && _y < Height)
                return gridArray[_x, _y];
            return default;
        }

        public TObject GetValue(Vector2Int _arrayPos)
        {
            return !IsOutsideBounds(_arrayPos.x, _arrayPos.y) ? gridArray[_arrayPos.x, _arrayPos.y] : default;
        }

        public TObject GetValue(Vector3 _worldPos)
        {
            int x, y;
            GetXY(_worldPos, out x, out y);
            return GetValue(x, y);
        }

        public bool IsChooseable(Vector3 _worldPos, bool _isTerrain)
        {
            int x, y;
            GetXY(_worldPos, out x, out y);
            if (_isTerrain)
            {
                if (!IsOutsideBounds(_worldPos) && tilemap.GetTile(new Vector3Int((int)_worldPos.x, (int)_worldPos.y)) == null) return gridArray[x, y] == null;
                return default;
            }

            if (!IsOutsideBounds(_worldPos) && tilemap.GetTile(new Vector3Int((int)_worldPos.x, (int)_worldPos.y)) != null) return gridArray[x, y] == null;
            return default;
        }

        /// <summary>
        /// This function returns if a position, given the world position, is on the grid or not. For direct checking of bounds use "IsInsideBounds" instead.
        /// </summary>
        /// <param name="_worldPos"></param>
        /// <returns></returns>
        public bool IsOutsideBounds(Vector3 _worldPos)
        {
            int x, y;
            GetXY(_worldPos, out x, out y);
            return !(x >= 0 && y >= 0 && x < Width && y < Height);
        }

        public bool IsOutsideBounds(int _x, int _y)
        {
            return !(_x >= 0 && _y >= 0 && _x < Width && _y < Height);
        }

        public bool IsOutsideBounds(Vector2Int _arrayPos)
        {
            return IsOutsideBounds(_arrayPos.x, _arrayPos.y);
        }
    }
}