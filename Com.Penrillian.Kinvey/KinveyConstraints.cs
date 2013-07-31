﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Com.Penrillian.Kinvey
{
    public class KinveyConstraints<T> where T : KinveyObject, new()
    {
        #region Backing

        private Dictionary<string, object> _keyConstraints;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public KinveyConstraints()
        {
            _keyConstraints = new Dictionary<string, object>();
        }

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
        public KinveyConstraints<T> Constrain<TVal>(Expression<Func<T, TVal>> target, IKinveyConstraint<TVal> constraint) where TVal : IComparable
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
        public KinveyConstraints<T> Constrain<TVal>(Expression<Func<T, TVal>> target, TVal constraint) where TVal : IComparable
        {
            return Constrain(target, (object)constraint);
        }

        private KinveyConstraints<T> Constrain<TVal>(Expression<Func<T, TVal>> target, object constraint) where TVal : IComparable
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
        public KinveyConstraints<T> Release<TVal>(Expression<Func<T, TVal>> target)
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

        #region Adoption

        public void Adopt(KinveyConstraints<T> constraints)
        {
            foreach (var keyConstraint in constraints._keyConstraints)
            {
                _keyConstraints[keyConstraint.Key] = keyConstraint.Value;
            }
        }

        #endregion

        #region Serialize

        public override string ToString()
        {
            var settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};
            return _keyConstraints.Count > 0 ? JsonConvert.SerializeObject(_keyConstraints, settings) : "{}";
        }

        #endregion
    }
}