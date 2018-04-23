Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Collections

Namespace CustomCheckedComboBox
	Public Class User
		Private name_Renamed As String
		Private lang_Renamed As String
		Public Sub New(ByVal name As String)
			Me.name_Renamed = name
			Me.lang_Renamed = ""
		End Sub
		Public Property Lang() As String
			Get
				Return lang_Renamed
			End Get
			Set(ByVal value As String)
				lang_Renamed = value
			End Set
		End Property

		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				name_Renamed = value
			End Set

		End Property
	End Class

	Public Class UsersList
		Inherits ArrayList
		Public Function GetUser(ByVal index As Integer) As User
			Return Me(index)
		End Function

		Default Public Shadows ReadOnly Property Item(ByVal index As Integer) As User
			Get
				Return TryCast(MyBase.Item(index), User)
			End Get
		End Property
	End Class
End Namespace
