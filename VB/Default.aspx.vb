Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxGridView

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Private Sub UpdateGridViewButtons(ByVal gridView As ASPxGridView)
		gridView.JSProperties("cpbtMoveUp_Enabled") = gridView.FocusedRowIndex > 0
		gridView.JSProperties("cpbtMoveDown_Enabled") = gridView.FocusedRowIndex < (gridView.VisibleRowCount - 1)
	End Sub

	Private Function GetGridViewKeyByVisibleIndex(ByVal gridView As ASPxGridView, ByVal visibleIndex As Integer) As Integer
		Return CInt(Fix(gridView.GetRowValues(visibleIndex, gridView.KeyFieldName)))
	End Function

	Protected Sub gvProducts_Init(ByVal sender As Object, ByVal e As EventArgs)
		Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
		If Session(gridView.UniqueID & "_Sort") Is Nothing Then
			Session(gridView.UniqueID & "_Sort") = New Dictionary(Of Integer, Integer)()
		End If
	End Sub

	Protected Sub gvProducts_CustomJSProperties(ByVal sender As Object, ByVal e As ASPxGridViewClientJSPropertiesEventArgs)
		Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
		UpdateGridViewButtons(gridView)
	End Sub

	Protected Sub gvProducts_CustomUnboundColumnData(ByVal sender As Object, ByVal e As ASPxGridViewColumnDataEventArgs)
		Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
		If e.Column.FieldName = "RowOrder" AndAlso e.IsGetData Then
			Dim rowKey As Integer = CInt(Fix(e.GetListSourceFieldValue(gridView.KeyFieldName)))

			Dim sortIndex As Dictionary(Of Integer, Integer) = CType(Session(gridView.UniqueID & "_Sort"), Dictionary(Of Integer, Integer))

			If (Not sortIndex.ContainsKey(rowKey)) Then
				sortIndex(rowKey) = e.ListSourceRowIndex
			End If
			e.Value = sortIndex(rowKey)
		End If
	End Sub

	Protected Sub gvProducts_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs)
		Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim parameters() As String = e.Parameters.Split("|"c)
		Dim command As String = parameters(0)

		Dim sortIndex As Dictionary(Of Integer, Integer) = CType(Session(gridView.UniqueID & "_Sort"), Dictionary(Of Integer, Integer))


		If command = "MOVEUP" OrElse command = "MOVEDOWN" Then
			Dim focusedRowKey As Integer = GetGridViewKeyByVisibleIndex(gridView, gridView.FocusedRowIndex)
			Dim index As Integer = sortIndex(focusedRowKey)
			Dim newIndex As Integer = index

			If command = "MOVEUP" Then
				If (index = 0) Then
					newIndex = index
				Else
					newIndex = index - 1
				End If
			End If
			If command = "MOVEDOWN" Then
				If (index = (gridView.VisibleRowCount - 1)) Then
					newIndex = index
				Else
					newIndex = index + 1
				End If
			End If

			For rowIndex As Integer = 0 To gridView.VisibleRowCount - 1
				Dim rowKey As Integer = GetGridViewKeyByVisibleIndex(gridView, rowIndex)
				If sortIndex(rowKey) = newIndex Then
					sortIndex(focusedRowKey) = newIndex
					sortIndex(rowKey) = index
					gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(rowKey)
					Exit For
				End If
			Next rowIndex

		End If

		If command = "DRAGROW" Then
			Dim draggingRowKey As Integer = Integer.Parse(parameters(1))
			Dim targetRowKey As Integer = Integer.Parse(parameters(2))

			Dim draggingIndex As Integer = sortIndex(draggingRowKey)
			Dim targetIndex As Integer = sortIndex(targetRowKey)

			Dim draggingDirection As Integer
			If (targetIndex < draggingIndex) Then
				draggingDirection = 1
			Else
				draggingDirection = -1
			End If

			For rowIndex As Integer = 0 To gridView.VisibleRowCount - 1
				Dim rowKey As Integer = GetGridViewKeyByVisibleIndex(gridView, rowIndex)

				If (sortIndex(rowKey) > Math.Min(targetIndex, draggingIndex)) AndAlso (sortIndex(rowKey) < Math.Max(targetIndex, draggingIndex)) Then
					sortIndex(rowKey) += draggingDirection
				End If
			Next rowIndex

			sortIndex(draggingRowKey) = targetIndex
			sortIndex(targetRowKey) = targetIndex + draggingDirection

		End If

		gridView.DataBind()
	End Sub

End Class