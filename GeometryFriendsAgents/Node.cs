using System;
using System.Drawing;

namespace GeometryFriendsAgents
{
    public class Node
    {
        private static int numberOfNodes = 0;
        /// <summary>
        /// The index of the node in the graph he was inserted into
        /// </summary>
        public int index;
        public enum Type
        {
            /// <summary>
            /// Represents an open space for the circle and rectangle
            /// </summary>
            Space,
            /// <summary>
            /// Represents a circle pixel
            /// </summary>
            Circle,
            /// <summary>
            /// Represents a rectangle pixel
            /// </summary>
            Rectangle,
            /// <summary>
            /// Represents a diamond (collectible) pixel
            /// </summary>
            Diamond,
            /// <summary>
            /// Represents an obstacle for the circle and rectangle
            /// </summary>
            Obstacle,
            /// <summary>
            /// Represents a platform which only the circle can go through
            /// </summary>
            CirclePlatform,
            /// <summary>
            /// Represents a platform which only the rectangle can go through
            /// </summary>
            RectanglePlatform
        };

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Point location { get; private set; }

        /// <summary>
        /// The type of pixel that the node represents.
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// Cost (length of the path) from the start node to this node 
        /// </summary>
        public float gCost { get; set; }
        /// <summary>
        /// Cost (straight-line distance) from this node to the end node
        /// </summary>
        public float hCost { get; set; }
        /// <summary>
        /// An estimate of the total distance if taking the current route. It's calculated by summing gCost and hCost.
        /// </summary>
        public float fCost { get { return this.gCost + this.hCost; } }

        /// <summary>
        /// The parent of the node (the previous node in a path)
        /// The private field holds the information
        /// The public field allows you to call get and set without making stack exceptions
        /// </summary>
    
        public Node parentNode { get; set; }

        /// <summary>
        /// State of the node in the current search.
        /// </summary>

        /// <summary>
        /// Creates a new instance of Node.
        /// </summary>
        /// <param name="x">The node's location along the X axis</param>
        /// <param name="y">The node's location along the Y axis</param>
        /// <param name="isWalkable">True if the node can be traversed, false if the node is a 'wall' for the agent</param>
        /// <param name="endLocation">The location of the destination node</param>
        public Node(int x, int y, Type type)
        {
            this.index = numberOfNodes;

            this.location = new Point(x, y);
            this.type = type;
            this.gCost = (float.MaxValue / 2);  // start gCost at 'infinity' (used float.MaxValue / 2 because of overflow when adding hCost)

            numberOfNodes++;
        }

        public override string ToString()
        {
            return string.Format("Node[X: {0}, Y: {1}, Type: {2}]", this.location.X, this.location.Y, this.type);
        }

        public Boolean isWalkable(AgentType agentType)
        {
            if (agentType == AgentType.Circle)
            {
                switch (this.type)
                {
                    case Type.Obstacle:
                    case Type.RectanglePlatform:
                        return false;
                    default:
                        return true;
                }
            }
            else
            {
                switch (this.type)
                {
                    case Type.Obstacle:
                    case Type.CirclePlatform:
                        return false;
                    default:
                        return true;
                }
            }
        }
    }
}