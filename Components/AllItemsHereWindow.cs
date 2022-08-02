using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using RF5.HisaCat.AllItemsHere.Utils;
using RF5.HisaCat.AllItemsHere.Extensions;
using RF5.HisaCat.AllItemsHere.Patches;

namespace RF5.HisaCat.AllItemsHere.Components
{
    internal class AllItemsHereWindow : MonoBehaviour
    {
        public static AllItemsHereWindow Instance { get; private set; }
        public AllItemsHereWindow(System.IntPtr pointer) : base(pointer) { }

        private GameObject ItemContentsArea = null;
        private GameObject Prefab_ItemsSlot_OneLine = null;
        private GameObject Prefab_ItemsSlot = null;

        private CampMenuMain campMenuMain = null;
        private void Awake()
        {
            CacheCampMenuMain();
        }
        private void Start()
        {
            CacheCampMenuMain();
        }
        private void OnEnable()
        {
            CampMenuPatch.onPageOpened += CampMenuPatch_onPageOpened;
        }
        private void OnDisable()
        {
            CampMenuPatch.onPageOpened -= CampMenuPatch_onPageOpened;
        }
        private void CampMenuPatch_onPageOpened(CampPage page)
        {
            BepInExLog.Log($"Page opened {page}");
        }

        private void CacheCampMenuMain()
        {
            if (campMenuMain != null)
            {
                if (this.transform.IsChildOf(campMenuMain.transform) == false)
                    campMenuMain = null;
            }
            if (campMenuMain == null)
            {
                campMenuMain = this.GetComponentInParent<CampMenuMain>();
            }
        }

        private void Update()
        {
            //There are 2 cursors in camp UI
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/UIObjectLoader/HeadObject/Cursor"
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/Cursor"

            //카테고리 텍스트
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/CampMenuObject/CenterMenu/RuckItemMenu/EquipItemGroup/ItemBoxWindow/SortDisp/ruck_sortbtn"
            //중앙 화면
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/CampMenuObject/CenterMenu"
            //좌측 플레이어 화면
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/CampMenuObject/CenterMenu/PlayerEquipMenu"
            //우측 인벤토리 화면
            // "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas/CampUI/CampMenuObject/CenterMenu/RuckItemMenu"

            //윈도우 생성은 "/StartUI/UIMainManager/UIMainCanvas(Clone)/MainWindowCanvas"에서?
            //Sbling은 우측 인벤토리 화면 위에 두도록 한다.

            #region Test logic
            if (InputHelper.GetKey(BepInEx.IL2CPP.UnityEngine.KeyCode.Home))
                this.transform.Translate(new Vector3(10, 0, 0));
            if (InputHelper.GetKey(BepInEx.IL2CPP.UnityEngine.KeyCode.End))
                this.transform.Translate(new Vector3(-10, 0, 0));
            if (InputHelper.GetKeyDown(BepInEx.IL2CPP.UnityEngine.KeyCode.Alpha1))
            {
                //Test
            }
            #endregion
        }

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

            CacheCampMenuMain();

            var buttonLinkersMap = new Dictionary<Vector2Int, ButtonLinker>();
            {
                GameObject curOneLineObj = null;
                int curAddedCount = 0;
                var curXY = new Vector2Int(0, -1);
                foreach (ItemID itemId in typeof(ItemID).GetEnumValues())
                {
                    if (CheckIsValidItemId(itemId) == false)
                        continue;

                    if (curAddedCount >= 50) break;

                    var dataTable = ItemDataTable.GetDataTable(itemId);

                    var curOneLineCount = curAddedCount % ONE_LINE_COUNT;
                    if (curOneLineCount == 0)
                    {
                        curOneLineObj = Instantiate(this.Prefab_ItemsSlot_OneLine, this.ItemContentsArea.transform);
                        curOneLineObj.DestroyAllChildren();
                        curXY.x = 0; curXY.y++;
                    }

                    var curItemSlot = Instantiate(this.Prefab_ItemsSlot, curOneLineObj.transform);
                    curItemSlot.name = $"{this.Prefab_ItemsSlot} {curAddedCount}";
                    var itemIconLoader = curItemSlot.AddComponent<ItemIconLoader>();
                    itemIconLoader.Image = curItemSlot.transform.Find("Image_Item").GetComponent<Image>();
                    itemIconLoader.SetLoadIcon(itemId);

                    var buttonLinker = curItemSlot.AddComponent<ButtonLinker>();
                    buttonLinker.TouchSelector = true; //Enable- touch
                    buttonLinkersMap.Add(curXY, buttonLinker);
                    //Note: ButtonLinker's Touch event seems like working based on InputCursor.
                    //And if each ButtonLinkers rect overlaps on touch point, the InputCursor trying focus on 'all the corresponding objects'.

                    buttonLinker.SubmitOnTouch = true;
                    var buttonWork = curItemSlot.AddComponent<GetItemButtonWork>();
                    buttonLinker.ButtonWork = buttonWork;
                    buttonWork.CursorLinker = buttonLinker;

                    curXY.x++;
                    curAddedCount++;
                }
            }
            //Add link to each buttonLinkers
            {
                var curXY = Vector2Int.zero;
                while (true)
                {
                    if (buttonLinkersMap.ContainsKey(curXY) == false)
                        break;
                    var curLinker = buttonLinkersMap[curXY];

                    var leftXY = new Vector2Int(curXY.x - 1, curXY.y);
                    var rightXY = new Vector2Int(curXY.x + 1, curXY.y);
                    var upXY = new Vector2Int(curXY.x, curXY.y - 1);
                    var downXY = new Vector2Int(curXY.x, curXY.y + 1);

                    if (buttonLinkersMap.ContainsKey(leftXY))
                        curLinker.AddLink(CursorLinker.InputMoveType.Left, buttonLinkersMap[leftXY].gameObject);
                    if (buttonLinkersMap.ContainsKey(rightXY))
                        curLinker.AddLink(CursorLinker.InputMoveType.Right, buttonLinkersMap[rightXY].gameObject);
                    if (buttonLinkersMap.ContainsKey(upXY))
                        curLinker.AddLink(CursorLinker.InputMoveType.Up, buttonLinkersMap[upXY].gameObject);
                    if (buttonLinkersMap.ContainsKey(downXY))
                        curLinker.AddLink(CursorLinker.InputMoveType.Down, buttonLinkersMap[downXY].gameObject);

                    curXY.x++;
                    if (curXY.x >= ONE_LINE_COUNT)
                    {
                        curXY.x = 0;
                        curXY.y++;
                    }
                }

                //Linkers for escape
                var rightEdgeLinkers = buttonLinkersMap.Where(x => x.Key.y >= ONE_LINE_COUNT).Select(x => x.Value);
                foreach (var linker in rightEdgeLinkers)
                {
                    //TODO: Add escape link.
                    //linker.AddLink(CursorLinker.InputMoveType.Right, null);
                }
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

            if (Instance.InitUI() == false)
            {
                BepInExLog.LogError("InstantiateAndAttach: Failed to InitUI!");
                Destroy(windowInstance);
                return false;
            }

            BepInExLog.Log("InstantiateAndAttach: Success: AllItemsHereWindow Attached!");
            return true;
        }
    }
}
