using System;
using Coffee.UIEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.Configs.Accounts
{
    [Serializable]
    public class UIGradientConfig
    {
        [SerializeField]
        private UIGradient.Direction _direction;

        [SerializeField, ShowIf(nameof(_direction), UIGradient.Direction.Diagonal)]
        private Color _topLeft = Color.white;

        [SerializeField, ShowIf(nameof(_direction), UIGradient.Direction.Diagonal)]
        private Color _topRight = Color.white;

        [SerializeField, LabelText("$GetColor1Name")]
        private Color _color1 = Color.white;

        [SerializeField, LabelText("$GetColor2Name")]
        private Color _color2 = Color.white;

        [SerializeField, Range(-180, 180), ShowIf(nameof(CanShowRotation))]
        private float _rotation;

        [SerializeField, Range(-1, 1), LabelText("$GetOffsetName")]
        private float _offset1;

        [SerializeField, Range(-1, 1), ShowIf(nameof(_direction), UIGradient.Direction.Diagonal)]
        private float _horizontalOffset;

        [SerializeField]
        private UIGradient.GradientStyle _gradientStyle;

        private bool CanShowRotation =>
            _direction == UIGradient.Direction.Diagonal || _direction == UIGradient.Direction.Angle;

        public UIGradient.Direction Direction => _direction;
        public Color Color1 => _color1;
        public Color Color2 => _color2;
        public Color TopLeft => _topLeft;
        public Color TopRight => _topRight;
        public float Rotation => _rotation;
        public float Offset1 => _offset1;
        public float Offset2 => _horizontalOffset;
        public UIGradient.GradientStyle GradientStyle => _gradientStyle;

        private string GetOffsetName()
        {
            switch (_direction)
            {
                case UIGradient.Direction.Horizontal:
                    return "Horizontal Offset";
                case UIGradient.Direction.Vertical:
                    return "Vertical offset";
                case UIGradient.Direction.Angle:
                    return "Offset";
                case UIGradient.Direction.Diagonal:
                    return "Vertical offset";
            }

            return "Offset";
        }

        private string GetColor1Name()
        {
            switch (_direction)
            {
                case UIGradient.Direction.Horizontal:
                    return "Left";
                case UIGradient.Direction.Vertical:
                    return "Top";
                case UIGradient.Direction.Angle:
                    return "Color 1";
                case UIGradient.Direction.Diagonal:
                    return "Bottom Left";
            }

            return "Color1";
        }

        private string GetColor2Name()
        {
            switch (_direction)
            {
                case UIGradient.Direction.Horizontal:
                    return "Right";
                case UIGradient.Direction.Vertical:
                    return "Bottom";
                case UIGradient.Direction.Angle:
                    return "Color 2";
                case UIGradient.Direction.Diagonal:
                    return "Bottom Right";
            }

            return "Color2";
        }
    }
}