using System;
using System.Collections.Generic;

namespace Com.Penrillian.Kinvey
{
    internal class Constraint<T> : Dictionary<string, T>, IKinveyConstraint<T> where T : IComparable
    {
    }
}