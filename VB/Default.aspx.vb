Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private Sub UpdateGridViewButtons(ByVal gridView As ASPxGridView)
        gridView.JSProperties("cpbtMoveUp_Enabled") = gridView.FocusedRowIndex > 0
        gridView.JSProperties("cpbtMoveDown_Enabled") = gridView.FocusedRowIndex < (gridView.VisibleRowCount - 1)
    End Sub

    Private Function GetGridViewKeyByVisibleIndex(ByVal gridView As ASPxGridView, ByVal visibleIndex As Integer) As Integer
        Return CInt((gridView.GetRowValues(visibleIndex, gridView.KeyFieldName)))
    End Function

    Protected Sub gvProducts_CustomJSProperties(ByVal sender As Object, ByVal e As ASPxGridViewClientJSPropertiesEventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        UpdateGridViewButtons(gridView)
    End Sub

    Private Function GetKeyIDBySortIndex(ByVal gridView As ASPxGridView, ByVal sortIndex As Integer) As Integer
        dsHelper.SelectParameters("DisplayOrder").DefaultValue = sortIndex.ToString()
        Dim rowKey As Integer = CInt(((TryCast(dsHelper.Select(DataSourceSelectArguments.Empty), System.Data.DataView))(0)(gridView.KeyFieldName)))
        Return rowKey
    End Function

    Private Sub UpdateSortIndex(ByVal rowKey As Integer, ByVal sortIndex As Integer)
        dsProducts.UpdateParameters("ProductID").DefaultValue = rowKey.ToString()
        dsProducts.UpdateParameters("DisplayOrder").DefaultValue = sortIndex.ToString()
        dsProducts.Update()
    End Sub

    Protected Sub gvProducts_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim parameters() As String = e.Parameters.Split("|"c)
        Dim command As String = parameters(0)
        If command = "MOVEUP" OrElse command = "MOVEDOWN" Then
            Dim focusedRowKey As Integer = GetGridViewKeyByVisibleIndex(gridView, gridView.FocusedRowIndex)
            Dim index As Integer = CInt((gridView.GetRowValues(gridView.FocusedRowIndex, "DisplayOrder")))
            Dim newIndex As Integer = index
            If command = "MOVEUP" Then
                newIndex = If(index = 0, index, index - 1)
            End If
            If command = "MOVEDOWN" Then
                newIndex = If(index = gridView.VisibleRowCount, index, index + 1)
            End If
            Dim rowKey As Integer = GetKeyIDBySortIndex(gridView, newIndex)
            UpdateSortIndex(focusedRowKey, newIndex)
            UpdateSortIndex(rowKey, index)
            gridView.FocusedRowIndex = gridView.FindVisibleIndexByKeyValue(rowKey)
        End If
        If command = "DRAGROW" Then
            Dim draggingIndex As Integer = Integer.Parse(parameters(1))
            Dim targetIndex As Integer = Integer.Parse(parameters(2))
            Dim draggingRowKey As Integer = GetKeyIDBySortIndex(gridView, draggingIndex)
            Dim targetRowKey As Integer = GetKeyIDBySortIndex(gridView, targetIndex)
            Dim draggingDirection As Integer = If(targetIndex < draggingIndex, 1, -1)
            For rowIndex As Integer = 0 To gridView.VisibleRowCount - 1
                Dim rowKey As Integer = GetGridViewKeyByVisibleIndex(gridView, rowIndex)
                Dim index As Integer = CInt((gridView.GetRowValuesByKeyValue(rowKey, "DisplayOrder")))
                If (index > Math.Min(targetIndex, draggingIndex)) AndAlso (index < Math.Max(targetIndex, draggingIndex)) Then
                    UpdateSortIndex(rowKey, index + draggingDirection)
                End If
            Next rowIndex
            UpdateSortIndex(draggingRowKey, targetIndex)
            UpdateSortIndex(targetRowKey, targetIndex + draggingDirection)
        End If
        gridView.DataBind()
    End Sub
    Protected Sub gvProducts_HtmlRowPrepared(ByVal sender As Object, ByVal e As ASPxGridViewTableRowEventArgs)
        If e.RowType = GridViewRowType.Data Then
            Dim rowOrder As Object = e.GetValue("DisplayOrder")
            If rowOrder IsNot Nothing Then
                e.Row.Attributes.Add("sortOrder", rowOrder.ToString())
            End If
        End If
    End Sub
End Class