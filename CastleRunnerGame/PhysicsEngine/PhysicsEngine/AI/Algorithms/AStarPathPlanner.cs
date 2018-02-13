using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using PhysicsEngine.Artificial_Intelligence;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine.AI.Algorithms
{
    class AStarPathPlanner
    {
        List<Node> _Locations;
        List<Path> _PathsBetweenLocations;

        private Dictionary<Node, Storage> _CalculationStorage;
        private List<Node> _OpenNodes; //all nodes start as open
        private List<Node> _ClosedNodes;

        internal AStarPathPlanner(List<Node> locations, List<Path> paths)
        {
            _Locations = locations;
            _PathsBetweenLocations = paths;

            _OpenNodes = new List<Node>();
            _ClosedNodes = new List<Node>();

            _CalculationStorage = new Dictionary<Node, Storage>();
            SwapPathNodes();
        }

        //TODO: Fix this issue properly without having to do a loop. 
        internal void SwapPathNodes()
        {
            foreach (var pathsBetweenLocation in _PathsBetweenLocations)
            {
                foreach (var node in _Locations)
                {
                    if (pathsBetweenLocation._PointA._Position == node._Position)
                        pathsBetweenLocation._PointA = node;
                    else if (pathsBetweenLocation._PointB._Position == node._Position)
                        pathsBetweenLocation._PointB = node;
                }
            }
        }

        internal Node GetClosestNode(Body npc) //get the node closest to the npc
        {
            Polygon poly = npc._Shape as Polygon; //will be polygon
            float minX = poly._Vertices[0].X; //furthest to left
            float maxX = poly._Vertices[1].X; //furthest to right
            float minY = poly._Vertices[0].Y; //highest up
            float maxY = poly._Vertices[2].Y; //lowest down

            foreach (Node node in _Locations)
            {
                if (node._Position.X < maxX && node._Position.X > minX) //if in x coodinates
                {
                    if (node._Position.Y < maxY + 5 && node._Position.Y > minY)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        //Main algorithm for planning a path
        internal List<Node> AStarFollowPlayer(Body npc, Body player)
        {
            Node node = GetClosestNode(player);

            if (node == null)
                return null;

            return AStarGetPath(npc, node);
        }

        internal List<Node> AStarGetPath(Body npc, Node end)
        {
            Node start = GetClosestNode(npc);

            if (start == null)
                return null;

            //Reset these each time.
            _OpenNodes.Clear();
            _ClosedNodes.Clear();
            _CalculationStorage.Clear();
            _OpenNodes.Add(start);

            //Create new storage entry.
            _CalculationStorage.Add(start, new Storage(null, 0)); //no parent, and distance starts at 0.
            
            while (true)
            {
                Node shortest = GetShortestNode(); //first we get the shortest node
                ExpandNode(shortest, end); //expand the shortest node, close the current

                if (_OpenNodes.Count <= 0)
                {
                    return null; //no path :( 
                }

                if (!_OpenNodes.Contains(end)) continue;

                //We have reached end. Get paths. 
                List<Node> path = new List<Node>();
                Node current = end;
                while (_CalculationStorage[current]._ParentNode != null)
                {
                    path.Add(current);
                    current = _CalculationStorage[current]._ParentNode;
                }
                path.Add(start);
                path.Reverse();

                path.Remove(path[0]);
                return path;
            }
        }

        private Node GetShortestNode()
        {
            Node current = null;
            foreach (Node node in _OpenNodes)
            {
                if (current == null)
                    current = node;
                else
                {
                    if (_CalculationStorage[node]._CurrentDistance < _CalculationStorage[current]._CurrentDistance)
                    {
                        current = node;
                    }
                }
            }
            if (current == null)
                throw new NotImplementedException(); //something has gone wrong

            return current;
        }

        private void ExpandNode(Node node, Node dest)
        {
            float distanceToDest = CalculateActualDistance(dest, node);

            foreach (Path path in _PathsBetweenLocations) //for every path
            {
                if (node != path._PointB && node != path._PointA) //not a path we need to deal with.
                    continue; 

                Node checkA = path._PointA == node ? path._PointA : path._PointB; //Our current node
                Node checkB = path._PointB == node ? path._PointA : path._PointB; //Node we're expanding out

                if (checkA._Position.Y > checkB._Position.Y && !path._JumpPath) //Check if we need jump path. Skip if not.
                    continue;

                if (_ClosedNodes.Contains(checkB)) //Skip if node is already closed.
                    continue;

                if (_OpenNodes.Contains(checkB)) //If node we're expanding to is already open.
                {
                    float checkADist = _CalculationStorage[checkA]._CurrentDistance;
                    float checkBDist = _CalculationStorage[checkB]._CurrentDistance;

                    if (checkBDist < checkADist + path._Weight + distanceToDest) continue; //Skip if current distance is shorter.

                    _CalculationStorage[checkB]._CurrentDistance = checkADist + path._Weight + distanceToDest; //Change distance to new shorter route we found.
                    _CalculationStorage[checkB]._ParentNode = checkA; //Change parent to node we're expanding
                }
                else //If node isn't open
                {
                    float checkBDist = _CalculationStorage[checkA]._CurrentDistance + path._Weight + distanceToDest;
                    _CalculationStorage.Add(checkB, new Storage(checkA, checkBDist));
                    _OpenNodes.Add(checkB);
                }
            }
            CloseNode(node);
        }

        private void CloseNode(Node node)
        {
            _ClosedNodes.Add(node); //put into closed node list. 
            node._Closed = true; //set the variable to say its closed 
            _OpenNodes.Remove(node); // remove from open nodes
        }

        //for heirarchical path planning
        private float CalculateActualDistance(Node goalNode, Node currentNode)
        {
            Vector2 posA = goalNode._Position;
            Vector2 posB = currentNode._Position;

            float difX = Math.Abs(posA.X - posB.X);
            float difY = Math.Abs(posA.Y - posB.Y);

            return (float)Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));
        }
    }
}
