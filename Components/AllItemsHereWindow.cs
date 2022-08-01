using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.AllItemsHere.Utils;
using RF5.HisaCat.AllItemsHere.Extensions;

namespace RF5.HisaCat.AllItemsHere.Components
{
    internal class AllItemsHereWindow : MonoBehaviour
    {
        public static AllItemsHereWindow Instance { get; private set; }
        //MainWindowCanvas
        public AllItemsHereWindow(System.IntPtr pointer) : base(pointer) { }

        private GameObject ItemContentsArea = null;
        private GameObject Prefab_ItemsSlot_OneLine = null;
        private GameObject Prefab_ItemsSlot = null;

        public void Update()
        {
            //There are 2 cursors in camp UI

            //if (InputHelper.GetKeyDown(RF5Input.Key.CK3))
            if (InputHelper.GetKeyDown(BepInEx.IL2CPP.UnityEngine.KeyCode.Alpha1))
            {
                CursorController.NowCursor.SetFocus(linkers[0]);

                //OnPointerClick ?
            }
        }

        List<SimpleButtonLinker> linkers = new List<SimpleButtonLinker>();
        private const int ONE_LINE_COUNT = 5;
        private bool InitUI()
        {
            //PreloadObjects
            ItemContentsArea = transform.Find("Window/Scroll View/Viewport/Content").gameObject;
            Prefab_ItemsSlot_OneLine = BundleLoader.MainBundle.LoadIL2CPP<GameObject>("Windows/AllItemsHereWindow/ItemSlot_OneLine");
            Prefab_ItemsSlot = BundleLoader.MainBundle.LoadIL2CPP<GameObject>("Windows/AllItemsHereWindow/ItemSlot");

            if (ItemContentsArea == null)
            {
                BepInExLog.LogError($"InitUI: cannot find ItemContentsArea");
                return false;
            }
            if (Prefab_ItemsSlot_OneLine == null)
            {
                BepInExLog.LogError($"InitUI: cannot load Prefab_ItemsSlot_OneLine");
                return false;
            }
            if (Prefab_ItemsSlot == null)
            {
                BepInExLog.LogError($"InitUI: cannot load Prefab_ItemsSlot");
                return false;
            }

            this.ItemContentsArea.DestroyAllChildren();

            List<List<GameObject>> itemButtons = new List<List<GameObject>>();
            GameObject curOneLineObj = null;
            int curAddedCount = 0;
            int curX = -1;
            int curY = -1;
            foreach (ItemID itemId in typeof(ItemID).GetEnumValues())
            {
                if (CheckIsValidItemId(itemId) == false)
                    continue;

                //if (curAddedCount >= 4) break;

                var dataTable = ItemDataTable.GetDataTable(itemId);

                var curOneLineCount = curAddedCount % ONE_LINE_COUNT;
                if (curOneLineCount == 0)
                {
                    curOneLineObj = Instantiate(this.Prefab_ItemsSlot_OneLine, this.ItemContentsArea.transform);
                    curOneLineObj.DestroyAllChildren();
                    curX = 0; curY++;
                }

                //ItemIconLoader.instan
                //dataTable.IconName
                var curItemSlot = Instantiate(this.Prefab_ItemsSlot, curOneLineObj.transform);
                curItemSlot.name = $"{this.Prefab_ItemsSlot} {curAddedCount}";
                var itemIconLoader = curItemSlot.AddComponent<ItemIconLoader>();
                itemIconLoader.Image = curItemSlot.transform.Find("Image_Item").GetComponent<Image>();
                itemIconLoader.SetLoadIcon(itemId);

                //UIScrollBox
                //UIShipmentItemButton ss = null;
                //ss.ItemID = ItemID.ITEM_EMPTY;
                //ss.SlotNo = 0;
                //ss.ButtonWork = null;
                //ss.BackLink = null;

                var buttonLinker = curItemSlot.AddComponent<SimpleButtonLinker>();
                linkers.Add(buttonLinker);
                //buttonLinker.uiscrollbox
                //buttonLinker.ButtonWork = null;
                //buttonLinker.BackLink = null;
                //buttonLinker.LinkObjectList = new Il2CppSystem.Collections.Generic.List<ButtonLinker.LinkObject>();
                //buttonLinker.rect = buttonLinker.transform as RectTransform;
                //buttonLinker.ButtonGuideId = Define.ButtonGuideId.calendar;
                //buttonLinker.ItemSlot = null;
                //buttonLinker.ButtonWork = null;
                //buttonLinker.ButtonGuideId = Define.ButtonGuideId.calendar;

                curX++;
                curAddedCount++;
            }

            return true;
        }
        public bool CheckIsValidItemId(ItemID itemId)
        {
            if (itemId == ItemID.ITEM_EMPTY)
                return false;
            if (itemId == ItemID.ITEM_MAX)
                return false;
            if (itemId >= ItemID.Item_Type_Kouseki && itemId <= ItemID.Item_Type_Kinoko)
                return false;

            return true;
        }

        public static bool InstantiateAndAttach()
        {
            BepInExLog.Log("InstantiateAndAttach: called.");

            if (Patches.ModInitializePatch.IsInitialized == false)
            {
                BepInExLog.LogError("InstantiateAndAttach: Mod does not initialized");
                return false;
            }

            if (Instance != null)
            {
                BepInExLog.LogWarning("InstantiateAndAttach: Window already exist");
                return true;
            }

            var cursorObj = GameObject.Find("/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/Cursor");
            if (cursorObj == null)
            {
                BepInExLog.LogError("InstantiateAndAttach: Cannot find MainWindowCanvas/CampUI/Cursor !");
                return false;
            }
            var campUIObj = cursorObj.transform.parent;
            var mainWindowCanvasObj = campUIObj.parent;

            //Right inventory menu
            var ruckItemMenu = GameObject.Find("/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/CampMenuObject/CenterMenu/RuckItemMenu");
            if (mainWindowCanvasObj == null)
            {
                BepInExLog.LogError("InstantiateAndAttach: Cannot find RuckItemMenu!");
                return false;
            }
            var centerMenuObj = ruckItemMenu.transform.parent;

            var windowPrefab = BundleLoader.MainBundle.LoadIL2CPP<GameObject>("Windows/AllItemsHereWindow");
            if (windowPrefab == null)
            {
                BepInExLog.LogError("InstantiateAndAttach: Cannot load AllItemsHereWindow prefab!");
                return false;
            }

            var windowInstance = Instantiate<GameObject>(windowPrefab, mainWindowCanvasObj.transform);
            if (windowInstance == null)
            {
                BepInExLog.LogError("InstantiateAndAttach: Cannot instantiate AllItemsHereWindow!");
                return false;
            }
            BepInExLog.Log("InstantiateAndAttach: AllItemsHereWindow instantiated.");

            //Set fit to parent
            var windowRT = windowInstance.GetComponent<RectTransform>();
            windowRT.SetFitToParent();
            BepInExLog.Log("InstantiateAndAttach: AllItemsHereWindow rect fitted.");

            //Set parent but rect stays
            windowInstance.transform.SetParent(centerMenuObj, true);
            BepInExLog.Log($"InstantiateAndAttach: AllItemsHereWindow parent setted to {centerMenuObj.name}.");

            //Set sibling index ?
            windowInstance.transform.SetAsLastSibling();

            Instance = windowInstance.AddComponent<AllItemsHereWindow>();
            if (Instance == null)
            {
                BepInExLog.LogError("InstantiateAndAttach: Cannot add AllItemsHereWindow component!");
                Destroy(windowInstance);
                return false;
            }

            if(Instance.InitUI() == false)
            {
                BepInExLog.LogError("InstantiateAndAttach: Failed to InitUI!");
                Destroy(windowInstance);
                return false;
            }

            BepInExLog.Log("InstantiateAndAttach: Success: AllItemsHereWindow Attacked!");
            return true;
        }
    }
}
