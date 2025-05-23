﻿//-----------------------------------------------------------------------
// <copyright file="AsyncLoader.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------

namespace Csla.Core.LoadManager
{
  /// <summary>
  /// Wraps async loading by static factory method or DataPortal.BeginXYZ methods
  /// </summary>
  /// <typeparam name="T"></typeparam>
  internal class AsyncLoader<T> : IAsyncLoader
  {
    private readonly Delegate _factory;
    private readonly object[] _parameters;

    public IPropertyInfo Property { get; }

    public AsyncLoader(IPropertyInfo property, Delegate factory, params object[] parameters)
    {
      Property = property;
      _factory = factory;
      _parameters = parameters;
    }

    public void Load(Action<IAsyncLoader, IDataPortalResult> callback)
    {
      var parameters = new List<object>();
      if (_parameters.Any())
        parameters.Add(_parameters.First());
  
      var myCallback = new EventHandler<DataPortalResult<T>>((_, result) => callback(this, result));
      parameters.Add(myCallback);
      _factory.DynamicInvoke(parameters.ToArray());
    }
  }
}