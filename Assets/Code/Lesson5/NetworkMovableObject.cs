using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
#pragma warning disable 618
    public abstract class NetworkMovableObject : NetworkBehaviour
#pragma warning restore 618
    {
        public NetworkManager manager;

        protected abstract float speed { get; }
        protected Action _onUpdateAction { get; set; }
        protected Action _onFixedUpdateAction { get; set; }
        protected Action _onLateUpdateAction { get; set; }
        protected Action _onPreRenderActionAction { get; set; }
        protected Action _onPostRenderAction { get; set; }

        protected UpdatePhase _updatePhase;
#pragma warning disable 618
        [SyncVar] protected Vector3 serverPosition;
        [SyncVar] protected Vector3 serverEuler;
       
#pragma warning restore 618
        public override void OnStartAuthority()
        {
            Initiate();
        }
        protected virtual void Initiate(UpdatePhase updatePhase = UpdatePhase.Update)
        {
            manager = FindObjectOfType<NetworkManager>();

            switch (updatePhase)
            {
                case UpdatePhase.Update:
                    _onUpdateAction += Movement;
                    break;
                case UpdatePhase.FixedUpdate:
                    _onFixedUpdateAction += Movement;
                    break;
                case UpdatePhase.LateUpdate:
                    _onLateUpdateAction += Movement;
                    break;
                case UpdatePhase.PostRender:
                    _onPostRenderAction += Movement;
                    break;
                case UpdatePhase.PreRender:
                    _onPreRenderActionAction += Movement;
                    break;
            }
        }
        [Command]
        protected void CmdUpdatePosition(Vector3 position, bool isDeadShip)
        {
            serverPosition = position;
            if (isDeadShip)
            {
                Debug.Log("Destoy Ship on server");
                //manager.OnServerAddPlayer(;

                //manager.StopClient();

            }
        }

        private void Update()
        {
            _onUpdateAction?.Invoke();
        }
        private void LateUpdate()
        {
            _onLateUpdateAction?.Invoke();
        }
        private void FixedUpdate()
        {
            _onFixedUpdateAction?.Invoke();
        }
        private void OnPreRender()
        {
            _onPreRenderActionAction?.Invoke();
        }
        private void OnPostRender()
        {
            _onPostRenderAction?.Invoke();
        }
        protected virtual void Movement()
        {
            if (hasAuthority)
            {
                HasAuthorityMovement();
            }
            else
            {
                FromServerUpdate();
            }
        }

        private void OnDestroy()
        {
            
        }
        protected abstract void HasAuthorityMovement();
        protected abstract void FromServerUpdate();
        protected abstract void SendToServer();
    }
}