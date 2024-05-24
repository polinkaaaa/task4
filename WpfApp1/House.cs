using System;

namespace WpfApp1.Models
{
    public class House
    {
        public event EventHandler MaterialsDepleted;
        public event EventHandler ReadyForRoofing;

        public bool FoundationLaid { get; private set; }
        public bool WallsBuilt { get; private set; }
        public bool RoofCovered { get; private set; }

        public void LayFoundation()
        {
            FoundationLaid = true;
            CheckStatus();
        }

        public void BuildWalls()
        {
            if (!FoundationLaid)
                throw new InvalidOperationException("Foundation must be laid first.");

            WallsBuilt = true;
            ReadyForRoofing?.Invoke(this, EventArgs.Empty);
        }

        public void CoverRoof()
        {
            if (!WallsBuilt)
                throw new InvalidOperationException("Walls must be built first.");

            RoofCovered = true;
        }

        private void CheckStatus()
        {
            if (FoundationLaid && !WallsBuilt)
            {
                MaterialsDepleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
