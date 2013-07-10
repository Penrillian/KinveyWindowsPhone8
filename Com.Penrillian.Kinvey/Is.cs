using System;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Static class which provides a Fuild API for building constraint objects.
    /// <example>
    /// <code>var integerConstraint = Is.GreaterThan&lt;int&gt;(3).LessThan&lt;int&gt;(6)</code>
    /// </example>
    /// </summary>
    public static class Is
    {
        /// <summary>
        /// A "greater than" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be greater than</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> GreaterThan<T>(T value) where T : IComparable
        {
            return Get<T>().GreaterThan(value);
        }

        /// <summary>
        /// A "less than" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be less than</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThan<T>(T value) where T : IComparable
        {
            return Get<T>().LessThan(value);
        }

        /// <summary>
        /// A "greater than or equal to" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be greater than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> GreaterThanEqualTo<T>(T value) where T : IComparable
        {
            return Get<T>().GreaterThanEqualTo(value);
        }

        /// <summary>
        /// A "less than or equal to" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be less than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThanEqualTo<T>(T value) where T : IComparable
        {
            return Get<T>().LessThanEqualTo(value);
        }

        private static IKinveyConstraint<T> Get<T>() where T : IComparable
        {
            return new Constraint<T>();
        }

        public static IKinveyConstraint<T> It<T>(string comparator, T value) where T : IComparable
        {
            var constraint = Get<T>();
            constraint.Add(comparator, value);
            return constraint;
        }

        // ReSharper disable MemberCanBePrivate.Global
        // usage implicit
        /// <summary>
        /// A "greater than" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the value the target must be greater than</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> GreaterThan<T>(this IKinveyConstraint<T> constraint, T value) where T : IComparable
        {
            constraint.Add("$gt", value);
            return constraint;
        }
        // ReSharper restore MemberCanBePrivate.Global

        /// <summary>
        /// A "less than" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the value the target must be less than</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThan<T>(this IKinveyConstraint<T> constraint, T value) where T : IComparable
        {
            constraint.Add("$lt", value);
            return constraint;
        }

        // ReSharper disable MemberCanBePrivate.Global
        // usage implicit
        /// <summary>
        /// A "greater than or equal to" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the value the target must be greater than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> GreaterThanEqualTo<T>(this IKinveyConstraint<T> constraint, T value) where T : IComparable
        {
            constraint.Add("$gte", value);
            return constraint;
        }
        // ReSharper restore MemberCanBePrivate.Global

        // ReSharper disable MemberCanBePrivate.Global
        // usage implicit
        /// <summary>
        /// A "less than or equal to" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the value the target must be less than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThanEqualTo<T>(this IKinveyConstraint<T> constraint, T value) where T : IComparable
        {
            constraint.Add("$lte", value);
            return constraint;
        }
        // ReSharper restore MemberCanBePrivate.Global
    }
}