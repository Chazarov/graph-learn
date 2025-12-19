using GraphMaster.UnityAdapter.VisualEffects;

namespace GraphMaster.UnityAdapter.VisualEffects
{
    public interface GraphObjectUiActionsInterface
    {
        public void Select();

        public void Deselect();

        public GraphObjectVisualEffectsInterface GetVisualEffects();
    }
}

