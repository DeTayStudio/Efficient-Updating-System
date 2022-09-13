using System;
using System.Collections.Generic;
using UnityEngine;

namespace Efficient_Updating_System
{
    public class Updater : MonoBehaviour
    {
        private static Updater _instance;
        
        private static readonly List<IUpdate> UpdateQueue = new();
        private static readonly List<IUpdate> UpdateAddQueue = new();
        private static readonly List<IUpdate> UpdateRemovalQueue = new();
        
        private static readonly List<IFixedUpdate> FixedUpdateQueue = new();
        private static readonly List<IFixedUpdate> FixedUpdateAddQueue = new();
        private static readonly List<IFixedUpdate> FixedUpdateRemovalQueue = new();
        
        private static readonly List<ILateUpdate> LateUpdateQueue = new();
        private static readonly List<ILateUpdate> LateUpdateAddQueue = new();
        private static readonly List<ILateUpdate> LateUpdateRemovalQueue = new();

        private void Awake()
        {
            if(_instance != null) Destroy(this);
            else
            {
                _instance = this;
            }
        }

        public void Update()
        {
            var updateQueueAddCount = UpdateAddQueue.Count;
            
            if (updateQueueAddCount > 0)
            {
                for (var i = 0; i < updateQueueAddCount; i++)
                {
                    var element = UpdateAddQueue[i];
                    
                    UpdateQueue.Add(element);
                    UpdateAddQueue.Remove(element);
                }
            }
            
            var updateQueueRemoveCount = UpdateRemovalQueue.Count;

            if (updateQueueRemoveCount > 0)
            {
                for (var i = 0; i < updateQueueRemoveCount; i++)
                {
                    var element = UpdateRemovalQueue[i];
                    
                    UpdateQueue.Remove(element);
                    UpdateRemovalQueue.Remove(element);
                }
            }

            foreach (var update in UpdateQueue)
            {
                update.EfficientUpdate(Time.deltaTime);
            }
        }
        public void FixedUpdate()
        {
            var fixedUpdateQueueAddCount = FixedUpdateAddQueue.Count;
            
            if (fixedUpdateQueueAddCount > 0)
            {
                for (var i = 0; i < fixedUpdateQueueAddCount; i++)
                {
                    var element = FixedUpdateAddQueue[i];
                    
                    FixedUpdateQueue.Add(element);
                    FixedUpdateAddQueue.Remove(element);
                }
            }
            
            var fixedUpdateQueueRemovalCount = FixedUpdateRemovalQueue.Count;

            if (fixedUpdateQueueRemovalCount > 0)
            {
                for (var i = 0; i < fixedUpdateQueueRemovalCount; i++)
                {
                    var element = FixedUpdateRemovalQueue[i];

                    FixedUpdateQueue.Remove(element);
                    FixedUpdateRemovalQueue.Remove(element);
                }
            }

            foreach (var fixedUpdate in FixedUpdateQueue)
            {
                fixedUpdate.EfficientFixedUpdate(Time.deltaTime);
            }
        } 
        public void LateUpdate()
        {
            var lateUpdateQueueAddCount = FixedUpdateAddQueue.Count;
            
            if (lateUpdateQueueAddCount > 0)
            {
                for (var i = 0; i < lateUpdateQueueAddCount; i++)
                {
                    var element = LateUpdateAddQueue[i];
                    
                    LateUpdateQueue.Add(element);
                    LateUpdateAddQueue.Remove(element);
                }
            }
            
            var lateUpdateQueueRemovalCount = FixedUpdateRemovalQueue.Count;

            if (lateUpdateQueueRemovalCount > 0)
            {
                for (var i = 0; i < lateUpdateQueueRemovalCount; i++)
                {
                    var element = LateUpdateRemovalQueue[i];

                    LateUpdateQueue.Remove(element);
                    LateUpdateRemovalQueue.Remove(element);
                }
            }

            foreach (var lateUpdate in LateUpdateQueue)
            {
                lateUpdate.EfficientLateUpdate(Time.deltaTime);
            }
        }

        public static void Register(IAbstractUpdate script, UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    UpdateAddQueue.Add((IUpdate)script);
                    break;
                case UpdateType.FixedUpdate:
                    FixedUpdateAddQueue.Add((IFixedUpdate)script);
                    break;
                case UpdateType.LateUpdate:
                    LateUpdateAddQueue.Add((ILateUpdate)script);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
            }
            
            Debug.Log($"Registered {script} to {updateType} queue. ");
        }
        public static void Unregister(IAbstractUpdate script, UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    UpdateRemovalQueue.Add((IUpdate)script);
                    break;
                case UpdateType.FixedUpdate:
                    FixedUpdateRemovalQueue.Add((IFixedUpdate)script);
                    break;
                case UpdateType.LateUpdate:
                    LateUpdateRemovalQueue.Add((ILateUpdate)script);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
            }
            
            Debug.Log($"Unregistered {script} to {updateType} queue. ");
        }
    }
}