using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace GraphMaster.Visualization
{
    public interface GraphObjectVisualEffectsInterface
    {
        public void SelectThisAnimation();

        public void DeselectThisAnimation();

        public void MarkThis();

        public void RemoveMark();

        public void SetColor(Color color);

        public void SetColorTobase();

        public void HideIt();

        public void ShowIt();

    
    }
}

