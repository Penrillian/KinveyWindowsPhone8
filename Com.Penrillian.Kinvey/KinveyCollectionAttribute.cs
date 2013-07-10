using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Marks a class that corresponds to a Kinvey collection name
    /// </summary>
    public class KinveyCollectionAttribute : Attribute
    {
        /// <summary>
        /// The name of the collection in Kinvey
        /// </summary>
        public string CollectionName { get { return _collectionName; }
        }

        private readonly string _collectionName;

        /// <param name="collectionName">the name of the collection in Kinvey</param>
        public KinveyCollectionAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }
    }
}