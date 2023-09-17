using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {

        private readonly Vector2Int[] directionsForQueen = new Vector2Int[]
        {
        new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
        new Vector2Int(0, -1), new Vector2Int(0, 1),
        new Vector2Int(1, -1), new Vector2Int(1, 0), new Vector2Int(1, 1)
        };

        private readonly Vector2Int[] directionsForBishop = new Vector2Int[]
        {
        new Vector2Int(-1, -1), new Vector2Int(-1, 1),
        new Vector2Int(1, -1), new Vector2Int(1, 1)
        };

        private readonly Vector2Int[] directionsForRook = new Vector2Int[]
        {
        new Vector2Int(-1, 0), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(0, 1)
        };

        private readonly Vector2Int[] directionsForKnight = new Vector2Int[]
        {
        new Vector2Int(-2, -1), new Vector2Int(-1, -2),
        new Vector2Int(1, -2), new Vector2Int(2, -1),
        new Vector2Int(-2, 1), new Vector2Int(-1, 2),
        new Vector2Int(1, 2), new Vector2Int(2, 1)
        };

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {

            switch (unit)
            {
                case ChessUnitType.Pon:
                    return FindPathForPon(from, to, grid);

                case ChessUnitType.Knight:
                    return FindPathForKnight(from, to, grid);

                case ChessUnitType.Bishop:
                    return FindPathForBishop(from, to, grid);

                case ChessUnitType.Rook:
                    return FindPathForRook(from, to, grid);

                case ChessUnitType.Queen:
                    return FindPathForQueen(from, to, grid);

                case ChessUnitType.King:
                    return FindPathForKing(from, to, grid);

            }


            return null;
        }

        private List<Vector2Int> FindPathForPon(Vector2Int from, Vector2Int to, ChessGrid grid)
        {

            List<Vector2Int> path = new List<Vector2Int>();

            if (from.x != to.x)
            {
                return null;
            }

            Vector2Int _currentPosition = from;
            int direction = from.y < to.y ? 1 : -1;

            while (_currentPosition.y != to.y)
            {
                _currentPosition.y += direction;
                if (grid.Get(_currentPosition) != null)
                {
                    return null;
                }
                path.Add(_currentPosition);
            }

            return path;
        }

        private List<Vector2Int> FindPathForKing(Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            return BFSOld(from, to, grid, directionsForQueen);
        }

        public List<Vector2Int> FindPathForKnight(Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            return BFSOld(from, to, grid,directionsForKnight);
        }

        

        public List<Vector2Int> FindPathForQueen(Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            return BFS(from, to, grid, directionsForQueen);
        }


        private List<Vector2Int> FindPathForBishop(Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            return BFS(from, to, grid, directionsForBishop);
        }


        private List<Vector2Int> FindPathForRook(Vector2Int from, Vector2Int to,ChessGrid grid)
        {
            return BFS(from, to, grid, directionsForRook);
        }



        public List<Vector2Int> BFSOld(Vector2Int from, Vector2Int to, ChessGrid grid, Vector2Int[] directions)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

            queue.Enqueue(from);
            parent[from] = from;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                if (current == to)
                {
                    break;
                }

                for (int i = 0; i < directions.Length; i++)
                {
                    int x = current.x + directions[i].x;
                    int y = current.y + directions[i].y;

                    if (x < 0 || x >= 8 || y < 0 || y >= 8)
                    {
                        continue;
                    }

                    Vector2Int next = new Vector2Int(x, y);

                    if (!parent.ContainsKey(next))
                    {

                        if (grid.Get(next) == null)
                        {
                            queue.Enqueue(next);
                            parent[next] = current;
                        }
                    }
                }
            }

            List<Vector2Int> path = new List<Vector2Int>();

            if (!parent.ContainsKey(to))
            {
                return path;
            }

            Vector2Int node = to;

            while (node != from)
            {
                path.Add(node);
                node = parent[node];
            }

            path.Add(from);
            path.Reverse();

            return path;
        }


        private List<Vector2Int> BFS(Vector2Int from, Vector2Int to, ChessGrid grid, Vector2Int[] directions)
        {
            var visited = new bool[8, 8];
            var queue = new Queue<Vector2Int>();
            var path = new Dictionary<Vector2Int, Vector2Int>();

            visited[from.x, from.y] = true;
            queue.Enqueue(from);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current == to)
                {
                    var result = new List<Vector2Int>();

                    while (current != from)
                    {
                        result.Add(current);
                        current = path[current];
                    }

                    result.Add(from);
                    result.Reverse();

                    return result;
                }

                foreach (var direction in directions)
                {
                    for (int i = 1; i < 8; i++)
                    {
                        var next = current + direction * i;

                        if (next.x < 0 || next.x >= 8 || next.y < 0 || next.y >= 8) break;
                        if (visited[next.x, next.y]) continue;
                        if (grid.Get(next) != null) break;

                        visited[next.x, next.y] = true;
                        path[next] = current;
                        queue.Enqueue(next);
                    }
                }
            }

            return null;
        }
    }
}


     
