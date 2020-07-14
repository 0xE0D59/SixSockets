using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements.InventoryElements;

namespace SixSockets
{
    public class Core : BaseSettingsPlugin<Settings>
    {
        public override void Render()
        {
            base.Render();

            var leftPanel = GameController?.Game?.IngameState?.IngameUi?.OpenLeftPanel;

            if (leftPanel == null || leftPanel.Address == 0x0 || !leftPanel.IsVisible)
                return;

            var stashOpen = GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible &&
                            GameController.Game.IngameState.IngameUi.StashElement.IsVisibleLocal;
            if (!stashOpen)
                return;

            var items = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash?.VisibleInventoryItems;
            if (items == null || items.Count == 0)
                return;
            
            foreach (var inventoryItem in items)
            {
                if (IsSixSocketItem(inventoryItem))
                {
                    DrawBorder(inventoryItem);
                }
            }
        }

        private void DrawBorder(NormalInventoryItem inventoryItem)
        {
            var rect = inventoryItem.GetClientRect();

            var borderColor = Settings.BorderColor.Value;

            var borderWidth = Settings.BorderWidth.Value;

            Graphics.DrawFrame(rect, borderColor, borderWidth);
        }

        private bool IsSixSocketItem(NormalInventoryItem inventoryItem)
        {
            if (inventoryItem == null || inventoryItem.Address == 0x0)
                return false;

            var item = inventoryItem.Item;
            if (item == null || item.Address == 0x0) return false;

            if (!item.HasComponent<Sockets>()) return false;

            var sockets = item.GetComponent<Sockets>();
            if (sockets == null || sockets.Address == 0x0) return false;

            return sockets.NumberOfSockets >= 6;
        }
    }
}