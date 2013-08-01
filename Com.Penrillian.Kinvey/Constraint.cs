using System;
using System.Collections.Generic;

namespace Com.Penrillian.Kinvey
{
    internal class Constraint<T> : Dictionary<string, object>, IKinveyConstraint<T>
    {
    }
}