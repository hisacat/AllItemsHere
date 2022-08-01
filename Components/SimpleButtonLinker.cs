using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RF5.HisaCat.AllItemsHere.Components
{
    public class SimpleButtonLinker : ButtonLinker
    {
        public override void Awake()
        {
            this.rect = this.transform.Find("CursorPos").GetComponent<RectTransform>();
            //
            //this.TouchSelector

            this.ButtonWork = this.AddComponent<GetItemButtonWork>();
            this.ButtonWork.CursorLinker = this;

            this.TouchSelector = true;
            this.SubmitOnTouch = true; ;
            this.rect = this.GetComponent<RectTransform>();

            this.inputLayer = INPUTLAYER.Default;
        }
        public void InputDown()
        {
            BepInExLog.Log($"{name} InputDown");
        }
        public void InputUp()
        {
            BepInExLog.Log($"{name} InputUp");
        }
        public override void OnTouch()
        {
            BepInExLog.Log($"{name} Touch");
        }
        public void OnPointerClick()
        {
            BepInExLog.Log($"{name} OnPointerClick");
        }
        public override bool CanUpdateCursor()
        {
            return false;
        }
        public override void ClearInputLayer()
        {
            //윈도우 전환될때 나타나더라
            this.EndFocus();
            BepInExLog.Log($"{name} ClearInputLayer");
        }

        /// <summary>
        /// When get focus
        /// </summary>
        public override void OnFocus()
        {
            BepInExLog.Log($"{name} OnFocus");
        }
        /// <summary>
        /// When lost(end) focus
        /// </summary>
        public override void EndFocus()
        {
            BepInExLog.Log($"{name} EndFocus");
        }
        /// <summary>
        /// IDK it called like window closed or etc.
        /// </summary>
        public override void InitInputLayer()
        {
            BepInExLog.Log($"{name} InitInputLayer");
            base.InitInputLayer();
        }

        //DO NOT OVERRIDE GetNextObject
        //DO NOT OVERRIDE InComingFocus
        //DO NOT OVERRIDE OnNextFocus
        //DO NOT OVERRIDE GetImageSize
    }
}
