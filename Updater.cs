using System;
using System.Collections.Generic;
using UnityEngine;

namespace Efficient_Updating_System
{
    public class Updater : MonoBehaviour
    {
        private readonly List<IUpdate> _updateQueue = new();
        private readonly List<IUpdate> _updateAddQueue = new();
        private readonly List<IUpdate> _updateRemovalQueue = new();
        
        private readonly List<IFixedUpdate> _fixedUpdateQueue = new();
        private readonly List<IFixedUpdate> _fixedUpdateAddQueue = new();
        private readonly List<IFixedUpdate> _fixedUpdateRemovalQueue = new();
        
        private readonly List<ILateUpdate> _lateUpdateQueue = new();
        private readonly List<ILateUpdate> _lateUpdateAddQueue = new();
        private readonly List<ILateUpdate> _lateUpdateRemovalQueue = new();
        
        public void Update()
        {
            if (_updateAddQueue.Count > 0)
            {
                for (var i = 0; i < _updateAddQueue.Count; i++)
                {
                    var element = _updateAddQueue[i];
                    
                    _updateQueue.Add(element);
                    _updateAddQueue.Remove(element);
                }
            }

            if (_updateRemovalQueue.Count > 0)
            {
                for (var i = 0; i < _updateRemovalQueue.Count; i++)
                {
                    var element = _updateRemovalQueue[i];
                    
                    _updateQueue.Remove(element);
                    _updateRemovalQueue.Remove(element);
                }
            }

            foreach (var update in _updateQueue)
            {
                update.EfficientUpdate(Time.deltaTime);
            }
        }
        public void FixedUpdate()
        {
            if (_fixedUpdateAddQueue.Count > 0)
            {
                for (int i = 0; i < _fixedUpdateAddQueue.Count; i++)
                {
                    var element = _fixedUpdateAddQueue[i];
                    
                    _fixedUpdateQueue.Add(element);
                    _fixedUpdateAddQueue.Remove(element);
                }
            }

            if (_fixedUpdateRemovalQueue.Count > 0)
            {
                for (var i = 0; i < _fixedUpdateRemovalQueue.Count; i++)
                {
                    var element = _fixedUpdateRemovalQueue[i];

                    _fixedUpdateQueue.Remove(element);
                    _fixedUpdateRemovalQueue.Remove(element);
                }
            }

            foreach (var fixedUpdate in _fixedUpdateQueue)
            {
                fixedUpdate.EfficientFixedUpdate(Time.deltaTime);
            }
        } 
        public void LateUpdate()
        {
            if (_lateUpdateAddQueue.Count > 0)
            {
                for (var i = 0; i < _lateUpdateAddQueue.Count; i++)
                {
                    var element = _lateUpdateAddQueue[i];
                    
                    _lateUpdateQueue.Add(element);
                    _lateUpdateAddQueue.Remove(element);
                }
            }

            if (_lateUpdateRemovalQueue.Count > 0)
            {
                for (var i = 0; i < _lateUpdateRemovalQueue.Count; i++)
                {
                    var element = _lateUpdateRemovalQueue[i];

                    _lateUpdateQueue.Remove(element);
                    _lateUpdateRemovalQueue.Remove(element);
                }
            }

            foreach (var lateUpdate in _lateUpdateQueue)
            {
                lateUpdate.EfficientLateUpdate(Time.deltaTime);
            }
        }

        public void RegisterUpdate(IAbstractUpdate script, UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    _updateAddQueue.Add((IUpdate)script);
                    break;
                case UpdateType.FixedUpdate:
                    _fixedUpdateAddQueue.Add((IFixedUpdate)script);
                    break;
                case UpdateType.LateUpdate:
                    _lateUpdateAddQueue.Add((ILateUpdate)script);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
            }
            
            Debug.Log($"Registered {script} to {updateType} queue. ");
        }
        public void UnregisterUpdate(IAbstractUpdate script, UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    _updateRemovalQueue.Add((IUpdate)script);
                    break;
                case UpdateType.FixedUpdate:
                    _fixedUpdateRemovalQueue.Add((IFixedUpdate)script);
                    break;
                case UpdateType.LateUpdate:
                    _lateUpdateRemovalQueue.Add((ILateUpdate)script);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
            }
            
            Debug.Log($"Unregistered {script} to {updateType} queue. ");
        }
    }
}