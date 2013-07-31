using System.Collections.Generic;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Represents a single constraint or set of constraints on one particular field of a collection.
    /// These constraint objects are used to build queries. Constraint objects are dictionaries and 
    /// are serialized into Json strings used to build request URIs with.
    /// </summary>
    /// <typeparam name="T">The type of the target field</typeparam>
    /// <see cref="Com.Penrillian.Kinvey.KinveyQuery&lt;T&gt;"/>
    public interface IKinveyConstraint<T> : IDictionary<string, object>
    {
    }
}