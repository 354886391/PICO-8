using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Normal.Character
{
    internal class MInput : MonoBehaviour
    {

        public static Vector2 Move;

        public static VirtualButton Jump;
        public static VirtualButton Dash;
        public static VirtualButton Grab;
        public static VirtualButton Talk;
        public static VirtualButton CrouchDash;


        private void Awake()
        {
            Jump = new VirtualButton(0.08f);
        }

        private void Update()
        {
            Jump.Value = Input.GetButton("C");
        }
    }
}
