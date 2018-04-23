Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace CustomCheckedComboBox
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
		End Sub

		Private myBindingSource As New BindingSource()
		Private myUsers As New UsersList()
		Private myCheckedComboBoxEdit As New CustomCheckedComboBoxEdit()

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Dim Languge() As String = { "English", "Franche", "Spanol", "Gapanise", "Ukrain" }

			myUsers.Add(New User("Antuan"))
			myUsers.Add(New User("Bill"))
			myUsers.Add(New User("Charli"))
			myBindingSource.DataSource = myUsers


			myCheckedComboBoxEdit.Bounds = New Rectangle(10, 10, 300, 20)
			Controls.Add(myCheckedComboBoxEdit)
			For Each item As String In Languge
				myCheckedComboBoxEdit.Properties.Items.Add(item, CheckState.Unchecked, True)
			Next item
			myCheckedComboBoxEdit.DataBindings.Add("EditValue", myBindingSource, "Lang")
			AddHandler myCheckedComboBoxEdit.Properties.ConvertCheckStateToEditValue, AddressOf Properties_ConvertCheckStateToEditValue
			AddHandler myCheckedComboBoxEdit.Properties.ConvertEditValueToCheckState, AddressOf Properties_ConvertEditValueToCheckState

			textEdit1.DataBindings.Add("EditValue", myBindingSource, "Name")
			textEdit2.DataBindings.Add("EditValue", myBindingSource, "Lang")

		End Sub

		Private Sub Properties_ConvertEditValueToCheckState(ByVal sender As Object, ByVal e As ConvertEditValueToCheckStateEventArgs)
			For i As Integer = 0 To e.EditValue.Length - 1
				If e.EditValue(i) = "1"c Then
					e.CheckedState(i) = True
				End If
			Next i
		End Sub

		Private Sub Properties_ConvertCheckStateToEditValue(ByVal sender As Object, ByVal e As ConvertCheckStateToEditValueEventArgs)
			Dim newValue As String = ""
			For i As Integer = 0 To e.CheckedState.Length - 1
				If e.CheckedState(i) Then
					newValue = newValue & "1"
				Else
					newValue = newValue & "0"
				End If
			Next i
			e.EditValue = newValue
		End Sub

		Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
			myBindingSource.MovePrevious()
		End Sub

		Private Sub simpleButton2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton2.Click
			myBindingSource.MoveNext()
		End Sub

	End Class
End Namespace