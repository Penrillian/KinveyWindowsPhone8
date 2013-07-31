using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Penrillian.Kinvey
{
    /// <summary>
    /// The IKinveyService interface provides access to CRUD methods for a particular object type. This type
    /// must be annotated with the [KinveyCollection] attribute and must extend from KinveyObject.
    /// </summary>
    /// <typeparam name="T">The type of object this service is associated with</typeparam>
    /// <seealso cref="KinveyCollectionAttribute"/>
    /// <seealso cref="KinveyObject"/>
    public interface IKinveyService<T> where T : KinveyObject, new()
    {
        /// <summary>
        /// Makes a POST request to create a new record.
        /// </summary>
        /// <param name="t">The data to create the record with</param>
        /// <returns>The created record</returns>
        Task<T> Create(T t);

        /// <summary>
        /// Makes a GET request to read a record.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve</param>
        /// <returns>The retrieved record</returns>
        Task<T> Read(string id);

        /// <summary>
        /// Makes a GEt request to query a set of records
        /// </summary>
        /// <param name="kinveyQuery">The constraints for the query</param>
        /// <returns>An enumerable collection of matching records</returns>
        Task<IEnumerable<T>> Read(KinveyConstraints<T> kinveyQuery);

        /// <summary>
        /// Makes a POST request to update an existing record.
        /// </summary>
        /// <param name="t">The record to update</param>
        /// <returns>The updated record</returns>
        Task<T> Update(T t);

        /// <summary>
        /// Makes a DELETE request to delete an existing record.
        /// </summary>
        /// <param name="t">The record to delete</param>
        /// <returns>The count of deleted records</returns>
        Task<int> Delete(T t);

        /// <summary>
        /// Makes a DELETE request to delete a set of records, specified by a query.
        /// </summary>
        /// <param name="query">The query, records matching these constraints will be deleted</param>
        /// <returns>The count of deleted records</returns>
        Task<int> Delete(KinveyConstraints<T> query);

        /// <summary>
        /// Makes a GET request to discover the count of records in the collection.
        /// </summary>
        /// <returns>The record count of this collection</returns>
        Task<int> Count();

        /// <summary>
        /// Makes a GET request to discover the count of records in the collection which match
        /// a query.
        /// </summary>
        /// <param name="query">The query to match</param>
        /// <returns>The record count of this query</returns>
        Task<int> Count(KinveyConstraints<T> query);
    }
}