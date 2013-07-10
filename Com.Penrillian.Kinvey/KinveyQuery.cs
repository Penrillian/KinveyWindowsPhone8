using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Represents a set of query constraints, used by IKinveyService instances.
    /// </summary>
    /// <typeparam name="T">The type of object to apply this query to</typeparam>
    public class KinveyQuery<T> where T : KinveyObject, new()
    {
        #region Backing

        private readonly Dictionary<string, object> _keyConstraints;

        private int? _limit;
        private int? _skip;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public KinveyQuery()
        {
            _keyConstraints = new Dictionary<string, object>();
        }

        #region Fields (Fluid)

        /// <summary>
        /// Limits the number of records returned from a query
        /// </summary>
        /// <param name="value">The limit, or null for no limit</param>
        /// <returns>This</returns>
        public KinveyQuery<T> Limit(int? value)
        {
            _limit = value;
            return this;
        }

        /// <summary>
        /// Skips the first <b>n</b> records
        /// </summary>
        /// <param name="value">The number of records to skip, or null for no records</param>
        /// <returns>This</returns>
        public KinveyQuery<T> Skip(int? value)
        {
            _skip = value;
            return this;
        }

        #endregion

        #region Constraints (Fluid)

        /// <summary>
        /// <p>Constrains the set of results returned for this query using a constraint object 
        /// targeting a particular field. Constraint objects provide logical checking of field
        /// values against rules. Only records with values matching these rules will be returned.</p>
        /// <p>For example...</p>
        /// <code>new KinveyQuery&lt;Person&gt;().Constrain(k =&gt; k.Age, Is.GreaterThan(3));</code>
        /// </summary>
        /// <typeparam name="TVal">The type of the target field</typeparam>
        /// <param name="target">Expression describing the target field</param>
        /// <param name="constraint">An object representing a constraint on the target fields value</param>
        public KinveyQuery<T> Constrain<TVal>(Expression<Func<T, TVal>> target, IKinveyConstraint<TVal> constraint) where TVal : IComparable
        {
            return Constrain(target, (object)constraint);
        }

        /// <summary>
        /// <p>Constrains the set of results returned for this query using a value 
        /// targeting a particular field. Only records with values matching the value
        /// provided will be returned.</p>
        /// <p>For example...</p>
        /// <code>new KinveyQuery&lt;Person&gt;().Constrain(k =&gt; k.Age, 6);</code>
        /// </summary>
        /// <typeparam name="TVal">The type of the target field</typeparam>
        /// <param name="target">Expression describing the target field</param>
        /// <param name="constraint">The desired value of the target field</param>
        public KinveyQuery<T> Constrain<TVal>(Expression<Func<T, TVal>> target, TVal constraint) where TVal : IComparable
        {
            return Constrain(target, (object)constraint);
        }

        private KinveyQuery<T> Constrain<TVal>(Expression<Func<T, TVal>> target, object constraint) where TVal : IComparable
        {
            var property = PropertyName(target);
            if (_keyConstraints.ContainsKey(property))
                _keyConstraints.Remove(property);
            _keyConstraints.Add(property, constraint);
            return this;
        }

        /// <summary>
        /// Releases a target field from constraint.
        /// </summary>
        /// <typeparam name="TVal">The type of the target field</typeparam>
        /// <param name="target">Expression describing the target field</param>
        public KinveyQuery<T> Release<TVal>(Expression<Func<T, TVal>> target)
        {
            var property = PropertyName(target);
            if (_keyConstraints.ContainsKey(property)) _keyConstraints.Remove(property);
            return this;
        }

        private static string PropertyName<TVal>(Expression<Func<T, TVal>> target)
        {
            var body = target.Body as MemberExpression;
            if (body == null) throw new KinveyQueryException("Constraint targets must be of the form \"c => c.Property\"");

            var propertyName = body.Member.Name;

            var property = typeof (T).GetProperty(propertyName);
            var attributes = Attribute.GetCustomAttributes(property);

            var attribute = attributes.OfType<JsonPropertyAttribute>().FirstOrDefault();
            if (attribute != null)
                propertyName = attribute.PropertyName ?? propertyName;

            return propertyName;
        }

        #endregion

        #region Serialize

        public override string ToString()
        {
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            var constraints = _keyConstraints.Count > 0 ? JsonConvert.SerializeObject(_keyConstraints, settings) : "{}";

            return string.Format("?query={0}{1}{2}", constraints, LimitToString(), SkipToString());
        }

        private string LimitToString()
        {
            return null == _limit ? string.Empty : string.Format("&limit={0}", _limit);
        }

        private string SkipToString()
        {
            return null == _skip ? string.Empty : string.Format("&skip={0}", _skip);
        }

        #endregion
    }
}