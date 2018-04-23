Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports DevExpress.XtraEditors.Registrator
Imports System.ComponentModel
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.Controls

Namespace CustomCheckedComboBox
	<UserRepositoryItem("RegisterCustomCheckedComboBoxEdit")> _
	Public Class RepositoryItemCustomCheckedComboBoxEdit
		Inherits RepositoryItemCheckedComboBoxEdit
		Shared Sub New()
			RegisterCustomCheckedComboBoxEdit()
		End Sub

		Public Sub New()
		End Sub

		Public Const CustomCheckedComboBoxEditName As String = "CustomCheckedComboBoxEdit"

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return CustomCheckedComboBoxEditName
			End Get
		End Property

		Public Shared Sub RegisterCustomCheckedComboBoxEdit()
			EditorRegistrationInfo.Default.Editors.Add(New EditorClassInfo(CustomCheckedComboBoxEditName, GetType(CustomCheckedComboBoxEdit), GetType(RepositoryItemCustomCheckedComboBoxEdit), GetType(PopupContainerEditViewInfo), New ButtonEditPainter(), True))
		End Sub

		Public Overrides Sub Assign(ByVal item As RepositoryItem)
			MyBase.Assign(item)
			Dim source As RepositoryItemCustomCheckedComboBoxEdit = TryCast(item, RepositoryItemCustomCheckedComboBoxEdit)
			Events.AddHandler(_convertCheckStateToEditValue, source.Events(_convertCheckStateToEditValue))
			Events.AddHandler(_convertEditValueToCheckState, source.Events(_convertEditValueToCheckState))
		End Sub

		Private Shared ReadOnly _convertCheckStateToEditValue As Object = New Object()

		Protected Overrides Sub PreQueryResultValue(ByVal e As QueryResultValueEventArgs)
			If CanRaiseConvertCheckStateToEditValue Then
				Dim ea As New ConvertCheckStateToEditValueEventArgs(Items.Count)
				For i As Integer = 0 To Items.Count - 1
					If Items(i).CheckState = System.Windows.Forms.CheckState.Checked Then
						ea.CheckedState(i) = Items(i).Enabled
					End If
				Next i
				RaiseConvertCheckStateToEditValue(ea)
				e.Value = ea.EditValue
			End If
		End Sub

		Protected Friend Overridable Function makeNormalValue(ByVal chekers() As Boolean) As String
			Dim res As String = ""
			If chekers IsNot Nothing Then
				For i As Integer = 0 To chekers.Length - 1
					If chekers(i) Then
						res = res & TryCast(Items(i).Value, String) + SeparatorChar & " "
					End If
				Next i
			End If
			If res.Length > 2 Then
				res = res.Substring(0, res.Length - 2)
			End If
			Return res
		End Function

		Protected Overrides Sub PreQueryDisplayText(ByVal e As QueryDisplayTextEventArgs)
			If CanRaiseConvertEditValueToCheckState AndAlso e.EditValue IsNot Nothing Then
				Dim ea As New ConvertEditValueToCheckStateEventArgs(TryCast(e.EditValue, String), Items.Count)
				RaiseConvertEditValueToCheckState(ea)
				e.DisplayText = makeNormalValue(ea.CheckedState)
			End If

			MyBase.PreQueryDisplayText(e)

		End Sub

		Public Custom Event ConvertCheckStateToEditValue As ConvertCheckStateToEditValueEventHandler
			AddHandler(ByVal value As ConvertCheckStateToEditValueEventHandler)
				Me.Events.AddHandler(_convertCheckStateToEditValue, value)
			End AddHandler
			RemoveHandler(ByVal value As ConvertCheckStateToEditValueEventHandler)
				Me.Events.RemoveHandler(_convertCheckStateToEditValue, value)
			End RemoveHandler
			RaiseEvent(ByVal sender As Object, ByVal e As ConvertCheckStateToEditValueEventArgs)
			End RaiseEvent
		End Event

		Protected Friend Overridable Sub RaiseConvertCheckStateToEditValue(ByVal e As ConvertCheckStateToEditValueEventArgs)
			Dim handler As ConvertCheckStateToEditValueEventHandler = CType(Events(_convertCheckStateToEditValue), ConvertCheckStateToEditValueEventHandler)
			If handler IsNot Nothing Then
				handler(GetEventSender(), e)
			End If
		End Sub
		Friend ReadOnly Property CanRaiseConvertCheckStateToEditValue() As Boolean
			Get
				Return CType(Events(_convertCheckStateToEditValue), ConvertCheckStateToEditValueEventHandler) IsNot Nothing
			End Get
		End Property


		Private Shared ReadOnly _convertEditValueToCheckState As Object = New Object()

		Public Custom Event ConvertEditValueToCheckState As ConvertEditValueToCheckStateEventHandler
			AddHandler(ByVal value As ConvertEditValueToCheckStateEventHandler)
				Me.Events.AddHandler(_convertEditValueToCheckState, value)
			End AddHandler
			RemoveHandler(ByVal value As ConvertEditValueToCheckStateEventHandler)
				Me.Events.RemoveHandler(_convertEditValueToCheckState, value)
			End RemoveHandler
			RaiseEvent(ByVal sender As Object, ByVal e As ConvertEditValueToCheckStateEventArgs)
			End RaiseEvent
		End Event

		Protected Friend Overridable Sub RaiseConvertEditValueToCheckState(ByVal e As ConvertEditValueToCheckStateEventArgs)
			Dim handler As ConvertEditValueToCheckStateEventHandler = CType(Events(_convertEditValueToCheckState), ConvertEditValueToCheckStateEventHandler)
			If handler IsNot Nothing Then
				handler(GetEventSender(), e)
			End If
		End Sub
		Friend ReadOnly Property CanRaiseConvertEditValueToCheckState() As Boolean
			Get
				Return CType(Events(_convertEditValueToCheckState), ConvertEditValueToCheckStateEventHandler) IsNot Nothing
			End Get
		End Property

	End Class

	Public Delegate Sub ConvertCheckStateToEditValueEventHandler(ByVal sender As Object, ByVal e As ConvertCheckStateToEditValueEventArgs)

	Public Class ConvertCheckStateToEditValueEventArgs
		Inherits EventArgs
		Private editValue_Renamed As String
		Private checkedState_Renamed() As Boolean
		Public Sub New(ByVal count As Integer)
			Me.checkedState_Renamed = New Boolean(count - 1){}
		End Sub
		Public Property EditValue() As String
			Get
				Return editValue_Renamed
			End Get
			Set(ByVal value As String)
				editValue_Renamed = value
			End Set
		End Property
		Public Property CheckedState() As Boolean()
			Get
				Return checkedState_Renamed
			End Get
			Set(ByVal value As Boolean())
				checkedState_Renamed = value
			End Set
		End Property
	End Class

	Public Delegate Sub ConvertEditValueToCheckStateEventHandler(ByVal sender As Object, ByVal e As ConvertEditValueToCheckStateEventArgs)

	Public Class ConvertEditValueToCheckStateEventArgs
		Inherits EventArgs
		Private editValue_Renamed As String
		Private checkedState_Renamed() As Boolean
		Public Sub New(ByVal value As String, ByVal count As Integer)
			Me.editValue_Renamed = value
			Me.checkedState_Renamed = New Boolean(count - 1){}
		End Sub
		Public Property EditValue() As String
			Get
				Return editValue_Renamed
			End Get
			Set(ByVal value As String)
				editValue_Renamed = value
			End Set
		End Property
		Public Property CheckedState() As Boolean()
			Get
				Return checkedState_Renamed
			End Get
			Set(ByVal value As Boolean())
				checkedState_Renamed = value
			End Set
		End Property
	End Class

	Public Class CustomCheckedComboBoxEdit
		Inherits CheckedComboBoxEdit
		Shared Sub New()
			RepositoryItemCustomCheckedComboBoxEdit.RegisterCustomCheckedComboBoxEdit()
		End Sub

		Public Sub New()
			MyBase.New()
		isShowPopup = False
		End Sub

		Public Overrides ReadOnly Property EditorTypeName() As String
			Get
				Return RepositoryItemCustomCheckedComboBoxEdit.CustomCheckedComboBoxEditName
			End Get
		End Property

		<DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
		Public Shadows ReadOnly Property Properties() As RepositoryItemCustomCheckedComboBoxEdit
			Get
				Return TryCast(MyBase.Properties, RepositoryItemCustomCheckedComboBoxEdit)
			End Get
		End Property

		Private isShowPopup As Boolean

		Public Overrides Property EditValue() As Object
			Get
				If (Not isShowPopup) Then
					Return MyBase.EditValue
				End If
				If Properties.CanRaiseConvertEditValueToCheckState AndAlso MyBase.EditValue IsNot Nothing Then
					Dim e As New ConvertEditValueToCheckStateEventArgs(TryCast(MyBase.EditValue, String), Properties.Items.Count)
					Properties.RaiseConvertEditValueToCheckState(e)
					Return Properties.makeNormalValue(e.CheckedState)
				Else
					Return MyBase.EditValue
				End If
			End Get
			Set(ByVal value As Object)
				MyBase.EditValue = value
			End Set
		End Property


		Protected Overrides Sub DoShowPopup()
			isShowPopup = True
			MyBase.DoShowPopup()
			isShowPopup = False
			PopupForm.OldEditValue = EditValue
		End Sub


	End Class
End Namespace
