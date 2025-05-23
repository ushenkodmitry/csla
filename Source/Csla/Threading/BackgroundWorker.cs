﻿//-----------------------------------------------------------------------
// <copyright file="BackgroundWorker.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Wraps a System.ComponentModel.BackgroundWorker and transfers ApplicationContext.User, ClientContest, CurrentCulture and CurrentUICulture to background thread.</summary>
//-----------------------------------------------------------------------

using System.ComponentModel;

namespace Csla.Threading
{
  /// <summary>
  /// A BackgroundWorker that wraps a System.ComponentModel.BackgroundWorkertransfers ApplicationContext.User, ClientContext, CurrentCulture 
  /// and CurrentUICulture to the background thread.
  /// </summary>
  public class BackgroundWorker : Component
  {
    private readonly System.ComponentModel.BackgroundWorker _myWorker = new();
    private readonly ApplicationContext _applicationContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorker"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="applicationContext"/> is <see langword="null"/>.</exception>
    public BackgroundWorker(ApplicationContext applicationContext)
    {
      _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
      _myWorker.DoWork += InternalDoWork;
      _myWorker.RunWorkerCompleted += InternalRunWorkerCompleted;
      _myWorker.ProgressChanged += InternalProgressChanged;
    }


    // overridden event handler to be invoked by this class
    private DoWorkEventHandler? _myDoWork;
    private RunWorkerCompletedEventHandler? _myWorkerCompleted;
    private ProgressChangedEventHandler? _myWorkerProgressChanged;
    
    /// <summary>
    /// Occurs when <see cref="M:System.ComponentModel.BackgroundWorker.RunWorkerAsync"/> is called.
    /// </summary>
    [Description("Event handler to be run on a different thread when the operation begins."), Category("Asynchronous")]
    public event DoWorkEventHandler DoWork
    {
      add => _myDoWork += value;
      remove => _myDoWork -= value;
    }

    /// <summary>
    /// Occurs when the background operation has completed, has been canceled, or has raised an exception.
    /// </summary>
    [Description("Raised when the worker has completed (either through success, failure or cancellation)."), Category("Asynchronous")]
    public event RunWorkerCompletedEventHandler RunWorkerCompleted
    {
      add => _myWorkerCompleted += value;
      remove => _myWorkerCompleted -= value;
    }


    /// <summary>
    /// Occurs when <see cref="M:System.ComponentModel.BackgroundWorker.ReportProgress"/> is called.
    /// </summary>
    [Description("Occurs when ReportProgress is called.).")]
    public event ProgressChangedEventHandler ProgressChanged
    {
      add => _myWorkerProgressChanged += value;
      remove => _myWorkerProgressChanged -= value;
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker"/> is running an asynchronous operation.
    /// </summary>
    /// <value></value>
    /// <returns>true, if the <see cref="T:System.ComponentModel.BackgroundWorker"/> is running an asynchronous operation; otherwise, false.
    /// </returns>
    public bool IsBusy => _myWorker.IsBusy;

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker"/> can report progress updates.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.ComponentModel.BackgroundWorker"/> supports progress updates; otherwise false. The default is false.
    /// </returns>
    public bool WorkerReportsProgress
    {
      get => _myWorker.WorkerReportsProgress;
      set => _myWorker.WorkerReportsProgress = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="T:System.ComponentModel.BackgroundWorker"/> supports asynchronous cancellation.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.ComponentModel.BackgroundWorker"/> supports cancellation; otherwise false. The default is false.
    /// </returns>
    public bool WorkerSupportsCancellation
    {
      get => _myWorker.WorkerSupportsCancellation;
      set => _myWorker.WorkerSupportsCancellation = value;
    }

    /// <summary>
    /// Request cancelation of async operation.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">
    ///  <see cref="P:System.ComponentModel.BackgroundWorker.WorkerSupportsCancellation"/> is false.
    /// </exception>
    /// <PermissionSet>
    ///  <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// </PermissionSet>
    public void CancelAsync()
    {
      _myWorker.CancelAsync();
    }

    /// <summary>
    /// Gets a value indicating whether the application has requested cancellation of a background operation.
    /// </summary>
    /// <value></value>
    /// <returns>true if the application has requested cancellation of a background operation; otherwise, false. The default is false.
    /// </returns>
    public bool CancellationPending => _myWorker.CancellationPending;

    #region Worker Async Request

    private class WorkerAsyncRequest : ContextParams
    {
      public object? Argument { get; }

      public WorkerAsyncRequest(ApplicationContext applicationContext, object? argument)
        : base(applicationContext)
      {
        Argument = argument;
      }
    }

    private class WorkerAsyncResult
    {
      public object? Result { get; }
      public Exception? Error { get; }
      public bool Cancelled { get; private set; }

      public WorkerAsyncResult(object? result, Exception? error)
      {
        Result = result;
        Error = error;
      }
    }

#endregion

#region RunWorkerAsync

    /// <summary>
    /// Starts execution of a background operation.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">
    ///  <see cref="P:System.ComponentModel.BackgroundWorker.IsBusy"/> is true.
    /// </exception>
    /// <PermissionSet>
    ///  <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
    /// </PermissionSet>
    public void RunWorkerAsync()
    {
      RunWorkerAsync(null);
    }

    /// <summary>
    /// Starts execution of a background operation.
    /// </summary>
    /// <param name="argument">A parameter for use by the background operation to be executed in the <see cref="E:System.ComponentModel.BackgroundWorker.DoWork"/> event handler.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///  <see cref="P:System.ComponentModel.BackgroundWorker.IsBusy"/> is true.
    /// </exception>
    public void RunWorkerAsync(object? argument)
    {
      _myWorker.RunWorkerAsync(new WorkerAsyncRequest(_applicationContext, argument));
    }


#endregion

#region Private methods

    /// <summary>
    /// Run the internal DoWork
    /// - set the thread context
    /// - run the background workers doWork events
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
    void InternalDoWork(object? sender, DoWorkEventArgs e)
    {
      var request = (WorkerAsyncRequest)e.Argument!;

      // set the background worker thread context
      request.SetThreadContext();

      try
      {
        var doWorkEventArgs = new DoWorkEventArgs(request.Argument);
        _myDoWork?.Invoke(this, doWorkEventArgs);
        e.Result = new WorkerAsyncResult(doWorkEventArgs.Result, null);
        e.Cancel = doWorkEventArgs.Cancel;
      }
      // must implement exception handling and return exception in WorkerAsyncResult
      catch (Exception ex)
      {
        e.Result = new WorkerAsyncResult(null, ex);
      }
    }

    /// <summary>
    /// Executes when the async operation is completed.
    ///
    /// Set the global context and then call the RunWorkerCompleted
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
    private void InternalRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
      Exception? error = null;
      object? result = null;

      if (!e.Cancelled)
      {
        var workerResult = (WorkerAsyncResult)e.Result!;

        // must check for error as accessing e.Result will throw exception
        // if e.Error is not null.
        error = workerResult.Error;
        if (workerResult.Error == null)
        {
          result = workerResult.Result;
        }
      }


      _myWorkerCompleted?.Invoke(this, new RunWorkerCompletedEventArgs(result, error, e.Cancelled));
    }

    /// <summary>
    /// Raises the <see cref="E:System.ComponentModel.BackgroundWorker.ProgressChanged"/> event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
    /// <exception cref="T:System.InvalidOperationException">
    /// The <see cref="P:System.ComponentModel.BackgroundWorker.WorkerReportsProgress"/> property is set to false.
    /// </exception>
    private void InternalProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
      _myWorkerProgressChanged?.Invoke(this, new ProgressChangedEventArgs(e.ProgressPercentage, e.UserState));
    }

    /// <summary>
    /// Calls report progress on the underlying background worker.
    /// </summary>
    /// <param name="percentProgress">The percentage, from 0 to 100, of the background operation that is complete.</param>
    public void ReportProgress(int percentProgress)
    {
      _myWorker.ReportProgress(percentProgress);
    }

    /// <summary>
    /// Calls report progress on the background worker.
    /// </summary>
    /// <param name="percentProgress">The percent progress.</param>
    /// <param name="userState">User state object.</param>
    public void ReportProgress(int percentProgress, object? userState)
    {
      _myWorker.ReportProgress(percentProgress, userState);
    }

#endregion
  }
}
