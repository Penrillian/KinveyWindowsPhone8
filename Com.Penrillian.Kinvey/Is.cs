using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Constrains a field to be in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> In<T>(T[] value) where T : IComparable
        {
            return Get<T>().In(value);
        }

        /// <summary>
        /// A "not equal to" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be not equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotEqualTo<T>(T value) where T : IComparable
        {
            return Get<T>().NotEqualTo(value);
        }

        /// <summary>
        /// Constrains a field to be not in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotIn<T>(T[] value) where T : IComparable
        {
            return Get<T>().NotIn(value);
        }

        /// <summary>
        /// Constrains a field to exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> Existent<T>() where T : IComparable
        {
            return Get<T>().Existent();
        }

        /// <summary>
        /// Constrains a field to not exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotExistent<T>() where T : IComparable
        {
            return Get<T>().NotExistent();
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

        /// <summary>
        /// Constrains a field to be in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> In<T>(this IKinveyConstraint<T> constraint, IEnumerable<T> value) where T : IComparable
        {
            constraint.Add("$in", value);
            return constraint;
        }

        /// <summary>
        /// A "not equal to" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the value the target must be not equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotEqualTo<T>(this IKinveyConstraint<T> constraint, T value) where T : IComparable
        {
            constraint.Add("$ne", value);
            return constraint;
        }

        /// <summary>
        /// Constrains a field to be not in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotIn<T>(this IKinveyConstraint<T> constraint, IEnumerable<T> value) where T : IComparable
        {
            constraint.Add("$nin", value);
            return constraint;
        }

        /// <summary>
        /// Constrains a field to exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> Existent<T>(this IKinveyConstraint<T> constraint) where T : IComparable
        {
            constraint.Add("$exists", true);
            return constraint;
        }

        /// <summary>
        /// Constrains a field to not exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotExistent<T>(this IKinveyConstraint<T> constraint) where T : IComparable
        {
            constraint.Add("$exists", false);
            return constraint;
        }
    }
}