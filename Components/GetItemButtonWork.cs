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
            
            //Give item like this.
            //ItemStorageManager.GetStorage(Define.StorageType.Rucksack).PushItemIn(ItemData.Instantiate(ItemID.Item_Aianejji, 1));
        }

        public override void OnFocus()
        {
            BepInExLog.Log($"{name} OnFocus");
        }
    }
}
