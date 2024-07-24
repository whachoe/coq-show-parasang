namespace XRL.World.Parts
{
    using System;
    using System.Text;
    using HarmonyLib;
    using Qud.API;
    using XRL;
    using XRL.World.ZoneParts;
    using XRL.UI;

    [Serializable]
    public class ShowParasong_EventListener : IPart
    {
        public static readonly string UnknownCoordinates = "--,--,--";
        private string FormattedCoordinates = UnknownCoordinates;
        private Cell curCell;

        public override bool WantEvent(int ID, int Cascade) => base.WantEvent(ID, Cascade) || ID == EnteringZoneEvent.ID;
        public override bool HandleEvent(EnteringZoneEvent E)
        {
            curCell = E.Cell;
          
            UpdateCoordinates();

            // Print message in Player Message Log on the right
            if (ShowParasangOptions.showParasangMessage) { 
                PrintCoordinates(); 
            
            }
            return base.HandleEvent(E);
        }
        public void PrintCoordinates() 
        {            
            XRL.Messages.MessageQueue.AddPlayerMessage("Parasang: " + FormattedCoordinates);            
        }

        public string CalculateParasang(int pX, int pY)
        {
            // Debug
            // XRL.Messages.MessageQueue.AddPlayerMessage("Parasang px,py: " + pX.ToString() + ',' + pY.ToString());

            if (pX == 1 && pY == 1)
                return "C";

            var sX = pX == 0 ? "W" : pX == 1 ? "" : "E";
            var sY = pY == 0 ? "N" : pY == 1 ? "" : "S";

            return $"{sY}{sX}";
        }

        public void SetCoordinateString(string pS, string x, string y, string z)
        {
            var paraSec = string.IsNullOrWhiteSpace(pS)
                        ? string.Empty
                        : pS + ", ";
            FormattedCoordinates = $"{paraSec} {x},{y},{z}";
        }

        public void ClearCoordinateString()
        {
            FormattedCoordinates = UnknownCoordinates;
        }

        public void UpdateCoordinates()
        {
            // Debugging World coords
            // var bla = Zone.XYToID(curCell.ParentZone.GetZoneWorld(), curCell.ParentZone.GetZoneX(), curCell.ParentZone.GetZoneY(), curCell.ParentZone.GetZoneZ());
            // XRL.Messages.MessageQueue.AddPlayerMessage("Parasang bla: " + bla);

            if (ParentObject.IsBroken() || curCell == null || FormattedCoordinates.Length <= 0) {
                ClearCoordinateString();
            } else {              
                GlobalLocation location = curCell.GetGlobalLocation();
                               
                // World Map Tiles start in the top left at 0, 1
                var x = curCell.ParentZone.IsWorldMap() ? curCell.X : curCell.ParentZone.wX;   // World map tile coord - joppa is 11
                var y = curCell.ParentZone.IsWorldMap() ? curCell.Y : curCell.ParentZone.wY;   // World map tile coord - joppa is 22 
                var z = curCell.ParentZone.IsWorldMap() ? 9 : curCell.ParentZone.Z;            // 'ground' floor is 10
              
                // Parasang tiles are 0, 1, or 2 in each direction with NW being 0,0. When on WorldMap, there's no parasang coords so we set them to 1,1
                var pX = curCell.ParentZone.IsWorldMap() ? 1 : curCell.ParentZone.X;           // Parasang coord - 0-2 - Joppa is 1 (center)
                var pY = curCell.ParentZone.IsWorldMap() ? 1 : curCell.ParentZone.Y;           // Parasang coord - 0-2 - Joppa is 1 (center)
                
                var parasangSection = CalculateParasang(pX, pY);
                SetCoordinateString(parasangSection, x.ToString(), y.ToString(), z.ToString());

                // Override the Zone name so it shows up nicely in the top-right corner and on detail screens
                int len = curCell.ParentZone.DisplayName.IndexOf('(') > 0 ? curCell.ParentZone.DisplayName.IndexOf('(') : curCell.ParentZone.DisplayName.Length;
                curCell.ParentZone.DisplayName = curCell.ParentZone.DisplayName.Substring(0, len) + $"(&y{parasangSection}&C)";
            }
        }
    }
}