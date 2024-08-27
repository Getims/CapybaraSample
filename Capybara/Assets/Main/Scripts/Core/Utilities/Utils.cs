﻿using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.Core.Utilities
{
    public static partial class Utils
    {
        public static bool IncludesInInterval(float number, Vector2 interval)
        {
            if (number >= interval.x && number <= interval.y)
                return true;

            return false;
        }

        public static bool IncludesInIntervals(float number, List<Vector2> intervals)
        {
            bool result = false;

            foreach (var interval in intervals)
            {
                if (number >= interval.x && number <= interval.y)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static float Normalize(float number)
        {
            if (number < 0)
                return -1;
            if (number > 0)
                return 1;
            return 0;
        }

        /// <summary>
        /// <para>
        /// Compare a and b with equalError
        /// </para>
        /// <para>Return 0 if a=b.
        /// Return -1 if a less then b.
        /// Return 1 if a more then b</para>
        /// </summary>
        public static int CompareFloats(float a, float b, float equalError)
        {
            int result = 0;
            if (b < a - equalError)
                result = 1;
            if (b > a + equalError)
                result = -1;
            return result;
        }
        
        public static bool CompareFloatsBool(float a, float b, float equalError)
        {
            bool result = true;
            if (b < a - equalError)
                result = false;
            if (b > a + equalError)
                result = false;
            return result;
        }

        public static Vector3 DevideVectors(Vector3 vect1, Vector3 vect2)
        {
            Vector3 result = vect1;
            result.x /= vect2.x;
            result.y /= vect2.y;
            result.z /= vect2.z;
            return result;
        }

        public static Vector2 GetSpritePivot(Sprite sprite)
        {
            Vector2 pixelPivot = sprite.pivot;
            Rect spriteRect = sprite.rect;
            
            Vector2 spriteSize = new Vector2(spriteRect.width, spriteRect.height);
            Vector2 normalizedPivot = new Vector2(pixelPivot.x / spriteSize.x, pixelPivot.y / spriteSize.y);

            return normalizedPivot;
        }
    }
}