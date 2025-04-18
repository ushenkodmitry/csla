﻿//-----------------------------------------------------------------------
// <copyright file="IDataPortal.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Interface defining the members of the data portal type</summary>
//-----------------------------------------------------------------------

namespace Csla
{
    /// <summary>
    /// Interface defining the members of the data portal type.
    /// </summary>
    public interface IDataPortal
  {
    /// <summary>
    /// Starts an asynchronous data portal operation to
    /// create a business object.
    /// </summary>
    /// <param name="criteria">
    /// Criteria describing the object to create.
    /// </param>
    Task<object> CreateAsync(params object?[]? criteria);
    /// <summary>
    /// Starts an asynchronous data portal operation to
    /// create a business object.
    /// </summary>
    /// <param name="criteria">
    /// Criteria describing the object to create.
    /// </param>
    Task<object> FetchAsync(params object?[]? criteria);
    /// <summary>
    /// Called by a factory method in a business class or
    /// by the UI to update an object.
    /// </summary>
    /// <param name="obj">Object to update.</param>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/>.</exception>
    Task<object> UpdateAsync(object obj);
    /// <summary>
    /// Called by a factory method in a business class or
    /// by the UI to execute a command object.
    /// </summary>
    /// <param name="command">Command object to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="command"/> is <see langword="null"/>.</exception>
    Task<object> ExecuteAsync(object command);
    /// <summary>
    /// Execute a command on the logical server.
    /// </summary>
    /// <param name="criteria">
    /// Criteria provided to the command object.
    /// </param>
    /// <returns>The resulting command object.</returns>
    Task<object> ExecuteAsync(params object?[]? criteria);
    /// <summary>
    /// Called by a factory method in a business class or
    /// by the UI to delete an object.
    /// </summary>
    /// <param name="criteria">Object-specific criteria.</param>
    Task DeleteAsync(params object?[]? criteria);
    /// <summary>
    /// Called by a factory method in a business class to create 
    /// a new object, which is loaded with default
    /// values from the database.
    /// </summary>
    /// <param name="criteria">Object-specific criteria.</param>
    /// <returns>A new object, populated with default values.</returns>
    object Create(params object?[]? criteria);
    /// <summary>
    /// Called by a factory method in a business class to retrieve
    /// an object, which is loaded with values from the database.
    /// </summary>
    /// <param name="criteria">Object-specific criteria.</param>
    /// <returns>An object populated with values from the database.</returns>
    object Fetch(params object?[]? criteria);
    /// <summary>
    /// Called to execute a Command object on the server.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To be a Command object, the object must inherit from
    /// CommandBase.
    /// </para><para>
    /// Note that this method returns a reference to the updated business object.
    /// If the server-side DataPortal is running remotely, this will be a new and
    /// different object from the original, and all object references MUST be updated
    /// to use this new object.
    /// </para><para>
    /// On the server, the Command object's DataPortal_Execute() method will
    /// be invoked and on an ObjectFactory the Execute method will be invoked. 
    /// Write any server-side code in that method. 
    /// </para>
    /// </remarks>
    /// <param name="obj">A reference to the Command object to be executed.</param>
    /// <returns>A reference to the updated Command object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/>.</exception>
    object Execute(object obj);
    /// <summary>
    /// Execute a command on the logical server.
    /// </summary>
    /// <param name="criteria">
    /// Criteria provided to the command object.
    /// </param>
    /// <returns>The resulting command object.</returns>
    object Execute(params object?[]? criteria);
    /// <summary>
    /// Insert, update or delete an object in the database.
    /// </summary>
    /// <remarks>
    /// Note that this method returns a reference to the updated business object.
    /// If the server-side DataPortal is running remotely, this will be a new and
    /// different object from the original, and all object references MUST be updated
    /// to use this new object.
    /// </remarks>
    /// <param name="obj">A reference to the business object to be updated.</param>
    /// <returns>A reference to the updated business object.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is <see langword="null"/>.</exception>
    object Update(object obj);
    /// <summary>
    /// Called by a Shared (static in C#) method in the business class to cause
    /// immediate deletion of a specific object from the database.
    /// </summary>
    /// <param name="criteria">Object-specific criteria.</param>
    void Delete(params object?[]? criteria);
  }
}
