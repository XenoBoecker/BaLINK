using UnityEngine;
using UnityEngine.UIElements;

namespace Custom
{
    [System.Serializable]
    public class AnimationFrame
    {
        public bool AnimateOnlyIfInView = false;

        public ToggleableField<Vector3> Position = new ToggleableField<Vector3>(default); 
        public ToggleableField<Quaternion> Rotation = new ToggleableField<Quaternion>(default);
        public ToggleableField<Vector3> Scale = new ToggleableField<Vector3>(default);

        public ToggleableField<Material> Material;
        public ToggleableField<Mesh> Mesh;

        internal void ApplyAnimationFrame(GameObject gameObject)
        {
            if (UpdateObjectValue(Position)) { gameObject.transform.position = Position.Value; }
            if (UpdateObjectValue(Rotation)) { gameObject.transform.rotation = Rotation.Value; }
            if (UpdateObjectValue(Scale)) { gameObject.transform.localScale = Scale.Value; }

            if (UpdateObjectValue(Material)) { gameObject.GetComponent<MeshRenderer>().material = Material.Value; }
            if (UpdateObjectValue(Mesh)) { gameObject.GetComponent<MeshFilter>().mesh = Mesh.Value; }
        }

        private bool UpdateObjectValue<T>(ToggleableField<T> field)
        {
            return field != null && field.IsActive;
        }
    }
}

