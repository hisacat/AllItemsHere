using RF5Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5.HisaCat.AllItemsHere.Components
{
    //See with RuckPageSwitchButton
    internal class GetItemButtonWork : ButtonWorkBase
    {
        public override void ButtonWork(Key btnType)
        {
            BepInExLog.Log($"{name} ButtonWork {btnType}");
        }

        public override void OnFocus()
        {
            BepInExLog.Log($"{name} OnFocus");
        }
    }
}
