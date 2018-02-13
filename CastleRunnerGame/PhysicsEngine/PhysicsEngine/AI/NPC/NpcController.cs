using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhysicsEngine.AI.Algorithms;
using PhysicsEngine.Artificial_Intelligence;
using PhysicsEngine.Physics.Shapes;

namespace PhysicsEngine.AI.NPC
{
    class NpcController
    {
        //Where all NPC behaviour takes place. 
        //Including where paths are created for each NPC.
        private CaveMan _NpcCaveman;
        private AStarPathPlanner _PathPlanner;

        internal NpcController(List<Node> nodes, List<Path> paths, CaveMan man)
        {
            _NpcCaveman = man;
            _PathPlanner = new AStarPathPlanner(nodes, paths);
        }

        /// <summary>
        /// Simple path smoothing technique. This works by checking whether the next node is next to the node. 
        /// If the node is next to the node, the node gets removed.
        /// </summary>
        internal void SmoothPath()
        {
            //Get rid of not necessary nodes. 
            List<Node> removeNodes = new List<Node>();

            for (int i = 0; i < _NpcCaveman._Path.Count - 1; i++)
            {
                if (_NpcCaveman._Path[0]._Position.X != _NpcCaveman._Path[1]._Position.X + 50 &&
                    _NpcCaveman._Path[0]._Position.X != _NpcCaveman._Path[1]._Position.X + 50) continue;

                if (_NpcCaveman._Path[i + 1]._StartNode || _NpcCaveman._Path[i + 1]._EndNode) continue; //Don't remove start and end nodes.
                if (_NpcCaveman._Path.Count <= i + 2) continue; //so we don't remove a node we need. 
                if (_NpcCaveman._Path.Count - 1 == i) continue; //in case its last node. Don't want that removed.
                if (_NpcCaveman._Path[i+2]._Position.Y == _NpcCaveman._Path[i+1]._Position.Y) //Make sure the next node is on same level as this node.
                    removeNodes.Add(_NpcCaveman._Path[i+1]);
            }
            foreach (var removeNode in removeNodes)
            {
                _NpcCaveman._Path.Remove(removeNode);
            }
        }

        /// <summary>
        /// This method is used for NPC movement. 
        /// Calculates where the node is, and if the node is to the left sets the velocity to the left. Or if the node is to the right,
        /// Sets the velocity to move right. This will also make the character jump if the next node is above his head. 
        /// </summary>
        internal void Update() //Where we need to get _NpcCaveman moving!
        {
            //If the path is null, or the path has 0 nodes. (Reached the end of path, or a path has not been generated as of yet)
            if (_NpcCaveman._Path == null || _NpcCaveman._Path.Count <= 0)
            {
                _NpcCaveman.Update(new Vector2(0, 0)); //Set NPC velocity to 0

                _NpcCaveman._Path = _PathPlanner.AStarFollowPlayer(DungeonEscape._NpcCaveman._CaveManBody._BoundingBox,
                    DungeonEscape._Player._CharacterBody._BoundingBox); //Calculate the path and store it within the NPC

                if (_NpcCaveman._Path != null && _NpcCaveman._Path.Count > 1) //if we have found a path
                {
                    SmoothPath(); //Do a simple smooth
                }
                return;
            }

            Polygon poly = _NpcCaveman._CaveManBody._BoundingBox._Shape as Polygon;
            float minX = poly._Vertices[0].X; //left side
            float maxX = poly._Vertices[1].X; //right side

            float centerX = poly._Vertices[0].X + 25;

            //within 2 of the center
            float checkMinX = minX + 20;
            float checkMaxX = maxX - 20;

            //Current top left of NPC
            Vector2 currentPt = _NpcCaveman._Path[0]._Position;


            if (currentPt.Y < poly._Vertices[0].Y) //If caveman is below new point 
            {
                _NpcCaveman.Jump();
            }
            else if (!_NpcCaveman._FacingRight && _NpcCaveman._Path[0]._EndNode && _NpcCaveman._Path.Count > 1 && Math.Abs(_NpcCaveman._Path[0]._Position.X - _NpcCaveman._Path[1]._Position.X) > 80) //If got to jump a gap left
            {
                _NpcCaveman.Jump();
            }
            else if (_NpcCaveman._FacingRight && _NpcCaveman._Path[0]._StartNode && _NpcCaveman._Path.Count > 1 && Math.Abs(_NpcCaveman._Path[0]._Position.X - _NpcCaveman._Path[1]._Position.X) > 80)
            {
                _NpcCaveman.Jump();
            }

            if (currentPt.Y < poly._Vertices[0].Y) //Only set velocity when the character is at the right height
            {
                _NpcCaveman.Update(new Vector2(0, _NpcCaveman._CaveManBody._BoundingBox._Velocity.Y));
            }
            else
            {
                int speed = _NpcCaveman._IsGrounded ? 20 : 25; //slightly faster jumping speed
                if (_NpcCaveman._CaveManBody._BoundingBox._Velocity.Y > 5)
                    speed = 20;

                if (currentPt.X < centerX) //if point is to left of character
                {
                    _NpcCaveman.Update(new Vector2(-speed, _NpcCaveman._CaveManBody._BoundingBox._Velocity.Y));
                }
                else if (currentPt.X > centerX) //if point is to right
                {
                    _NpcCaveman.Update(new Vector2(speed, _NpcCaveman._CaveManBody._BoundingBox._Velocity.Y));
                }
            }

            if (currentPt.X < checkMaxX && currentPt.X > checkMinX) //middle of character
            {
                _NpcCaveman._Path.Remove(_NpcCaveman._Path[0]); //remove the point as we have reached it.
            }
        }
    }
}
