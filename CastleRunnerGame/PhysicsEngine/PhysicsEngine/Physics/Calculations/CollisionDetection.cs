using System;
using PhysicsEngine.Physics.Shapes;
using Microsoft.Xna.Framework;

namespace PhysicsEngine.Physics.Calculations
{
    class CollisionDetection
    {
        internal CollisionResult DetectCollision(Body body, Body body2)
        {
            if (body._Shape is Polygon && body2._Shape is Polygon)
            {
                return PolyPolyCollision(body, body2);
            }
            switch (body._Shape)
            {
                case Circle _ when body2._Shape is Circle:
                    return CircleCircleCollision(body, body2);
                case Polygon _ when body2._Shape is Circle:
                    break;
                case Circle _ when body2._Shape is Polygon:
                    break;
            }


            throw new NotImplementedException();
        }

        #region Polygon Collisions
        private CollisionResult PolyPolyCollision(Body shapeA, Body shapeB)
        {
            Polygon a = shapeA._Shape as Polygon;
            Polygon b = shapeB._Shape  as Polygon;
            CollisionResult result = new CollisionResult();

            float overlap = float.PositiveInfinity;
            int edgeCountA = a._Edges.Count;
            int edgeCountB = b._Edges.Count;

            Vector2 translationAxis = new Vector2();
            Vector2 currentAxis;

            for (int i = 0; i < edgeCountA + edgeCountB; i++)
            {
                currentAxis = (i < edgeCountA ? a._Edges[i] : b._Edges[i - edgeCountA]);

                //get the normal to the axis
                Vector2 axis = new Vector2(-currentAxis.Y, currentAxis.X);
                axis.Normalize();

                //project both polygons onto the axis
                Vector2 projectionA = ProjectToAxis(axis, a);
                Vector2 projectionB = ProjectToAxis(axis, b);

                //get the overlap amount
                float collisionDistance = GetOverlapAmount(projectionA, projectionB);

                //if positive value... we don't overlap so break out of loop!
                if (collisionDistance >= 0)
                {
                    result._CollisionDetected = false;
                    break;
                }

                //get the positive collision distance
                collisionDistance = Math.Abs(collisionDistance);

                //if current collision is the shortest distance
                if (collisionDistance < overlap)
                {
                    //save the minimum overlap amount, and the axis we are overlapping on
                    overlap = collisionDistance;
                    translationAxis = axis;
                }
            }

            //==================================================
            //===Now we work out which way to push each shape!==
            //==================================================
            Vector2 directionA = CalculateDirection(a.GetCentreOfMass(), b.GetCentreOfMass());
            Vector2 directionB = CalculateDirection(b.GetCentreOfMass(), a.GetCentreOfMass());

            //Now work out the ratio of which we push each shape, which depends on the mass
            float totalMass = shapeA._Mass + shapeB._Mass;
            float ratioA = (shapeA._Mass == 0 ? 0 : totalMass / shapeA._Mass);
            float ratioB = (shapeB._Mass == 0 ? 0 : totalMass / shapeB._Mass);

            //Now we set the translation vectors to positive
            result._MinimumTranslationVectorA.X = Math.Abs(translationAxis.X * (overlap * ratioA));
            result._MinimumTranslationVectorA.Y = Math.Abs(translationAxis.Y * (overlap * ratioA));
            result._MinimumTranslationVectorB.X = Math.Abs(translationAxis.X * (overlap * ratioB));
            result._MinimumTranslationVectorB.Y = Math.Abs(translationAxis.Y * (overlap * ratioB));

            //now we change the direction its pushed in
            result._MinimumTranslationVectorA *= directionA;
            result._MinimumTranslationVectorB *= directionB;

            return result;
        }

        internal float GetOverlapAmount(Vector2 projectionA, Vector2 projectionB)
        {
            //Returns the amount of overlap
            //In each vector, X is min value, Y is max value of the projection.
            if (projectionA.X < projectionB.X)
                return projectionB.X - projectionA.Y;
            else
                return projectionA.X - projectionB.Y;
        }

        internal Vector2 ProjectToAxis(Vector2 axis, Polygon polygon)
        {
            //Project polygon onto axis with the dot product
            float axisProjection = Vector2.Dot(axis, polygon._Vertices[0]);
            Vector2 minMax = new Vector2(axisProjection);

            for (int i = 0; i < polygon._Vertices.Count; i++)
            {
                axisProjection = Vector2.Dot(polygon._Vertices[i], axis);

                if (axisProjection < minMax.X)
                    minMax.X = axisProjection;
                else if (minMax.Y < axisProjection)
                    minMax.Y = axisProjection;
            }

            //return the min and max projections of polygon onto axis
            return minMax;
        }
        #endregion

        #region Circle Collisions
        //determines whether there is an overlap. If so, also calculates the minimum translation vector
        private CollisionResult CircleCircleCollision(Body shapeA, Body shapeB)
        {
            CollisionResult result = new CollisionResult();
            Circle circleA = shapeA._Shape as Circle;
            Circle circleB = shapeB._Shape as Circle;
            Vector2 centreA = shapeA._Shape.GetPos();
            Vector2 centreB = shapeB._Shape.GetPos();
            double overlap;

            float dx, dy;

            //determine if they are colliding 
            if (centreA.X < centreB.X)
                dx = centreB.X - centreA.X;
            else
                dx = centreA.X - centreB.X;

            if (centreA.Y < centreB.Y)
                dy = centreB.Y - centreA.Y;
            else
                dy = centreA.Y - centreB.Y;

            double distanceBetweenCircleCentres = Math.Sqrt(dx * dx + dy * dy);

            if (distanceBetweenCircleCentres <= circleA._Radius + circleB._Radius) //if touching or overlapping
            {
                //if we get a collision, calculate the overlap
                overlap = (circleA._Radius + circleB._Radius) - distanceBetweenCircleCentres;
            }
            else
            {
                //no collision. No need to calculate any further!
                result._CollisionDetected = false;
                return result;
            }

            //=============================================================
            //Work out the minimum translation vector to push the shapes!==
            //=============================================================
            Vector2 lineBetweenCenters = new Vector2(centreA.X - centreB.X, centreA.Y - centreB.Y);
            lineBetweenCenters.Normalize();
            //Vector2 translationVec = lineBetweenCenters * (float)overlap;

            //get the mass, see which is pushed more...
            float totalMass = shapeA._Mass + shapeB._Mass;
            float ratioA = (shapeA._Mass == 0 ? 0 : totalMass / shapeA._Mass);
            float ratioB = (shapeB._Mass == 0 ? 0 : totalMass / shapeB._Mass);

            //get the direction we need to push each shape
            Vector2 directionA = CalculateDirection(shapeA._Shape.GetCentreOfMass(), shapeB._Shape.GetCentreOfMass());
            Vector2 directionB = CalculateDirection(shapeB._Shape.GetCentreOfMass(), shapeA._Shape.GetCentreOfMass());

            result._MinimumTranslationVectorA = lineBetweenCenters * ((float)overlap * ratioA);
            result._MinimumTranslationVectorB = lineBetweenCenters * ((float)overlap * ratioB);

            //We worked out the direction to push with the centre of mass, now need to apply this to the vectors.
            //Make them all positive
            result._MinimumTranslationVectorA.X = Math.Abs(result._MinimumTranslationVectorA.X);
            result._MinimumTranslationVectorA.Y = Math.Abs(result._MinimumTranslationVectorA.Y);
            result._MinimumTranslationVectorB.X = Math.Abs(result._MinimumTranslationVectorB.X);
            result._MinimumTranslationVectorB.Y = Math.Abs(result._MinimumTranslationVectorB.Y);

            //Now multiply by the direction (will only change signs)
            result._MinimumTranslationVectorA *= directionA;
            result._MinimumTranslationVectorB *= directionB;

            return result;
        }
        #endregion

        private Vector2 CalculateDirection(Vector2 centreOfMassA, Vector2 centreOfMassB)
        {
            //result will contain +1 or -1 to determine the direction to push each shape
            Vector2 result = new Vector2();

            if (centreOfMassA.X < centreOfMassB.X)
                result.X = -1;
            else
                result.X = 1;

            if (centreOfMassA.Y < centreOfMassB.Y)
                result.Y = -1;
            else
                result.Y = 1;

            return result;
        }
    }
}
