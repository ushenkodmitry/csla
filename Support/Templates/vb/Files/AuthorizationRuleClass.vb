﻿Imports Csla.Core
Imports Csla.Rules

Public Class AuthorizationRuleClass
  Inherits Csla.Rules.AuthorizationRule
  ' TODO: Add additional parameters to your rule to the constructor
  ''' <summary>
  ''' Initializes a new instance of the <see cref="AuthorizationRule"/> class.
  ''' </summary>
  ''' <param name="action">Action this rule will enforce.</param>
  ''' <param name="element">Method or property.</param>
  Public Sub New(ByVal action As AuthorizationActions, ByVal element As IMemberInfo)
    MyBase.New(action, element)
    ' TODO: Add additional constructor code here 


  End Sub


  ' TODO: Add additional parameters to your rule to the constructor
  ''' <summary>
  ''' Initializes a new instance of the <see cref="AuthorizationRule"/> class.
  ''' </summary>
  ''' <param name="action">The action.</param>
  Public Sub New(ByVal action As AuthorizationActions)
    MyBase.New(action)
    ' TODO: Add additional constructor code here 

  End Sub
 
  ' TODO: Uncomment this property if rule result is not static. 
  ''' <summary>
  '''  Notify RuelEngine that the result of this AuthzRule can not be cached. 
  '''  Default is true so AuthzRules will only run once.
  ''' </summary>
  'Public Overrides ReadOnly Property CacheResult() as Boolean
  '   Get
  '     Return False
  '  End Get
  'End Property
  

  Protected Overrides Sub Execute(ByVal context As IAuthorizationContext)
    ' TODO: Add actual rule code here. 
    'if (!access_condition)
    '{
    '  context.HasPermission = false;
    '}
  End Sub
End Class
