using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Khami.Utilities
{
    public static class TransformExtensions
    {
        public static void SetPosX(this Transform _transform, float _value)
        {
            var _newPos = _transform.position;
            _newPos.x = _value;
            _transform.position = _newPos;
        }
        
        public static void SetPosY(this Transform _transform, float _value)
        {
            var _newPos = _transform.position;
            _newPos.y = _value;
            _transform.position = _newPos;
        }
        
        public static void SetPosZ(this Transform _transform, float _value)
        {
            var _newPos = _transform.position;
            _newPos.z = _value;
            _transform.position = _newPos;
        }

        public static void SetLocalPosX(this Transform _transform, float _value)
        {
            var _newPos = _transform.localPosition;
            _newPos.x = _value;
            _transform.localPosition = _newPos;
        }
        
        public static void SetLocalPosY(this Transform _transform, float _value)
        {
            var _newPos = _transform.localPosition;
            _newPos.y = _value;
            _transform.localPosition = _newPos;
        }
        
        public static void SetLocalPosZ(this Transform _transform, float _value)
        {
            var _newPos = _transform.localPosition;
            _newPos.z = _value;
            _transform.localPosition = _newPos;
        }

        public static void SetScaleX(this Transform _transform, float _value)
        {
            var _newScale = _transform.localScale;
            _newScale.x = _value;
            _transform.localScale = _newScale;
        }

        public static bool IsDeepChildOf(this Transform _obj, Transform _parent)
        {
            Queue<Transform> _queue = new Queue<Transform>();
            _queue.Enqueue(_parent);
            while (_queue.Count > 0)
            {
                var _child = _queue.Dequeue();
                if (_child == _obj)
                    return true;
                foreach (Transform _transform in _child)
                    _queue.Enqueue(_transform);
            }
            return false;
        }
    }
}
