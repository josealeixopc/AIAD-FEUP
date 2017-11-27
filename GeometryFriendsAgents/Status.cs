﻿
using GeometryFriends;
using GeometryFriends.AI.Perceptions.Information;
using System;
using static GeometryFriendsAgents.Utils;

namespace GeometryFriendsAgents
{
    //Each Agent will have a state machine, which functions according to the next diamond to catch
    public class Status
    {
        private Quantifier Left_From_Target;
        private Quantifier Right_From_Target;
        private Quantifier Above_Target;
        private Quantifier Below_Target;

        private bool With_Obstacle_Between; //TODO

        private Quantifier Above_Other_Agent;
        private Quantifier Below_Other_Agent;
        private Quantifier Right_From_Other_Agent;
        private Quantifier Left_From_Other_Agent;
        private bool Near_Other_Agent;

        private bool Moving;
        private bool Other_Agent_Moving;

        private bool Moving_Right;
        private bool Moving_Left;

        private bool Blocked; //TODO When Agent is MOVING but his position doesn't change
        //TODO Implement Status Moving Fast, and Moving Slow OR Moving Direction

        private float margin = 10;
        private float circle_radius;
        private float rectangle_height;

        private float a_lot_distance = 200;
        private float a_bit_distance = 50;
        //TODO Implement a velocity margin



        public Status()
        {

        }

        public Quantifier LEFT_FROM_TARGET { get => Left_From_Target; set => Left_From_Target = value; }
        public Quantifier RIGHT_FROM_TARGET { get => Right_From_Target; set => Right_From_Target = value; }
        public Quantifier ABOVE_TARGET { get => Above_Target; set => Above_Target = value; }
        public Quantifier BELOW_TARGET { get => Below_Target; set => Below_Target = value; }
        public bool WITH_OBSTACLE_BETWEEN { get => With_Obstacle_Between; set => With_Obstacle_Between = value; }
        public Quantifier ABOVE_OTHER_AGENT { get => Above_Other_Agent; set => Above_Other_Agent = value; }
        public Quantifier BELOW_OTHER_AGENT { get => Below_Other_Agent; set => Below_Other_Agent = value; }
        public Quantifier RIGHT_FROM_OTHER_AGENT { get => Right_From_Other_Agent; set => Right_From_Other_Agent = value; }
        public Quantifier LEFT_FROM_OTHER_AGENT { get => Left_From_Other_Agent; set => Left_From_Other_Agent = value; }
        public bool NEAR_OTHER_AGENT { get => Near_Other_Agent; set => Near_Other_Agent = value; }
        public bool MOVING { get => Moving; set => Moving = value; }
        public bool OTHER_AGENT_MOVING { get => Other_Agent_Moving; set => Other_Agent_Moving = value; }
        public bool MOVING_RIGHT { get => Moving_Right; set => Moving_Right = value; }
        public bool MOVING_LEFT { get => Moving_Left; set => Moving_Left = value; }

        //This function only cares for the current Agent
        /*
        public void Update(CircleRepresentation actualCircle, RectangleRepresentation actualRectangle, CollectibleRepresentation diamondToCatch, AgentType thisAgent)
        {
            this.circle_radius = actualCircle.Radius;
            this.rectangle_height = actualRectangle.Height;
            if (thisAgent == AgentType.Circle)
            {
                compareAgentWithTarget(actualCircle.X, actualCircle.Y, diamondToCatch.X, diamondToCatch.Y);
                compareAgents(actualCircle.X, actualCircle.Y, actualRectangle.X, actualRectangle.Y);
                checkMovement(actualCircle.VelocityX, actualCircle.VelocityY, actualRectangle.VelocityX, actualRectangle.VelocityY);
            }
            else
            {
                compareAgentWithTarget(actualRectangle.X, actualRectangle.Y, diamondToCatch.X, diamondToCatch.Y);
                compareAgents(actualRectangle.X, actualRectangle.Y, actualCircle.X, actualCircle.Y);
                checkMovement(actualRectangle.VelocityX, actualRectangle.VelocityY, actualCircle.VelocityX, actualCircle.VelocityY);
            }
        }*/

        //This function takes into account also the future representation of the agent
        public void Update(CircleRepresentation[] circles, RectangleRepresentation[] rectangles, CollectibleRepresentation diamondToCatch, AgentType thisAgent)
        {
            
            CircleRepresentation actualCircle = circles[0];
            CircleRepresentation futureCircle = circles[1];
            RectangleRepresentation actualRectangle = rectangles[0];
            RectangleRepresentation futureRectangle = rectangles[1];


            this.circle_radius = actualCircle.Radius;
            this.rectangle_height = actualRectangle.Height;
            if (thisAgent == AgentType.Circle)
            {
                compareAgentWithTarget(actualCircle.X, actualCircle.Y, diamondToCatch.X, diamondToCatch.Y);
                compareAgents(actualCircle.X, actualCircle.Y, actualRectangle.X, actualRectangle.Y);
                checkMovement(actualCircle.VelocityX, actualCircle.VelocityY, actualRectangle.VelocityX, actualRectangle.VelocityY);
            }
            else
            {
                compareAgentWithTarget(actualRectangle.X, actualRectangle.Y, diamondToCatch.X, diamondToCatch.Y);
                compareAgents(actualRectangle.X, actualRectangle.Y, actualCircle.X, actualCircle.Y);
                checkMovement(actualRectangle.VelocityX, actualRectangle.VelocityY, actualCircle.VelocityX, actualCircle.VelocityY);
            }
        }

        private void compareAgentWithTarget(float agentXposition, float agentYposition, float targetXposition, float targetYposition)
        {
            float targetRightBound = targetXposition + margin;
            float targetLeftBound = targetXposition - margin;
            float targetUpperBound = targetYposition - margin; //Because Y is inverted
            float targetLowerBound = targetYposition + margin; 
            //X Axis
            //Agent is right from the target diamond
            if (agentXposition > targetRightBound)
            {
                //a lot
                if(agentXposition > targetRightBound + a_lot_distance)
                {
                    this.LEFT_FROM_TARGET = Quantifier.NONE;
                    this.RIGHT_FROM_TARGET = Quantifier.A_LOT;
                }
                //just a bit
                else if(agentXposition > targetRightBound + a_bit_distance)
                {
                    this.LEFT_FROM_TARGET = Quantifier.NONE;
                    this.RIGHT_FROM_TARGET = Quantifier.A_BIT;
                }
                else
                {
                    this.LEFT_FROM_TARGET = Quantifier.NONE;
                    this.RIGHT_FROM_TARGET = Quantifier.SLIGHTLY;
                }
            }
            //Agent is left from the target diamond
            else if (agentXposition < targetLeftBound)
            {
                //a lot
                if(agentXposition < targetLeftBound - a_lot_distance)
                {
                    this.LEFT_FROM_TARGET = Quantifier.A_LOT;
                    this.RIGHT_FROM_TARGET = Quantifier.NONE;
                }
                //just a bit
                else if(agentXposition < targetLeftBound - a_bit_distance)
                {
                    this.LEFT_FROM_TARGET = Quantifier.A_BIT;
                    this.RIGHT_FROM_TARGET = Quantifier.NONE;
                }
                else
                {
                    this.LEFT_FROM_TARGET = Quantifier.SLIGHTLY;
                    this.RIGHT_FROM_TARGET = Quantifier.NONE;
                }
            }
            //Agent is aligned vertically with target diamond
            else
            {
                this.LEFT_FROM_TARGET = Quantifier.NONE;
                this.RIGHT_FROM_TARGET = Quantifier.NONE;
            }


            //Y Axis (inverted)
            //Agent is above the target diamond
            if (agentYposition < targetUpperBound)
            {
                //a lot
                if(agentYposition < targetUpperBound - a_lot_distance)
                {
                    this.ABOVE_TARGET = Quantifier.A_LOT;
                    this.BELOW_TARGET = Quantifier.NONE;
                }
                //just a bit
                else if(agentYposition < targetUpperBound - a_bit_distance)
                {
                    this.ABOVE_TARGET = Quantifier.A_BIT;
                    this.BELOW_TARGET = Quantifier.NONE;
                }
                else
                {
                    this.ABOVE_TARGET = Quantifier.SLIGHTLY;
                    this.BELOW_TARGET = Quantifier.NONE;
                }
                
            }
            //Agent is below the target diamond
            else if (agentYposition > targetLowerBound)
            {
                //a lot
                if(agentYposition > targetLowerBound + a_lot_distance)
                {
                    this.ABOVE_TARGET = Quantifier.NONE;
                    this.BELOW_TARGET = Quantifier.A_LOT;
                }
                //just a bit
                else if(agentYposition > targetLowerBound + a_bit_distance)
                {
                    this.ABOVE_TARGET = Quantifier.NONE;
                    this.BELOW_TARGET = Quantifier.A_BIT;
                }
                else
                {
                    this.ABOVE_TARGET = Quantifier.NONE;
                    this.BELOW_TARGET = Quantifier.SLIGHTLY;
                }
            }
            //Agent is aligned horizontally with target diamond
            else
            {
                this.ABOVE_TARGET = Quantifier.NONE;
                this.BELOW_TARGET = Quantifier.NONE;
            }
        }

        private void compareAgents(float agent1Xposition, float agent1Yposition, float agent2Xposition, float agent2Yposition)
        {
            float minimum_distance_from_agent_centres_X = circle_radius + Utils.getRectangleWidth(rectangle_height)/2;
            float minimum_distance_from_agent_centres_Y = circle_radius + rectangle_height/2;

            float agent2RightBound = agent2Xposition + minimum_distance_from_agent_centres_X; //Minimum distances between centres for the agents to be next to each other
            float agent2LeftBound = agent2Xposition - minimum_distance_from_agent_centres_X;
            float agent2UpperBound = agent2Yposition - minimum_distance_from_agent_centres_Y;
            float agent2LowerBound = agent2Yposition + minimum_distance_from_agent_centres_Y;

            //X Axis
            if (agent1Xposition > agent2RightBound)
            {
                if(agent1Xposition > agent2RightBound + a_lot_distance)
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.A_LOT;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.NONE;
                }
                else if(agent1Xposition > agent2RightBound + a_bit_distance) 
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.A_BIT;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.NONE;
                }
                else
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.SLIGHTLY;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.NONE;
                }
            }
            else if (agent1Xposition < agent2LeftBound)
            {
                if(agent1Xposition < agent2LeftBound - a_lot_distance)
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.NONE;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.A_LOT;
                }
                else if(agent1Xposition < agent2LeftBound - a_bit_distance)
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.NONE;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.A_BIT;
                }
                else
                {
                    this.RIGHT_FROM_OTHER_AGENT = Quantifier.NONE;
                    this.LEFT_FROM_OTHER_AGENT = Quantifier.SLIGHTLY;
                }
            }
            else
            {
                this.RIGHT_FROM_OTHER_AGENT = Quantifier.NONE;
                this.LEFT_FROM_OTHER_AGENT = Quantifier.NONE;
            }


            //Y Axis
            if (agent1Yposition < agent2UpperBound)
            {
                if(agent1Yposition < agent2UpperBound - a_lot_distance)
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.A_LOT;
                    this.BELOW_OTHER_AGENT = Quantifier.NONE;
                }
                else if(agent1Yposition < agent2UpperBound - a_bit_distance)
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.A_BIT;
                    this.BELOW_OTHER_AGENT = Quantifier.NONE;
                }
                else
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.SLIGHTLY;
                    this.BELOW_OTHER_AGENT = Quantifier.NONE;
                }
            }
            else if (agent1Yposition > agent2LowerBound)
            {
                if(agent1Yposition > agent2LowerBound + a_lot_distance)
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.NONE;
                    this.BELOW_OTHER_AGENT = Quantifier.A_LOT;
                }
                else if(agent1Yposition > agent2LowerBound + a_bit_distance)
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.NONE;
                    this.BELOW_OTHER_AGENT = Quantifier.A_BIT;
                }
                else
                {
                    this.ABOVE_OTHER_AGENT = Quantifier.NONE;
                    this.BELOW_OTHER_AGENT = Quantifier.SLIGHTLY;
                }
            }
            else
            {
                this.ABOVE_OTHER_AGENT = Quantifier.NONE;
                this.BELOW_OTHER_AGENT = Quantifier.NONE;
            }

            if(checkNear(new Quantifier[4] { this.ABOVE_OTHER_AGENT, this.BELOW_OTHER_AGENT, this.RIGHT_FROM_OTHER_AGENT, this.LEFT_FROM_OTHER_AGENT}))
            {
                this.NEAR_OTHER_AGENT = true;
            }
            else
            {
                this.NEAR_OTHER_AGENT = false;
            }
        }

        private void checkMovement(float agent1Xvel, float agent1Yvel, float agent2Xvel, float agent2Yvel)
        {
            if(Math.Abs(agent2Xvel) > 0 || Math.Abs(agent2Yvel) > 0)
            {
                this.OTHER_AGENT_MOVING = true;
            }
            else
            {
                this.OTHER_AGENT_MOVING = false;
            }

            if(agent1Xvel > 0)
            {
                this.MOVING_RIGHT = true;
                this.MOVING_LEFT = false;
                this.MOVING = true;
            }
            else if(agent1Xvel < 0)
            {
                this.MOVING_RIGHT = false;
                this.MOVING_LEFT = true;
                this.MOVING = true;
            }
            else
            {
                this.MOVING_RIGHT = false;
                this.MOVING_LEFT = false;
                this.MOVING = false;
            }
        }

        public override string ToString()
        {
            return "LEFT FROM TARGET: " + Left_From_Target.ToString() + " | "
                + "RIGHT FROM TARGET: " + Right_From_Target.ToString() + " | "
                + "ABOVE TARGET: " + Above_Target.ToString() + " | "
                + "BELOW TARGET: " + Below_Target.ToString() + " | "
                + "WITH OBSTACLE BETWEEN: " + With_Obstacle_Between.ToString() + " | "
                + "ABOVE OTHER AGENT: " + Above_Other_Agent.ToString() + " | "
                + "BELOW OTHER AGENT: " + Below_Other_Agent.ToString() + " | "
                + "RIGHT FROM OTHER AGENT: " + Right_From_Other_Agent.ToString() + " | "
                + "LEFT FROM OTHER AGENT: " + Left_From_Other_Agent.ToString() + " | "
                + "NEAR OTHER AGENT: " + Near_Other_Agent.ToString() + " | "
                + "CIRCLE_RADIUS: " + circle_radius.ToString() + " | "
                + "RECTANGLE_HEIGHT: " + rectangle_height.ToString();
        }

        //Number of flags should be 4 (Above, Below, Right, Left)
        private bool checkNear(Quantifier[] flags)
        {
            if(flags.Length != 4)
            {
                throw new ArgumentException("There should be exactly 4 flags");
            }
            else
            {
                int slightly_count = 0;
                int none_count = 0;
                for(int i = 0; i < flags.Length; i++)
                {
                    if(flags[i] == Quantifier.NONE)
                    {
                        none_count++;
                    }
                    else if(flags[i] == Quantifier.SLIGHTLY)
                    {
                        slightly_count++;
                    }
                }

                if(slightly_count == 1 && none_count == 3) //To be near, 1 flag should be SLIGHTLY and the other 3 NONE
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

}