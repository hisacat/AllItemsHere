using RF5Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere.Components
{
    internal class GetItemButtonWork : ButtonWorkBase
    {
        public override void ButtonWork(Key btnType)
        {
            BepInExLog.Log($"{name} ButtonWork {btnType}");
        }
        public override void EndFocus()
        {
            BepInExLog.Log($"{name} ButtonWork EndFocus");
            base.EndFocus();
        }
        public override void OnFocus()
        {
            BepInExLog.Log($"{name} ButtonWork OnFocus");
            base.OnFocus();
        }
    }
}
