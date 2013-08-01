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
        public static IKinveyConstraint<T> GreaterThan<T>(T value)
        {
            return Get<T>().GreaterThan(value);
        }

        /// <summary>
        /// A "less than" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be less than</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThan<T>(T value)
        {
            return Get<T>().LessThan(value);
        }

        /// <summary>
        /// A "greater than or equal to" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be greater than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> GreaterThanEqualTo<T>(T value)
        {
            return Get<T>().GreaterThanEqualTo(value);
        }

        /// <summary>
        /// A "less than or equal to" constraint
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be less than or equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> LessThanEqualTo<T>(T value)
        {
            return Get<T>().LessThanEqualTo(value);
        }

        /// <summary>
        /// Constrains a field to be in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> In<T>(T[] value)
        {
            return Get<T>().In(value);
        }

        /// <summary>
        /// A "not equal to" constraint extension
        /// </summary>
        /// <typeparam name="T">A comparable target type</typeparam>
        /// <param name="value">the value the target must be not equal to</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotEqualTo<T>(T value)
        {
            return Get<T>().NotEqualTo(value);
        }

        /// <summary>
        /// Constrains a field to be not in a specified collection of values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="value">the array of values which constrains the target field</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotIn<T>(T[] value)
        {
            return Get<T>().NotIn(value);
        }

        /// <summary>
        /// Constrains a field to exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> Existent<T>()
        {
            return Get<T>().Existent();
        }

        /// <summary>
        /// Constrains a field to not exist
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> NotExistent<T>()
        {
            return Get<T>().NotExistent();
        }

        /// <summary>
        /// Constrains a field to fulfil a javascript predicate
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="predicate">the predicate to fulfill</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> Where<T>(string predicate)
        {
            return Get<T>().Where(predicate);
        }

        /// <summary>
        /// Constrains a string field to match a regex
        /// </summary>
        /// <param name="regex">the regex to match</param>
        /// <param name="options">the regex options</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<string> Regex(string regex, string options)
        {
            var constraint = Get<string>();
            constraint.Add("$regex", regex);
            constraint.Add("$options", options);
            return constraint;
        }

        /// <summary>
        /// Constrains a field containing an array to contain all of a set of target values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="target">the array to match</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<IEnumerable<T>> All<T>(IEnumerable<T> target)
        {
            return Get<IEnumerable<T>>().All(target);
        }

        /// <summary>
        /// Constrains a field containing an array to be a certain size
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="target">the size to match</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<IEnumerable<T>> Size<T>(int target)
        {
            return Get<IEnumerable<T>>().Size(target);
        }

        private static IKinveyConstraint<T> Get<T>()
        {
            return new Constraint<T>();
        }

        public static IKinveyConstraint<T> It<T>(string comparator, T value)
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
        public static IKinveyConstraint<T> GreaterThan<T>(this IKinveyConstraint<T> constraint, T value)
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
        public static IKinveyConstraint<T> LessThan<T>(this IKinveyConstraint<T> constraint, T value)
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
        public static IKinveyConstraint<T> GreaterThanEqualTo<T>(this IKinveyConstraint<T> constraint, T value)
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
        public static IKinveyConstraint<T> LessThanEqualTo<T>(this IKinveyConstraint<T> constraint, T value)
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
        public static IKinveyConstraint<T> In<T>(this IKinveyConstraint<T> constraint, IEnumerable<T> value)
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
        public static IKinveyConstraint<T> NotEqualTo<T>(this IKinveyConstraint<T> constraint, T value)
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
        public static IKinveyConstraint<T> NotIn<T>(this IKinveyConstraint<T> constraint, IEnumerable<T> value)
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
        public static IKinveyConstraint<T> Existent<T>(this IKinveyConstraint<T> constraint)
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
        public static IKinveyConstraint<T> NotExistent<T>(this IKinveyConstraint<T> constraint)
        {
            constraint.Add("$exists", false);
            return constraint;
        }

        /// <summary>
        /// Constrains a field to fulfil a javascript predicate
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="predicate">the predicate to fulfill</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<T> Where<T>(this IKinveyConstraint<T> constraint, string predicate)
        {
            constraint.Add("$where", predicate);
            return constraint;
        }

        /// <summary>
        /// Constrains a string field to match a regex
        /// </summary>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="regex">the regex to match</param>
        /// <param name="options">the regex options</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<string> Regex(this IKinveyConstraint<string> constraint, string regex, string options)
        {
            constraint.Add("$regex", regex);
            constraint.Add("$options", options);
            return constraint;
        }

        /// <summary>
        /// Constrains a field containing an array to contain all of a set of target values
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="target">the array to match</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<IEnumerable<T>> All<T>(this IKinveyConstraint<IEnumerable<T>> constraint, IEnumerable<T> target)
        {
            constraint.Add("$all", target);
            return constraint;
        }

        /// <summary>
        /// Constrains a field containing an array to be a certain size
        /// </summary>
        /// <typeparam name="T">>A comparable target type</typeparam>
        /// <param name="constraint">the constraint to extend</param>
        /// <param name="target">the size to match</param>
        /// <returns>a constraint object</returns>
        public static IKinveyConstraint<IEnumerable<T>> Size<T>(this IKinveyConstraint<IEnumerable<T>> constraint, int target)
        {
            constraint.Add("$size", target);
            return constraint;
        }
    }
}