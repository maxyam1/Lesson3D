using UnityEngine;

namespace Utils
{
    public struct RigidBodySave
    {
        public float mass;
        public bool useGravity;
        public bool isKinematic;
        public RigidbodyConstraints constraints;
        public Vector3 centerOfMass;
        
        
        public static void ToStruct(Rigidbody rigidbody, out RigidBodySave rigidbodySave)
        {
            rigidbodySave = new RigidBodySave()
            {
                mass = rigidbody.mass,
                useGravity = rigidbody.useGravity,
                isKinematic = rigidbody.isKinematic,
                constraints = rigidbody.constraints,
                centerOfMass = rigidbody.centerOfMass
            };
        }

        public static void FromStruct(Rigidbody rigidbody, RigidBodySave rigidbodySave)
        {
            rigidbody.mass = rigidbodySave.mass;
            rigidbody.useGravity = rigidbodySave.useGravity;
            rigidbody.isKinematic = rigidbodySave.isKinematic;
            rigidbody.constraints = rigidbodySave.constraints;
            rigidbody.centerOfMass = rigidbodySave.centerOfMass;
        }
    }
}
