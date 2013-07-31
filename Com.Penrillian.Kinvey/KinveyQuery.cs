namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// Represents a set of query constraints, used by IKinveyService instances.
    /// </summary>
    /// <typeparam name="T">The type of object to apply this query to</typeparam>
    public class KinveyQuery<T> : KinveyConstraints<T> where T : KinveyObject, new()
    {
        #region Backing

        private int? _limit;
        private int? _skip;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public KinveyQuery()
        {
            
        }

        /// <summary>
        /// This constructor adopts the constraints of a pre-defined constraints object
        /// </summary>
        /// <param name="constraints">The constraints to adopt</param>
        public KinveyQuery(KinveyConstraints<T> constraints)
        {
            if (null != constraints)
            {
                Adopt(constraints);
            }
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

        #region Serialize

        public override string ToString()
        {
            var constraints = base.ToString();
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