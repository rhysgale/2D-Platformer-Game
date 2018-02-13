using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhysicsEngine.Artificial_Intelligence;
using PhysicsEngine.GameDrawing;

namespace PhysicsEngine.AI.Algorithms
{
    internal class PathCreation
    {
        // Complicated path creating algorithm. 
        // Couldn't get tiled or any map creator working, which would have been easier for path creating 
        // and node creating 

        /// <summary>
        /// Creates the jump and drop paths for the map.
        /// </summary>
        /// <param name="nodeList">List of objects in the map</param>
        /// <returns></returns>
        internal List<Path> CreatePath(List<Node> nodeList)
        {
            //Rather messy way of generating paths between nodes, that are drops or jumps.

            List<Path> pathsList = new List<Path>();
            foreach (Node node in nodeList) //Go through every node
            {
                if (node._StartNode != true && node._EndNode != true) //Has to be a start node or end node for path creation. 
                    continue;

                Vector2 checkpos1 = new Vector2(node._Position.X + (node._StartNode ? -100 : 100), node._Position.Y); //If at start, check to the left, else check to the right.
                for (int i = 0; i < 25; i++) //Check 250 down, one to the left or right.
                {
                    checkpos1.Y += 10; //Check down 10 more each loop.
                    bool pathFound = false; //If a path has been found or not, so we can break out of the nested loops.

                    foreach (Node node2 in nodeList) //Go through every node again. 
                    {
                        if (node == node2) //Don't check node against node.
                            continue;

                        if (node2._Position != checkpos1) continue; //If node isn't on position we're checking. 

                        Path path = new Path(node, node2, i < 15); //Create a path if there was a node, can only be a "jump" path if under 150 tall. 
                        pathsList.Add(path); //Add the path.
                        pathFound = true; //Set this to true so we can break out of nested loops.
                    }

                    if (pathFound)
                        break; //break out of nested loop.
                }


                //=======================================================================
                //===================CHECK ACROSS AND UP=================================
                //=======================================================================
                bool found = false; 
                Vector2 checkpos2 = new Vector2(node._Position.X + (node._StartNode ? -50 : 50), node._Position.Y);

                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        foreach (Node node2 in nodeList)
                        {
                            if (node == node2)
                                continue;

                            if (!node._EndNode || !node2._StartNode) continue; //Only want to create paths between start and end nodes. 

                            if (node2._Position != checkpos2) continue;

                            //===============================================
                            //=====CHECK ABOVE POINT, MAKE SURE ITS CLEAR====
                            //===============================================
                            Vector2 clearPos = checkpos2;

                            for (int l = 0; l < 25; l++)
                            {
                                clearPos.Y -= 10;
                                foreach (var node3 in nodeList)
                                {
                                    if (node3._Position != clearPos) continue;

                                    found = true; //not clear
                                    break;
                                }
                                if (found)
                                    break;
                            }

                            if (found)
                                break;

                            Path path = new Path(node, node2, true); //Create new path between the nodes.
                            pathsList.Add(path); //Add the path to our list of paths.
                            found = true; //So we can break out of nested loops. 
                            break; //break;
                        }
                        checkpos2.Y -= 10; //go up 10, until 150 up
                        if (found) //break out of loop
                            break;
                    }
                    checkpos2.X += node._StartNode ? -50 : 50; //move across a tile, max total of 3
                    checkpos2.Y = node._Position.Y;

                    if (found) //break out of loop
                        break;
                }
            }
            return pathsList;
        }

        /// <summary>
        /// Get a list of nodes from the given floor
        /// </summary>
        /// <param name="obj">The floor object that was created.</param>
        /// <returns></returns>
        internal List<Node> GetNodesFromObject(MapObject obj)
        {
            var nodeList = new List<Node>();

            if (obj._Type != TextureType.Floor) return null; //only floor can be nodes.

            for (int i = 0; i < obj._DrawRectList.Count; i++) //for every rectangle
            {
                var pos = new Vector2(obj._DrawRectList[i].Center.X, obj._DrawRectList[i].Center.Y - 25);
                var node = new Node(pos, i == 0, i == obj._DrawRectList.Count - 1);
                nodeList.Add(node); //create a node on its top edge
            }
            return nodeList; //return the list of nodes.
        }

        /// <summary>
        /// Creates paths for each of the nodes. The nodelist should be one map object only. 
        /// </summary>
        /// <param name="mapObj">A floor map object.</param>
        /// <returns></returns>
        internal List<Path> CreatePathList(MapObject mapObj)
        {
            if (mapObj._Type != TextureType.Floor) return null;
            var nodes = GetNodesFromObject(mapObj);
            List<Path> pathList = new List<Path>();

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Path path = new Path(nodes[i], nodes[i+1], false);
                pathList.Add(path);
            }
            return pathList;
        }
    }
}
